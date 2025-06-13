using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BTP.RoR2Plugin.Pools {

    internal struct StunAndPiercePoolKey {
        public bool isCrit;
        public ProcChainMask procChainMask;
    }

    internal sealed class StunAndPiercePool : Pool<StunAndPiercePool, StunAndPiercePoolKey, StunAndPiercePool.ProjectileInfo> {
        protected override float Interval => StunAndPierceTweak.Interval;

        public void AddProjectile(in StunAndPiercePoolKey key, CharacterBody attackerBody, float damageValue) {
            if (pool.TryGetValue(key, out var projectileInfo)) {
                projectileInfo.info.damage += damageValue;
            } else {
                pool.Add(key, new() {
                    info = new FireProjectileInfo {
                        crit = key.isCrit,
                        damage = damageValue,
                        damageColorIndex = DamageColorIndex.Item,
                        owner = Owner,
                        procChainMask = key.procChainMask,
                        projectilePrefab = AssetReferences.stunAndPierceBoomerang,
                    },
                    attackerBody = attackerBody,
                });
            }
        }

        protected override void OnTimeOut(in StunAndPiercePoolKey key, in ProjectileInfo value) {
            value.info.position = value.attackerBody.inputBank.aimOrigin;
            value.info.rotation = Quaternion.LookRotation(value.attackerBody.inputBank.aimDirection);
            ProjectileManager.instance.FireProjectile(value.info);
        }

        internal class ProjectileInfo {
            public FireProjectileInfo info;
            public CharacterBody attackerBody;
        }
    }
}