using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;

namespace BtpTweak.Tweaks {

    internal class ShrineHealingTweak : TweakBase<ShrineHealingTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.ShrineHealing.LoadComponent<ShrineHealingBehavior>().maxPurchaseCount = 10;
        }
    }
}