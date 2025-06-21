using BTP.RoR2Plugin.RoR2Indexes;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class CombatDirectorTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private float _精英转化几率;
        private EliteDef _特殊环境精英属性;

        void IModLoadMessageHandler.Handle() {
            On.RoR2.CombatDirector.Awake += CombatDirector_Awake;
            On.RoR2.CombatDirector.SetNextSpawnAsBoss += CombatDirector_SetNextSpawnAsBoss;
            On.RoR2.CombatDirector.Spawn += CombatDirector_Spawn;
            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
            SceneCatalog.onMostRecentSceneDefChanged += 更新当前环境的敌人精英属性;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            EliteAPI.AddCustomEliteTier(new CombatDirector.EliteTierDef() {
                costMultiplier = CombatDirector.baseEliteCostMultiplier * 10f,
                eliteTypes = EliteCatalog.eliteDefs,
                isAvailable = (rules) => Run.instance.loopClearCount > 1 && rules == SpawnCard.EliteRules.Default,
                canSelectWithoutAvailableEliteDef = false
            }, -1);
            TeamCatalog.GetTeamDef(TeamIndex.Player).softCharacterLimit = 40;
        }

        public void Run_onRunStartGlobal(Run run) {
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = 40;
            TeamCatalog.GetTeamDef(TeamIndex.Void).softCharacterLimit = 40;
        }

        private void CombatDirector_Awake(On.RoR2.CombatDirector.orig_Awake orig, CombatDirector self) {
            orig(self);
            self.ignoreTeamSizeLimit = false;
            if (Random.value < 0.5f) {
                self.skipSpawnIfTooCheap = false;
            }
        }

        private void CombatDirector_SetNextSpawnAsBoss(On.RoR2.CombatDirector.orig_SetNextSpawnAsBoss orig, CombatDirector self) {
            orig(self);
            self.ignoreTeamSizeLimit = true;
            self.skipSpawnIfTooCheap = false;
        }

        private bool CombatDirector_Spawn(On.RoR2.CombatDirector.orig_Spawn orig, CombatDirector self, SpawnCard spawnCard, EliteDef eliteDef, Transform spawnTarget, DirectorCore.MonsterSpawnDistance spawnDistance, bool preventOverhead, float valueMultiplier, DirectorPlacementRule.PlacementMode placementMode) {
            eliteDef = Util.CheckRoll(_精英转化几率) ? _特殊环境精英属性 : eliteDef;
            return orig(self, spawnCard, eliteDef, spawnTarget, spawnDistance, preventOverhead, valueMultiplier, placementMode);
        }

        private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene) {
            orig(self, nextScene);
            int newSoftCharacterLimit = Mathf.Max(16, 40 - 4 * self.stageClearCount);
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = newSoftCharacterLimit;
            TeamCatalog.GetTeamDef(TeamIndex.Void).softCharacterLimit = newSoftCharacterLimit;
        }

        private void 更新当前环境的敌人精英属性(SceneDef newSceneDef) {
            var sceneIndex = newSceneDef.sceneDefIndex;
            _精英转化几率 = 0;
            _特殊环境精英属性 = null;
            if (sceneIndex == SceneIndexes.GoldShores) {
                _特殊环境精英属性 = JunkContent.Elites.Gold;
                _精英转化几率 = 100;
            } else if (sceneIndex == SceneIndexes.DampCaveSimple) {
                _特殊环境精英属性 = RoR2Content.Elites.Fire;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexes.FrozenWall) {
                _特殊环境精英属性 = RoR2Content.Elites.Ice;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexes.SnowyForest) {
                _特殊环境精英属性 = RoR2Content.Elites.Ice;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexes.GolemPlains || sceneIndex == SceneIndexes.GolemPlains2) {
                _特殊环境精英属性 = DLC1Content.Elites.Earth;
                _精英转化几率 = 25;
            } else if (sceneIndex == SceneIndexes.VoidStage) {
                _特殊环境精英属性 = DLC1Content.Elites.Void;
                _精英转化几率 = 100;
            }
        }
    }
}