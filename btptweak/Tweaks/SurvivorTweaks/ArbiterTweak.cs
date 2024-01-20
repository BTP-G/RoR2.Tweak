using BtpTweak.RoR2Indexes;
using RoR2;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class ArbiterTweak : TweakBase<ArbiterTweak>, IOnRoR2LoadedBehavior {
        public const float StatUpCoefficient = 0.05f;

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Arbiter, RecalculateArbiterStats);
        }

        private void RecalculateArbiterStats(CharacterBody body, Inventory inventory, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            var statUpPercent = StatUpCoefficient * inventory.GetItemCount(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
            args.healthMultAdd += statUpPercent;
            args.regenMultAdd += statUpPercent;
        }
    }
}