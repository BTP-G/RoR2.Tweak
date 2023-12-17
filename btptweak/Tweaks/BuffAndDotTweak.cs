using BtpTweak.RoR2Indexes;
using GrooveSaladSpikestripContent.Content;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using System.Linq;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class BuffAndDotTweak : TweakBase<BuffAndDotTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private readonly DamageType CoroDamageType = DamageType.PoisonOnHit | DamageType.BlightOnHit;
        public static int DeepRotSkillIndex { get; private set; }
        public static BuffIndex DeepRotBuffIndex { get; private set; }
        public static BuffIndex VoidPoisonBuffIndex { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            DotController.onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
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
                        victimBody.AddTimedBuff(VoidPoisonBuffIndex, 12f);
                        if (victimBody.GetBuffCount(VoidPoisonBuffIndex) >= 3 * (victimBody.GetBuffCount(DeepRotBuffIndex) + 1)) {
                            DotController.InflictDot(victimBody.gameObject, damageInfo.attacker, DeepRot.deepRotDOT, 20f);
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
            if (attackerBody?.bodyIndex == BodyIndexes.Croco) {
                lastDotStack.timer *= 1 + 0.4f * attackerBody.inventory.GetItemCount(RoR2Content.Items.DeathMark.itemIndex);
            }
            if (victimBody.bodyIndex == BodyIndexes.Mage) {
                lastDotStack.damage *= 0.5f;
            }
            if (victimBody.healthComponent.shield > 0) {
                lastDotStack.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.25f : 0.5f;
            }
            var dotIndex = inflictDotInfo.dotIndex;
            switch (dotIndex) {
                case DotController.DotIndex.Bleed:
                case DotController.DotIndex.SuperBleed:
                    if (victimBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex) == 2 || victimBody.GetBuffCount(RoR2Content.Buffs.SuperBleed.buffIndex) == 2) {
                        for (int i = dotStackList.Count - 2; i >= 0; --i) {
                            var dotStack = dotStackList[i];
                            if (dotStack.dotIndex == dotIndex) {
                                lastDotStack.damage += dotStack.damage;
                                lastDotStack.timer = Mathf.Max(lastDotStack.timer, dotStack.timer);
                                dotController.RemoveDotStackAtServer(i);
                                break;
                            }
                        }
                    }
                    break;

                case DotController.DotIndex.Poison:
                case DotController.DotIndex.Blight:
                    lastDotStack.damageType |= DamageType.BypassArmor;
                    break;

                default:
                    if (dotIndex == DeepRot.deepRotDOT) {
                        lastDotStack.damageType |= DamageType.BypassArmor;
                    }
                    break;
            }
        }
    }
}