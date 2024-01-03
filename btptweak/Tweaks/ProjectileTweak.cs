using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.BrotherMonster;
using EntityStates.BrotherMonster.Weapon;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class ProjectileTweak : TweakBase<ProjectileTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var daggerProjectile = GameObjectPaths.DaggerProjectile.Load<GameObject>();
            daggerProjectile.GetComponent<ProjectileController>().procCoefficient = 0.33f;
            daggerProjectile.GetComponent<ProjectileSimple>().lifetime = 10f;
            GameObjectPaths.DeathProjectile18.LoadComponent<ProjectileController>().procCoefficient = 0;
            WeaponSlam.pillarProjectilePrefab.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            WeaponSlam.pillarProjectilePrefab.AddComponent<BrotherPillarProjectileAwakeAction>();
            EditLunarMissile();
            EditLunarShards();
        }

        private void EditLunarMissile() {
            var lunarMissilePrefab = GameObjectPaths.LunarMissileProjectile.Load<GameObject>();
            var lunarMissileSteer = lunarMissilePrefab.GetComponent<ProjectileSteerTowardTarget>();
            lunarMissileSteer.rotationSpeed = 150f;
            lunarMissilePrefab.GetComponent<ProjectileDamage>().damageType |= DamageType.BypassArmor;
            var lunarMissileTargetFinder = lunarMissilePrefab.GetComponent<ProjectileDirectionalTargetFinder>();
            lunarMissileTargetFinder.lookRange = 60f;
            lunarMissileTargetFinder.lookCone = 120f;
            lunarMissileTargetFinder.allowTargetLoss = false;
        }

        private void EditLunarShards() {
            var lunarShardSteer = FireLunarShards.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>();
            lunarShardSteer.rotationSpeed = 60f;  // vanilla 20
            var lunarShardTargetFinder = FireLunarShards.projectilePrefab.GetComponent<ProjectileDirectionalTargetFinder>();
            lunarShardTargetFinder.lookRange = 90f;  // vanilla 80
            lunarShardTargetFinder.lookCone = 90f;
            lunarShardTargetFinder.allowTargetLoss = true;
        }

        private class BrotherPillarProjectileAwakeAction : MonoBehaviour {

            private void Awake() {
                if (PhaseCounter.instance && PhaseCounter.instance.phase < 4) {
                    transform.localScale = Vector3.one * (PhaseCounter.instance.phase + 1);
                }
            }
        }
    }
}