using BTP.RoR2Plugin.Utils;
using EntityStates.BrotherMonster;
using R2API.AddressReferencedAssets;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

namespace BTP.RoR2Plugin.Tweaks.MithrixTweaks {

    internal class PhaseTweak : ModComponent, IModLoadMessageHandler {
        private static readonly SpawnCard brotherSpawnCard = CharacterSpawnCardPaths.cscBrother.Load<CharacterSpawnCard>();
        private static readonly AddressReferencedAsset<PostProcessProfile> ppMeteorStorm = "RoR2/Base/title/PostProcessing/ppLocalMeteorStorm.asset";
        private static readonly EntityStateMachine brotherEntityStateMachine = GameObjectPaths.BrotherBody.LoadComponent<EntityStateMachine>();

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.Missions.BrotherEncounter.Phase1.OnEnter += Phase1_OnEnter;
            On.EntityStates.Missions.BrotherEncounter.Phase2.OnEnter += Phase2_OnEnter;
            On.EntityStates.Missions.BrotherEncounter.Phase3.OnEnter += Phase3_OnEnter;
            On.EntityStates.Missions.BrotherEncounter.Phase4.OnEnter += Phase4_OnEnter;
        }

        private void Phase1_OnEnter(On.EntityStates.Missions.BrotherEncounter.Phase1.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase1 self) {
            orig(self);
            brotherEntityStateMachine.initialStateType = new EntityStates.SerializableEntityStateType(typeof(ThroneSpawnState));
        }

        private void Phase2_OnEnter(On.EntityStates.Missions.BrotherEncounter.Phase2.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase2 self) {
            orig(self);
            var sceneChildLocator = SceneInfo.instance.GetComponent<ChildLocator>();
            self.phaseScriptedCombatEncounter.spawns = [new ScriptedCombatEncounter.SpawnInfo {
                explicitSpawnPosition = sceneChildLocator.FindChild("CenterOfArena"),
                spawnCard = brotherSpawnCard,
            }];
            brotherEntityStateMachine.initialStateType = new EntityStates.SerializableEntityStateType(typeof(SkySpawnState));
        }

        private void Phase3_OnEnter(On.EntityStates.Missions.BrotherEncounter.Phase3.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase3 self) {
            orig(self);
            new AddressReferencedAsset<PostProcessProfile>();
            var volume = SceneInfo.instance.gameObject.AddComponent<PostProcessVolume>();
            volume.enabled = true;
            volume.isGlobal = true;
            volume.priority = 9999f;
            volume.profile = ppMeteorStorm;
            var esController = GameObject.Find("EscapeSequenceController");
            if (esController) {
                var megaGlow = Object.Instantiate(esController.transform.GetChild(0).GetChild(8).gameObject, new Vector3(-88.5f, 491.5f, -0.3f), Quaternion.identity);
                megaGlow.AddComponent<NetworkIdentity>();
                megaGlow.transform.eulerAngles = new Vector3(270, 0, 0);
                megaGlow.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                NetworkServer.Spawn(megaGlow);
            }
        }

        private void Phase4_OnEnter(On.EntityStates.Missions.BrotherEncounter.Phase4.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase4 self) {
            orig(self);
            var blockingPillars = self.childLocator.FindChild("BlockingPillars");
            if (blockingPillars) {
                blockingPillars.gameObject.SetActive(false);
            }
            if (NetworkServer.active) {
                foreach (var teamMember in TeamComponent.GetTeamMembers(TeamIndex.Player)) {
                    teamMember.body.SetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex, 0);
                    teamMember.body.AddBuff(RoR2Content.Buffs.LunarShell.buffIndex);
                }
            }
        }
    }
}