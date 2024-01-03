using BtpTweak.Tweaks.ItemTweaks;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Pools.ProjectilePools {

    internal class FireFountain : ProjectileFountain<FireFountain> {
        protected override float Interval => FireballsOnHitTweak.Interval;

        protected override GameObject ProjectilePrefab => AssetReferences.fireMeatBallProjectile;

        protected override void OnTimeOut(in ProjectilePoolKey key, in ProjectileInfo projectileInfo) {
            projectileInfo.info.procChainMask.AddProc(RoR2.ProcType.Meatball);
            base.OnTimeOut(key, projectileInfo);
            projectileInfo.info.rotation = Random.rotationUniform;
            ProjectileManager.instance.FireProjectile(projectileInfo.info);
            projectileInfo.info.rotation = Random.rotationUniform;
            ProjectileManager.instance.FireProjectile(projectileInfo.info);
        }
    }
}