using RoR2;

namespace Btp {

    internal class Stats {

        public static void 角色属性调整() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.PhaseCounter.GoToNextPhase += PhaseCounter_GoToNextPhase;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterMaster.OnBodyStart -= CharacterMaster_OnBodyStart;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.PhaseCounter.GoToNextPhase -= PhaseCounter_GoToNextPhase;
        }

        private static void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body) {
            orig(self, body);
            body.autoCalculateLevelStats = false;
            if (TeamIndex.Player == body.teamComponent.teamIndex) {  // 保证强盗标记不因过关和死亡消失
                if (body.name.StartsWith("Band")) {
                    for (int i = BtpTweak.盗贼标记_; i > 0; --i) {
                        body.AddBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                    }
                }
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            if (self.isPlayerControlled) {
                self.levelMaxHealth = BtpTweak.玩家等级生命值系数_ * self.level * self.baseMaxHealth * 0.02f;
            } else if (TeamIndex.Monster == self.teamComponent.teamIndex) {
                self.levelMaxHealth = BtpTweak.怪物等级生命值系数_ * self.baseMaxHealth * 0.3f;
            }
            orig(self);
            if (self.isPlayerControlled) {
                if (BtpTweak.玩家等级_ != (int)self.level) {
                    BtpTweak.玩家等级_ = (int)self.level;
                    Skills.按等级重新调整技能();
                }
            }
            if (self.moveSpeed > 72 && (self.bodyFlags & CharacterBody.BodyFlags.IgnoreFallDamage) != CharacterBody.BodyFlags.IgnoreFallDamage) {
                self.moveSpeed = 72;
            }
        }

        private static void PhaseCounter_GoToNextPhase(On.RoR2.PhaseCounter.orig_GoToNextPhase orig, PhaseCounter self) {
            if (BtpTweak.是否选择造物难度_ && PhaseCounter.instance?.phase != 0) {
                BtpTweak.玩家等级生命值系数_ *= 1.5f;
                BtpTweak.怪物等级生命值系数_ *= 1.2f;
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=110%>你吸收了部分月之力，最大生命值提升！</size></color>" });
            }
            orig(self);
        }
    }
}