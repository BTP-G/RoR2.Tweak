using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.EquipmentTweaks {

    internal class FireballVehicleTweak : ModComponent, IModLoadMessageHandler {
        public const float FireBallsDamageCoeffcient = 5f;

        void IModLoadMessageHandler.Handle() {
            On.RoR2.FireballVehicle.FixedUpdate += FireballVehicle_FixedUpdate;
        }

        private void FireballVehicle_FixedUpdate(On.RoR2.FireballVehicle.orig_FixedUpdate orig, FireballVehicle self) {
            if (NetworkServer.active && self.overlapResetAge + Time.fixedDeltaTime >= 1f / self.overlapResetFrequency) {
                CharacterBody currentPassengerBody = self.vehicleSeat.currentPassengerBody;
                if (currentPassengerBody != null) {
                    FireProjectileInfo fireProjectileInfo = new() {
                        crit = currentPassengerBody.RollCrit(),
                        damage = currentPassengerBody.damage * FireBallsDamageCoeffcient * (1 + currentPassengerBody.inventory.GetItemCount(RoR2Content.Items.FireballsOnHit.itemIndex)),
                        damageColorIndex = DamageColorIndex.Item,
                        force = 100f,
                        fuseOverride = 3,
                        owner = currentPassengerBody.gameObject,
                        position = currentPassengerBody.corePosition,
                        procChainMask = default,
                        projectilePrefab = AssetReferences.magmaOrbProjectile,
                        rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f))),
                        speedOverride = Random.Range(10, 30),
                        target = null,
                        useFuseOverride = true,
                        useSpeedOverride = true,
                    };
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f)));
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                    fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(new(Random.Range(-2f, 2f), 6f, Random.Range(-2f, 2f)));
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            }
            orig(self);
        }
    }
}