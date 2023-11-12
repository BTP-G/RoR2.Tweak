namespace BtpTweak.Tweaks.ItemTweaks {

    internal class SiphonOnLowHealthTweak : TweakBase<SiphonOnLowHealthTweak> {
        public const int DamageCoefficient = 1;
        public const int BaseRadius = 15;
        public const int StackRadius = 5;
        public const int MaxTargets = 1;

        public override void SetEventHandlers() {
            On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.FixedUpdate += SiphonOnLowHealthItemBodyBehavior_FixedUpdate;
        }

        public override void ClearEventHandlers() {
            On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.FixedUpdate -= SiphonOnLowHealthItemBodyBehavior_FixedUpdate;
        }

        private void SiphonOnLowHealthItemBodyBehavior_FixedUpdate(On.RoR2.Items.SiphonOnLowHealthItemBodyBehavior.orig_FixedUpdate orig, RoR2.Items.SiphonOnLowHealthItemBodyBehavior self) {
            if (self.siphonNearbyController.NetworkmaxTargets != self.stack) {
                self.siphonNearbyController.NetworkmaxTargets = self.stack;
                self.siphonNearbyController.Networkradius = BaseRadius + StackRadius * (self.stack - 1);
                self.siphonNearbyController.damagePerSecondCoefficient = self.stack;
            }
        }
    }
}