using BtpTweak.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Pools {

    internal struct MissilePoolKey {
        public bool isCrit;
        public CharacterBody attackerBody;
        public GameObject missilePrefab;
        public GameObject target;
        public ProcChainMask procChainMask;
    }

    internal sealed class MissilePool : Pool<MissilePool, MissilePoolKey, MissilePool.MissileInfo> {
        protected override float Interval => MissileTweak.Interval;

        public void AddMissile(in MissilePoolKey missileInfo, float damageValue) {
            if (pool.TryGetValue(missileInfo, out var missile)) {
                missile.damage += damageValue;
                if (!missile.target) {
                    missile.target = missileInfo.target;
                }
            } else {
                pool.Add(missileInfo, new() {
                    damage = damageValue,
                    target = missileInfo.target,
                });
            }
        }

        protected override void OnTimeOut(in MissilePoolKey info1, in MissileInfo info2) {
            var attackerBody = info1.attackerBody;
            var fireProjectileInfo = new FireProjectileInfo {
                crit = info1.isCrit,
                damage = info2.damage,
                damageColorIndex = DamageColorIndex.Item,
                force = 0,
                owner = attackerBody.gameObject,
                position = attackerBody.corePosition,
                procChainMask = info1.procChainMask,
                projectilePrefab = info1.missilePrefab,
                target = info2.target,
            };
            fireProjectileInfo.procChainMask.AddProc(ProcType.Missile);
            var itemCount = attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile);
            if (itemCount > 0) {
                fireProjectileInfo.damage *= (itemCount + 1) * 0.5f;
                var axis = attackerBody.inputBank.aimDirection;
                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(45f, axis) * Vector3.up);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(-45f, axis) * Vector3.up);
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(Vector3.up);
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }

        internal class MissileInfo {
            public GameObject target;
            public float damage;
        }
    }
}