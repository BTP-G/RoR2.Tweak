using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains {

    public class LightningFountain : ProjectileFountain {

        public override void AddProjectile(GameObject projectilePrefab, GameObject attacker, float baseDamage, bool isCrit, ProcChainMask procChainMask) {
            if (!isCrit) {
                procChainMask.AddProc(ProcType.LightningStrikeOnHit);
            }
            base.AddProjectile(projectilePrefab, attacker, baseDamage, isCrit, procChainMask);
        }

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            base.ModifyProjectile(ref info);
            if (info.crit) {
                info.procChainMask.AddProc(ProcType.LightningStrikeOnHit);
            }
        }
    }
}