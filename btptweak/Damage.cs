using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.DotController;

namespace BtpTweak {

    internal class Damage {

        public static void 伤害调整() {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        public static void RemoveHook() {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            if (NetworkServer.active) {
                if (BtpTweak.虚灵战斗阶段计数_ != 0 && damageInfo.attacker) {
                    if (TeamIndex.Player == self.body?.teamComponent.teamIndex) {
                        damageInfo.damage = Mathf.Max(damageInfo.damage, self.health / 10);
                    } else {
                        damageInfo.procCoefficient *= 0.75f;
                        damageInfo.damage = Mathf.Min(damageInfo.damage, self.fullCombinedHealth / 100);
                    }
                }
            }
            orig(self, damageInfo);
        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, RoR2.CharacterBody self, RoR2.BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == DeepRot.scriptableObject.buffs[1].buffIndex
                && self.HasBuff(DeepRot.scriptableObject.buffs[0])) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private static void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotController.DotIndex? preUpgradeDotIndex) {
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