using BtpTweak.Utils;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak {

    internal class StatHook {
        public static Dictionary<BodyIndex, BodyName> BodyIndexToName_ = new();
        public static ItemIndex 穿甲弹_;
        public static ItemIndex 刽子手的重负_;
        public static ItemIndex 护甲板_;
        public static ItemIndex 黄金隆起_;
        public static ItemIndex 黄晶胸针_;
        public static ItemIndex 巨型隆起_;
        public static ItemIndex 摩卡_;
        public static ItemIndex 燃料电池_;
        public static ItemIndex 肉排_;
        public static ItemIndex 特拉法梅的祝福_;
        public static ItemIndex 注射器_;

        public enum BodyName {
            None,
            Arbiter,
            Bandit2,
            Captain,
            CHEF,
            Commando,
            Croco,
            Engi,
            EngiTurret,
            EngiWalkerTurret,
            Heretic,
            Huntress,
            Loader,
            Mage,
            Merc,
            Pathfinder,
            Railgunner,
            RedMist,
            RobPaladin,
            SniperClassic,
            Toolbot,
            Treebot,
            VoidSurvivor,
        }

        public static void AddHook() {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.EquipmentSlot.FixedUpdate += EquipmentSlot_FixedUpdate;
        }

        public static void RemoveHook() {
            R2API.RecalculateStatsAPI.GetStatCoefficients -= RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.EquipmentSlot.FixedUpdate -= EquipmentSlot_FixedUpdate;
        }

        public static void LateInit() {
            穿甲弹_ = RoR2Content.Items.BossDamageBonus.itemIndex;
            刽子手的重负_ = vanillaVoid.Items.ExeBlade.instance.ItemDef.itemIndex;
            护甲板_ = RoR2Content.Items.ArmorPlate.itemIndex;
            黄金隆起_ = GoldenCoastPlus.GoldenCoastPlus.goldenKnurlDef.itemIndex;
            黄晶胸针_ = RoR2Content.Items.BarrierOnKill.itemIndex;
            巨型隆起_ = RoR2Content.Items.Knurl.itemIndex;
            摩卡_ = DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex;
            燃料电池_ = RoR2Content.Items.EquipmentMagazine.itemIndex;
            肉排_ = RoR2Content.Items.FlatHealth.itemIndex;
            特拉法梅的祝福_ = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarWings").itemIndex;
            注射器_ = RoR2Content.Items.Syringe.itemIndex;
            BuffAndDotHook.工匠_ = BodyCatalog.FindBodyIndex("MageBody");
            GlobalEventHook.工程师固定炮台_ = BodyCatalog.FindBodyIndex("EngiTurretBody");
            GlobalEventHook.工程师移动炮台_ = BodyCatalog.FindBodyIndex("EngiWalkerTurretBody");
            GlobalEventHook.雇佣兵_ = BodyCatalog.FindBodyIndex("MercBody");
            SkillHook.Heretic = BodyCatalog.FindBodyIndex("HereticBody");
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("ArbiterBody"), BodyName.Arbiter);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("Bandit2Body"), BodyName.Bandit2);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CaptainBody"), BodyName.Captain);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CHEF"), BodyName.CHEF);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CommandoBody"), BodyName.Commando);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CrocoBody"), BodyName.Croco);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("EngiBody"), BodyName.Engi);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("HuntressBody"), BodyName.Huntress);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("LoaderBody"), BodyName.Loader);
            BodyIndexToName_.Add(BuffAndDotHook.工匠_, BodyName.Mage);
            BodyIndexToName_.Add(GlobalEventHook.雇佣兵_, BodyName.Merc);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("RailgunnerBody"), BodyName.Railgunner);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("RedMistBody"), BodyName.RedMist);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("PathfinderBody"), BodyName.Pathfinder);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("RobPaladinBody"), BodyName.RobPaladin);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("SniperClassicBody"), BodyName.SniperClassic);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("ToolbotBody"), BodyName.Toolbot);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("TreebotBody"), BodyName.Treebot);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("VoidSurvivorBody"), BodyName.VoidSurvivor);
            BodyIndexToName_.Add(SkillHook.Heretic, BodyName.Heretic);
            BodyIndexToName_.Add(GlobalEventHook.工程师固定炮台_, BodyName.EngiTurret);
            BodyIndexToName_.Add(GlobalEventHook.工程师移动炮台_, BodyName.EngiWalkerTurret);
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            Inventory inventory = sender.inventory;
            if (inventory) {
                float upLevel = sender.level - 1;
                int itemCount = inventory.GetItemCount(肉排_);
                float levelMaxHealthAdd = 2.5f * itemCount;
                if (BodyIndexToName_.TryGetValue(sender.bodyIndex, out BodyName loc)) {
                    switch (loc) {
                        case BodyName.Arbiter: {
                            args.cooldownMultAdd -= upLevel / (15 + upLevel); float statUpPercent = 0.05f * inventory.GetItemCount(摩卡_);
                            args.healthMultAdd += statUpPercent;
                            args.regenMultAdd += statUpPercent;
                            break;
                        }
                        case BodyName.Bandit2: { break; }
                        case BodyName.Captain: {
                            itemCount = inventory.GetItemCount(黄晶胸针_);
                            args.baseShieldAdd += 15 * itemCount;
                            args.shieldMultAdd += 0.25f * itemCount;
                            break;
                        }
                        case BodyName.CHEF: {
                            levelMaxHealthAdd *= 2;
                            break;
                        }
                        case BodyName.Commando: {
                            float statUpPercent = 0.03f * inventory.GetItemCount(注射器_);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.healthMultAdd += statUpPercent;
                            args.moveSpeedMultAdd += statUpPercent;
                            args.baseDamageAdd += 2 * inventory.GetItemCount(穿甲弹_);
                            break;
                        }
                        case BodyName.Croco: { break; }
                        case BodyName.Engi: {
                            args.armorAdd += 10 * inventory.GetItemCount(护甲板_);
                            break;
                        }
                        case BodyName.EngiTurret: {
                            float statUpPercent = 0.25f * inventory.GetItemCount(SkillHook.古代权杖_);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.damageMultAdd += statUpPercent;
                            break;
                        }
                        case BodyName.EngiWalkerTurret: {
                            float statUpPercent = 0.25f * inventory.GetItemCount(SkillHook.古代权杖_);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.damageMultAdd += statUpPercent;
                            break;
                        }
                        case BodyName.Heretic: {
                            args.cooldownReductionAdd += 2 * inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement.itemIndex);
                            args.attackSpeedMultAdd += inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                            args.moveSpeedMultAdd += inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement.itemIndex);
                            args.healthMultAdd += inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
                            break;
                        }
                        case BodyName.Huntress: { break; }
                        case BodyName.Loader: { break; }
                        case BodyName.Mage: { break; }
                        case BodyName.Merc: { break; }
                        case BodyName.Pathfinder: { break; }
                        case BodyName.Railgunner: { break; }
                        case BodyName.RedMist: { break; }
                        case BodyName.RobPaladin: {
                            args.baseDamageAdd += 3 * inventory.GetItemCount(刽子手的重负_);
                            break;
                        }
                        case BodyName.SniperClassic: { break; }
                        case BodyName.Toolbot: { break; }
                        case BodyName.Treebot: {
                            args.cooldownMultAdd += Mathf.Pow(0.9f, inventory.GetItemCount(燃料电池_)) - 1f;
                            break;
                        }
                        case BodyName.VoidSurvivor: { break; }
                    }
                }
                itemCount = inventory.GetItemCount(巨型隆起_);
                levelMaxHealthAdd += 0.25f * (sender.levelMaxHealth + levelMaxHealthAdd) * itemCount;
                args.baseHealthAdd += levelMaxHealthAdd * upLevel;
                itemCount = inventory.GetItemCount(黄金隆起_);
                if (itemCount > 0 && sender.master) {
                    args.regenMultAdd += 0.5f * itemCount + 0.01f * (sender.master.money / 1000000);
                }
                if (sender.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex)) {
                    args.baseRegenAdd += 0.01f * (1 + inventory.GetItemCount(RoR2Content.Items.ShieldOnly.itemIndex)) * sender.maxShield;
                }
                if (inventory.HasItem(特拉法梅的祝福_)) {
                    args.moveSpeedMultAdd += 1;
                }
            }
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self.moveSpeed > 49) {
                self.moveSpeed = 49;
            }
        }

        private static void EquipmentSlot_FixedUpdate(On.RoR2.EquipmentSlot.orig_FixedUpdate orig, EquipmentSlot self) {
            orig(self);
            if (Input.GetKeyDown(KeyCode.LeftAlt) && self.inventory.HasItem(特拉法梅的祝福_)) {
                JetpackController jetpackController = JetpackController.FindJetpackController(self.gameObject);
                if (jetpackController) {
                    jetpackController.stopwatch = jetpackController.duration;
                } else {
                    GameObject gameObject = Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BodyAttachments/JetpackController"));
                    gameObject.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(self.gameObject, null);
                    JetpackController.FindJetpackController(self.gameObject).duration = 600f;
                }
            }
        }
    }
}