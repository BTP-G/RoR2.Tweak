using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using R2API;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class ChefTweak : ModComponent, IRoR2LoadedMessageHandler {

        void IRoR2LoadedMessageHandler.Handle() {
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Chef, RecalculateCHEFStats);
            DLC2Content.Buffs.Oiled.canStack = true;
        }

        private void RecalculateCHEFStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            var itemCount = inventory.GetItemCount(RoR2Content.Items.FlatHealth.itemIndex);
            args.baseHealthAdd += 25 * itemCount;
            args.levelHealthAdd += body.levelMaxHealth * FlatHealthTweak.LevelHealthAddCoefficient * itemCount;
        }
    }
}