using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class PhaseHook {

        public static void AddHook() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter += SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter += DeathState_OnEnter;
            On.EntityStates.BrotherMonster.TrueDeathState.Dissolve += TrueDeathState_Dissolve;
            On.RoR2.PhaseCounter.GoToNextPhase += PhaseCounter_GoToNextPhase;
        }

        public static void RemoveHook() {
            On.EntityStates.VoidRaidCrab.SpawnState.OnEnter -= SpawnState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter -= DeathState_OnEnter;
            On.EntityStates.BrotherMonster.TrueDeathState.Dissolve -= TrueDeathState_Dissolve;
            On.RoR2.PhaseCounter.GoToNextPhase -= PhaseCounter_GoToNextPhase;
        }

        private static void SpawnState_OnEnter(On.EntityStates.VoidRaidCrab.SpawnState.orig_OnEnter orig, EntityStates.VoidRaidCrab.SpawnState self) {
            orig(self);
            if (SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("void")) {
                ++BtpTweak.虚灵战斗阶段计数_;
                if (BtpTweak.是否选择造物难度_) {
                    switch (BtpTweak.虚灵战斗阶段计数_) {
                        case 1: {
                            BtpTweak.玩家生命值提升倍数_ = 0.1f * (Run.instance.stageClearCount - BtpTweak.虚灵战斗阶段计数_);
                            BtpTweak.敌人生命值提升倍数_ = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f) + BtpTweak.虚灵战斗阶段计数_;
                            if (NetworkServer.active) {
                                ChatMessage.SendColored("你感到十分虚弱，虚空限制了你的部分力量！你的细胞正在告诉你不要被它伤到！", Color.red);
                            }
                            break;
                        }
                        case 2: {
                            BtpTweak.玩家生命值提升倍数_ = 0.1f * (Run.instance.stageClearCount - BtpTweak.虚灵战斗阶段计数_);
                            BtpTweak.敌人生命值提升倍数_ = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f) + BtpTweak.虚灵战斗阶段计数_;
                            if (NetworkServer.active) {
                                ChatMessage.SendColored("它变虚弱了，虚空出现了裂缝，有外界的生物闯入！", Color.red);
                            }
                            break;
                        }
                        case 3: {
                            BtpTweak.玩家生命值提升倍数_ = 0.1f * (Run.instance.stageClearCount - BtpTweak.虚灵战斗阶段计数_);
                            BtpTweak.敌人生命值提升倍数_ = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f) + BtpTweak.虚灵战斗阶段计数_;
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
                BtpTweak.玩家生命值提升倍数_ = 0.1f * (Run.instance.stageClearCount);
                BtpTweak.敌人生命值提升倍数_ = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f);
                if (NetworkServer.active) {
                    ChatMessage.SendColored("你不再感到不适，虚空暂时重归于平静~", Color.green);
                }
            }
        }

        private static void PhaseCounter_GoToNextPhase(On.RoR2.PhaseCounter.orig_GoToNextPhase orig, PhaseCounter self) {
            if (BtpTweak.是否选择造物难度_ && PhaseCounter.instance.phase > 0) {
                BtpTweak.玩家生命值提升倍数_ = 0.1f * (Run.instance.stageClearCount + PhaseCounter.instance.phase);
                BtpTweak.敌人生命值提升倍数_ = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f) + PhaseCounter.instance.phase;
                if (NetworkServer.active) {
                    ChatMessage.SendColored("你吸收了部分月之力，最大生命值提升！", Color.green);
                }
            }
            orig(self);
        }

        public static void TrueDeathState_Dissolve(On.EntityStates.BrotherMonster.TrueDeathState.orig_Dissolve orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (BtpTweak.是否选择造物难度_ && Run.instance.loopClearCount == 0) {
                BtpTweak.玩家生命值提升倍数_ = 0.1f * Run.instance.stageClearCount;
                BtpTweak.敌人生命值提升倍数_ = Mathf.Pow(1 + Run.instance.stageClearCount, Run.instance.stageClearCount / 5f);
                if (NetworkServer.active) {
                    ChatMessage.SendColored("--世界不再是你熟悉的那样！！！", Color.blue);
                }
            }
        }
    }
}