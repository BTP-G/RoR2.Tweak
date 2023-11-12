using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains {

    public class FireFountain : ProjectileFountain {

        public override void AddProjectile(GameObject projectilePrefab, GameObject attacker, float baseDamage, bool isCrit, ProcChainMask procChainMask) {
            if (!isCrit) {
                procChainMask.AddProc(ProcType.Meatball);
            }
            base.AddProjectile(projectilePrefab, attacker, baseDamage, isCrit, procChainMask);
        }

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            base.ModifyProjectile(ref info);
            if (info.crit) {
                info.procChainMask.AddProc(ProcType.Meatball);
            }
            ProjectileManager.instance.FireProjectile(info);
            info.rotation = Random.rotationUniform;
            ProjectileManager.instance.FireProjectile(info);
            info.rotation = Random.rotationUniform;
        }
    }
}