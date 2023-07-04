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
            GameObject iceExplosion = Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast"), transform.position, Quaternion.identity);
            iceExplosion.transform.localScale = new Vector3(SkillHook.iceExplosionRadius, SkillHook.iceExplosionRadius, SkillHook.iceExplosionRadius);
            DelayBlast delayBlast = iceExplosion.GetComponent<DelayBlast>();
            if (delayBlast) {
                delayBlast.position = transform.position;
                delayBlast.baseDamage = projectileDamage.damage;
                delayBlast.baseForce = 1000f;
                delayBlast.attacker = projectileController.owner?.gameObject;
                delayBlast.radius = SkillHook.iceExplosionRadius;
                delayBlast.crit = projectileDamage.crit;
                delayBlast.procCoefficient = 0.75f;
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