using BTP.RoR2Plugin.RoR2Indexes;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class ArbiterTweak : ModComponent, IRoR2LoadedMessageHandler {
        public const float StatUpCoefficient = 0.05f;

        void IRoR2LoadedMessageHandler.Handle() {
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Arbiter, RecalculateArbiterStats);
        }

        private void RecalculateArbiterStats(CharacterBody body, Inventory inventory, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            var statUpPercent = StatUpCoefficient * inventory.GetItemCount(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
            args.healthMultAdd += statUpPercent;
            args.regenMultAdd += statUpPercent;
        }
    }
}