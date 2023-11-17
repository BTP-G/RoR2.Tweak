using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.ProjectileFountains {

    public class FireFountain : ProjectileFountain {
        protected override GameObject ProjectilePrefab => AssetReferences.fireMeatBallProjectile;

        protected override void ModifyProjectile(ref FireProjectileInfo info) {
            info.procChainMask.AddProc(RoR2.ProcType.Meatball);
            ProjectileManager.instance.FireProjectile(info);
            info.rotation = Random.rotationUniform;
            ProjectileManager.instance.FireProjectile(info);
            info.rotation = Random.rotationUniform;
        }
    }
}