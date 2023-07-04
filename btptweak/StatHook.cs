using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;

namespace BtpTweak {

    internal class StatHook {

        public static void AddHook() {
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.CharacterBody.RecalculateStats += HealthTweak;
            IL.RoR2.CharacterBody.RecalculateStats += RegenTweak;
            IL.RoR2.CharacterBody.RecalculateStats += AttackSpeedTweak;
            IL.RoR2.CharacterBody.RecalculateStats += MoveSpeedTweak;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.OnLevelUp -= CharacterBody_OnLevelUp;
            R2API.RecalculateStatsAPI.GetStatCoefficients -= RecalculateStatsAPI_GetStatCoefficients;
            IL.RoR2.CharacterBody.RecalculateStats -= HealthTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= RegenTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= AttackSpeedTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= MoveSpeedTweak;
        }

        private static void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self) {
            orig(self);
            if (self.isPlayerControlled && BtpTweak.玩家等级_ != (int)self.level) {
                BtpTweak.玩家等级_ = (int)self.level;
                SkillHook.LevelUp();
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            if (sender?.inventory) {
                if (sender.isPlayerControlled) {
                    args.armorAdd += 0.5f * sender.level;
                } else {
                    args.armorAdd += 0.05f * sender.level;
                }
                args.baseHealthAdd += (sender.level - 1) * (10 * sender.inventory.GetItemCount(RoR2Content.Items.FlatHealth) + 0.5f * sender.levelMaxHealth * sender.inventory.GetItemCount(RoR2Content.Items.Knurl));
            }
        }

        private static void HealthTweak(ILContext il) {
            ILCursor ilcursor = new ILCursor(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[4];
            array[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 62));
            array[1] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 63));
            array[2] = ((Instruction x) => ILPatternMatchingExt.MatchMul(x));
            array[3] = ((Instruction x) => ILPatternMatchingExt.MatchStloc(x, 62));
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 62);
                ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float>>(delegate (RoR2.CharacterBody self, float value) {
                    if (self.isPlayerControlled) {
                        return value  // 原值
                        + BtpTweak.玩家生命值增加系数_ * self.levelMaxHealth * (self.level - 1)  // 增加
                        * self.level * BtpTweak.玩家生命值倍数_;  // 倍数
                    } else {
                        return value  // 原值
                        + BtpTweak.怪物生命值增加系数_ * self.levelMaxHealth * (self.level - 1)  // 增加
                        * BtpTweak.怪物生命值倍数_;  // 倍数
                    }
                });
                ilcursor.Emit(OpCodes.Stloc, 62);
            }
        }

        private static void RegenTweak(ILContext il) {
            ILCursor ilcursor = new ILCursor(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[2];
            array[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 1f));
            array[1] = ((Instruction x) => ILPatternMatchingExt.MatchStloc(x, 72));
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 67);
                ilcursor.Emit(OpCodes.Ldloc, 66);
                ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float, float>>(delegate (RoR2.CharacterBody self, float value, float scaling) {
                    return value + (self.isPlayerControlled ? 1.6f * (self.level - 1) * self.inventory.GetItemCount(RoR2Content.Items.Knurl) : 0);
                });
                ilcursor.Emit(OpCodes.Stloc, 67);
            }
        }

        private static void AttackSpeedTweak(ILContext il) {
            ILCursor ilcursor = new ILCursor(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[4];
            array[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 82));
            array[1] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 83));
            array[2] = ((Instruction x) => ILPatternMatchingExt.MatchMul(x));
            array[3] = ((Instruction x) => ILPatternMatchingExt.MatchStloc(x, 82));
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 83);
                ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float>>(delegate (RoR2.CharacterBody self, float value) {
                    return value + (self.isPlayerControlled ? 0.02f * (self.level - 1) : 0);
                });
                ilcursor.Emit(OpCodes.Stloc, 83);
            }
        }

        private static void MoveSpeedTweak(ILContext il) {
            ILCursor ilcursor = new ILCursor(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[6];
            array[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 74));
            array[1] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 75));
            array[2] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 76));
            array[3] = ((Instruction x) => ILPatternMatchingExt.MatchDiv(x));
            array[4] = ((Instruction x) => ILPatternMatchingExt.MatchMul(x));
            array[5] = ((Instruction x) => ILPatternMatchingExt.MatchStloc(x, 74));
            if (ilcursor.TryGotoNext(array)) {
                ++ilcursor.Index;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 75);
                ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float>>(delegate (RoR2.CharacterBody self, float value) {
                    return value + (self.isPlayerControlled ? 0.02f * (self.level - 1) : 0);
                });
                ilcursor.Emit(OpCodes.Stloc, 75);
                ilcursor.Emit(OpCodes.Ldloc, 74);
            }
        }
    }
}