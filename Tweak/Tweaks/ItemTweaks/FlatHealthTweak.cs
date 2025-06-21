using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class FlatHealthTweak : ModComponent, IRoR2LoadedMessageHandler {
        public const float LevelHealthAddCoefficient = 0.25f;

        void IRoR2LoadedMessageHandler.Handle() {
            RoR2Content.Items.FlatHealth.TryRemoveTag(ItemTag.OnKillEffect);
        }
    }
}