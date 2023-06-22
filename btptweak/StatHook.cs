using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class StatHook {

        public static void AddHook() {
            IL.RoR2.CharacterBody.RecalculateStats += HealthTweak;
            IL.RoR2.CharacterBody.RecalculateStats += RegenTweak;
            IL.RoR2.CharacterBody.RecalculateStats += AttackSpeedTweak;
            IL.RoR2.CharacterBody.RecalculateStats += MoveSpeedTweak;
        }

        public static void RemoveHook() {
            IL.RoR2.CharacterBody.RecalculateStats -= HealthTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= RegenTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= AttackSpeedTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= MoveSpeedTweak;
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
                        //=== 技能
                        if (BtpTweak.玩家等级_ != (int)self.level) {
                            BtpTweak.玩家等级_ = (int)self.level;
                            SkillHook.LevelUp();
                        }
                        //===
                        return value + BtpTweak.玩家生命值增加系数_ * (self.baseMaxHealth > 200 ? 200 : self.baseMaxHealth) * (self.level - 1) * self.level;
                    } else {
                        return value + BtpTweak.怪物生命值增加系数_ * self.baseMaxHealth * (self.level - 1);
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
                    return value + (self.isPlayerControlled ? 0.001f * self.level * self.maxHealth : 0);
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
                    return value + (self.isPlayerControlled ? 0.03f * (self.level - 1) : 0);
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
                ilcursor.Index++;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 75);
                ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float>>(delegate (RoR2.CharacterBody self, float value) {
                    return value + (self.isPlayerControlled ? 0.03f * (self.level - 1) : 0);
                });
                ilcursor.Emit(OpCodes.Stloc, 75);
                ilcursor.Emit(OpCodes.Ldloc, 74);
            }
        }
    }
}