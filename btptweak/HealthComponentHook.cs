using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace BtpTweak {

    internal class HealthComponentHook {

        public static void AddHook() {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.HealthComponent.TriggerOneShotProtection += HealthComponent_TriggerOneShotProtection;
        }

        public static void RemoveHook() {
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
            On.RoR2.HealthComponent.TriggerOneShotProtection -= HealthComponent_TriggerOneShotProtection;
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            if (NetworkServer.active) {
                if (BtpTweak.虚灵战斗阶段计数_ != 0) {
                    if (TeamIndex.Player == self.body.teamComponent.teamIndex && damageInfo.attacker) {
                        damageInfo.damage = Mathf.Max(0.1f * BtpTweak.虚灵战斗阶段计数_ * self.fullCombinedHealth, damageInfo.damage);
                    } else if (TeamIndex.Void == self.body.teamComponent.teamIndex) {
                        switch (BtpTweak.虚灵战斗阶段计数_) {
                            case 1: {
                                damageInfo.damage = Mathf.Min(damageInfo.damage, 0.1f * self.fullCombinedHealth);
                                break;
                            }
                            case 2: {
                                damageInfo.damage = Mathf.Min(damageInfo.damage, 0.01f * self.fullCombinedHealth);
                                break;
                            }
                            case 3: {
                                damageInfo.damage = Mathf.Min(damageInfo.damage, 0.001f * self.fullCombinedHealth);
                                break;
                            }
                        }
                    }
                }
                orig(self, damageInfo);
                if (BtpTweak.是否选择造物难度_
                    && TeamIndex.Monster == self.body.teamComponent.teamIndex
                    && self.isHealthLow
                    && !self.body.HasBuff(RoR2Content.Buffs.TonicBuff.buffIndex)
                    && (self.body.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction) == 0)) {
                    if (PhaseCounter.instance && self.body.name.StartsWith("Brother")) {
                        self.body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                        self.body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost.buffIndex, 10 * (1 + Run.instance.stageClearCount) * PhaseCounter.instance.phase);
                    } else {
                        self.body.AddTimedBuff(RoR2Content.Buffs.TonicBuff.buffIndex, 10 * (1 + Run.instance.stageClearCount));
                        self.body.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex, 1 + Run.instance.stageClearCount);
                    }
                    self.ospTimer = 0.25f;
                }
            }
        }

        private static void HealthComponent_TriggerOneShotProtection(On.RoR2.HealthComponent.orig_TriggerOneShotProtection orig, HealthComponent self) {
            orig(self);
            if (NetworkServer.active) {
                self.ospTimer = 0.5f;
            }
        }
    }
}