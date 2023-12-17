using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class VoidSurvivorTweak : TweakBase<VoidSurvivorTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += ChargeMegaBlaster_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += FireMegaBlasterBase_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.OnEnter += FireCorruptDisks_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var gameObject = GameObjectPaths.VoidSurvivorMegaBlasterBigProjectileCorrupted.Load<GameObject>();
            var projectileSimple = gameObject.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 40f;
            projectileSimple.lifetime = 6.6f;
            projectileSimple.lifetimeExpiredEffect = GameObjectPaths.VoidSurvivorMegaBlasterExplosionCorrupted.Load<GameObject>();
            gameObject.GetComponent<ProjectileImpactExplosion>().blastRadius = 25f; ;
            var radialForce = gameObject.AddComponent<RadialForce>();
            radialForce.radius = 25f;
            radialForce.damping = 0.5f;
            radialForce.forceMagnitude = -2500f;
            radialForce.forceCoefficientAtEdge = 0.5f;
            gameObject.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
        }

        private void ChargeMegaBlaster_OnEnter(On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster self) {
            self.baseDuration = 4;
            orig(self);
        }

        private void FireCorruptDisks_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self) {
            self.damageCoefficient = 25;
            self.selfKnockbackForce = 0;
            orig(self);
        }

        private void FireMegaBlasterBase_FireProjectiles(On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_FireProjectiles orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self) {
            if (self is EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBig) {
                self.force = 4444;
                self.damageCoefficient = 44.44f;
            } else {
                self.force = 666;
                self.damageCoefficient = 6.66f;
            }
            orig(self);
        }
    }
}