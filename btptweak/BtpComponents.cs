using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class Meatball : MonoBehaviour {
        public static readonly GameObject electricOrbProjectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricOrbProjectile.prefab").WaitForCompletion();
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            if (NetworkServer.active) {
                FireMeatballs(BtpTweak.玩家等级_ / 4, 0.5f * projectileDamage.damage);
            }
        }

        public void FireMeatballs(float meatballCount, float meatbalDamage) {
            float 每球偏移角度_Y = 360f / meatballCount;
            Vector3 pos = transform.position + Vector3.up;
            Quaternion rotation;
            for (int i = 0, 随机偏移角_Y = Random.Range(0, 360); i < meatballCount; ++i) {
                rotation = Quaternion.Euler(-75, i * 每球偏移角度_Y + 随机偏移角_Y, 0);
                ProjectileManager.instance.FireProjectile(electricOrbProjectilePrefab, pos, rotation, projectileController.owner?.gameObject, meatbalDamage, projectileDamage.force, projectileDamage.crit);
            }
        }
    }

    internal class IceExplosion : MonoBehaviour {
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public float explosionRadius = 6;

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
            GameObject iceExplosion = Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast"), transform.position, Quaternion.identity);
            if (projectileDamage.crit) {
                explosionRadius *= 2;
            }
            iceExplosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, explosionRadius);
            DelayBlast delayBlast = iceExplosion.GetComponent<DelayBlast>();
            if (delayBlast) {
                delayBlast.position = transform.position;
                delayBlast.baseDamage = 0.5f * projectileDamage.damage;
                delayBlast.baseForce = projectileDamage.force;
                delayBlast.attacker = projectileController.owner?.gameObject;
                delayBlast.radius = explosionRadius;
                delayBlast.crit = projectileDamage.crit;
                delayBlast.procCoefficient = 0.5f;
                delayBlast.maxTimer = 2f;
                delayBlast.falloffModel = BlastAttack.FalloffModel.None;
                delayBlast.explosionEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/AffixWhiteExplosion");
                delayBlast.delayEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/AffixWhiteDelayEffect");
                delayBlast.damageType = DamageType.Freeze2s;
                TeamFilter teamFilter = iceExplosion.GetComponent<TeamFilter>();
                if (teamFilter) {
                    teamFilter.teamIndex = projectileController.teamFilter.teamIndex;
                }
            }
        }
    }

    internal class CallAirstrike : MonoBehaviour {
        public static readonly GameObject captainAirstrikePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Captain/CaptainAirstrikeProjectile1.prefab").WaitForCompletion();
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