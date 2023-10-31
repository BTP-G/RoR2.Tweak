using BtpTweak.IndexCollections;
using GrooveSaladSpikestripContent.Content;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using System.Linq;
using UnityEngine;
using static RoR2.DotController;

namespace BtpTweak.Tweaks {

    internal class BuffAndDotTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public override void Load() {
            base.Load();
            PlatedElite.damageReductionBuff.canStack = false;
            RoR2Content.Buffs.LunarDetonationCharge.isDebuff = false;
        }

        public override void RemoveHooks() {
            base.RemoveHooks();
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            onDotInflictedServerGlobal -= DotController_onDotInflictedServerGlobal;
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
            if (attackerBody?.bodyIndex == BodyIndexCollection.CrocoBody) {
                lastDotStack.timer *= 1 + 0.4f * attackerBody.inventory.GetItemCount(RoR2Content.Items.DeathMark.itemIndex);
            }
            if (victimBody.bodyIndex == BodyIndexCollection.MageBody) {
                lastDotStack.damage *= 0.5f;
            }
            if (victimBody.healthComponent.shield > 0) {
                lastDotStack.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.25f : 0.5f;
            }
            var dotIndex = inflictDotInfo.dotIndex;
            switch (dotIndex) {
                case DotIndex.Bleed:
                case DotIndex.SuperBleed:
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

                case DotIndex.Poison:
                case DotIndex.Blight:
                    lastDotStack.damageType |= DamageType.BypassArmor;
                    break;

                default:
                    if (dotIndex == DeepRot.deepRotDOT) {
                        lastDotStack.damageType |= DamageType.BypassArmor;
                        victimBody.ClearTimedBuffs(DeepRot.scriptableObject.buffs[1].buffIndex);
                    }
                    break;
            }
        }
    }
}