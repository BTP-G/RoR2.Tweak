using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains
{
    public class FireFountain : ProjectileFountain
    {

        public override void AddProjectile(GameObject projectilePrefab, GameObject attacker, float baseDamage, bool isCrit, ProcChainMask procChainMask)
        {
            if (!isCrit) { procChainMask.AddProc(ProcType.Meatball); }
            base.AddProjectile(projectilePrefab, attacker, baseDamage, isCrit, procChainMask);
        }

        protected override void Fire(FireProjectileInfo info)
        {
            info.position = victimBody.corePosition + (victimBody.characterMotor ? Vector3.up * (victimBody.characterMotor.capsuleHeight + 2f) : Vector3.up * (victimBody.bestFitRadius * 2f));
            if (info.crit) { info.procChainMask.AddProc(ProcType.Meatball); }
            ProjectileManager.instance.FireProjectile(info);
        }
    }
}