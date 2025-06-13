using BTP.RoR2Plugin.RoR2Indexes;
using R2API;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class CHEFTweak : TweakBase<CHEFTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Chef, RecalculateCHEFStats);
        }

        private void RecalculateCHEFStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            var itemCount = inventory.GetItemCount(RoR2Content.Items.FlatHealth.itemIndex);
            args.baseHealthAdd += 25 * itemCount;
            args.levelHealthAdd += body.levelMaxHealth * 0.2f * itemCount;
        }
    }
}