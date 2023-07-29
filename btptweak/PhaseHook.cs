using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class PhaseHook {

        public static void AddHook() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter += SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter += DeathState_OnEnter;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
        }

        public static void RemoveHook() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter -= SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter -= DeathState_OnEnter;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= TrueDeathState_OnEnter;
        }

        private static void SpawnState_OnEnter(On.EntityStates.VoidRaidCrab.SpawnState.orig_OnEnter orig, EntityStates.VoidRaidCrab.SpawnState self) {
            orig(self);
            if (SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("void")) {
                ++BtpTweak.虚灵战斗阶段计数_;
                HealthHook.伤害阈值 = 0.01f * (Run.instance.stageClearCount + BtpTweak.虚灵战斗阶段计数_);
                if (BtpTweak.是否选择造物难度_) {
                    switch (BtpTweak.虚灵战斗阶段计数_) {
                        case 1: {
                            if (NetworkServer.active) {
                                ChatMessage.SendColored("你感到十分虚弱，虚空限制了你的部分力量！你的细胞正在告诉你不要被它伤到！", Color.red);
                            }
                            break;
                        }
                        case 2: {
                            if (NetworkServer.active) {
                                ChatMessage.SendColored("它变虚弱了，虚空出现了裂缝，有外界的生物闯入！", Color.red);
                            }
                            break;
                        }
                        case 3: {
                            if (NetworkServer.active) {
                                ChatMessage.SendColored("一定小心！", Color.red);
                            }
                            break;
                        }
                    }
                }
            }
        }

        private static void DeathState_OnEnter(On.EntityStates.VoidRaidCrab.DeathState.orig_OnEnter orig, EntityStates.VoidRaidCrab.DeathState self) {
            orig(self);
            BtpTweak.虚灵战斗阶段计数_ = 0;
            if (BtpTweak.是否选择造物难度_) {
                if (NetworkServer.active) {
                    ChatMessage.SendColored("你不再感到不适，虚空暂时重归于平静~", Color.green);
                }
            }
        }

        private static void TrueDeathState_OnEnter(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (BtpTweak.是否选择造物难度_ && MiscHook.往日不再 == false) {
                if (NetworkServer.active) {
                    ChatMessage.SendColored("--世界不再是你熟悉的那样！！！", Color.red);
                }
                MiscHook.往日不再 = true;
            }
        }
    }
}