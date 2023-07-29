using PlasmaCoreSpikestripContent.Content.Skills;
using static RoR2.DotController;
using UnityEngine;
using RoR2;

namespace BtpTweak {

    internal class BuffAndDotHook {
        public static BodyIndex mageBodyindex;

        public static void AddHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == RoR2Content.Buffs.Nullified.buffIndex && self.teamComponent.teamIndex == TeamIndex.Void) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private static void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotIndex? preUpgradeDotIndex) {
            CharacterBody victimBody = self.victimBody;
            HealthComponent victimHealthComponent = self.victimHealthComponent;
            if (dotIndex == DotIndex.Bleed) {
                if (victimBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex) == 1000) {
                    dotIndex = DotIndex.SuperBleed;
                    totalDamage = 0.1f * victimHealthComponent.fullHealth;
                    damageMultiplier *= BtpTweak.玩家等级_ * (1 + victimBody.GetBuffCount(RoR2Content.Buffs.SuperBleed.buffIndex));
                    self.dotTimers[(int)DotIndex.Bleed] = 0;
                    for (int i = self.dotStackList.Count - 1; i >= 0; --i) {
                        if (self.dotStackList[i].dotIndex == DotIndex.Bleed) {
                            self.RemoveDotStackAtServer(i);
                        }
                    }
                }
            } else if (dotIndex == DeepRot.deepRotDOT) {
                victimBody.ClearTimedBuffs(DeepRot.scriptableObject.buffs[1].buffIndex);
            }
            if ((victimHealthComponent.shield + victimHealthComponent.barrier) > 0 || victimBody.bodyIndex == mageBodyindex) {
                if (totalDamage != null) {
                    totalDamage *= 0.5f;
                } else {
                    duration *= 0.5f;
                }
            }
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }
    }
}