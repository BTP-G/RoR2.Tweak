using RoR2;
using UnityEngine;
using static BtpTweak.RoR2Indexes.BodyIndexes;

namespace BtpTweak.Tweaks {

    internal class RecalculateStatsTweak : TweakBase<RecalculateStatsTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            var inventory = sender.inventory;
            if (inventory) {
                float upLevel = sender.level - 1;
                int itemCount = inventory.GetItemCount(RoR2Content.Items.FlatHealth.itemIndex);
                float levelMaxHealthAdd = sender.levelMaxHealth / 9f * itemCount;
                if (BodyIndexToNameIndex.TryGetValue((int)sender.bodyIndex, out BodyNameIndex loc)) {
                    switch (loc) {
                        case BodyNameIndex.ArbiterBody: {
                            float statUpPercent = 0.05f * inventory.GetItemCount(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
                            args.healthMultAdd += statUpPercent;
                            args.regenMultAdd += statUpPercent;
                            break;
                        }
                        case BodyNameIndex.Bandit2Body: { break; }
                        case BodyNameIndex.CaptainBody: {
                            itemCount = inventory.GetItemCount(RoR2Content.Items.BarrierOnKill.itemIndex);
                            args.baseShieldAdd += 10 * itemCount;
                            args.shieldMultAdd += 0.1f * itemCount;
                            break;
                        }
                        case BodyNameIndex.CHEF: {
                            args.baseHealthAdd += 25 * itemCount;
                            levelMaxHealthAdd *= 2;
                            break;
                        }
                        case BodyNameIndex.CommandoBody: {
                            float statUpPercent = 0.03f * inventory.GetItemCount(RoR2Content.Items.Syringe.itemIndex);
                            args.attackSpeedMultAdd += statUpPercent;
                            args.healthMultAdd += statUpPercent;
                            args.moveSpeedMultAdd += statUpPercent;
                            args.baseDamageAdd += 2 * inventory.GetItemCount(RoR2Content.Items.BossDamageBonus.itemIndex);
                            break;
                        }
                        case BodyNameIndex.CrocoBody: { break; }
                        case BodyNameIndex.EngiBody:
                        case BodyNameIndex.EngiTurretBody:
                        case BodyNameIndex.EngiWalkerTurretBody: {
                            args.armorAdd += 10 * inventory.GetItemCount(RoR2Content.Items.ArmorPlate.itemIndex);
                            break;
                        }
                        case BodyNameIndex.HereticBody: {
                            args.cooldownReductionAdd += 2 * inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement.itemIndex);
                            args.attackSpeedMultAdd += 0.1f * inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                            args.moveSpeedMultAdd += 0.1f * inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement.itemIndex);
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
                            args.baseDamageAdd += 3 * inventory.GetItemCount(vanillaVoid.Items.ExeBlade.instance.ItemDef);
                            break;
                        }
                        case BodyNameIndex.SniperClassicBody: { break; }
                        case BodyNameIndex.ToolbotBody: { break; }
                        case BodyNameIndex.TreebotBody: {
                            args.cooldownMultAdd += Mathf.Pow(0.9f, inventory.GetItemCount(RoR2Content.Items.EquipmentMagazine.itemIndex)) - 1f;
                            break;
                        }
                        case BodyNameIndex.VoidSurvivorBody: { break; }
                        case BodyNameIndex.BrotherHurtBody: { break; }
                    }
                }
                itemCount = inventory.GetItemCount(RoR2Content.Items.Knurl.itemIndex);
                float regenFraction = 0.016f * itemCount;
                levelMaxHealthAdd += sender.levelMaxHealth * 0.5f * itemCount;
                args.baseHealthAdd += levelMaxHealthAdd * upLevel;
                args.critAdd += 5 * inventory.GetItemCount(RoR2Content.Items.HealOnCrit.itemIndex);
                args.regenMultAdd += 0.5f * inventory.GetItemCount(GoldenCoastPlus.GoldenCoastPlus.goldenKnurlDef);
                args.baseRegenAdd += 0.01f * inventory.GetItemCount(RoR2Content.Items.ShieldOnly.itemIndex) * sender.maxShield;
                args.armorAdd += (50 + upLevel) * inventory.GetItemCount(RoR2Content.Items.BarrierOnOverHeal.itemIndex) - 10 * sender.GetBuffCount(RoR2Content.Buffs.BeetleJuice.buffIndex);
                if (sender.HasBuff(RoR2Content.Buffs.Warbanner.buffIndex)) {
                    args.armorAdd += 30f;
                }
                if (sender.HasBuff(RoR2Content.Buffs.FullCrit.buffIndex) && sender.crit > 100f) {
                    args.critDamageMultAdd += (sender.crit - 100f) * 0.01f;
                }
                if (sender.HasBuff(RoR2Content.Buffs.TeamWarCry.buffIndex)) {
                    itemCount = inventory.GetItemCount(RoR2Content.Items.EnergizedOnEquipmentUse.itemIndex);
                    args.attackSpeedMultAdd += 0.5f * itemCount;
                    args.moveSpeedMultAdd += 0.25f * itemCount;
                }
                var warCryBuffCount = sender.GetBuffCount(RoR2Content.Buffs.WarCryBuff.buffIndex);
                if (warCryBuffCount > 0) {
                    args.attackSpeedMultAdd += 0.1f * warCryBuffCount;
                    args.moveSpeedMultAdd += 0.2f * warCryBuffCount;
                }
                if (sender.outOfDanger) {
                    regenFraction += 0.016f * inventory.GetItemCount(RoR2Content.Items.HealWhileSafe.itemIndex);
                }
                args.baseRegenAdd += regenFraction * sender.maxHealth;
                if (sender.healthComponent.barrier > sender.maxBarrier) {
                    sender.healthComponent.Networkbarrier = sender.maxBarrier;
                }
            }
        }
    }
}