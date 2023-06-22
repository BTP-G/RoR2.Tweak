using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class Meatball : MonoBehaviour {
        private static readonly GameObject electricOrbProjectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricOrbProjectile.prefab").WaitForCompletion();
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            if (NetworkServer.active) {
                FireMeatballs(transform.position + Vector3.up, BtpTweak.玩家等级_ / 3, -75, 1000);
            }
        }

        public void FireMeatballs(Vector3 impactPosition, float meatballCount, float meatballAngle, float meatballForce) {
            float 每球偏移角度_Y = 360f / meatballCount;
            Quaternion rotation;
            for (int i = 0, 随机偏移角_Y = Random.Range(0, 360); i < meatballCount; ++i) {
                rotation = Quaternion.Euler(meatballAngle, i * 每球偏移角度_Y + 随机偏移角_Y, 0);
                ProjectileManager.instance.FireProjectile(electricOrbProjectilePrefab, impactPosition, rotation, projectileController.owner?.gameObject, projectileDamage.damage * 0.5f, meatballForce, projectileDamage.crit);
            }
        }
    }

    internal class IceExplosion : MonoBehaviour {
        private static readonly GameObject whiteExplosionPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/AffixWhiteExplosion");
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            if (NetworkServer.active) {
                Explosion();
            }
        }

        private void Explosion() {
            BlastAttack blastAttack = new BlastAttack {
                attacker = projectileController.owner?.gameObject,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = projectileDamage.damage,
                baseForce = 0f,
                bonusForce = Vector3.zero,
                canRejectForce = true,
                crit = projectileDamage.crit,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Freeze2s,
                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                inflictor = projectileController.owner?.gameObject,
                losType = BlastAttack.LoSType.NearestHit,
                position = transform.position,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1f,
                radius = SkillHook.iceExplosionRadius,
                teamIndex = projectileController.teamFilter.teamIndex
            };
            EffectData effectData = new EffectData {
                origin = blastAttack.position,
                scale = blastAttack.radius
            };
            EffectManager.SpawnEffect(whiteExplosionPrefab, effectData, true);
            blastAttack.Fire();
        }
    }

    internal class CallAirstrike : MonoBehaviour {
        private static readonly GameObject captainAirstrikePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Captain/CaptainAirstrikeProjectile1.prefab").WaitForCompletion();
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            if (NetworkServer.active) {
                Call();
            }
        }

        private void Call() {
            ProjectileManager.instance.FireProjectile(captainAirstrikePrefab, transform.position, transform.rotation, projectileController.owner?.gameObject, projectileDamage.damage, 0, projectileDamage.crit);
        }
    }
}