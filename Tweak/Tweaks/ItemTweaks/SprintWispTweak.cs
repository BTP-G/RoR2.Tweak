using RoR2.Items;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class SprintWispTweak : TweakBase<SprintWispTweak>, IOnRoR2LoadedBehavior {
        public const float FireInterval = 1.0f;

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            SprintWispBodyBehavior.searchRadius = 66.6f;
            SprintWispBodyBehavior.fireRate = 1f / 7f;
        }
    }
}