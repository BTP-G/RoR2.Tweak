using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using RoR2.Artifacts;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks
{

    internal class GlobalEventTweak : TweakBase {
        private float 牺牲保底概率_;

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.Artifacts.SacrificeArtifactManager.OnServerCharacterDeath += SacrificeArtifactManager_OnServerCharacterDeath;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (victimBody?.master == null) {
                GoldenCoastPlus.GoldenCoastPlus.EnableGoldElites.Value = false;
            }
            orig(self, damageInfo, victim);
            GoldenCoastPlus.GoldenCoastPlus.EnableGoldElites.Value = true;
            if (NetworkServer.active) {
                if (victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[1].buffIndex) > 5 * victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[0].buffIndex)) {
                    DotController.InflictDot(victim, damageInfo.attacker, DeepRot.deepRotDOT, 20f);
                }
            }
        }

        private void IL_GlobalEventManager_OnCharacterDeath(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/Effects/ImpactEffects/Bandit2ResetEffect"))) {
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.EmitDelegate((CharacterBody attackerBody) => attackerBody.inventory.DeductActiveEquipmentCooldown(float.MaxValue));
            } else {
                Main.logger_.LogError("ResetCooldownsOnKill Hook Error");
            }
        }

        private void SacrificeArtifactManager_OnServerCharacterDeath(On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport) {
            if (!damageReport.victimMaster) {
                return;
            }
            if (damageReport.attackerTeamIndex == damageReport.victimTeamIndex && damageReport.victimMaster.minionOwnership.ownerMaster) {
                return;
            }
            float expAdjustedDropChancePercent = Helpers.GetExpAdjustedDropChancePercent(5 + 牺牲保底概率_++, damageReport.victim.gameObject);
            Debug.LogFormat("Drop chance from {0} == {1}", damageReport.victimBody, expAdjustedDropChancePercent);
            if (Util.CheckRoll(expAdjustedDropChancePercent)) {
                PickupIndex pickupIndex = SacrificeArtifactManager.dropTable.GenerateDrop(SacrificeArtifactManager.treasureRng);
                if (pickupIndex != PickupIndex.none) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, damageReport.victimBody.corePosition, Vector3.up * 20f);
                    牺牲保底概率_ = 0;
                }
            }
        }
    }
}