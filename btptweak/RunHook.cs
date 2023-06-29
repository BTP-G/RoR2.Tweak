using RoR2;

namespace BtpTweak {

    internal class RunHook {

        public static void AddHook() {
            On.RoR2.Run.Start += Run_Start_Init;
            On.RoR2.Run.BeginGameOver += Run_BeginGameOver;
        }

        public static void RemoveHook() {
            On.RoR2.Run.Start -= Run_Start_Init;
            On.RoR2.Run.BeginGameOver -= Run_BeginGameOver;
        }

        private static void Run_Start_Init(On.RoR2.Run.orig_Start orig, Run self) {
            orig(self);
            MainMenuHook.后续汉化();
            SkillHook.Init();
            BtpTweak.是否选择造物难度_ = self.selectedDifficulty == ConfigurableDifficulty.ConfigurableDifficultyPlugin.configurableDifficultyIndex;
            BtpTweak.玩家等级_ = 1;
            BtpTweak.玩家生命值增加系数_ = 0.1f;
            BtpTweak.怪物生命值增加系数_ = 0.1f;
            BtpTweak.怪物生命值倍数_ = 1;
            BtpTweak.虚灵战斗阶段计数_ = 0;
        }

        private static void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEndingDef) {
            orig(self, gameEndingDef);
            BtpTweak.是否选择造物难度_ = false;
            BtpTweak.虚灵战斗阶段计数_ = 0;
        }
    }
}