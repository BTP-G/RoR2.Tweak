using RoR2.Projectile;
using RoR2;
using UnityEngine;
using BtpTweak.Tweaks.ItemTweaks;

namespace BtpTweak.Pools.ProjectilePools {

    internal sealed class FireTornadoPool : Pool<FireTornadoPool, ProjectilePoolKey, ProjectileInfo> {
        protected override float Interval => RingsTweak.FireRingInterval;

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
                        damageTypeOverride = DamageType.IgniteOnHit,
                        owner = simpleProjectileInfo.attacker,
                        position = position,
                        procChainMask = simpleProjectileInfo.procChainMask,
                        projectilePrefab = AssetReferences.fireTornado,
                        rotation = Quaternion.identity,
                    }
                });
            }
        }

        protected override void OnTimeOut(in ProjectilePoolKey key, in ProjectileInfo projectileInfo) {
            projectileInfo.info.procChainMask.AddProc(ProcType.Rings);
            ProjectileManager.instance.FireProjectile(projectileInfo.info);
        }
    }
}