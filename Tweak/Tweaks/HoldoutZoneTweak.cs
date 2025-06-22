﻿using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class HoldoutZoneTweak : ModComponent, IModLoadMessageHandler {

        void IModLoadMessageHandler.Handle() {
            On.RoR2.HoldoutZoneController.Awake += HoldoutZoneController_Awake;
        }

        private void HoldoutZoneController_Awake(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self) {
            orig(self);
            if (RunInfo.已选择大旋风难度) {
                self.minimumRadius = Mathf.Max(7f, self.minimumRadius);
                self.dischargeRate = 0.5f / self.baseChargeDuration;
                self.calcRadius += (ref radius) => {
                    radius -= Mathf.Lerp(0f, radius - self.minimumRadius, self.charge);
                };
            }
        }
    }
}