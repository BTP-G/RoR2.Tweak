using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class HoldoutZoneTweak : TweakBase<HoldoutZoneTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.HoldoutZoneController.Awake += HoldoutZoneController_Awake;
        }

        private void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self) {
            orig(self);
            if (RunInfo.是否选择造物难度) {
                self.minimumRadius = Mathf.Max(7f, self.minimumRadius);
                self.dischargeRate = 0.25f / self.baseChargeDuration;
                self.calcRadius += (ref float radius) => {
                    radius -= Mathf.Lerp(0, radius - self.minimumRadius, self.charge);
                };
            }
        }
    }
}