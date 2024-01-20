using BtpTweak.RoR2Indexes;
using R2API;
using RoR2;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class RobPaladinTweak : TweakBase<RobPaladinTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.RobPaladin, RecalculateRobPaladinStats);
        }

        private void RecalculateRobPaladinStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            args.baseDamageAdd += 3 * inventory.GetItemCount(vanillaVoid.Items.ExeBlade.instance.ItemDef);
        }
    }
}