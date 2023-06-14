using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak {

    internal class FireMeatball : MonoBehaviour {
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            FireMeatballs(transform.position + Vector3.up, BtpTweak.玩家角色等级_ / 3, -75, 1000);
        }

        public void FireMeatballs(Vector3 impactPosition, float meatballCount, float meatballAngle, float meatballForce) {
            float num = 360f / meatballCount;
            for (int i = 0, 偏移角 = Random.Range(0, 360); i < meatballCount; ++i) {
                Quaternion rotation = Quaternion.Euler(meatballAngle, i * num + 偏移角, 0);
                ProjectileManager.instance.FireProjectile(BtpTweak.electricOrbProjectilePrefab, impactPosition, rotation, projectileController.owner?.gameObject, projectileDamage.damage * 0.5f, meatballForce, projectileDamage.crit);
            }
        }
    }
}