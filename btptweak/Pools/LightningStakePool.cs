using BtpTweak.ProjectileFountains;
using RoR2;
using RoR2.Projectile;
using TPDespair.ZetAspects;
using UnityEngine;

namespace BtpTweak.Pools {

    internal class LightningStakePool : Pool<LightningStakePool, SimpleProjectileInfo, ProjectileInfo> {
        protected override float Interval => 0.1f;

        public void AddProjectile(in SimpleProjectileInfo simpleProjectileInfo, in Vector3 position, float damageValue) {
            if (pool.TryGetValue(simpleProjectileInfo, out var projectileInfo)) {
                projectileInfo.fireProjectileInfo.position = position;
                projectileInfo.fireProjectileInfo.damage += damageValue;
            } else {
                pool.Add(simpleProjectileInfo, new() {
                    fireProjectileInfo = new FireProjectileInfo {
                        crit = simpleProjectileInfo.isCrit,
                        damage = damageValue,
                        damageColorIndex = DamageColorIndex.Item,
                        fuseOverride = Configuration.AspectBlueBombDuration.Value,
                        owner = simpleProjectileInfo.attacker,
                        position = position,
                        projectilePrefab = AssetReferences.lightningStake,
                        useFuseOverride = true,
                    }
                });
            }
        }

        protected override void OnTimeOut(ProjectileInfo value) {
            ProjectileManager.instance.FireProjectile(value.fireProjectileInfo);
        }
    }
}