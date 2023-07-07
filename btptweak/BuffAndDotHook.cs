using PlasmaCoreSpikestripContent.Content.Skills;
using static RoR2.DotController;
using UnityEngine;
using RoR2;

namespace BtpTweak {

    internal class BuffAndDotHook {

        public static void AddHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == DeepRot.scriptableObject.buffs[1].buffIndex
                && self.HasBuff(DeepRot.scriptableObject.buffs[0].buffIndex)) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private static void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotIndex? preUpgradeDotIndex) {
            if (dotIndex == DeepRot.deepRotDOT) {
                duration = 666f;
            } else if (dotIndex == DotIndex.Blight) {
                totalDamage = self.victimBody.healthComponent.health;
            } else if (dotIndex == DotIndex.Poison) {
                totalDamage = self.victimBody.healthComponent.fullCombinedHealth;
            }
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }
    }
}