using BtpTweak.Utils;
using RoR2;
using UnityEngine;

namespace BtpTweak {

    internal class RunHook {
        public static bool 往日不再_ = false;

        public static void AddHook() {
            Run.onRunAmbientLevelUp += Run_onRunAmbientLevelUp;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        public static void RemoveHook() {
            Run.onRunAmbientLevelUp -= Run_onRunAmbientLevelUp;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
        }

        private static void Run_onRunAmbientLevelUp(Run run) {
            MiscHook.造物难度敌人珍珠_ = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(run.ambientLevel * 0.1f, 往日不再_ ? 1 + 0.1f * run.stageClearCount : 1), 10000000));  //
            if (run.ambientLevel > 100) {
                HealthHook.老米触发伤害保护_ = 0.1f / run.ambientLevel;
                HealthHook.老米爆发伤害保护_ = 100f / run.ambientLevel;
                HealthHook.虚灵触发伤害保护_ = 0.05f / run.ambientLevel;
                HealthHook.虚灵爆发伤害保护_ = 50f / run.ambientLevel;
            } else {
                HealthHook.老米触发伤害保护_ = 0.001f;
                HealthHook.老米爆发伤害保护_ = 1;
                HealthHook.虚灵触发伤害保护_ = 0.0005f;
                HealthHook.虚灵爆发伤害保护_ = 1;
            }
        }

        private static void Run_onRunStartGlobal(Run run) {
            往日不再_ = false;
            run.DisableItemDrop(SkillHook.古代权杖_);
            Localization.权杖技能汉化();
            SkillHook.RunStartInit();
            CombatHook.LateInit();
            Main.是否选择造物难度_ = run.selectedDifficulty == ConfigurableDifficulty.ConfigurableDifficultyPlugin.configurableDifficultyIndex;
            MiscHook.造物难度敌人珍珠_ = 0;
        }

        private static void Stage_onStageStartGlobal(Stage stage) {
            int stageCount = Run.instance.stageClearCount + 1;
            string sceneName = stage.sceneDef.cachedName;
            CombatDirector.cvDirectorCombatEnableInternalLogs.SetBool(ModConfig.开启战斗日志_.Value);
            CombatHook.敌人最大生成数_ = Mathf.Max(6, 25 - stageCount);
            CombatHook.精英转化几率 = 0;
            CombatHook.特殊环境精英属性_ = null;
            if (sceneName == "goldshores") {
                CombatHook.特殊环境精英属性_ = JunkContent.Elites.Gold;
                CombatHook.精英转化几率 = 100;
            } else if (sceneName == "dampcavesimple") {
                CombatHook.特殊环境精英属性_ = RoR2Content.Elites.Fire;
                CombatHook.精英转化几率 = 50;
            } else if (sceneName == "frozenwall") {
                CombatHook.特殊环境精英属性_ = RoR2Content.Elites.Ice;
                CombatHook.精英转化几率 = 60;
            } else if (sceneName == "snowyforest") {
                CombatHook.特殊环境精英属性_ = RoR2Content.Elites.Ice;
                CombatHook.精英转化几率 = 36;
            } else if (sceneName.StartsWith("golemplains")) {
                CombatHook.特殊环境精英属性_ = DLC1Content.Elites.Earth;
                CombatHook.精英转化几率 = 36;
            }
            FinalBossHook.处于天文馆_ = sceneName == "voidraid";
            HealthHook.伤害阈值_ = 0.01f * stageCount;
            MiscHook.处于虚空之境 = sceneName == "arena";
            MiscHook.战斗祭坛物品掉落数_ = Mathf.Min(Mathf.RoundToInt(stageCount * 0.51f), 5) * Run.instance.participatingPlayerCount;
        }
    }
}