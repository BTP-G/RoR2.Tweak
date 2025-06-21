using GrooveSaladSpikestripContent.Content;
using GuestUnion.ObjectPool.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.DotController;

namespace BTP.RoR2Plugin.Tweaks {

    internal class BuffAndDotTweak : TweakBase<BuffAndDotTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float BurnDuration = 4f;
        public static readonly DamageType CoroDamageType = DamageType.PoisonOnHit | DamageType.BlightOnHit;
        public static int DeepRotSkillIndex { get; private set; }
        public static BuffIndex DeepRotBuffIndex { get; private set; }
        public static BuffIndex VoidPoisonBuffIndex { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.GlobalEventManager_OnHitEnemy += DeepRot_GlobalEventManager_OnHitEnemy;
            On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.DotController_AddDot += DeepRot_DotController_AddDot;
            IL.RoR2.DotController.FixedUpdate += DotController_FixedUpdate;
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RoR2Content.Buffs.LunarDetonationCharge.isDebuff = false;
            RoR2Content.Buffs.WarCryBuff.canStack = true;
            DeepRotSkillIndex = DeepRot.instance.GetSkillDef().skillIndex;
            DeepRotBuffIndex = DeepRot.scriptableObject.buffs[0].buffIndex;
            VoidPoisonBuffIndex = DeepRot.scriptableObject.buffs[1].buffIndex;
            PlatedElite.damageReductionBuff.canStack = false;
            GetDotDef(DotIndex.Burn).terminalTimedBuff = null;
            GetDotDef(DotIndex.StrongerBurn).terminalTimedBuff = null;
            HelfireIgniteEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HelfireIgniteEffect");
        }

        private bool BetterEvaluateDotStacksForType(DotController self, DotDef dotDef, DotIndex dotIndex) {
            var result = false;
            var dt = dotDef.interval;
            using (ListPool<PendingDamage>.Rent(out var list)) {
                for (var i = self.dotStackList.Count - 1; i >= 0; --i) {
                    var dotStack = self.dotStackList[i];
                    if (dotStack.dotIndex == dotIndex) {
                        dotStack.timer -= dt;
                        AddPendingDamageEntry(list, dotStack.attackerObject, dotStack.damage, dotStack.damageType);
                        if (dotStack.timer <= 0f) {
                            self.RemoveDotStackAtServer(i);
                        } else if (!result) {
                            result = true;
                        }
                    }
                }
                var body = self.victimBody;
                var healthComponent = body?.healthComponent;
                if (healthComponent != null) {
                    var corePosition = body.corePosition;
                    if (dotIndex == DotIndex.Fracture && list.Count > 0) {
                        EffectManager.SpawnEffect(AssetReferences.fractureImpactEffect, new EffectData {
                            origin = corePosition
                        }, transmit: true);
                    }
                    foreach (var pendingDamage in list) {
                        healthComponent.TakeDamage(new DamageInfo {
                            attacker = pendingDamage.attackerObject,
                            damage = pendingDamage.totalDamage,
                            damageColorIndex = dotDef.damageColorIndex,
                            damageType = pendingDamage.damageType | DamageType.DoT,
                            dotIndex = dotIndex,
                            inflictor = self.gameObject,
                            position = corePosition,
                        });
                        pendingDamagePool.Return(pendingDamage);
                    }
                } else {
                    foreach (var item in list) {
                        pendingDamagePool.Return(item);
                    }
                }
            }
            return result;
        }

        private void DotController_FixedUpdate(ILContext il) {
            var cursor = new ILCursor(il);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate((DotController dotController) => {
                dotController.UpdateDotVisuals();
                if (!NetworkServer.active) {
                    return;
                }
                if (dotController.victimObject == null) {
                    Object.Destroy(dotController.gameObject);
                    return;
                }
                for (var dotIndex = DotIndex.Bleed; dotIndex < DotIndex.Count; ++dotIndex) {
                    var num = 1U << (int)dotIndex;
                    if ((dotController.activeDotFlags & num) == 0) {
                        continue;
                    }
                    var num2 = dotController.dotTimers[(int)dotIndex] - Time.fixedDeltaTime;
                    if (num2 <= 0f) {
                        var dotDef = GetDotDef(dotIndex);
                        num2 += dotDef.interval;
                        dotController.NetworkactiveDotFlags = dotController.activeDotFlags & ~num;
                        if (BetterEvaluateDotStacksForType(dotController, dotDef, dotIndex)) {
                            dotController.NetworkactiveDotFlags = dotController.activeDotFlags | num;
                        }
                    }
                    dotController.dotTimers[(int)dotIndex] = num2;
                }
                if (dotController.dotStackList.Count == 0) {
                    Object.Destroy(dotController.gameObject);
                }
            });
            cursor.Emit(OpCodes.Ret);
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchLdarg(1),
                c => c.MatchLdfld<DamageInfo>("damageType"),
                c => c.MatchLdcI4(4096))) {
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.Emit(OpCodes.Ldloc_1);
                cursor.EmitDelegate((DamageInfo damageInfo, CharacterBody victimBody) => {
                    if ((damageInfo.damageType & CoroDamageType) > DamageType.Generic
                    && damageInfo.attacker.TryGetComponent<CrocoDamageTypeController>(out var crocoDamageTypeController)
                    && crocoDamageTypeController.passiveSkillSlot.skillDef.skillIndex == DeepRotSkillIndex) {
                        damageInfo.damageType &= ~CoroDamageType;
                        victimBody.AddTimedBuff(VoidPoisonBuffIndex, 20f);
                        if (victimBody.GetBuffCount(VoidPoisonBuffIndex) >= 3 * (victimBody.GetBuffCount(DeepRotBuffIndex) + 1)) {
                            var dotInfo = new InflictDotInfo {
                                victimObject = victimBody.gameObject,
                                attackerObject = damageInfo.attacker,
                                dotIndex = DeepRot.deepRotDOT,
                                duration = 20f,
                            };
                            InflictDot(ref dotInfo);
                            victimBody.ClearTimedBuffs(VoidPoisonBuffIndex);
                        }
                    }
                });
            } else {
                "DeepRot :: Hook Failed!".LogError();
            }
        }

        private void DeepRot_GlobalEventManager_OnHitEnemy(On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.orig_GlobalEventManager_OnHitEnemy orig, DeepRot self, object orig2, GlobalEventManager self2, DamageInfo damageInfo, GameObject victim) {
            (orig2 as On.RoR2.GlobalEventManager.orig_OnHitEnemy)(self2, damageInfo, victim);
        }

        private void DeepRot_DotController_AddDot(On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.orig_DotController_AddDot orig, DeepRot self, object orig2, DotController self2, GameObject attackerObject, float duration, DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotIndex? preUpgradeDotIndex) {
            if (!NetworkServer.active) {
                Debug.LogWarning("[Server] function 'System.Void RoR2.DotController::AddDot(...)' called on client");
                return;
            }

            // 获取攻击方信息
            var attackerTeam = TeamIndex.Neutral;
            var dotDamage = 0f;
            var victimBody = self2.victimBody;
            var dotStackList = self2.dotStackList;
            var dotIndexInt32 = (int)dotIndex;
            var dotDef = dotDefs[dotIndexInt32];
            var dotDamageType = DamageType.Generic;
            if (attackerObject.TryGetComponent<CharacterBody>(out var attackerBody)) {
                dotDamage = attackerBody.damage * dotDef.damageCoefficient * damageMultiplier;
                attackerTeam = attackerBody.teamComponent.teamIndex;
            }
            if (dotDamage < 1f) {
                dotDamage = 1f; // 确保dot伤害至少为1
            }
            if (totalDamage.HasValue) {
                if (duration > 0) {
                    dotDamage = totalDamage.Value * dotDef.interval / duration;
                } else {
                    duration = totalDamage.Value * dotDef.interval / dotDamage;
                }
            }

            // 统计当前dotIndex的堆叠数
            var sameDotCount = 0;
            foreach (var stack in dotStackList) {
                if (stack.dotIndex == dotIndex) {
                    ++sameDotCount;
                }
            }

            // 针对不同dot类型的特殊处理
            switch (dotIndex) {
                case DotIndex.Bleed:
                case DotIndex.SuperBleed:
                    foreach (var stack in dotStackList) {
                        if (stack.dotIndex == dotIndex && stack.attackerObject == attackerObject) {
                            stack.damage += dotDamage;
                            if (stack.timer < duration) {
                                stack.timer = duration;
                                stack.totalDuration = duration;
                            }
                            return;
                        }
                    }
                    break;

                case DotIndex.Fracture:
                case DotIndex.LunarRuin:
                case DotIndex.Frost:
                    foreach (var stack in dotStackList) {
                        if (stack.dotIndex == dotIndex && stack.timer < duration) {
                            stack.timer = duration;
                            stack.totalDuration = duration;
                        }
                    }
                    break;

                case DotIndex.Burn:
                case DotIndex.StrongerBurn:
                    if (preUpgradeDotIndex == DotIndex.Helfire && self2.victimObject == attackerObject) {
                        dotDamage = Mathf.Min(dotDamage, victimBody.healthComponent.fullCombinedHealth * 0.01f * damageMultiplier);
                        // Helfire自伤静默
                        dotDamageType |= DamageType.NonLethal | DamageType.Silent;
                    } else {
                        foreach (var stack in dotStackList) {
                            if (stack.dotIndex == dotIndex && stack.attackerObject == attackerObject) {
                                var tickInterval = dotDef.interval;
                                var allStackTotalDamage = (totalDamage ?? dotDamage * Mathf.Ceil(duration / tickInterval)) + stack.damage * Mathf.Ceil(stack.timer / tickInterval);
                                var tickCount = Mathf.Ceil(allStackTotalDamage / (dotDamage + stack.damage));
                                stack.timer = tickCount * tickInterval;
                                stack.damage = allStackTotalDamage / tickCount;
                                return;
                            }
                        }
                    }
                    break;

                case DotIndex.PercentBurn:
                    dotDamage = Mathf.Min(dotDamage, victimBody.healthComponent.fullCombinedHealth * 0.01f);
                    break;

                case DotIndex.Helfire:
                    if (attackerBody == null || attackerBody.healthComponent == null)
                        return;
                    dotDamage = Mathf.Min(
                        attackerBody.healthComponent.fullCombinedHealth * 0.01f * damageMultiplier,
                        victimBody.healthComponent.fullCombinedHealth * 0.01f * damageMultiplier
                    );
                    EffectManager.SpawnEffect(HelfireIgniteEffectPrefab, new EffectData {
                        origin = victimBody.corePosition
                    }, transmit: true);
                    // Helfire自伤静默
                    if (self2.victimObject == attackerObject)
                        dotDamageType |= DamageType.NonLethal | DamageType.Silent;
                    break;

                case DotIndex.Poison:
                    dotDamage = self2.victimHealthComponent.fullCombinedHealth * 0.01f * dotDef.interval;
                    dotDamageType = DamageType.NonLethal | DamageType.BypassArmor;
                    // 合并同类毒
                    foreach (var stack in dotStackList) {
                        if (stack.dotIndex == DotIndex.Poison) {
                            if (stack.timer < duration) {
                                stack.timer = duration;
                                stack.totalDuration = duration;
                            }
                            stack.damage = dotDamage;
                            return;
                        }
                    }
                    if (sameDotCount == 0)
                        attackerBody?.master?.playerStatsComponent?.currentStats.PushStatValue(StatDef.totalCrocoInfectionsInflicted, 1uL);
                    break;

                case DotIndex.Blight:
                    dotDamageType |= DamageType.BypassArmor;
                    break;

                default: {
                    if (dotIndex == DeepRot.deepRotDOT) {
                        dotDamage = Mathf.Max(dotDamage, victimBody.healthComponent.fullCombinedHealth * 0.025f * dotDef.interval);
                        dotDamageType |= DamageType.BypassArmor;
                    }
                    break;
                }
            }

            // 处理最大堆叠
            if (maxStacksFromAttacker.HasValue) {
                var attackerStackCount = 0;
                DotStack minStack = null;
                var minStackTimer = float.MaxValue;
                foreach (var stack in dotStackList) {
                    if (stack.dotIndex == dotIndex && stack.attackerObject == attackerObject) {
                        attackerStackCount++;
                        if (stack.timer < minStackTimer) {
                            minStackTimer = stack.timer;
                            minStack = stack;
                        }
                    }
                }
                if (attackerStackCount >= maxStacksFromAttacker.Value && minStack != null) {
                    if (minStack.timer < duration) {
                        minStack.timer = duration;
                        minStack.totalDuration = duration;
                        minStack.damage = dotDamage;
                        minStack.damageType = dotDamageType;
                    }
                    return;
                }
            }
            // 计时器和激活标志
            if (sameDotCount == 0 || dotDef.resetTimerOnAdd) {
                self2.NetworkactiveDotFlags = self2.activeDotFlags | (1u << dotIndexInt32);
                self2.dotTimers[dotIndexInt32] = dotDef.interval;
            }
            var dotStack = dotStackPool.Request();
            dotStack.dotIndex = dotIndex;
            dotStack.dotDef = dotDef;
            dotStack.attackerObject = attackerObject;
            dotStack.attackerTeam = attackerTeam;
            dotStack.timer = duration;
            dotStack.totalDuration = duration;
            dotStack.damage = dotDamage;
            dotStack.damageType = dotDamageType;
            dotStackList.Add(dotStack);
            victimBody.AddBuff(dotDef.associatedBuff);
        }

        private void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == RoR2Content.Buffs.Nullified.buffIndex
                && (self.isBoss || self.teamComponent.teamIndex == TeamIndex.Void)) {
                return;
            }
            orig(self, buffDef, duration);
        }

        //private void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo) {
        //    var victimBody = dotController.victimBody;
        //    var dotStackList = dotController.dotStackList;
        //    var lastDotStack = dotStackList[^1];
        //    if (victimBody.bodyIndex == BodyIndexes.Mage) {
        //        lastDotStack.damage *= 0.5f;
        //    }
        //    if (victimBody.healthComponent.shield > 0) {
        //        lastDotStack.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.1f : 0.2f;
        //    }
        //    switch (inflictDotInfo.dotIndex) {
        //        case DotIndex.Burn:
        //        case DotIndex.StrongerBurn: {
        //            for (int i = dotStackList.Count - 2; i > -1; --i) {
        //                var dotStack = dotStackList[i];
        //                if (dotStack.dotIndex == lastDotStack.dotIndex && dotStack.attackerObject == lastDotStack.attackerObject) {
        //                    var interval = lastDotStack.dotDef.interval;
        //                    var allStackTotalDamage = (inflictDotInfo.totalDamage ?? lastDotStack.damage * Mathf.Ceil(lastDotStack.timer / interval)) + dotStack.damage * Mathf.Ceil(dotStack.timer / interval);
        //                    var tickCount = Mathf.Ceil(allStackTotalDamage / (lastDotStack.damage + dotStack.damage));
        //                    dotStack.timer = tickCount * interval;
        //                    dotStack.damage = allStackTotalDamage / tickCount;
        //                    dotController.RemoveDotStackAtServer(dotStackList.Count - 1);
        //                    break;
        //                }
        //            }
        //            break;
        //        }
        //        case DotIndex.Fracture:
        //        case DotIndex.Poison:
        //        case DotIndex.Blight: {
        //            lastDotStack.damageType |= DamageType.BypassArmor;
        //            break;
        //        }
        //        default: {
        //            if (inflictDotInfo.dotIndex == DeepRot.deepRotDOT) {
        //                lastDotStack.damage = Mathf.Max(lastDotStack.damage, victimBody.healthComponent.fullCombinedHealth * 0.025f * lastDotStack.dotDef.interval);
        //                lastDotStack.damageType |= DamageType.BypassArmor;
        //            }
        //            break;
        //        }
        //    }
        //}

        //private void DotController_AddDot(ILContext il) {
        //    var cursor = new ILCursor(il);
        //    //===============流血===============//
        //    cursor.GotoNext(c => c.MatchLdcI4(0), c => c.MatchStloc(9))
        //        .GotoNext(i => i.MatchStloc(10))
        //        .Emit(OpCodes.Pop)
        //        .Emit(OpCodes.Ldc_I4_0)
        //        .Emit(OpCodes.Ldarg_0)
        //        .Emit(OpCodes.Ldloc, 5)
        //        .EmitDelegate((DotController dotController, DotStack newDotStack) => {
        //            var dotStackList = dotController.dotStackList;
        //            for (var i = dotStackList.Count - 1; i > -1; --i) {
        //                var oldDotStack = dotStackList[i];
        //                if (oldDotStack.dotIndex == newDotStack.dotIndex && oldDotStack.attackerObject == newDotStack.attackerObject) {
        //                    newDotStack.damage += oldDotStack.damage;
        //                    if (newDotStack.timer < oldDotStack.timer) {
        //                        newDotStack.timer = oldDotStack.timer;
        //                    }
        //                    dotController.RemoveDotStackAtServer(i);
        //                    break;
        //                }
        //            }
        //        });
        //    ////===============燃烧===============//
        //    //cursor.GotoNext(c => c.MatchLdloc(5), c => c.MatchLdloc(5));
        //    //labels = cursor.IncomingLabels;
        //    //cursor.RemoveRange(13).Emit(OpCodes.Ldarg_0).Emit(OpCodes.Ldloc, 5).EmitDelegate((DotController dotController, DotStack newDotStack) => {
        //    //    var dotStackList = dotController.dotStackList;
        //    //    for (int i = dotStackList.Count - 1; i > -1; --i) {
        //    //        var oldDotStack = dotStackList[i];
        //    //        if (oldDotStack.dotIndex == newDotStack.dotIndex && oldDotStack.attackerObject == newDotStack.attackerObject) {
        //    //            var interval = newDotStack.dotDef.interval;
        //    //            var stackDamagePerTick = newDotStack.damage + oldDotStack.damage;
        //    //            var totalDamage = (newDotStack.damage * Mathf.Ceil(newDotStack.timer / interval) + oldDotStack.damage * Mathf.Ceil(oldDotStack.timer / interval));
        //    //            $"total damage before stack == {totalDamage}".Qlog();
        //    //            newDotStack.timer = Mathf.Ceil(totalDamage / stackDamagePerTick) * interval;
        //    //            $"total damage after stack == {stackDamagePerTick * Mathf.Ceil(newDotStack.timer / interval)}".Qlog();
        //    //            newDotStack.damage = stackDamagePerTick;
        //    //            dotController.RemoveDotStackAtServer(i);
        //    //            break;
        //    //        }
        //    //    }
        //    //});
        //    //cursor.GotoPrev(x => x.MatchLdarg(0));
        //    //foreach (var label in labels) {
        //    //    cursor.MarkLabel(label);
        //    //}
        //    //===========移除不必要判断===========//
        //    cursor.GotoNext(c => c.MatchLdloc(5),
        //                    c => c.MatchLdfld<DotStack>("damage"),
        //                    c => c.MatchLdcR4(0f));
        //    cursor.RemoveRange(4);
        //}
    }
}