using BtpTweak.Utils;
using RoR2;

namespace BtpTweak.Tweaks {

    internal class HoldoutZoneTweak : TweakBase<HoldoutZoneTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.HoldoutZoneController.Awake += HoldoutZoneController_Awake;
        }

        private void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self) {
            orig(self);
            if (RunInfo.是否选择造物难度) {
                self.minimumRadius = 6f;
                self.dischargeRate = 0.5f / self.baseChargeDuration;
                self.calcRadius += (ref float radius) => {
                    radius -= self.charge * radius;
                };
            }
        }
    }
}