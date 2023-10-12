using BtpTweak.IndexCollections;
using GrooveSaladSpikestripContent.Content;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class BuffAndDotTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
            DotController.onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public override void Load() {
            base.Load();
            PlatedElite.damageReductionBuff.canStack = false;
        }

        public override void RemoveHooks() {
            base.RemoveHooks();
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
            DotController.onDotInflictedServerGlobal -= DotController_onDotInflictedServerGlobal;
        }

        private void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            if (buffDef.buffIndex == RoR2Content.Buffs.Nullified.buffIndex && self.teamComponent.teamIndex == TeamIndex.Void) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotController.DotIndex? preUpgradeDotIndex) {
            if (self.victimHealthComponent.shield > 0) {
                damageMultiplier *= self.victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.25f : 0.5f;
            }
            if (self.victimBody.bodyIndex == BodyIndexCollection.MageBody) {
                damageMultiplier *= 0.5f;
            }
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }

        private void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo) {
            CharacterBody victimBody = dotController.victimBody;
            if (inflictDotInfo.dotIndex == DotController.DotIndex.Bleed) {
                if (victimBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex) == 1000) {
                    CharacterBody attackerBody = inflictDotInfo.attackerObject?.GetComponent<CharacterBody>();
                    if (attackerBody) {
                        inflictDotInfo.totalDamage = 0;
                        for (int i = dotController.dotStackList.Count - 1; i >= 0; --i) {
                            var dotStack = dotController.dotStackList[i];
                            if (dotStack.dotIndex == DotController.DotIndex.Bleed) {
                                inflictDotInfo.totalDamage += dotStack.timer * 4 * dotStack.damage;
                                dotController.RemoveDotStackAtServer(i);
                            }
                        }
                        inflictDotInfo.dotIndex = DotController.DotIndex.SuperBleed;
                        inflictDotInfo.damageMultiplier = 600.6f;
                        dotController.AddDot(inflictDotInfo.attackerObject, inflictDotInfo.duration, inflictDotInfo.dotIndex, inflictDotInfo.damageMultiplier, inflictDotInfo.maxStacksFromAttacker, inflictDotInfo.totalDamage, inflictDotInfo.preUpgradeDotIndex);
                    }
                }
            } else if (inflictDotInfo.dotIndex == DeepRot.deepRotDOT) {
                victimBody.ClearTimedBuffs(DeepRot.scriptableObject.buffs[1].buffIndex);
            }
        }
    }
}