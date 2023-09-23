using GrooveSaladSpikestripContent.Content;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks
{

    internal class BuffAndDotTweak : TweakBase {
        private BodyIndex mageBodyIndex;

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float += CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot += DotController_AddDot;
            DotController.onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }

        public override void Load() {
            base.Load();
            mageBodyIndex = BodyCatalog.FindBodyIndex(RecalculateStatsTweak.BodyName.MageBody.ToString());
        }

        public override void RemoveHooks() {
            base.RemoveHooks();
            On.RoR2.CharacterBody.AddTimedBuff_BuffDef_float -= CharacterBody_AddTimedBuff_BuffDef_float;
            On.RoR2.DotController.AddDot -= DotController_AddDot;
            DotController.onDotInflictedServerGlobal -= DotController_onDotInflictedServerGlobal;
        }

        private void CharacterBody_AddTimedBuff_BuffDef_float(On.RoR2.CharacterBody.orig_AddTimedBuff_BuffDef_float orig, CharacterBody self, BuffDef buffDef, float duration) {
            BuffIndex buffIndex = buffDef.buffIndex;
            if (buffIndex == PlatedElite.damageReductionBuff.buffIndex) {
                duration *= Mathf.Pow(0.5f, 1 + self.GetBuffCount(buffIndex));
            } else if (buffIndex == RoR2Content.Buffs.Nullified.buffIndex && self.teamComponent.teamIndex == TeamIndex.Void) {
                return;
            }
            orig(self, buffDef, duration);
        }

        private void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotController.DotIndex? preUpgradeDotIndex) {
            if (self.victimHealthComponent.shield > 0 || self.victimBody.bodyIndex == mageBodyIndex) {
                if (totalDamage != null) {
                    totalDamage *= 0.5f;
                } else {
                    duration *= 0.5f;
                }
            }
            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }

        private void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo) {
            CharacterBody victimBody = dotController.victimBody;
            if (inflictDotInfo.dotIndex == DotController.DotIndex.Bleed) {
                if (victimBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex) == 1000) {
                    CharacterBody attackerBody = inflictDotInfo.attackerObject?.GetComponent<CharacterBody>();
                    if (attackerBody == null) { return; }
                    inflictDotInfo.dotIndex = DotController.DotIndex.SuperBleed;
                    inflictDotInfo.totalDamage = dotController.victimHealthComponent.fullHealth;
                    inflictDotInfo.damageMultiplier += attackerBody.inventory.GetItemCount(RoR2Content.Items.BleedOnHitAndExplode.itemIndex);
                    float baseDamage = 0;
                    for (int i = dotController.dotStackList.Count - 1; i >= 0; --i) {
                        var dotStack = dotController.dotStackList[i];
                        if (dotStack.dotIndex == DotController.DotIndex.Bleed) {
                            baseDamage += dotStack.timer /= dotStack.dotDef.interval * dotStack.damage;
                            dotController.RemoveDotStackAtServer(i);
                        }
                    }
                    dotController.AddDot(inflictDotInfo.attackerObject, inflictDotInfo.duration, inflictDotInfo.dotIndex, inflictDotInfo.damageMultiplier, inflictDotInfo.maxStacksFromAttacker, inflictDotInfo.totalDamage, inflictDotInfo.preUpgradeDotIndex);
                    GameObject bleedExplode = Object.Instantiate(GlobalEventManager.CommonAssets.bleedOnHitAndExplodeBlastEffect, victimBody.corePosition, Quaternion.identity);
                    DelayBlast delayBlast = bleedExplode.GetComponent<DelayBlast>();
                    delayBlast.position = victimBody.corePosition;
                    delayBlast.baseDamage = baseDamage;
                    delayBlast.baseForce = 0f;
                    delayBlast.radius = 16f;
                    delayBlast.attacker = inflictDotInfo.attackerObject;
                    delayBlast.inflictor = null;
                    delayBlast.crit = Util.CheckRoll(attackerBody.crit, attackerBody.master);
                    delayBlast.maxTimer = 0f;
                    delayBlast.damageColorIndex = DamageColorIndex.Item;
                    delayBlast.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                    bleedExplode.GetComponent<TeamFilter>().teamIndex = attackerBody.teamComponent.teamIndex;
                    NetworkServer.Spawn(bleedExplode);
                }
            } else if (inflictDotInfo.dotIndex == DeepRot.deepRotDOT) {
                victimBody.ClearTimedBuffs(DeepRot.scriptableObject.buffs[1].buffIndex);
            }
        }
    }
}