using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains {

    public class LightningFountain : ProjectileFountain {
        protected override GameObject ProjectilePrefab => AssetReferences.electricOrbProjectile;

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            info.procChainMask.AddProc(RoR2.ProcType.LightningStrikeOnHit);
        }
    }
}