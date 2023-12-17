using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.EquipmentTweaks {

    internal class EquipmentMiscTweak : TweakBase<EquipmentMiscTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var sawmerang = GameObjectPaths.Sawmerang.Load<GameObject>();
            var boomerangProjectile = sawmerang.GetComponent<BoomerangProjectile>();
            boomerangProjectile.transitionDuration *= 5f;
            boomerangProjectile.travelSpeed = 36f;
            FireballVehicle fireballVehicle = GameObjectPaths.FireballVehicle.LoadComponent<FireballVehicle>();
            fireballVehicle.duration = 6;
            fireballVehicle.overlapResetFrequency = 3f;
            EntityStates.GoldGat.GoldGatFire.maxFireFrequency *= 2;
            GameObject beamSphere = GameObjectPaths.BeamSphere.Load<GameObject>();
            ProjectileProximityBeamController proximityBeamController = beamSphere.GetComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange = 66.6f;
            proximityBeamController.damageCoefficient = 6.66f;
            ProjectileImpactExplosion projectileImpactExplosion = beamSphere.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.blastDamageCoefficient = 66.6f;
            projectileImpactExplosion.lifetime = 66.6f;
            ProjectileSimple projectileSimple = beamSphere.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 10f;
            projectileSimple.lifetime = 66.6f;
            RoR2Content.Equipment.Saw.cooldown = 20f;
            RoR2Content.Equipment.LifestealOnHit.cooldown = 40f;
            DLC1Content.Equipment.Molotov.cooldown = 30f;
        }
    }
}