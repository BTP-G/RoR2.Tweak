using RoR2;
using RoR2.Projectile;
using System.Linq;
using UnityEngine;

namespace BtpTweak.Tweaks.EquipmentTweaks {

    internal class RecycleTweak : TweakBase<RecycleTweak> {

        public override void ClearEventHandlers() {
            On.RoR2.EquipmentSlot.FireRecycle -= EquipmentSlot_FireRecycle;
        }

        public override void SetEventHandlers() {
            On.RoR2.EquipmentSlot.FireRecycle += EquipmentSlot_FireRecycle;
        }

        private bool EquipmentSlot_FireRecycle(On.RoR2.EquipmentSlot.orig_FireRecycle orig, EquipmentSlot self) {
            self.UpdateTargets(RoR2Content.Equipment.Recycle.equipmentIndex, false);
            GenericPickupController pickupController = self.currentTarget.pickupController;
            if (!pickupController || pickupController.pickupIndex.pickupDef.equipmentIndex == RoR2Content.Equipment.Recycle.equipmentIndex) {
                self.InvalidateCurrentTarget();
                return false;
            }
            PickupIndex initialPickupIndex = pickupController.pickupIndex;
            self.subcooldownTimer = 0.2f;
            PickupIndex[] array = (from pickupIndex in PickupTransmutationManager.GetAvailableGroupFromPickupIndex(pickupController.pickupIndex)
                                   where pickupIndex != initialPickupIndex
                                   select pickupIndex).ToArray();
            if (array == null || array.Length == 0) {
                return false;
            }
            PickupIndex newPickupIndex = Run.instance.treasureRng.NextElementUniform(array);
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
                    if (!SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("moon") && Util.CheckRoll(15)) {
                        Object.Destroy(pickupController.gameObject);
                        EffectManager.SpawnEffect(AssetReferences.lunarBlink, new EffectData {
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
                        CharacterBody body = self.characterBody;
                        FireProjectileInfo fireProjectileInfo = new() {
                            projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile,
                            position = pickupController.pickupDisplay.transform.position,
                            rotation = Quaternion.identity,
                            owner = body.gameObject,
                            damage = body.damage,
                            crit = body.RollCrit()
                        };
                        ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                        return true;
                    }
                    break;

                case ItemTier.NoTier:
                    if (newPickupIndex.pickupDef.equipmentIndex != EquipmentIndex.None && newPickupIndex.pickupDef.isLunar) {
                        if (!SceneCatalog.GetSceneDefForCurrentScene().cachedName.StartsWith("moon") && Util.CheckRoll(15)) {
                            Object.Destroy(pickupController.gameObject);
                            EffectManager.SpawnEffect(AssetReferences.lunarBlink, new EffectData {
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