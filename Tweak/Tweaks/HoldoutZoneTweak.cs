using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class HoldoutZoneTweak : TweakBase<HoldoutZoneTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.HoldoutZoneController.Awake += HoldoutZoneController_Awake;
        }

        private void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self) {
            orig(self);
            if (RunInfo.已选择造物难度) {
                self.minimumRadius = Mathf.Max(7f, self.minimumRadius);
                self.dischargeRate = 0.5f / self.baseChargeDuration;
                self.calcRadius += (ref radius) => {
                    radius -= Mathf.Lerp(0f, radius - self.minimumRadius, self.charge);
                };
            }
        }
    }
}