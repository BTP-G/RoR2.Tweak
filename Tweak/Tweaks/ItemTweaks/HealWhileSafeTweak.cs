using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class HealWhileSafeTweak : IRoR2LoadedMessageHandler {
        public const float RegenFraction = 0.015f;

        void IRoR2LoadedMessageHandler.Handle() {
            RoR2Content.Items.HealWhileSafe.TryApplyTag(ItemTag.AIBlacklist);
        }
    }
}