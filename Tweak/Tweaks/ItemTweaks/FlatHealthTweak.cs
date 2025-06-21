using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class FlatHealthTweak : TweakBase<FlatHealthTweak>, IOnRoR2LoadedBehavior {
        public const float LevelHealthAddCoefficient = 0.25f;

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RoR2Content.Items.FlatHealth.TryRemoveTag(ItemTag.OnKillEffect);
        }
    }
}