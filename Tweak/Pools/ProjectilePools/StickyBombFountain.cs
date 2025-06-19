using BTP.RoR2Plugin.Tweaks;
using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using UnityEngine;

namespace BTP.RoR2Plugin.Pools.ProjectilePools {

    internal sealed class StickyBombFountain : ProjectileFountain<StickyBombFountain> {
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