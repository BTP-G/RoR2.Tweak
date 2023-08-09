using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace BtpTweak {

    public class 爆炸产生闪电链 : MonoBehaviour {
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            if (NetworkServer.active) {
                LightningOrb lightningOrb2 = new LightningOrb();
                lightningOrb2.attacker = projectileController.owner?.gameObject;
                lightningOrb2.bouncedObjects = new List<HealthComponent>();
                lightningOrb2.bouncesRemaining = 30;
                lightningOrb2.damageColorIndex = DamageColorIndex.Default;
                lightningOrb2.damageType = projectileDamage.damageType;
                lightningOrb2.damageValue = projectileDamage.damage;
                lightningOrb2.isCrit = projectileDamage.crit;
                lightningOrb2.lightningType = LightningOrb.LightningType.Ukulele;
                lightningOrb2.origin = transform.position;
                lightningOrb2.procChainMask = default;
                lightningOrb2.procChainMask.AddProc(ProcType.ChainLightning);
                lightningOrb2.procCoefficient = 1f;
                lightningOrb2.range = 30;
                lightningOrb2.teamIndex = projectileController.teamFilter.teamIndex;
                HurtBox hurtBox = lightningOrb2.PickNextTarget(transform.position);
                if (hurtBox) {
                    lightningOrb2.target = hurtBox;
                    OrbManager.instance.AddOrb(lightningOrb2);
                }
            }
        }
    }

    public class 爆炸发射闪电球 : MonoBehaviour {
        public static readonly GameObject electricOrbProjectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricOrbProjectile.prefab").WaitForCompletion();
        public ProjectileController projectileController;
        public ProjectileDamage projectileDamage;
        public int meatballCount = 0;

        public void Awake() {
            projectileController = GetComponent<ProjectileController>();
            projectileDamage = GetComponent<ProjectileDamage>();
        }

        public void OnDestroy() {
            if (NetworkServer.active) {
                FireMeatballs(meatballCount, -75, 0.5f * projectileDamage.damage);
            }
        }

        private void FireMeatballs(float meatballCount, float angle_x, float meatbalDamage) {
            float 每球偏移角度_Y = 360f / meatballCount;
            Vector3 pos = transform.position + Vector3.up;
            Quaternion rotation;
            for (int i = 0, 随机偏移角_Y = Random.Range(0, 360); i < meatballCount; ++i) {
                rotation = Quaternion.Euler(angle_x, i * 每球偏移角度_Y + 随机偏移角_Y, 0);
                ProjectileManager.instance.FireProjectile(electricOrbProjectilePrefab, pos, rotation, projectileController.owner?.gameObject, meatbalDamage, projectileDamage.force, projectileDamage.crit);
            }
        }
    }

    public class 爆炸产生冰冻炸弹 : MonoBehaviour {
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
            GameObject iceExplosion = Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast"), transform.position, Quaternion.identity);
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

    public class 爆炸呼叫顺风号打击 : MonoBehaviour {
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

    public abstract class 自动寻敌 : MonoBehaviour {
        public ProjectileTargetComponent targetComponent;
        public float fixedAge = 0;

        public void Start() {
            targetComponent = GetComponent<ProjectileTargetComponent>();
        }

        public void FixedUpdate() {
            fixedAge += Time.fixedDeltaTime;
        }
    }

    public class 跟随目标 : 自动寻敌 {
        public float speed = 10;

        public new void FixedUpdate() {
            base.FixedUpdate();
            if (targetComponent.target) {
                transform.position = Vector3.MoveTowards(transform.position, targetComponent.target.position, Time.fixedDeltaTime * speed);
            }
        }
    }

    public class 粘住目标 : 自动寻敌 {
        public float TPInterval = 0.5f;

        public new void FixedUpdate() {
            base.FixedUpdate();
            if (targetComponent.target) {
                if (fixedAge > TPInterval) {
                    fixedAge = 0;
                    transform.SetPositionAndRotation(targetComponent.target.position, targetComponent.target.rotation);
                }
            }
        }
    }
}