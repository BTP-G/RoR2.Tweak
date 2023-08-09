using PlasmaCoreSpikestripContent.Content.Skills;
using static RoR2.DotController;
using UnityEngine;
using RoR2;
using System.Collections.Generic;

namespace BtpTweak {

    internal class BuffAndDotHook {
        public static BodyIndex 工匠_;
        public static Dictionary<BuffIndex, int> buff_caseLoc_ = new();

        public static void AddHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
            DotController.onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
            DotController.onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public static void LateInit() {
            dotDefs[(int)DotIndex.Bleed].damageCoefficient *= 0.5f;
            buff_caseLoc_.Add(RoR2Content.Buffs.Immune.buffIndex, 1);
            buff_caseLoc_.Add(RoR2Content.Buffs.Nullified.buffIndex, 2);
        }

        private static void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buff_caseLoc_.TryGetValue(buffDef.buffIndex, out int loc)) {
                switch (loc) {
                    case 1: {
                        buffDef = RoR2Content.Buffs.HiddenInvincibility;
                        break;
                    }
                    case 2: {
                        if (self.bodyIndex == HealthHook.虚灵_) {
                            return;
                        }
                        break;
                    }
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
                    inflictDotInfo.damageMultiplier = 10 * (1 + victimBody.GetBuffCount(RoR2Content.Buffs.SuperBleed.buffIndex));
                    dotController.dotTimers[(int)DotIndex.Bleed] = 0;
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