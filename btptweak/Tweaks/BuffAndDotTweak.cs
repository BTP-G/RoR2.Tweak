using BTP.RoR2Plugin.RoR2Indexes;
using GrooveSaladSpikestripContent.Content;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using System.Collections.Generic;
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
            onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.GlobalEventManager_OnHitEnemy += DeepRot_GlobalEventManager_OnHitEnemy;
            On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.DotController_AddDot += DeepRot_DotController_AddDot;
            IL.RoR2.DotController.FixedUpdate += DotController_FixedUpdate;
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
            IL.RoR2.DotController.AddDot += DotController_AddDot;
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
        }

        private bool BetterEvaluateDotStacksForType(DotController self, DotDef dotDef, DotIndex dotIndex) {
            var list = CollectionPool<PendingDamage, List<PendingDamage>>.RentCollection();
            var result = false;
            var dt = dotDef.interval;
            for (int i = self.dotStackList.Count - 1; i >= 0; --i) {
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
            if (healthComponent) {
                var corePosition = body.corePosition;
                if (dotIndex == DotIndex.Fracture && list.Count > 0) {
                    EffectManager.SpawnEffect(AssetReferences.fractureImpactEffect, new EffectData {
                        origin = corePosition
                    }, transmit: true);
                }
                for (int i = 0; i < list.Count; ++i) {
                    var pendingDamage = list[i];
                    healthComponent.TakeDamage(new DamageInfo {
                        attacker = pendingDamage.attackerObject,
                        crit = false,
                        damage = pendingDamage.totalDamage,
                        damageColorIndex = dotDef.damageColorIndex,
                        damageType = pendingDamage.damageType | DamageType.DoT,
                        dotIndex = dotIndex,
                        force = Vector3.zero,
                        inflictor = self.gameObject,
                        position = corePosition,
                        procCoefficient = 0f,
                    });
                    pendingDamagePool.Return(pendingDamage);
                }
            } else {
                for (int i = 0; i < list.Count; ++i) {
                    pendingDamagePool.Return(list[i]);
                }
            }
            CollectionPool<PendingDamage, List<PendingDamage>>.ReturnCollection(list);
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
                if (!dotController.victimObject) {
                    UnityEngine.Object.Destroy(dotController.gameObject);
                    return;
                }
                for (DotIndex dotIndex = DotIndex.Bleed; dotIndex < DotIndex.Count; ++dotIndex) {
                    uint num = 1U << (int)dotIndex;
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
                    UnityEngine.Object.Destroy(dotController.gameObject);
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
            (orig2 as On.RoR2.DotController.orig_AddDot)(self2, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }

        private void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == RoR2Content.Buffs.Nullified.buffIndex
                && (self.isBoss || self.teamComponent.teamIndex == TeamIndex.Void)) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo) {
            var victimBody = dotController.victimBody;
            var dotStackList = dotController.dotStackList;
            var lastDotStack = dotStackList[^1];
            if (victimBody.bodyIndex == BodyIndexes.Mage) {
                lastDotStack.damage *= 0.5f;
            }
            if (victimBody.healthComponent.shield > 0) {
                lastDotStack.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.1f : 0.2f;
            }
            switch (inflictDotInfo.dotIndex) {
                case DotIndex.Burn:
                case DotIndex.StrongerBurn: {
                    for (int i = dotStackList.Count - 2; i > -1; --i) {
                        var dotStack = dotStackList[i];
                        if (dotStack.dotIndex == lastDotStack.dotIndex && dotStack.attackerObject == lastDotStack.attackerObject) {
                            var interval = lastDotStack.dotDef.interval;
                            var allStackTotalDamage = (inflictDotInfo.totalDamage ?? lastDotStack.damage * Mathf.Ceil(lastDotStack.timer / interval)) + dotStack.damage * Mathf.Ceil(dotStack.timer / interval);
                            var tickCount = Mathf.Ceil(allStackTotalDamage / (lastDotStack.damage + dotStack.damage));
                            dotStack.timer = tickCount * interval;
                            dotStack.damage = allStackTotalDamage / tickCount;
                            dotController.RemoveDotStackAtServer(dotStackList.Count - 1);
                            break;
                        }
                    }
                    break;
                }
                case DotIndex.Fracture:
                case DotIndex.Poison:
                case DotIndex.Blight: {
                    lastDotStack.damageType |= DamageType.BypassArmor;
                    break;
                }
                default: {
                    if (inflictDotInfo.dotIndex == DeepRot.deepRotDOT) {
                        lastDotStack.damage = Mathf.Max(lastDotStack.damage, victimBody.healthComponent.fullCombinedHealth * 0.025f * lastDotStack.dotDef.interval);
                        lastDotStack.damageType |= DamageType.BypassArmor;
                    }
                    break;
                }
            }
        }

        private void DotController_AddDot(ILContext il) {
            var cursor = new ILCursor(il);
            //===============流血===============//
            cursor.GotoNext(c => c.MatchLdcI4(0), c => c.MatchStloc(9))
                .GotoNext(i => i.MatchStloc(10))
                .Emit(OpCodes.Pop)
                .Emit(OpCodes.Ldc_I4_0)
                .Emit(OpCodes.Ldarg_0)
                .Emit(OpCodes.Ldloc, 5)
                .EmitDelegate((DotController dotController, DotStack newDotStack) => {
                    var dotStackList = dotController.dotStackList;
                    for (var i = dotStackList.Count - 1; i > -1; --i) {
                        var oldDotStack = dotStackList[i];
                        if (oldDotStack.dotIndex == newDotStack.dotIndex && oldDotStack.attackerObject == newDotStack.attackerObject) {
                            newDotStack.damage += oldDotStack.damage;
                            if (newDotStack.timer < oldDotStack.timer) {
                                newDotStack.timer = oldDotStack.timer;
                            }
                            dotController.RemoveDotStackAtServer(i);
                            break;
                        }
                    }
                });
            ////===============燃烧===============//
            //cursor.GotoNext(c => c.MatchLdloc(5), c => c.MatchLdloc(5));
            //labels = cursor.IncomingLabels;
            //cursor.RemoveRange(13).Emit(OpCodes.Ldarg_0).Emit(OpCodes.Ldloc, 5).EmitDelegate((DotController dotController, DotStack newDotStack) => {
            //    var dotStackList = dotController.dotStackList;
            //    for (int i = dotStackList.Count - 1; i > -1; --i) {
            //        var oldDotStack = dotStackList[i];
            //        if (oldDotStack.dotIndex == newDotStack.dotIndex && oldDotStack.attackerObject == newDotStack.attackerObject) {
            //            var interval = newDotStack.dotDef.interval;
            //            var stackDamagePerTick = newDotStack.damage + oldDotStack.damage;
            //            var totalDamage = (newDotStack.damage * Mathf.Ceil(newDotStack.timer / interval) + oldDotStack.damage * Mathf.Ceil(oldDotStack.timer / interval));
            //            $"total damage before stack == {totalDamage}".Qlog();
            //            newDotStack.timer = Mathf.Ceil(totalDamage / stackDamagePerTick) * interval;
            //            $"total damage after stack == {stackDamagePerTick * Mathf.Ceil(newDotStack.timer / interval)}".Qlog();
            //            newDotStack.damage = stackDamagePerTick;
            //            dotController.RemoveDotStackAtServer(i);
            //            break;
            //        }
            //    }
            //});
            //cursor.GotoPrev(x => x.MatchLdarg(0));
            //foreach (var label in labels) {
            //    cursor.MarkLabel(label);
            //}
            //===========移除不必要判断===========//
            cursor.GotoNext(c => c.MatchLdloc(5),
                            c => c.MatchLdfld<DotStack>("damage"),
                            c => c.MatchLdcR4(0f));
            cursor.RemoveRange(4);
        }
    }
}