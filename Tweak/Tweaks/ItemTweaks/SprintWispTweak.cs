using RoR2.Items;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class SprintWispTweak : ModComponent, IRoR2LoadedMessageHandler {
        public const float FireInterval = 1.0f;

        void IRoR2LoadedMessageHandler.Handle() {
            SprintWispBodyBehavior.searchRadius = 66.6f;
            SprintWispBodyBehavior.fireRate = 1f / 7f;
        }
    }
}