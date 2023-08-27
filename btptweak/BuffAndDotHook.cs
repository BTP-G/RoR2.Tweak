using PlasmaCoreSpikestripContent.Content.Skills;
using UnityEngine;
using RoR2;
using static RoR2.DotController;
using System.Collections.Generic;
using GrooveSaladSpikestripContent.Content;

namespace BtpTweak {

    internal class BuffAndDotHook {
        public static Dictionary<BuffIndex, BuffName> BuffIndexToName_ = new();
        public static BodyIndex 工匠_;

        public enum BuffName {
            None = 0,
            Immune,
            Nullified,
            damageReductionBuff,
        }

        public static void AddHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
            onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
            onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public static void LateInit() {
            BuffIndexToName_.Add(RoR2Content.Buffs.Immune.buffIndex, BuffName.Immune);
            BuffIndexToName_.Add(RoR2Content.Buffs.Nullified.buffIndex, BuffName.Nullified);
            BuffIndexToName_.Add(PlatedElite.damageReductionBuff.buffIndex, BuffName.damageReductionBuff);
            GetDotDef(DotIndex.Bleed).damageCoefficient *= 0.5f;
        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (BuffIndexToName_.TryGetValue(buffDef.buffIndex, out BuffName buffName)) {
                switch (buffName) {
                    case BuffName.Immune:
                        buffDef = RoR2Content.Buffs.HiddenInvincibility;
                        break;

                    case BuffName.Nullified:
                        if (self.teamComponent.teamIndex == TeamIndex.Void) {
                            return;
                        }
                        break;

                    case BuffName.damageReductionBuff:
                        duration *= Mathf.Pow(0.5f, 1 + self.GetBuffCount(buffDef.buffIndex));
                        break;
                }
            }
            orig(self, buffDef, duration);
        }

        private static void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotIndex? preUpgradeDotIndex) {
            if (self.victimHealthComponent.shield > 0 || self.victimBody.bodyIndex == 工匠_) {
                if (totalDamage != null) {
                    totalDamage *= 0.5f;
                } else {
                    duration *= 0.5f;
                }
            }
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }

        private static void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo) {
            CharacterBody victimBody = dotController.victimBody;
            if (inflictDotInfo.dotIndex == DotIndex.Bleed) {
                if (victimBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex) == 1000) {
                    inflictDotInfo.dotIndex = DotIndex.SuperBleed;
                    inflictDotInfo.totalDamage = 0.1f * dotController.victimHealthComponent.fullHealth;
                    inflictDotInfo.damageMultiplier *= 100 * (1 + victimBody.GetBuffCount(RoR2Content.Buffs.SuperBleed.buffIndex));
                    for (int i = dotController.dotStackList.Count - 1; i >= 0; --i) {
                        if (dotController.dotStackList[i].dotIndex == DotIndex.Bleed) {
                            dotController.RemoveDotStackAtServer(i);
                        }
                    }
                    dotController.AddDot(inflictDotInfo.attackerObject, inflictDotInfo.duration, inflictDotInfo.dotIndex, inflictDotInfo.damageMultiplier, inflictDotInfo.maxStacksFromAttacker, inflictDotInfo.totalDamage, inflictDotInfo.preUpgradeDotIndex);
                }
            } else if (inflictDotInfo.dotIndex == DeepRot.deepRotDOT) {
                victimBody.ClearTimedBuffs(DeepRot.scriptableObject.buffs[1].buffIndex);
            }
        }
    }
}