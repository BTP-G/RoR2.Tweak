﻿using RoR2;
using RoR2.Artifacts;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class ArtifactTweak : ModComponent, IModLoadMessageHandler {
        private float _牺牲保底概率;
        private float _牺牲衰减概率;

        void IModLoadMessageHandler.Handle() {
            On.RoR2.Artifacts.SacrificeArtifactManager.OnServerCharacterDeath += SacrificeArtifactManager_OnServerCharacterDeath;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        private void SacrificeArtifactManager_OnServerCharacterDeath(On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport) {
            if (damageReport.victimMaster == null) {
                return;
            }
            if (damageReport.attackerTeamIndex == damageReport.victimTeamIndex && damageReport.victimMaster.minionOwnership.ownerMaster) {
                return;
            }
            var baseDropChancePercent = Settings.牺牲基础掉率.Value + _牺牲保底概率 - _牺牲衰减概率;
            if (damageReport.victimIsChampion) {
                baseDropChancePercent *= 1.5f;
            } else if (damageReport.victimIsElite) {
                baseDropChancePercent *= 1.25f;
            }
            var expAdjustedDropChancePercent = Util.GetExpAdjustedDropChancePercent(baseDropChancePercent, damageReport.victim.gameObject);
            Debug.LogFormat("Drop chance from {0} == {1}", damageReport.victimBody, expAdjustedDropChancePercent);
            if (Util.CheckRoll(expAdjustedDropChancePercent)) {
                var pickupIndex = SacrificeArtifactManager.dropTable.GenerateDrop(SacrificeArtifactManager.treasureRng);
                if (pickupIndex != PickupIndex.none) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, damageReport.victimBody.corePosition, new Vector3(0, 20f, 0));
                    var itemScore = 0f;
                    switch (PickupCatalog.GetPickupDef(pickupIndex).itemTier) {
                        case ItemTier.Tier1:
                            itemScore = 0.01f;
                            break;

                        case ItemTier.Tier2:
                            itemScore = 0.03f;
                            break;

                        case ItemTier.Tier3:
                            itemScore = 0.15f;
                            break;
                    }
                    if (itemScore > 0f) {
                        _牺牲保底概率 = 0;
                        _牺牲衰减概率 += Settings.牺牲基础掉率.Value * itemScore / Run.instance.participatingPlayerCount;
                    }
                }
            } else {
                _牺牲保底概率 += Settings.牺牲基础掉率.Value * 0.04f * Run.instance.participatingPlayerCount;
            }
        }

        private void Stage_onStageStartGlobal(Stage stage) {
            _牺牲保底概率 = _牺牲衰减概率 = 0;
        }
    }
}