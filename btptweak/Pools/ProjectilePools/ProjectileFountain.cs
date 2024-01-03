using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Pools.ProjectilePools {

    internal struct ProjectilePoolKey {
        public GameObject attacker;
        public CharacterBody targetBody;
        public ProcChainMask procChainMask;
        public bool isCrit;
    }

    internal abstract class ProjectileFountain<T> : Pool<T, ProjectilePoolKey, ProjectileInfo> where T : ProjectileFountain<T> {
        protected abstract GameObject ProjectilePrefab { get; }

        public virtual void AddProjectile(in ProjectilePoolKey simpleProjectileInfo, float damageValue) {
            if (pool.TryGetValue(simpleProjectileInfo, out var projectileInfo)) {
                projectileInfo.info.damage += damageValue;
            } else {
                pool.Add(simpleProjectileInfo, new() {
                    info = new FireProjectileInfo {
                        crit = simpleProjectileInfo.isCrit,
                        damage = damageValue,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        fuseOverride = 1f,
                        owner = simpleProjectileInfo.attacker,
                        procChainMask = simpleProjectileInfo.procChainMask,
                        projectilePrefab = ProjectilePrefab,
                        rotation = Random.rotationUniform,
                        speedOverride = Random.Range(10f, 30f),
                        target = simpleProjectileInfo.targetBody.gameObject,
                        useFuseOverride = true,
                        useSpeedOverride = true,
                    }
                });
            }
        }

        protected override void OnTimeOut(in ProjectilePoolKey key, in ProjectileInfo projectileInfo) {
            projectileInfo.info.position = key.targetBody.corePosition + key.targetBody.bestFitRadius * 0.5f * Vector3.up;
            ProjectileManager.instance.FireProjectile(projectileInfo.info);
        }
    }

    internal sealed class ProjectileInfo {
        public FireProjectileInfo info;
    }
}