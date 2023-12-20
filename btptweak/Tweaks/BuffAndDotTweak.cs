using BtpTweak.RoR2Indexes;
using GrooveSaladSpikestripContent.Content;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.DotController;

namespace BtpTweak.Tweaks {

    internal class BuffAndDotTweak : TweakBase<BuffAndDotTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public static readonly DamageType CoroDamageType = DamageType.PoisonOnHit | DamageType.BlightOnHit;
        public static int DeepRotSkillIndex { get; private set; }
        public static BuffIndex DeepRotBuffIndex { get; private set; }
        public static BuffIndex VoidPoisonBuffIndex { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
            On.RoR2.DotController.FixedUpdate += DotController_FixedUpdate;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.GlobalEventManager_OnHitEnemy += DeepRot_GlobalEventManager_OnHitEnemy;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            PlatedElite.damageReductionBuff.canStack = false;
            RoR2Content.Buffs.LunarDetonationCharge.isDebuff = false;
            RoR2Content.Buffs.WarCryBuff.canStack = true;
            DeepRotSkillIndex = DeepRot.instance.GetSkillDef().skillIndex;
            DeepRotBuffIndex = DeepRot.scriptableObject.buffs[0].buffIndex;
            VoidPoisonBuffIndex = DeepRot.scriptableObject.buffs[1].buffIndex;
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

        private void DotController_FixedUpdate(On.RoR2.DotController.orig_FixedUpdate orig, DotController self) {
            self.UpdateDotVisuals();
            if (!NetworkServer.active) {
                return;
            }
            if (!self.victimObject) {
                UnityEngine.Object.Destroy(self.gameObject);
                return;
            }
            for (DotIndex dotIndex = DotIndex.Bleed; dotIndex < DotIndex.Count; ++dotIndex) {
                uint num = 1U << (int)dotIndex;
                if ((self.activeDotFlags & num) == 0) {
                    continue;
                }
                var num2 = self.dotTimers[(int)dotIndex] - Time.fixedDeltaTime;
                if (num2 <= 0f) {
                    var dotDef = GetDotDef(dotIndex);
                    num2 += dotDef.interval;
                    self.NetworkactiveDotFlags = self.activeDotFlags & ~num;
                    if (BetterEvaluateDotStacksForType(self, dotDef, dotIndex)) {
                        self.NetworkactiveDotFlags = self.activeDotFlags | num;
                    }
                }
                self.dotTimers[(int)dotIndex] = num2;
            }
            if (self.dotStackList.Count == 0) {
                UnityEngine.Object.Destroy(self.gameObject);
            }
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchLdarg(1), c => c.MatchLdfld<DamageInfo>("damageType"), c => c.MatchLdcI4(0x1000))) {
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.Emit(OpCodes.Ldloc_2);
                cursor.EmitDelegate((DamageInfo damageInfo, CharacterBody victimBody) => {
                    if ((damageInfo.damageType & CoroDamageType) > DamageType.Generic
                    && damageInfo.attacker.TryGetComponent<CrocoDamageTypeController>(out var crocoDamageTypeController)
                    && crocoDamageTypeController.passiveSkillSlot.skillDef.skillIndex == DeepRotSkillIndex) {
                        damageInfo.damageType &= ~CoroDamageType;
                        victimBody.AddTimedBuff(VoidPoisonBuffIndex, 20f);
                        if (victimBody.GetBuffCount(VoidPoisonBuffIndex) >= 3 * (victimBody.GetBuffCount(DeepRotBuffIndex) + 1)) {
                            InflictDot(victimBody.gameObject, damageInfo.attacker, DeepRot.deepRotDOT, 20f);
                            victimBody.ClearTimedBuffs(VoidPoisonBuffIndex);
                        }
                    }
                });
            } else {
                Main.Logger.LogError("DeepRot :: Hook Failed!");
            }
        }

        private void DeepRot_GlobalEventManager_OnHitEnemy(On.PlasmaCoreSpikestripContent.Content.Skills.DeepRot.orig_GlobalEventManager_OnHitEnemy orig, DeepRot self, object orig2, GlobalEventManager self2, DamageInfo damageInfo, GameObject victim) {
            (orig2 as On.RoR2.GlobalEventManager.orig_OnHitEnemy)(self2, damageInfo, victim);
        }

        private void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == RoR2Content.Buffs.Nullified.buffIndex && self.teamComponent.teamIndex == TeamIndex.Void) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo) {
            var attackerBody = inflictDotInfo.attackerObject?.GetComponent<CharacterBody>();
            var victimBody = dotController.victimBody;
            var dotStackList = dotController.dotStackList;
            var lastDotStack = dotStackList.Last();
            if (attackerBody != null) {
                if (attackerBody.bodyIndex == BodyIndexes.Croco) {
                    lastDotStack.timer *= 1 + 0.4f * attackerBody.inventory.GetItemCount(RoR2Content.Items.DeathMark.itemIndex);
                } else if (victimBody.bodyIndex == BodyIndexes.Mage) {
                    lastDotStack.damage *= 0.5f;
                }
            }
            if (victimBody.healthComponent.shield > 0) {
                lastDotStack.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.25f : 0.5f;
            }
            var dotIndex = inflictDotInfo.dotIndex;
            switch (dotIndex) {
                case DotIndex.Bleed:
                case DotIndex.SuperBleed: {
                    if (victimBody.GetBuffCount(lastDotStack.dotDef.associatedBuff) > 1) {
                        for (int i = dotStackList.Count - 2; i >= 0; --i) {
                            var dotStack = dotStackList[i];
                            if (dotStack.dotIndex == dotIndex) {
                                dotStack.damage += lastDotStack.damage;
                                dotController.RemoveDotStackAtServer(dotStackList.Count - 1);
                                break;
                            }
                        }
                    }
                    break;
                }
                case DotIndex.Poison:
                case DotIndex.Blight: {
                    lastDotStack.damageType |= DamageType.BypassArmor;
                    break;
                }
                default: {
                    if (dotIndex == DeepRot.deepRotDOT) {
                        lastDotStack.damageType |= DamageType.BypassArmor;
                    }
                    break;
                }
            }
        }
    }
}