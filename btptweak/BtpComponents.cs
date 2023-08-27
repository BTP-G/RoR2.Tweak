using BtpTweak.Utils;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    [RequireComponent(typeof(ProjectileController))]
    public class ProjectileImpactActionCaller : MonoBehaviour, IProjectileImpactBehavior {

        public void Awake() {
            if (!NetworkServer.active) {
                enabled = false;
                return;
            }
            action = ProjectileCatalog.GetProjectilePrefab(projectileIndex).GetComponent<ProjectileImpactActionCaller>().action;
        }

        public void OnProjectileImpact(ProjectileImpactInfo impactInfo) {
            if (action != null) {
                action(this, impactInfo);
            } else {
                Main.logger_.LogWarning(gameObject + " not bind any Projectile Impact Action.");
            }
        }

        public void BindAction(Action<ProjectileImpactActionCaller, ProjectileImpactInfo> actionWaitForAdd) {
            action = actionWaitForAdd;
            projectileIndex = ProjectileCatalog.GetProjectileIndex(gameObject);
            Helpers.Log("Bind action to projectile：" + gameObject + "->" + actionWaitForAdd.GetMethodName());
        }

        [SerializeField]
        protected int projectileIndex;

        protected Action<ProjectileImpactActionCaller, ProjectileImpactInfo> action;
    }

    public class SuperLightningOrb : LightningOrb {

        public override void OnArrival() {
            if (target && target.healthComponent) {
                HealthComponent healthComponent = target.healthComponent;
                if (bouncesRemaining > 0) {
                    for (int i = 0; i < targetsToFindPerBounce; i++) {
                        if (bouncedObjects != null) {
                            if (canBounceOnSameTarget) {
                                bouncedObjects.Clear();
                            }
                            bouncedObjects.Add(healthComponent);
                        }
                        HurtBox hurtBox = PickNextTarget(target.transform.position);
                        if (hurtBox) {
                            SuperLightningOrb lightningOrb = new() {
                                search = search,
                                origin = target.transform.position,
                                target = hurtBox,
                                attacker = attacker,
                                inflictor = inflictor,
                                teamIndex = teamIndex,
                                damageValue = damageValue * damageCoefficientPerBounce,
                                bouncesRemaining = bouncesRemaining - 1,
                                isCrit = isCrit,
                                bouncedObjects = bouncedObjects,
                                lightningType = lightningType,
                                procChainMask = procChainMask,
                                procCoefficient = procCoefficient,
                                damageColorIndex = damageColorIndex,
                                damageCoefficientPerBounce = damageCoefficientPerBounce,
                                speed = speed,
                                range = range,
                                damageType = damageType,
                                failedToKill = failedToKill
                            };
                            OrbManager.instance.AddOrb(lightningOrb);
                        }
                    }
                }
            }
        }
    }

    public class 莉莉丝打击追踪 : MonoBehaviour {
        public ProjectileTargetComponent targetComponent;
        public float speed = 0;

        public void Start() {
            if (!NetworkServer.active) {
                enabled = false;
                return;
            }
            targetComponent = GetComponent<ProjectileTargetComponent>();
            CharacterBody owner = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>();
            if (owner != null) {
                speed = owner.level + owner.inventory.GetItemCount(SkillHook.古代权杖_);
            }
        }

        public void FixedUpdate() {
            if (targetComponent.target) {
                transform.position = Vector3.MoveTowards(transform.position, targetComponent.target.position, Time.fixedDeltaTime * speed);
            }
        }
    }

    public class 火雨传送 : MonoBehaviour {
        public ProjectileTargetComponent targetComponent;
        public float TPtimer = 0;

        public void Start() {
            if (!NetworkServer.active) {
                enabled = false;
                return;
            }
            targetComponent = GetComponent<ProjectileTargetComponent>();
        }

        public void FixedUpdate() {
            TPtimer += Time.fixedDeltaTime;
            if (targetComponent.target && TPtimer > 0.5f) {
                transform.SetPositionAndRotation(targetComponent.target.position, targetComponent.target.rotation);
                TPtimer = 0;
            }
        }
    }
}