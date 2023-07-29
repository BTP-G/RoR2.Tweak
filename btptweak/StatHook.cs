using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak {

    internal class StatHook {
        public static Dictionary<BodyIndex, int> body_caseLoc = new Dictionary<BodyIndex, int>();

        public static ItemIndex 护甲板;
        public static ItemIndex 巨型隆起;
        public static ItemIndex 红茶;
        public static ItemIndex 燃料电池;
        public static ItemIndex 肉排;
        public static ItemIndex 注射器;
        public static ItemIndex 黄金隆起;

        public static void AddHook() {
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            //IL.RoR2.CharacterBody.RecalculateStats += HealthTweak;
            //IL.RoR2.CharacterBody.RecalculateStats += RegenTweak;
            IL.RoR2.CharacterBody.RecalculateStats += AttackSpeedTweak;
            IL.RoR2.CharacterBody.RecalculateStats += MoveSpeedTweak;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterBody.OnLevelUp -= CharacterBody_OnLevelUp;
            R2API.RecalculateStatsAPI.GetStatCoefficients -= RecalculateStatsAPI_GetStatCoefficients;
            //IL.RoR2.CharacterBody.RecalculateStats -= HealthTweak;
            //IL.RoR2.CharacterBody.RecalculateStats -= RegenTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= AttackSpeedTweak;
            IL.RoR2.CharacterBody.RecalculateStats -= MoveSpeedTweak;
        }

        public static void LateInit() {
            护甲板 = RoR2Content.Items.ArmorPlate.itemIndex;
            注射器 = RoR2Content.Items.Syringe.itemIndex;
            燃料电池 = RoR2Content.Items.EquipmentMagazine.itemIndex;
            红茶 = ItemCatalog.FindItemIndex("RuinaBlackTea");
            肉排 = RoR2Content.Items.FlatHealth.itemIndex;
            巨型隆起 = RoR2Content.Items.Knurl.itemIndex;
            黄金隆起 = GoldenCoastPlus.GoldenCoastPlus.goldenKnurlDef.itemIndex;
            MiscHook.bandit2Bodyindex = BodyCatalog.FindBodyIndex("Bandit2Body");
            BuffAndDotHook.mageBodyindex = BodyCatalog.FindBodyIndex("MageBody");
            //======
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("ArbiterBody"), 1);
            body_caseLoc.Add(MiscHook.bandit2Bodyindex, 2);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("CaptainBody"), 3);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("CHEF"), 4);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("CommandoBody"), 5);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("CrocoBody"), 6);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("EngiBody"), 7);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("HuntressBody"), 8);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("LoaderBody"), 9);
            body_caseLoc.Add(BuffAndDotHook.mageBodyindex, 10);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("MercBody"), 11);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("RailgunnerBody"), 12);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("RedMistBody"), 13);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("PathfinderBody"), 14);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("RobPaladinBody"), 15);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("SniperClassicBody"), 16);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("ToolbotBody"), 17);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("TreebotBody"), 18);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("VoidSurvivorBody"), 19);

            body_caseLoc.Add(BodyCatalog.FindBodyIndex("EngiTurretBody"), 20);
            body_caseLoc.Add(BodyCatalog.FindBodyIndex("EngiWalkerTurretBody"), 20);
            //======
        }

        private static void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self) {
            orig(self);
            if (TeamIndex.Player == self.teamComponent.teamIndex && BtpTweak.玩家等级_ != (int)self.level) {
                BtpTweak.玩家等级_ = (int)self.level;
                SkillHook.LevelUp();
            } else {
                float 敌人等级 = Run.instance.ambientLevel;
                MiscHook.造物难度敌人珍珠 = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(敌人等级 * 0.1f, MiscHook.往日不再 ? 1 + 0.1f * Run.instance.stageClearCount : 1), 1000000));  //
                if (敌人等级 > 100) {
                    HealthHook.老米触发伤害保护 = 0.1f / 敌人等级;
                    HealthHook.老米爆发伤害保护 = 100f / 敌人等级;
                    HealthHook.虚灵触发伤害保护 = 0.05f / 敌人等级;
                    HealthHook.虚灵爆发伤害保护 = 50f / 敌人等级;
                } else {
                    HealthHook.老米触发伤害保护 = 0.001f;
                    HealthHook.老米爆发伤害保护 = 1;
                    HealthHook.虚灵触发伤害保护 = 0.0005f;
                    HealthHook.虚灵爆发伤害保护 = 1;
                }
            }
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            Inventory inventory = sender.inventory;
            if (inventory) {
                float upLevel = sender.level - 1;
                int itemCount = inventory.GetItemCount(肉排);
                float levelMaxHealthAdd = 2.5f * itemCount;
                if (body_caseLoc.TryGetValue(sender.bodyIndex, out int loc)) {
                    switch (loc) {
                        case 1: {  // Arbiter
                            args.cooldownMultAdd -= upLevel / (10 + upLevel);
                            float statUpPercent = 0.05f * inventory.GetItemCount(红茶);
                            args.healthMultAdd += statUpPercent;
                            args.regenMultAdd += statUpPercent;
                            break;
                        }
                        case 2: {  // Bandit2
                            break;
                        }
                        case 3: {  // Captain
                            break;
                        }
                        case 4: {  // CHEF
                            levelMaxHealthAdd *= 2;
                            break;
                        }
                        case 5: {  // Commando
                            float statUpPercent = 0.03f * inventory.GetItemCount(注射器);
                            if (statUpPercent > 0) {
                                args.attackSpeedMultAdd += statUpPercent;
                                args.critAdd += 100 * statUpPercent;
                                args.damageMultAdd += statUpPercent;
                                args.healthMultAdd += statUpPercent;
                                args.moveSpeedMultAdd += statUpPercent;
                                args.regenMultAdd += statUpPercent;
                            }
                            break;
                        }
                        case 6: {  // Croco
                            break;
                        }
                        case 7: {  // Engi
                            args.armorAdd += 10 * inventory.GetItemCount(护甲板);
                            break;
                        }
                        case 8: {  // Huntress
                            args.critAdd += upLevel;
                            break;
                        }
                        case 9: {  // Loader
                            break;
                        }
                        case 10: {  // Mage
                            break;
                        }
                        case 11: {  // Merc
                            break;
                        }
                        case 12: {  // Pathfinder
                            break;
                        }
                        case 13: {  // Railgunner
                            break;
                        }
                        case 14: {  // RedMist
                            break;
                        }
                        case 15: {  // RobPaladin
                            break;
                        }
                        case 16: {  // SniperClassic
                            break;
                        }
                        case 17: {  // Toolbot
                            break;
                        }
                        case 18: {  // Treebot
                            args.cooldownMultAdd += Mathf.Pow(0.9f, inventory.GetItemCount(燃料电池)) - 1f;
                            break;
                        }
                        case 19: {  // VoidSurvivor
                            break;
                        }
                        case 20: {  // EngiTurretBody & EngiWalkerTurretBody
                            float statUpPercent = 0.25f * inventory.GetItemCount(SkillHook.权杖);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.damageMultAdd += statUpPercent;
                            break;
                        }
                    }
                }
                levelMaxHealthAdd += 0.25f * (sender.levelMaxHealth + levelMaxHealthAdd) * inventory.GetItemCount(巨型隆起);
                args.baseHealthAdd += levelMaxHealthAdd * upLevel;
                itemCount = inventory.GetItemCount(黄金隆起);
                if (itemCount > 0 && sender.master) {
                    args.baseRegenAdd += (itemCount + sender.master.money / 100000000) * 0.01f * sender.maxHealth;
                }
            }
        }

        //private static void HealthTweak(ILContext il) {
        //    ILCursor ilcursor = new ILCursor(il);
        //    Func<Instruction, bool>[] array = new Func<Instruction, bool>[4];
        //    array[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 62));
        //    array[1] = ((Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 63));
        //    array[2] = ((Instruction x) => ILPatternMatchingExt.MatchMul(x));
        //    array[3] = ((Instruction x) => ILPatternMatchingExt.MatchStloc(x, 62));
        //    if (ilcursor.TryGotoNext(array)) {
        //        ilcursor.Emit(OpCodes.Ldarg, 0);
        //        ilcursor.Emit(OpCodes.Ldloc, 62);
        //        ilcursor.Emit(OpCodes.Ldloc, 63);
        //        ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float, float>>(delegate (RoR2.CharacterBody self, float num50, float num51) {
        //            float levelHealthAdd = self.levelMaxHealth + 5 * self.inventory.GetItemCount(RoR2Content.Items.FlatHealth);
        //            levelHealthAdd += 0.5f * levelHealthAdd * self.inventory.GetItemCount(RoR2Content.Items.Knurl);
        //            if (self.isPlayerControlled) {
        //                return num50  // 原值
        //                + BtpTweak.玩家生命值提升系数_ * levelHealthAdd * (self.level - 1)  // 增加
        //                * BtpTweak.玩家生命值提升倍数_ * num51;  // 倍数
        //            } else {
        //                return num50  // 原值
        //                + BtpTweak.敌人生命值提升系数_ * levelHealthAdd * (self.level - 1)  // 增加
        //                * BtpTweak.敌人生命值提升倍数_ * num51;  // 倍数
        //            }
        //        });
        //        ilcursor.Emit(OpCodes.Stloc, 62);
        //    }
        //}

        //private static void RegenTweak(ILContext il) {
        //    ILCursor ilcursor = new ILCursor(il);
        //    Func<Instruction, bool>[] array = new Func<Instruction, bool>[2];
        //    array[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 1f));
        //    array[1] = ((Instruction x) => ILPatternMatchingExt.MatchStloc(x, 72));
        //    if (ilcursor.TryGotoNext(array)) {
        //        ilcursor.Emit(OpCodes.Ldarg, 0);
        //        ilcursor.Emit(OpCodes.Ldloc, 67);
        //        ilcursor.Emit(OpCodes.Ldloc, 66);
        //        ilcursor.EmitDelegate<Func<RoR2.CharacterBody, float, float, float>>(delegate (RoR2.CharacterBody self, float value, float scaling) {
        //            return value;
        //        });
        //        ilcursor.Emit(OpCodes.Stloc, 67);
        //    }
        //}

        private static void AttackSpeedTweak(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[4];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 82);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 83);
            array[2] = (Instruction x) => ILPatternMatchingExt.MatchMul(x);
            array[3] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 82);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 83);
                ilcursor.EmitDelegate(delegate (CharacterBody self, float value) {
                    return value + (self.isPlayerControlled ? 0.02f * (self.level - 1) : 0);
                });
                ilcursor.Emit(OpCodes.Stloc, 83);
            }
        }

        private static void MoveSpeedTweak(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[6];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 74);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 75);
            array[2] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 76);
            array[3] = (Instruction x) => ILPatternMatchingExt.MatchDiv(x);
            array[4] = (Instruction x) => ILPatternMatchingExt.MatchMul(x);
            array[5] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 74);
            if (ilcursor.TryGotoNext(array)) {
                ++ilcursor.Index;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldloc, 75);
                ilcursor.EmitDelegate(delegate (CharacterBody self, float value) {
                    return value + (self.isPlayerControlled ? 0.02f * (self.level - 1) : 0);
                });
                ilcursor.Emit(OpCodes.Stloc, 75);
                ilcursor.Emit(OpCodes.Ldloc, 74);
            }
        }
    }
}