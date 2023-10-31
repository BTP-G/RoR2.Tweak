using BtpTweak.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains {

    public class StickyBombFountain : ProjectileFountain {

        public override void AddProjectile(GameObject projectilePrefab, GameObject attacker, float baseDamage, bool isCrit, ProcChainMask procChainMask) {
            if (!isCrit) {
                procChainMask.AddProc(ProcType.Count);
            }
            base.AddProjectile(projectilePrefab, attacker, baseDamage, isCrit, procChainMask);
        }

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            base.ModifyProjectile(ref info);
            info.useFuseOverride = false;
            info.speedOverride *= 0.33f;
            if (info.crit) {
                info.procChainMask.AddProc(ProcType.Count);
            }
        }
    }
}