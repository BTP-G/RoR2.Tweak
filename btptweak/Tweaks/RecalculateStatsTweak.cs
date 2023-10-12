using BtpTweak.IndexCollections;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.Tweaks
{

    internal partial class RecalculateStatsTweak : TweakBase {
        public static readonly Dictionary<BodyIndex, BodyNameIndex> BodyIndexToName_ = new();
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

        public override void AddHooks() {
            base.AddHooks();
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        public override void Load() {
            base.Load();
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
            for (BodyNameIndex bodyName = BodyNameIndex.None + 1; bodyName < BodyNameIndex.Count; ++bodyName) {
                BodyIndexToName_.Add(BodyCatalog.FindBodyIndex(bodyName.ToString()), bodyName);
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            Inventory inventory = sender.inventory;
            if (inventory) {
                float upLevel = sender.level - 1;
                int itemCount = inventory.GetItemCount(肉排_);
                float levelMaxHealthAdd = 2.5f * itemCount;
                if (BodyIndexToName_.TryGetValue(sender.bodyIndex, out BodyNameIndex loc)) {
                    switch (loc) {
                        case BodyNameIndex.ArbiterBody: {
                            args.cooldownMultAdd -= upLevel / (15 + upLevel); float statUpPercent = 0.05f * inventory.GetItemCount(摩卡_);
                            args.healthMultAdd += statUpPercent;
                            args.regenMultAdd += statUpPercent;
                            break;
                        }
                        case BodyNameIndex.Bandit2Body: { break; }
                        case BodyNameIndex.CaptainBody: {
                            itemCount = inventory.GetItemCount(黄晶胸针_);
                            args.baseShieldAdd += 15 * itemCount;
                            args.shieldMultAdd += 0.25f * itemCount;
                            break;
                        }
                        case BodyNameIndex.CHEF: {
                            args.baseHealthAdd += 25 * itemCount;
                            levelMaxHealthAdd *= 2;
                            break;
                        }
                        case BodyNameIndex.CommandoBody: {
                            float statUpPercent = 0.03f * inventory.GetItemCount(注射器_);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.healthMultAdd += statUpPercent;
                            args.moveSpeedMultAdd += statUpPercent;
                            args.baseDamageAdd += 2 * inventory.GetItemCount(穿甲弹_);
                            break;
                        }
                        case BodyNameIndex.CrocoBody: { break; }
                        case BodyNameIndex.EngiBody:
                        case BodyNameIndex.EngiTurretBody:
                        case BodyNameIndex.EngiWalkerTurretBody: {
                            args.armorAdd += 10 * inventory.GetItemCount(护甲板_);
                            break;
                        }
                        case BodyNameIndex.HereticBody: {
                            args.cooldownReductionAdd += 2 * inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement.itemIndex);
                            args.attackSpeedMultAdd += 0.5f * inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                            args.moveSpeedMultAdd += 0.5f * inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement.itemIndex);
                            args.healthMultAdd += 0.1f * inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
                            break;
                        }
                        case BodyNameIndex.HuntressBody: { break; }
                        case BodyNameIndex.LoaderBody: { break; }
                        case BodyNameIndex.MageBody: { break; }
                        case BodyNameIndex.MercBody: { break; }
                        case BodyNameIndex.PathfinderBody: { break; }
                        case BodyNameIndex.RailgunnerBody: { break; }
                        case BodyNameIndex.RedMistBody: { break; }
                        case BodyNameIndex.RobPaladinBody: {
                            args.baseDamageAdd += 3 * inventory.GetItemCount(刽子手的重负_);
                            break;
                        }
                        case BodyNameIndex.SniperClassicBody: { break; }
                        case BodyNameIndex.ToolbotBody: { break; }
                        case BodyNameIndex.TreebotBody: {
                            args.cooldownMultAdd += Mathf.Pow(0.9f, inventory.GetItemCount(燃料电池_)) - 1f;
                            break;
                        }
                        case BodyNameIndex.VoidSurvivorBody: { break; }
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
                if (sender.HasBuff(RoR2Content.Buffs.FullCrit.buffIndex) && sender.crit > 100f) {
                    args.critDamageMultAdd += (sender.crit - 100f) * 0.01f;
                }
            }
        }
    }
}