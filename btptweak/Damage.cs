using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using static RoR2.DotController;
using UnityEngine.Networking;
using UnityEngine;

namespace Btp {

    internal class Damage {

        public static void 伤害调整() {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
        }

        public static void RemoveHook() {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            if (NetworkServer.active) {
                if (BtpTweak.虚灵战斗阶段计数_ != 0 && damageInfo.attacker) {
                    if (TeamIndex.Player == self.body.teamComponent.teamIndex) {
                        damageInfo.damage = Mathf.Max(self.health / 10, damageInfo.damage);
                    } else {
                        damageInfo.procCoefficient *= 0.5f;
                        damageInfo.damage = Mathf.Min(damageInfo.damage, self.fullCombinedHealth / 1000);
                    }
                }
                orig(self, damageInfo);
                if (PhaseCounter.instance?.phase == 3
                        && TeamIndex.Player != self.body.teamComponent.teamIndex
                        && self.isHealthLow
                        && self.body.name.StartsWith("Brot")
                        && !self.body.HasBuff(RoR2Content.Buffs.TonicBuff.buffIndex)) {
                    self.body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                    return;
                }
                if (BtpTweak.是否选择造物难度_
                        && TeamIndex.Monster == self.body.teamComponent.teamIndex
                        && self.isHealthLow
                        && !self.body.HasBuff(RoR2Content.Buffs.TonicBuff.buffIndex)
                        && (self.body.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction) == 0)) {
                    self.body.AddTimedBuff(RoR2Content.Buffs.TonicBuff.buffIndex, 10);
                }
            }
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