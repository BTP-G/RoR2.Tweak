using BtpTweak.Tweaks;
using BtpTweak.Tweaks.ItemTweaks;
using UnityEngine;

namespace BtpTweak.Pools.ProjectilePools {

    internal class StickyBombFountain : ProjectileFountain<StickyBombFountain> {
        protected override GameObject ProjectilePrefab => AssetReferences.stickyBombProjectile;

        protected override float Interval => StickyBombTweak.Interval;

        protected override void OnTimeOut(in ProjectilePoolKey key, in ProjectileInfo projectileInfo) {
            projectileInfo.info.procChainMask.AddProc(ProcChainTweak.StickyBombOnHit);
            projectileInfo.info.useFuseOverride = false;
            projectileInfo.info.speedOverride *= 0.33f;
            base.OnTimeOut(key, projectileInfo);
        }
    }
}