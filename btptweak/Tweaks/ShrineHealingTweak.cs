using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks {

    internal class ShrineHealingTweak : TweakBase<ShrineHealingTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.ShrineHealing.LoadComponent<ShrineHealingBehavior>().maxPurchaseCount = 10;
        }
    }
}