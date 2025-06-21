namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class SiphonOnLowHealthTweak : ModComponent, IModLoadMessageHandler {
        public const float DamageCoefficient = 1.5f;
        public const int BaseRadius = 15;
        public const int StackRadius = 5;
        public const int MaxTargets = 1;

        void IModLoadMessageHandler.Handle() {
            On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.FixedUpdate += SiphonOnLowHealthItemBodyBehavior_FixedUpdate;
        }

        private void SiphonOnLowHealthItemBodyBehavior_FixedUpdate(On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.orig_FixedUpdate orig, RoR2.Items.SiphonOnLowHealthItemBodyBehavior self) {
            if (self.siphonNearbyController.NetworkmaxTargets != self.stack) {
                self.siphonNearbyController.NetworkmaxTargets = self.stack;
                self.siphonNearbyController.Networkradius = BaseRadius + StackRadius * (self.stack - 1);
                self.siphonNearbyController.damagePerSecondCoefficient = DamageCoefficient * self.stack;
            }
        }
    }
}