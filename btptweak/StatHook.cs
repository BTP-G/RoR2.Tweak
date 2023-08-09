using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak {

    internal class StatHook {
        public static Dictionary<BodyIndex, int> body_caseLoc_ = new();

        public static ItemIndex 穿甲弹_;
        public static ItemIndex 护甲板_;
        public static ItemIndex 黄金隆起_;
        public static ItemIndex 巨型隆起_;
        public static ItemIndex 摩卡_;
        public static ItemIndex 燃料电池_;
        public static ItemIndex 肉排_;
        public static ItemIndex 异端幻象_;
        public static ItemIndex 注射器_;

        public static void AddHook() {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public static void RemoveHook() {
            R2API.RecalculateStatsAPI.GetStatCoefficients -= RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
        }

        public static void LateInit() {
            CharacterBody huntressBody = RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<CharacterBody>();  // 女猎人调整
            huntressBody.baseDamage = 15;
            huntressBody.levelDamage = huntressBody.baseDamage * 0.2f;
            huntressBody.baseCrit = 10;
            huntressBody.levelCrit = 1;
            //======
            穿甲弹_ = RoR2Content.Items.BossDamageBonus.itemIndex;
            护甲板_ = RoR2Content.Items.ArmorPlate.itemIndex;
            黄金隆起_ = GoldenCoastPlus.GoldenCoastPlus.goldenKnurlDef.itemIndex;
            巨型隆起_ = RoR2Content.Items.Knurl.itemIndex;
            摩卡_ = DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex;
            燃料电池_ = RoR2Content.Items.EquipmentMagazine.itemIndex;
            肉排_ = RoR2Content.Items.FlatHealth.itemIndex;
            异端幻象_ = RoR2Content.Items.LunarPrimaryReplacement.itemIndex;
            注射器_ = RoR2Content.Items.Syringe.itemIndex;
            BuffAndDotHook.工匠_ = BodyCatalog.FindBodyIndex("MageBody");
            GlobalEventHook.工程师固定炮台_ = BodyCatalog.FindBodyIndex("EngiTurretBody");
            GlobalEventHook.工程师移动炮台_ = BodyCatalog.FindBodyIndex("EngiWalkerTurretBody");
            GlobalEventHook.雇佣兵_ = BodyCatalog.FindBodyIndex("MercBody");
            //======
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("ArbiterBody"), 1);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("Bandit2Body"), 2);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("CaptainBody"), 3);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("CHEF"), 4);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("CommandoBody"), 5);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("CrocoBody"), 6);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("EngiBody"), 7);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("HuntressBody"), 8);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("LoaderBody"), 9);
            body_caseLoc_.Add(BuffAndDotHook.工匠_, 10);
            body_caseLoc_.Add(GlobalEventHook.雇佣兵_, 11);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("RailgunnerBody"), 12);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("RedMistBody"), 13);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("PathfinderBody"), 14);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("RobPaladinBody"), 15);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("SniperClassicBody"), 16);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("ToolbotBody"), 17);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("TreebotBody"), 18);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("VoidSurvivorBody"), 19);
            body_caseLoc_.Add(BodyCatalog.FindBodyIndex("HereticBody"), 20);
            body_caseLoc_.Add(GlobalEventHook.工程师固定炮台_, 21);
            body_caseLoc_.Add(GlobalEventHook.工程师移动炮台_, 21);
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            Inventory inventory = sender.inventory;
            if (inventory) {
                float upLevel = sender.level - 1;
                int itemCount = inventory.GetItemCount(肉排_);
                float levelMaxHealthAdd = 2.5f * itemCount;
                if (body_caseLoc_.TryGetValue(sender.bodyIndex, out int loc)) {
                    switch (loc) {
                        case 1: {  // Arbiter
                            args.cooldownMultAdd -= upLevel / (15 + upLevel);
                            float statUpPercent = 0.05f * inventory.GetItemCount(摩卡_);
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
                            float statUpPercent = 0.03f * inventory.GetItemCount(注射器_);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.healthMultAdd += statUpPercent;
                            args.moveSpeedMultAdd += statUpPercent;
                            args.baseDamageAdd += 2 * inventory.GetItemCount(穿甲弹_);
                            break;
                        }
                        case 6: {  // Croco
                            break;
                        }
                        case 7: {  // Engi
                            args.armorAdd += 10 * inventory.GetItemCount(护甲板_);
                            break;
                        }
                        case 8: {  // Huntress
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
                            args.cooldownMultAdd += Mathf.Pow(0.9f, inventory.GetItemCount(燃料电池_)) - 1f;
                            break;
                        }
                        case 19: {  // VoidSurvivor
                            break;
                        }
                        case 20: {  // Heretic
                            args.primaryCooldownMultAdd -= inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement.itemIndex);
                            break;
                        }
                        case 21: {  // EngiTurretBody & EngiWalkerTurretBody
                            float statUpPercent = 0.25f * inventory.GetItemCount(SkillHook.古代权杖_);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.damageMultAdd += statUpPercent;
                            break;
                        }
                    }
                }
                itemCount = inventory.GetItemCount(巨型隆起_);
                levelMaxHealthAdd += 0.25f * (sender.levelMaxHealth + levelMaxHealthAdd) * itemCount;
                args.baseHealthAdd += levelMaxHealthAdd * upLevel;
                args.regenMultAdd += 0.25f * itemCount;
                itemCount = inventory.GetItemCount(黄金隆起_);
                if (itemCount > 0 && sender.master) {
                    args.baseRegenAdd += (itemCount + sender.master.money / 100000000) * 0.01f * sender.maxHealth;
                }
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self.moveSpeed > 49) {
                self.moveSpeed = 49;
            }
        }
    }
}