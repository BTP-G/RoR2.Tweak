using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks {

    internal class ShrineHealingTweak : ModComponent, IRoR2LoadedMessageHandler {

        void IRoR2LoadedMessageHandler.Handle() {
            GameObjectPaths.ShrineHealing.LoadComponent<ShrineHealingBehavior>().maxPurchaseCount = 10;
        }
    }
}