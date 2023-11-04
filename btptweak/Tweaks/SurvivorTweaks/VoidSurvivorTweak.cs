using BtpTweak.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class VoidSurvivorTweak : TweakBase<VoidSurvivorTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter += ChargeMegaBlaster_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles += FireMegaBlasterBase_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.OnEnter += FireCorruptDisks_OnEnter;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter -= FireHandBeam_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.ChargeMegaBlaster.OnEnter -= ChargeMegaBlaster_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.FireProjectiles -= FireMegaBlasterBase_FireProjectiles;
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.OnEnter -= FireCorruptDisks_OnEnter;
        }

        public void Load() {
            var gameObject = "RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectileCorrupted.prefab".Load<GameObject>();
            var projectileSimple = gameObject.GetComponent<ProjectileSimple>();
            projectileSimple.desiredForwardSpeed = 40f;
            projectileSimple.lifetime = 6.6f;
            projectileSimple.lifetimeExpiredEffect = "RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosionCorrupted.prefab".Load<GameObject>();
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

        private void FireHandBeam_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireHandBeam self) {
            self.damageCoefficient = 2.4f;
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