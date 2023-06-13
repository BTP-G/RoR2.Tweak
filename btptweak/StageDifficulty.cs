using RoR2;

namespace BtpTweak {

    internal class StageDifficulty {

        public static void 关卡难度缩放修改() {
            On.RoR2.Run.Start += Run_Start;
            On.RoR2.Run.BeginGameOver += Run_BeginGameOver;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter += ChargedState_OnEnter;
            虚灵战斗消息();
        }

        public static void RemoveHook() {
            On.RoR2.Run.Start -= Run_Start;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter -= ChargedState_OnEnter;
        }

        private static void Run_Start(On.RoR2.Run.orig_Start orig, Run self) {
            orig(self);
            Localization.后续汉化();
            BtpTweak.玩家角色等级_ = 1;
            BtpTweak.玩家角色等级生命值系数_ = 1;
            BtpTweak.虚灵战斗阶段计数_ = 0;
            //=== 女猎人
            HIFUHuntressTweaks.Skills.Strafe.damage = 1.8f;
            HIFUHuntressTweaks.Skills.Flurry.damage = 1.2f;
            HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60 + (BtpTweak.女猎人射程每级增加距离_.Value * BtpTweak.玩家角色等级_);
            //=== 船 长
            HIFUCaptainTweaks.Skills.VulcanShotgun.PelletCount = 6;
            //=== 盗贼
            BtpTweak.banditSkullCount_ = 0;
            if (self.selectedDifficulty == ConfigurableDifficulty.ConfigurableDifficultyPlugin.configurableDifficultyIndex) {
                BtpTweak.是否选择造物难度_ = true;
                DifficultyCatalog.GetDifficultyDef(ConfigurableDifficulty.ConfigurableDifficultyPlugin.configurableDifficultyIndex).scalingValue = 2 + ConfigurableDifficulty.ConfigurableDifficultyPlugin.difficultyScaling.Value * 0.02f;
            } else {
                BtpTweak.是否选择造物难度_ = false;
            }
        }

        private static void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEndingDef) {
            orig(self, gameEndingDef);
            BtpTweak.虚灵战斗阶段计数_ = 0;
            BtpTweak.是否选择造物难度_ = false;
        }

        private static void ChargedState_OnEnter(On.RoR2.TeleporterInteraction.ChargedState.orig_OnEnter orig, EntityStates.BaseState self) {
            orig(self);
            if (BtpTweak.是否选择造物难度_) {
                BtpTweak.怪物等级生命值系数_ += 0.3f;
                BtpTweak.怪物等级伤害系数_ += 0.1f;
                ++BtpTweak.玩家角色等级生命值系数_;
                foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                    if (characterBody) {
                        Stats.Re_CalculateLevelStats(characterBody);
                    }
                }
                ++DifficultyCatalog.GetDifficultyDef(Run.instance.selectedDifficulty).scalingValue;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=100%>传送器充能完毕，获得最大生命值奖励！</size></color>" });
            }
        }

        private static void 虚灵战斗消息() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter += SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter += DeathState_OnEnter;
        }

        private static void SpawnState_OnEnter(On.EntityStates.VoidRaidCrab.SpawnState.orig_OnEnter orig, EntityStates.VoidRaidCrab.SpawnState self) {
            orig(self);
            if (SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("void")) {
                switch (++BtpTweak.虚灵战斗阶段计数_) {
                    case 1: {
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=red><size=110%>你感到十分虚弱，虚空限制了你的部分力量！</size></color>" });
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=red><size=110%>你的细胞正在告诉你不要被它伤到！</size></color>" });
                        break;
                    }
                    case 2: {
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=red><size=120%>它又出现了，赶快跟上！</size></color>" });
                        break;
                    }
                    case 3: {
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=red><size=130%>它变虚弱了，虚空出现了裂缝，有外界的生物闯入！</size></color>" });
                        break;
                    }
                }
            }
        }

        private static void DeathState_OnEnter(On.EntityStates.VoidRaidCrab.DeathState.orig_OnEnter orig, EntityStates.VoidRaidCrab.DeathState self) {
            orig(self);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=100%>虚空暂时重归于平静~</size></color>" });
            BtpTweak.虚灵战斗阶段计数_ = 0;
        }
    }
}