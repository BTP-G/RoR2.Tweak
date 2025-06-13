using BTP.RoR2Plugin.Utils;
using RoR2;
using System.Linq;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.EquipmentTweaks {

    internal class RecycleTweak : TweakBase<RecycleTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.EquipmentSlot.FireRecycle += EquipmentSlot_FireRecycle;
        }

        private bool EquipmentSlot_FireRecycle(On.RoR2.EquipmentSlot.orig_FireRecycle orig, EquipmentSlot self) {
            self.UpdateTargets(RoR2Content.Equipment.Recycle.equipmentIndex, false);
            GenericPickupController pickupController = self.currentTarget.pickupController;
            if (!pickupController || pickupController.pickupIndex.pickupDef.equipmentIndex == RoR2Content.Equipment.Recycle.equipmentIndex) {
                self.InvalidateCurrentTarget();
                return false;
            }
            var initialPickupIndex = pickupController.pickupIndex;
            self.subcooldownTimer = 0.2f;
            var pickupIndices = PickupTransmutationManager.GetAvailableGroupFromPickupIndex(pickupController.pickupIndex).Where(p => p != initialPickupIndex).ToList();
            if (pickupIndices == null || pickupIndices.Count == 0) {
                return false;
            }
            var newPickupIndex = self.rng.NextElementUniform(pickupIndices);
            switch (newPickupIndex.pickupDef.itemTier) {
                case ItemTier.Tier1:
                    if (Util.CheckRoll(5)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapWhite.itemIndex);
                    }
                    break;

                case ItemTier.Tier2:
                    if (Util.CheckRoll(10)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapGreen.itemIndex);
                    }
                    break;

                case ItemTier.Tier3:
                    if (Util.CheckRoll(20)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapRed.itemIndex);
                    }
                    break;

                case ItemTier.Lunar:
                    if (!RunInfo.位于月球 && Util.CheckRoll(15f)) {
                        Object.Destroy(pickupController.gameObject);
                        EffectManager.SpawnEffect(AssetReferences.moonExitArenaOrbEffect, new EffectData {
                            origin = pickupController.pickupDisplay.transform.position,
                            rotation = default,
                        }, true);
                        return true;
                    }
                    break;

                case ItemTier.Boss:
                    if (Util.CheckRoll(20)) {
                        newPickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.ScrapYellow.itemIndex);
                    }
                    break;

                case ItemTier.VoidTier1:
                case ItemTier.VoidTier2:
                case ItemTier.VoidTier3:
                case ItemTier.VoidBoss:
                    if (Util.CheckRoll(Mathf.Pow((float)newPickupIndex.pickupDef.itemTier, 1.5f))) {
                        Object.Destroy(pickupController.gameObject);
                        BtpUtils.SpawnVoidDeathBomb(pickupController.pickupDisplay.transform.position);
                        return true;
                    }
                    break;

                case ItemTier.NoTier:
                    if (newPickupIndex.pickupDef.equipmentIndex != EquipmentIndex.None && newPickupIndex.pickupDef.isLunar) {
                        if (!RunInfo.位于月球 && Util.CheckRoll(15f)) {
                            Object.Destroy(pickupController.gameObject);
                            EffectManager.SpawnEffect(AssetReferences.moonExitArenaOrbEffect, new EffectData {
                                origin = pickupController.pickupDisplay.transform.position,
                                rotation = default,
                            }, true);
                            return true;
                        }
                    }
                    break;
            }
            pickupController.NetworkpickupIndex = newPickupIndex;
            EffectManager.SimpleEffect(AssetReferences.omniRecycleEffect, pickupController.pickupDisplay.transform.position, Quaternion.identity, true);
            return true;
        }
    }
}