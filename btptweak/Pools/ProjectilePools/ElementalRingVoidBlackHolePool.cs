using RoR2.Projectile;
using RoR2;
using UnityEngine;

namespace BtpTweak.Pools.ProjectilePools {

    internal class ElementalRingVoidBlackHolePool : Pool<ElementalRingVoidBlackHolePool, ProjectilePoolKey, ProjectileInfo> {
        protected override float Interval => 1f;

        public void AddProjectile(in ProjectilePoolKey simpleProjectileInfo, in Vector3 position, float damageValue) {
            if (pool.TryGetValue(simpleProjectileInfo, out var projectileInfo)) {
                projectileInfo.info.position = position;
                projectileInfo.info.damage += damageValue;
            } else {
                pool.Add(simpleProjectileInfo, new() {
                    info = new FireProjectileInfo {
                        crit = simpleProjectileInfo.isCrit,
                        damage = damageValue,
                        damageColorIndex = DamageColorIndex.Void,
                        force = 6000f,
                        owner = simpleProjectileInfo.attacker,
                        position = position,
                        procChainMask = simpleProjectileInfo.procChainMask,
                        projectilePrefab = AssetReferences.elementalRingVoidBlackHole,
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