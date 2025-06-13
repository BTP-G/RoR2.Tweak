using EntityStates;
using EntityStates.LaserTurbine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RoR2.BulletAttack;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class LaserTurbineTweak : TweakBase<LaserTurbineTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float MainBeamDamageCoefficient = 1f;
        public const float SecondBombDamageCoefficient = 3f;
        public const int ChargeDuration = 10;
        public const int ChargesRequired = 0;
        public const float ChargeCoefficient = 0.01f;
        public const float ChargeCoefficientPerKill = 0.01f;

        private static readonly BullseyeSearch _search = new() {
            sortMode = BullseyeSearch.SortMode.Distance,
            maxDistanceFilter = float.MaxValue,
        };

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.LaserTurbineController.FixedUpdate += LaserTurbineController_FixedUpdate;
            On.RoR2.LaserTurbineController.OnCharacterDeathGlobalServer += LaserTurbineController_OnCharacterDeathGlobalServer;
            On.EntityStates.LaserTurbine.RechargeState.FixedUpdate += RechargeState_FixedUpdate;
            IL.EntityStates.LaserTurbine.AimState.OnEnter += AimState_OnEnter;
            On.EntityStates.LaserTurbine.FireMainBeamState.FireBeamServer += FireMainBeamState_FireBeamServer;
            On.EntityStates.LaserTurbine.FireMainBeamState.OnExit += FireMainBeamState_OnExit;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RechargeState.killChargeDuration = ChargeDuration;
            RechargeState.killChargesRequired = ChargesRequired;
            AimState.targetAcquisitionRadius = float.MaxValue;
            ReadyState.baseDuration = 0.5f;
            FireMainBeamState.baseDuration = 0.1f;
            FireMainBeamState.mainBeamDamageCoefficient = MainBeamDamageCoefficient;
            FireMainBeamState.secondBombDamageCoefficient = SecondBombDamageCoefficient;
        }

        private void LaserTurbineController_FixedUpdate(On.RoR2.LaserTurbineController.orig_FixedUpdate orig, LaserTurbineController self) {
            if (self.cachedOwnerBody) {
                self.charge = Mathf.Clamp01(self.charge
                    + (ChargeCoefficient * self.cachedOwnerBody.inventory.GetItemCount(RoR2Content.Items.LaserTurbine.itemIndex)
                        + ChargeCoefficientPerKill * self.cachedOwnerBody.GetBuffCount(RoR2Content.Buffs.LaserTurbineKillCharge))
                    * Time.fixedDeltaTime);
            } else {
                self.charge = Mathf.Clamp01(self.charge + ChargeCoefficient * Time.fixedDeltaTime);
            }
            if (self.turbineDisplayRoot) {
                self.turbineDisplayRoot.gameObject.SetActive(self.showTurbineDisplay);
            }
        }

        private void LaserTurbineController_OnCharacterDeathGlobalServer(On.RoR2.LaserTurbineController.orig_OnCharacterDeathGlobalServer orig, LaserTurbineController self, DamageReport damageReport) {
            if (damageReport.attackerBody == self.cachedOwnerBody && self.cachedOwnerBody) {
                self.cachedOwnerBody.AddTimedBuff(RoR2Content.Buffs.LaserTurbineKillCharge, ChargeDuration);
            }
        }

        private void RechargeState_FixedUpdate(On.EntityStates.LaserTurbine.RechargeState.orig_FixedUpdate orig, RechargeState self) {
            self.fixedAge += Time.fixedDeltaTime;
            if (self.isAuthority && self.laserTurbineController.charge == 1) {
                self.outer.SetNextState(new ReadyState());
            }
        }

        private void AimState_OnEnter(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After, c => c.MatchCall<EntityState>("get_isAuthority"))) {
                cursor.Emit(OpCodes.Ldarg_0)
                      .EmitDelegate((bool isAuthority, AimState aimState) => {
                          if (isAuthority) {
                              _search.searchOrigin = aimState.transform.position;
                              _search.teamMaskFilter = TeamMask.all;
                              _search.viewer = aimState.ownerBody;
                              _search.RefreshCandidates();
                              _search.FilterOutGameObject(aimState.ownerBody.gameObject);
                              var targetHurtBox = _search.GetResults().FirstOrDefault();
                              if (targetHurtBox) {
                                  aimState.simpleRotateToDirection.targetRotation = Quaternion.LookRotation(targetHurtBox.transform.position - _search.searchOrigin);
                                  aimState.foundTarget = true;
                              }
                          }
                      });
                cursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError(GetType().FullName + " add hook 'AimState_OnEnter' failed!");
            }
        }

        private void FireMainBeamState_FireBeamServer(On.EntityStates.LaserTurbine.FireMainBeamState.orig_FireBeamServer orig, FireMainBeamState self, Ray aimRay, GameObject tracerEffectPrefab, float maxDistance, bool isInitialBeam) {
            if (isInitialBeam) {
                var laserTurbineBounceOrb = new LaserTurbineBounceOrb {
                    bouncedObjects = [self.ownerBody.healthComponent],
                    currentViewerBody = self.ownerBody,
                    damageValue = self.GetDamage(),
                    duration = 0.1f,
                    isCrit = self.isCrit,
                    origin = aimRay.origin,
                    ownerBody = self.ownerBody,
                    weapon = self.gameObject,
                };
                if (laserTurbineBounceOrb.target = laserTurbineBounceOrb.PickNextTarget(laserTurbineBounceOrb.origin)) {
                    laserTurbineBounceOrb.secondProjectileInfo = new() {
                        crit = laserTurbineBounceOrb.isCrit,
                        damage = laserTurbineBounceOrb.damageValue * SecondBombDamageCoefficient,
                        damageColorIndex = DamageColorIndex.Item,
                        fuseOverride = 0f,
                        owner = laserTurbineBounceOrb.ownerBody.gameObject,
                        projectilePrefab = FireMainBeamState.secondBombPrefab,
                        rotation = Quaternion.identity,
                        useFuseOverride = true,
                    };
                    OrbManager.instance.AddOrb(laserTurbineBounceOrb);
                    self.laserTurbineController.ExpendCharge();
                }
            }
        }

        private void FireMainBeamState_OnExit(On.EntityStates.LaserTurbine.FireMainBeamState.orig_OnExit orig, FireMainBeamState self) {
            orig(self);
            self.laserTurbineController.charge = 0f;
        }

        public class LaserTurbineBounceOrb : Orb {
            public bool isCrit;
            public CharacterBody currentViewerBody;
            public CharacterBody ownerBody;
            public float damageValue;
            public GameObject weapon;
            public List<HealthComponent> bouncedObjects;
            public FireProjectileInfo secondProjectileInfo;

            private static readonly BulletAttack bulletAttack = new() {
                bulletCount = 1U,
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.Generic,
                falloffModel = FalloffModel.None,
                force = FireMainBeamState.mainBeamForce,
                HitEffectNormal = false,
                hitEffectPrefab = FireMainBeamState.mainBeamImpactEffect,
                hitMask = LayerIndex.entityPrecise.mask,
                maxSpread = 0f,
                minSpread = 0f,
                muzzleName = "",
                procChainMask = default,
                procCoefficient = FireMainBeamState.mainBeamProcCoefficient,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                radius = FireMainBeamState.mainBeamRadius,
                smartCollision = true,
                sniper = false,
                spreadPitchScale = 1f,
                spreadYawScale = 1f,
                stopperMask = LayerIndex.world.mask,
            };

            public HurtBox PickNextTarget(Vector3 position) {
                _search.searchOrigin = position;
                _search.teamMaskFilter = TeamMask.all;
                _search.viewer = currentViewerBody;
                _search.RefreshCandidates();
                var targetHurtBox = _search.GetResults().Where((h) => !bouncedObjects.Contains(h.healthComponent)).FirstOrDefault();
                if (targetHurtBox) {
                    bouncedObjects.Add(targetHurtBox.healthComponent);
                }
                return targetHurtBox;
            }

            public override void OnArrival() {
                base.OnArrival();
                if (!target || !target.healthComponent) {
                    return;
                }
                var targetPosition = target.healthComponent.body.corePosition;
                bulletAttack.aimVector = (targetPosition - origin).normalized;
                bulletAttack.damage = damageValue * MainBeamDamageCoefficient;
                bulletAttack.isCrit = isCrit;
                bulletAttack.maxDistance = Vector3.Distance(origin, targetPosition) + target.healthComponent.body.bestFitRadius;
                bulletAttack.origin = origin;
                bulletAttack.owner = ownerBody ? ownerBody.gameObject : null;
                bulletAttack.weapon = weapon;
                bulletAttack.modifyOutgoingDamageCallback = OnBulletAttackHit;
                EffectManager.SpawnEffect(FireMainBeamState.forwardBeamTracerEffect, new EffectData {
                    origin = targetPosition,
                    start = origin,
                }, true);
                bulletAttack.Fire();
                currentViewerBody = target.healthComponent.body;
                if (target = PickNextTarget(targetPosition)) {
                    origin = targetPosition;
                    OrbManager.instance.AddOrb(this);
                } else {
                    bouncedObjects.Clear();
                }
            }

            private void OnBulletAttackHit(BulletAttack bulletAttack, ref BulletHit hitInfo, DamageInfo damageInfo) {
                secondProjectileInfo.position = hitInfo.point;
                ProjectileManager.instance.FireProjectile(secondProjectileInfo);
            }
        }
    }
}