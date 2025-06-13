using BTP.RoR2Plugin.Utils;
using  RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class HealWhileSafeTweak : IOnRoR2LoadedBehavior  {
        public const float RegenFraction = 0.015f;

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RoR2Content.Items.HealWhileSafe.TryApplyTag(ItemTag.AIBlacklist);
        }
    }
}