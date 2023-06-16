using RoR2;
using UnityEngine;

namespace Btp {

    internal class Stats {

        public static void 角色属性调整() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.PhaseCounter.GoToNextPhase += PhaseCounter_GoToNextPhase;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterMaster.OnBodyStart -= CharacterMaster_OnBodyStart;
            On.RoR2.CharacterBody.OnLevelUp -= CharacterBody_OnLevelUp;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.PhaseCounter.GoToNextPhase -= PhaseCounter_GoToNextPhase;
        }

        public static void Re_CalculateLevelStats(CharacterBody body) {
            if (TeamIndex.Player == body.teamComponent.teamIndex) {
                body.levelMaxHealth =
                        Mathf.Round(BtpTweak.玩家角色等级生命值系数_ * body.level * body.baseMaxHealth * 0.02f);
            } else if (TeamIndex.Monster == body.teamComponent.teamIndex) {
                body.levelMaxHealth =
                    Mathf.Round(BtpTweak.怪物等级生命值系数_ * body.baseMaxHealth * 0.3f);
                body.levelDamage = BtpTweak.怪物等级伤害系数_ * body.baseDamage * 0.21f;
            }
        }

        private static void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body) {
            orig(self, body);
            body.autoCalculateLevelStats = false;
            Re_CalculateLevelStats(body);
            if (TeamIndex.Player == body.teamComponent.teamIndex) {  // 保证强盗标记不因过关和死亡消失
                if (body.name.StartsWith("Band")) {
                    for (int i = BtpTweak.banditSkullCount_; i > 0; --i) {
                        body.AddBuff(RoR2Content.Buffs.BanditSkull);
                    }
                }
            }
        }

        private static void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self) {
            orig(self);
            Re_CalculateLevelStats(self);
            if (self.isPlayerControlled) {
                BtpTweak.玩家等级_ = (int)self.level;
                Skills.按等级重新调整技能();
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
        }

        private static void PhaseCounter_GoToNextPhase(On.RoR2.PhaseCounter.orig_GoToNextPhase orig, PhaseCounter self) {
            if (BtpTweak.是否选择造物难度_ && PhaseCounter.instance?.phase != 0) {
                BtpTweak.玩家角色等级生命值系数_ *= 1.5f;
                BtpTweak.怪物等级生命值系数_ *= 1.1f;
                foreach (CharacterBody characterBody in CharacterBody.readOnlyInstancesList) {
                    if (characterBody) {
                        Re_CalculateLevelStats(characterBody);
                    }
                }
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=110%>你吸收了部分月之力，最大生命值提升！</size></color>" });
            }
            orig(self);
        }
    }
}