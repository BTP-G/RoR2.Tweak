using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.EquipmentTweaks {

    internal class SawTweak : TweakBase<SawTweak>, IOnRoR2LoadedBehavior {
        public const float DamageCoefficient = 3f;
        public const float DamageCoefficientPerSecond = 3f;

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RoR2Content.Equipment.Saw.cooldown = 20f;
            var sawmerang = GameObjectPaths.Sawmerang.Load<GameObject>();
            sawmerang.GetComponent<ProjectileDamage>().damageType |= DamageType.BleedOnHit | DamageType.SuperBleedOnCrit;
            var boomerangProjectile = sawmerang.GetComponent<BoomerangProjectile>();
            boomerangProjectile.transitionDuration *= 5f;
            boomerangProjectile.travelSpeed = 36f;
            var overlapAttack = sawmerang.GetComponent<ProjectileOverlapAttack>();
            overlapAttack.damageCoefficient = DamageCoefficient;  // 4
            overlapAttack.overlapProcCoefficient = 1;  //1
            overlapAttack.fireFrequency = 30f;  // 60
            overlapAttack.resetInterval = -1;  // -1
            var dotZone = sawmerang.GetComponent<ProjectileDotZone>();
            dotZone.damageCoefficient = 0.1f;  // 0.2
            dotZone.overlapProcCoefficient = 0.1f;  // 0.2
            dotZone.fireFrequency = 30f;  // 30
            dotZone.resetFrequency = 30f;  // 10
        }
    }
}