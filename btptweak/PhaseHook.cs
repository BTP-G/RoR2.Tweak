using RoR2;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class PhaseHook {

        public static void AddHook() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter += SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter += DeathState_OnEnter;
            On.RoR2.PhaseCounter.GoToNextPhase += PhaseCounter_GoToNextPhase;
        }

        public static void RemoveHook() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter -= SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter -= DeathState_OnEnter;
            On.RoR2.PhaseCounter.GoToNextPhase -= PhaseCounter_GoToNextPhase;
        }

        private static void SpawnState_OnEnter(On.EntityStates.VoidRaidCrab.SpawnState.orig_OnEnter orig, EntityStates.VoidRaidCrab.SpawnState self) {
            orig(self);
            if (NetworkServer.active) {
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
        }

        private static void DeathState_OnEnter(On.EntityStates.VoidRaidCrab.DeathState.orig_OnEnter orig, EntityStates.VoidRaidCrab.DeathState self) {
            orig(self);
            BtpTweak.虚灵战斗阶段计数_ = 0;
            if (NetworkServer.active) {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=100%>虚空暂时重归于平静~</size></color>" });
            }
        }

        private static void PhaseCounter_GoToNextPhase(On.RoR2.PhaseCounter.orig_GoToNextPhase orig, PhaseCounter self) {
            if (NetworkServer.active && BtpTweak.是否选择造物难度_ && PhaseCounter.instance?.phase != 0) {
                BtpTweak.玩家生命值增加系数_ *= 1.5f;
                BtpTweak.怪物生命值增加系数_ *= 1.2f;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=110%>你吸收了部分月之力，最大生命值提升！</size></color>" });
            }
            orig(self);
        }
    }
}