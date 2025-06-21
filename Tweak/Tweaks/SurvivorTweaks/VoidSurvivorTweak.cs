using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class VoidSurvivorTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float ChargeMegaBlasterBaseDuration = 4f;
        public const float FireCorruptDisksDamageCoefficient = 25f;
        public const float FireCorruptDisksSelfKnockbackForce = 0;
        public const float FireMegaBlasterBigDamageCoefficient = 44.44f;
        public const float FireMegaBlasterBigForce = 4444f;
        public const float FireMegaBlasterSmallDamageCoefficient = 4.44f;
        public const float FireMegaBlasterSmallForce = 444f;

        void IModLoadMessageHandler.Handle() {
            EntityStateConfigurationPaths.EntityStatesVoidSurvivorWeaponChargeMegaBlaster.Load<EntityStateConfiguration>().Set("baseDuration", ChargeMegaBlasterBaseDuration.ToString());
            EntityStateConfigurationPaths.EntityStatesVoidSurvivorWeaponFireCorruptDisks.Load<EntityStateConfiguration>().Set(new System.Collections.Generic.Dictionary<string, string> {
                ["damageCoefficient"] = FireCorruptDisksDamageCoefficient.ToString(),
                ["selfKnockbackForce"] = FireCorruptDisksSelfKnockbackForce.ToString(),
            });
            EntityStateConfigurationPaths.EntityStatesVoidSurvivorWeaponFireMegaBlasterBig.Load<EntityStateConfiguration>().Set(new System.Collections.Generic.Dictionary<string, string> {
                ["damageCoefficient"] = FireMegaBlasterBigDamageCoefficient.ToString(),
                ["force"] = FireMegaBlasterBigForce.ToString(),
            });
            EntityStateConfigurationPaths.EntityStatesVoidSurvivorWeaponFireMegaBlasterSmall.Load<EntityStateConfiguration>().Set(new System.Collections.Generic.Dictionary<string, string> {
                ["damageCoefficient"] = FireMegaBlasterSmallDamageCoefficient.ToString(),
                ["force"] = FireMegaBlasterSmallForce.ToString(),
            });
        }

        void IRoR2LoadedMessageHandler.Handle() {
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
    }
}