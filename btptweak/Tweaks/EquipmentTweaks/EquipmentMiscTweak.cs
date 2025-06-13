using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.EquipmentTweaks {

    internal class EquipmentMiscTweak : TweakBase<EquipmentMiscTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var fireballVehicle = GameObjectPaths.FireballVehicle.LoadComponent<FireballVehicle>();
            fireballVehicle.duration = 6;
            fireballVehicle.overlapResetFrequency = 3f;
            EntityStates.GoldGat.GoldGatFire.maxFireFrequency *= 2;
            var beamSphere = GameObjectPaths.BeamSphere.Load<GameObject>();
            var proximityBeamController = beamSphere.GetComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange = 66.6f;
            proximityBeamController.damageCoefficient = 6.66f;
            var projectileImpactExplosion = beamSphere.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.blastDamageCoefficient = 66.6f;
            projectileImpactExplosion.lifetime = 66.6f;
            var projectileSimple = beamSphere.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 10f;
            projectileSimple.lifetime = 66.6f;
            RoR2Content.Equipment.LifestealOnHit.cooldown = 40f;
            DLC1Content.Equipment.Molotov.cooldown = 30f;
            foreach (var equipment in EquipmentCatalog.equipmentDefs) {
                equipment.pickupToken = equipment.descriptionToken;
            }
        }
    }
}