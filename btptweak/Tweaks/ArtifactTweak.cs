using RoR2;
using RoR2.Artifacts;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class ArtifactTweak : TweakBase {
        private float _牺牲保底概率;
        private float _牺牲衰减概率;

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.Artifacts.SacrificeArtifactManager.OnServerCharacterDeath += SacrificeArtifactManager_OnServerCharacterDeath;
        }

        public override void StageStartAction(Stage stage) {
            base.StageStartAction(stage);
            _牺牲保底概率 = _牺牲衰减概率 = 0;
        }

        private void SacrificeArtifactManager_OnServerCharacterDeath(On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport) {
            if (!damageReport.victimMaster) {
                return;
            }
            if (damageReport.attackerTeamIndex == damageReport.victimTeamIndex && damageReport.victimMaster.minionOwnership.ownerMaster) {
                return;
            }
            float expAdjustedDropChancePercent = Util.GetExpAdjustedDropChancePercent((damageReport.victimIsChampion ? 50 : 5) + _牺牲保底概率++ + _牺牲衰减概率, damageReport.victim.gameObject);
            Debug.LogFormat("Drop chance from {0} == {1}", damageReport.victimBody, expAdjustedDropChancePercent);
            if (Util.CheckRoll(expAdjustedDropChancePercent)) {
                PickupIndex pickupIndex = SacrificeArtifactManager.dropTable.GenerateDrop(SacrificeArtifactManager.treasureRng);
                if (pickupIndex != PickupIndex.none) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, damageReport.victimBody.corePosition, Vector3.up * 20f);
                    _牺牲保底概率 = 0;
                    _牺牲衰减概率 -= 0.1f / Run.instance.participatingPlayerCount;
                }
            }
        }
    }
}