using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;

namespace BTP.RoR2Plugin.Tweaks {

    internal class RecalculateStatsTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private static readonly Dictionary<int, Action<CharacterBody, Inventory, RecalculateStatsAPI.StatHookEventArgs>> BodyIndexToRecalculateStatsAction = [];
        private ItemIndex _goldenKnurlIndex;

        public static void AddRecalculateStatsActionToBody(BodyIndex bodyIndex, Action<CharacterBody, Inventory, RecalculateStatsAPI.StatHookEventArgs> action) {
            if (bodyIndex == BodyIndex.None || action == null) {
                return;
            }
            if (BodyIndexToRecalculateStatsAction.ContainsKey((int)bodyIndex)) {
                BodyIndexToRecalculateStatsAction[(int)bodyIndex] += action;
            } else {
                BodyIndexToRecalculateStatsAction.Add((int)bodyIndex, action);
            }
        }

        void IModLoadMessageHandler.Handle() {
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            _goldenKnurlIndex = ItemCatalog.FindItemIndex("Golden Knurl");
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args) {
            if (sender.inventory) {
                var inventory = sender.inventory;
                var knurlCount = inventory.GetItemCount(RoR2Content.Items.Knurl.itemIndex);
                var regenFraction = KnurlTweak.RegenFraction * knurlCount;
                args.levelHealthAdd += sender.levelMaxHealth * (KnurlTweak.LevelHealthAddCoefficient * knurlCount + FlatHealthTweak.LevelHealthAddCoefficient * inventory.GetItemCount(RoR2Content.Items.FlatHealth.itemIndex));
                if (sender.outOfDanger) {
                    regenFraction += HealWhileSafeTweak.RegenFraction * inventory.GetItemCount(RoR2Content.Items.HealWhileSafe.itemIndex);
                }
                args.critAdd += HealOnCritTweak.StackCrit * inventory.GetItemCount(RoR2Content.Items.HealOnCrit.itemIndex);
                args.baseRegenAdd += regenFraction * sender.maxHealth + 0.01f * inventory.GetItemCount(RoR2Content.Items.ShieldOnly.itemIndex) * sender.maxShield;
                args.regenMultAdd += 0.5f * inventory.GetItemCount(_goldenKnurlIndex);
                var barrierOnOverHealCount = inventory.GetItemCount(RoR2Content.Items.BarrierOnOverHeal.itemIndex);
                args.armorAdd += 10 * inventory.GetItemCount(RoR2Content.Items.Bear.itemIndex) + 50 * barrierOnOverHealCount - 10 * sender.GetBuffCount(RoR2Content.Buffs.BeetleJuice.buffIndex);
                args.levelArmorAdd += barrierOnOverHealCount;
                if (BodyIndexToRecalculateStatsAction.TryGetValue((int)sender.bodyIndex, out var action)) {
                    action.Invoke(sender, inventory, args);
                }
            }
            if (sender.HasBuff(RoR2Content.Buffs.FullCrit.buffIndex) && sender.crit > 100f) {
                args.critDamageMultAdd += (sender.crit - 100f) * 0.01f;
            }
            if (RunInfo.已选择大旋风难度 && sender.teamComponent.teamIndex != TeamIndex.Player) {
                args.levelArmorAdd += 0.05f
                    + (sender.isElite ? 0.1f : 0f)
                    + (sender.isBoss ? 0.15f : 0f)
                    + (sender.isChampion ? 0.15f : 0f);
            }
        }
    }
}