using BtpTweak.IndexCollections;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class CombatDirectorTweak : TweakBase {
        private float _精英转化几率;
        private EliteDef _特殊环境精英属性;

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.CombatDirector.Awake += CombatDirector_Awake;
            On.RoR2.CombatDirector.SetNextSpawnAsBoss += CombatDirector_SetNextSpawnAsBoss;
            On.RoR2.CombatDirector.Spawn += CombatDirector_Spawn;
            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
        }

        public override void Load() {
            base.Load();
            CombatDirector.EliteTierDef eliteTierDef = new() {
                costMultiplier = CombatDirector.baseEliteCostMultiplier,
                eliteTypes = EliteCatalog.eliteDefs,
                isAvailable = (SpawnCard.EliteRules rules) => Run.instance.loopClearCount > 1 && rules == SpawnCard.EliteRules.Default,
                canSelectWithoutAvailableEliteDef = false
            };
            R2API.EliteAPI.AddCustomEliteTier(eliteTierDef);
        }

        public override void RemoveHooks() {
            base.RemoveHooks();
            On.RoR2.CombatDirector.Awake -= CombatDirector_Awake;
            On.RoR2.CombatDirector.Spawn -= CombatDirector_Spawn;
            On.RoR2.Run.AdvanceStage -= Run_AdvanceStage;
        }

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = 40;
            TeamCatalog.GetTeamDef(TeamIndex.Void).softCharacterLimit = 40;
            SetEliteForCurrentScene(SceneCatalog.GetSceneDefForCurrentScene().sceneDefIndex);
        }

        private void CombatDirector_Awake(On.RoR2.CombatDirector.orig_Awake orig, CombatDirector self) {
            orig(self);
            self.ignoreTeamSizeLimit = false;
        }

        private void CombatDirector_SetNextSpawnAsBoss(On.RoR2.CombatDirector.orig_SetNextSpawnAsBoss orig, CombatDirector self) {
            orig(self);
            self.ignoreTeamSizeLimit = true;
        }

        private bool CombatDirector_Spawn(On.RoR2.CombatDirector.orig_Spawn orig, CombatDirector self, SpawnCard spawnCard, EliteDef eliteDef, Transform spawnTarget, DirectorCore.MonsterSpawnDistance spawnDistance, bool preventOverhead, float valueMultiplier, DirectorPlacementRule.PlacementMode placementMode) {
            eliteDef = Util.CheckRoll(_精英转化几率) ? _特殊环境精英属性 : eliteDef;
            return orig(self, spawnCard, eliteDef, spawnTarget, spawnDistance, preventOverhead, valueMultiplier, placementMode);
        }

        private void SetEliteForCurrentScene(SceneIndex sceneIndex) {
            _精英转化几率 = 0;
            _特殊环境精英属性 = null;
            if (sceneIndex == SceneIndexCollection.goldshores) {
                _特殊环境精英属性 = JunkContent.Elites.Gold;
                _精英转化几率 = 100;
            } else if (sceneIndex == SceneIndexCollection.dampcavesimple) {
                _特殊环境精英属性 = RoR2Content.Elites.Fire;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexCollection.frozenwall) {
                _特殊环境精英属性 = RoR2Content.Elites.Ice;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexCollection.snowyforest) {
                _特殊环境精英属性 = RoR2Content.Elites.Ice;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexCollection.golemplains || sceneIndex == SceneIndexCollection.golemplains2) {
                _特殊环境精英属性 = DLC1Content.Elites.Earth;
                _精英转化几率 = 25;
            }
        }

        private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene) {
            orig(self, nextScene);
            int newSoftCharacterLimit = Mathf.Max(18, 40 - 4 * self.stageClearCount);
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = newSoftCharacterLimit;
            TeamCatalog.GetTeamDef(TeamIndex.Void).softCharacterLimit = newSoftCharacterLimit;
            SetEliteForCurrentScene(nextScene.sceneDefIndex);
        }
    }
}