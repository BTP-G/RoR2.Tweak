using RoR2;
using UnityEngine;

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
            BtpTweak.玩家生命值提升系数_ = 0;
            BtpTweak.玩家生命值提升倍数_ = 0;
            BtpTweak.敌人生命值提升系数_ = 0;
            BtpTweak.敌人生命值提升倍数_ = 0;
            MiscHook.战斗祭坛额外奖励数量 = 0;
            BtpTweak.虚灵战斗阶段计数_ = 0;
        }

        private static void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene) {
            orig(self, nextScene);
            MiscHook.战斗祭坛额外奖励数量 = 0;
            BtpTweak.虚灵战斗阶段计数_ = 0;
            if (BtpTweak.是否选择造物难度_) {
                float num = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f);
                BtpTweak.敌人生命值提升系数_ = 0.05f * num;
                BtpTweak.敌人生命值提升倍数_ = num;
            }
        }

        private static void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEndingDef) {
            orig(self, gameEndingDef);
            BtpTweak.是否选择造物难度_ = false;
            MiscHook.战斗祭坛额外奖励数量 = 0;
            BtpTweak.虚灵战斗阶段计数_ = 0;
        }
    }
}