using BtpTweak.Utils;
using RoR2;
using System;
using UnityEngine;

namespace BtpTweak.Tweaks
{

    internal class CombatDirectorTweak : TweakBase {
        private int 当前敌人生成数_;
        private int 敌人最大生成数_;
        private short 精英转化几率_;
        private EliteDef 特殊环境精英属性_;

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.CombatDirector.Simulate += CombatDirector_Simulate;
            On.RoR2.CombatDirector.Spawn += CombatDirector_Spawn;
            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
        }

        public void CombatDirector_Simulate(On.RoR2.CombatDirector.orig_Simulate orig, CombatDirector self, float deltaTime) {
            if (当前敌人生成数_ > 敌人最大生成数_ * (TeleporterInteraction.instance?.activationState == TeleporterInteraction.ActivationState.Charging ? 1.5f : 1)) {
                当前敌人生成数_ = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
                return;
            }
            orig(self, deltaTime);
        }

        public override void Load() {
            base.Load();
            CombatDirector.EliteTierDef eliteTierDef = Array.Find(CombatDirector.eliteTiers, match => match.costMultiplier == 36);
            eliteTierDef.eliteTypes = EliteCatalog.eliteDefs;
        }

        public override void RemoveHooks() {
            base.RemoveHooks();
            On.RoR2.CombatDirector.Simulate -= CombatDirector_Simulate;
            On.RoR2.CombatDirector.Spawn -= CombatDirector_Spawn;
        }

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            int stageCount = 1;
            string sceneName = SceneCatalog.GetSceneDefForCurrentScene().cachedName;
            敌人最大生成数_ = Mathf.Max(6, 25 - stageCount);
            精英转化几率_ = 0;
            特殊环境精英属性_ = null;
            if (sceneName == "goldshores") {
                特殊环境精英属性_ = JunkContent.Elites.Gold;
                精英转化几率_ = 100;
            } else if (sceneName == "dampcavesimple") {
                特殊环境精英属性_ = RoR2Content.Elites.Fire;
                精英转化几率_ = 50;
            } else if (sceneName == "frozenwall") {
                特殊环境精英属性_ = RoR2Content.Elites.Ice;
                精英转化几率_ = 60;
            } else if (sceneName == "snowyforest") {
                特殊环境精英属性_ = RoR2Content.Elites.Ice;
                精英转化几率_ = 36;
            } else if (sceneName.StartsWith("golemplains")) {
                特殊环境精英属性_ = DLC1Content.Elites.Earth;
                精英转化几率_ = 36;
            }
        }

        private bool CombatDirector_Spawn(On.RoR2.CombatDirector.orig_Spawn orig, CombatDirector self, SpawnCard spawnCard, EliteDef eliteDef, Transform spawnTarget, DirectorCore.MonsterSpawnDistance spawnDistance, bool preventOverhead, float valueMultiplier, DirectorPlacementRule.PlacementMode placementMode) {
            当前敌人生成数_ = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count + TeamComponent.GetTeamMembers(TeamIndex.Void).Count;
            if (eliteDef) {
                if (特殊环境精英属性_ && Util.CheckRoll(精英转化几率_)) {
                    eliteDef = 特殊环境精英属性_;
                }
            }
            return orig(self, spawnCard, eliteDef, spawnTarget, spawnDistance, preventOverhead, valueMultiplier, placementMode);
        }

        private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene) {
            orig(self, nextScene);
            int stageCount = self.stageClearCount + 1;
            string sceneName = nextScene.cachedName;
            敌人最大生成数_ = Mathf.Max(6, 25 - stageCount);
            精英转化几率_ = 0;
            特殊环境精英属性_ = null;
            if (sceneName == "goldshores") {
                特殊环境精英属性_ = JunkContent.Elites.Gold;
                精英转化几率_ = 100;
            } else if (sceneName == "dampcavesimple") {
                特殊环境精英属性_ = RoR2Content.Elites.Fire;
                精英转化几率_ = 50;
            } else if (sceneName == "frozenwall") {
                特殊环境精英属性_ = RoR2Content.Elites.Ice;
                精英转化几率_ = 60;
            } else if (sceneName == "snowyforest") {
                特殊环境精英属性_ = RoR2Content.Elites.Ice;
                精英转化几率_ = 36;
            } else if (sceneName.StartsWith("golemplains")) {
                特殊环境精英属性_ = DLC1Content.Elites.Earth;
                精英转化几率_ = 36;
            }
        }
    }
}