using RoR2;
using RoR2.Projectile;
using TPDespair.ZetAspects;
using UnityEngine;

namespace BTP.RoR2Plugin.Pools.ProjectilePools {

    internal sealed class LightningStakePool : Pool<LightningStakePool, ProjectilePoolKey, ProjectileInfo> {
        protected override float Interval => 0.1f;

        public void AddProjectile(in ProjectilePoolKey simpleProjectileInfo, in Vector3 position, float damageValue) {
            if (pool.TryGetValue(simpleProjectileInfo, out var projectileInfo)) {
                projectileInfo.info.position = position;
                projectileInfo.info.damage += damageValue;
            } else {
                pool.Add(simpleProjectileInfo, new() {
                    info = new FireProjectileInfo {
                        crit = simpleProjectileInfo.isCrit,
                        damage = damageValue,
                        damageColorIndex = DamageColorIndex.Item,
                        fuseOverride = Configuration.AspectBlueBombDuration.Value,
                        owner = simpleProjectileInfo.attacker,
                        position = position,
                        procChainMask = simpleProjectileInfo.procChainMask,
                        projectilePrefab = AssetReferences.lightningStake,
                        useFuseOverride = true,
                    }
                });
            }
        }

        protected override void OnTimeOut(in ProjectilePoolKey key, in ProjectileInfo projectileInfo) {
            ProjectileManager.instance.FireProjectile(projectileInfo.info);
        }
    }
}