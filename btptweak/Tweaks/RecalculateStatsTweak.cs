using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class RecalculateStatsTweak : TweakBase {
        public static readonly Dictionary<BodyIndex, BodyName> BodyIndexToName_ = new();
        private ItemIndex 穿甲弹_;
        private ItemIndex 刽子手的重负_;
        private ItemIndex 护甲板_;
        private ItemIndex 黄金隆起_;
        private ItemIndex 黄晶胸针_;
        private ItemIndex 巨型隆起_;
        private ItemIndex 摩卡_;
        private ItemIndex 燃料电池_;
        private ItemIndex 肉排_;
        private ItemIndex 注射器_;

        public enum BodyName : byte {
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

        public override void AddHooks() {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public override void Load() {
            穿甲弹_ = RoR2Content.Items.BossDamageBonus.itemIndex;
            刽子手的重负_ = vanillaVoid.Items.ExeBlade.instance.ItemDef.itemIndex;
            护甲板_ = RoR2Content.Items.ArmorPlate.itemIndex;
            黄金隆起_ = GoldenCoastPlus.GoldenCoastPlus.goldenKnurlDef.itemIndex;
            黄晶胸针_ = RoR2Content.Items.BarrierOnKill.itemIndex;
            巨型隆起_ = RoR2Content.Items.Knurl.itemIndex;
            摩卡_ = DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex;
            燃料电池_ = RoR2Content.Items.EquipmentMagazine.itemIndex;
            肉排_ = RoR2Content.Items.FlatHealth.itemIndex;
            注射器_ = RoR2Content.Items.Syringe.itemIndex;
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("ArbiterBody"), BodyName.Arbiter);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("Bandit2Body"), BodyName.Bandit2);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CaptainBody"), BodyName.Captain);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CHEF"), BodyName.CHEF);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CommandoBody"), BodyName.Commando);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("CrocoBody"), BodyName.Croco);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("EngiBody"), BodyName.Engi);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("HuntressBody"), BodyName.Huntress);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("LoaderBody"), BodyName.Loader);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("MageBody"), BodyName.Mage);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("MercBody"), BodyName.Merc);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("RailgunnerBody"), BodyName.Railgunner);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("RedMistBody"), BodyName.RedMist);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("PathfinderBody"), BodyName.Pathfinder);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("RobPaladinBody"), BodyName.RobPaladin);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("SniperClassicBody"), BodyName.SniperClassic);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("ToolbotBody"), BodyName.Toolbot);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("TreebotBody"), BodyName.Treebot);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("VoidSurvivorBody"), BodyName.VoidSurvivor);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("HereticBody"), BodyName.Heretic);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("EngiTurretBody"), BodyName.EngiTurret);
            BodyIndexToName_.Add(BodyCatalog.FindBodyIndex("EngiWalkerTurretBody"), BodyName.EngiWalkerTurret);
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
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
                            args.baseHealthAdd += 25 * itemCount;
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
                        case BodyName.Engi:
                        case BodyName.EngiTurret:
                        case BodyName.EngiWalkerTurret: {
                            args.armorAdd += 10 * inventory.GetItemCount(护甲板_);
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
                if (sender.HasBuff(RoR2Content.Buffs.Warbanner.buffIndex)) {
                    args.armorAdd += 30;
                }
                if (sender.HasBuff(RoR2Content.Buffs.FullCrit.buffIndex) && sender.crit > 100) {
                    args.critDamageMultAdd += (sender.crit - 100f) * 0.01f;
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self) {
            orig(self);
            if (self.moveSpeed > 49) {
                self.moveSpeed = 49;
            }
        }
    }
}