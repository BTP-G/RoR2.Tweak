using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains {

    public class StickyBombFountain : ProjectileFountain {
        protected override GameObject ProjectilePrefab => AssetReferences.stickyBombProjectile;

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            info.procChainMask.AddProc(RoR2.ProcType.Count);
            info.useFuseOverride = false;
            info.speedOverride *= 0.33f;
        }
    }
}