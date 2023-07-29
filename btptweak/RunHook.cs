using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BtpTweak {

    internal class RunHook {

        public static void AddHook() {
            On.RoR2.Run.Start += Run_Start_Init;
            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
            On.RoR2.Run.BeginGameOver += Run_BeginGameOver;
        }

        public static void RemoveHook() {
            On.RoR2.Run.Start -= Run_Start_Init;
            On.RoR2.Run.AdvanceStage -= Run_AdvanceStage;
            On.RoR2.Run.BeginGameOver -= Run_BeginGameOver;
        }

        private static void Run_Start_Init(On.RoR2.Run.orig_Start orig, Run self) {
            orig(self);
            Helpers.后续汉化();
            SkillHook.Init();
            BtpTweak.是否选择造物难度_ = self.selectedDifficulty == ConfigurableDifficulty.ConfigurableDifficultyPlugin.configurableDifficultyIndex;
            BtpTweak.玩家等级_ = 1;
            BtpTweak.虚灵战斗阶段计数_ = 0;
            CombatHook.敌人最大生成数 = 24;
            HealthHook.伤害阈值 = 0.01f;
            MiscHook.古代权杖掉落数 = self.participatingPlayerCount;
            MiscHook.往日不再 = false;
            MiscHook.造物难度敌人珍珠 = 0;
            MiscHook.战斗祭坛物品掉落数 = self.participatingPlayerCount;
            Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineBlood/iscShrineBlood.asset").WaitForCompletion().maxSpawnsPerStage = self.participatingPlayerCount;
            Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineCombat/iscShrineCombat.asset").WaitForCompletion().maxSpawnsPerStage = MiscHook.战斗祭坛物品掉落数;
        }

        private static void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene) {
            orig(self, nextScene);
            BtpTweak.虚灵战斗阶段计数_ = 0;
            CombatHook.敌人最大生成数 = Mathf.Max(6, 24 - self.stageClearCount);
            HealthHook.伤害阈值 = 0.01f * self.stageClearCount;
            MiscHook.战斗祭坛物品掉落数 = Mathf.Min(self.participatingPlayerCount + self.stageClearCount, 5 * self.participatingPlayerCount);
            Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/ShrineCombat/iscShrineCombat.asset").WaitForCompletion().maxSpawnsPerStage = MiscHook.战斗祭坛物品掉落数;
        }

        private static void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEndingDef) {
            orig(self, gameEndingDef);
            BtpTweak.是否选择造物难度_ = false;
            BtpTweak.虚灵战斗阶段计数_ = 0;
            MiscHook.战斗祭坛物品掉落数 = 0;
            MiscHook.古代权杖掉落数 = 0;
        }
    }
}