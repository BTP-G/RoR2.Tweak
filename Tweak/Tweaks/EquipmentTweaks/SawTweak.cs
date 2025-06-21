using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.EquipmentTweaks {

    internal class SawTweak : ModComponent, IRoR2LoadedMessageHandler {
        public const float DamageCoefficient = 3f;
        public const float DamageCoefficientPerSecond = 3f;

        void IRoR2LoadedMessageHandler.Handle() {
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