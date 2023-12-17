using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.BrotherMonster;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.MithrixTweaks {

    internal class BrotherHurtTweak : TweakBase<BrotherHurtTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const int missileCountPerFire = 5;
        public const float missileSpeed = 60;
        public const float missileTurretPitchFrequency = 0.5f;
        public const float missileTurretPitchMagnitude = 20;
        public const float missileTurretYawFrequency = 0.5f;
        public const float fireInterval = 0.05f;
        private float fireTimer;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.BrotherMonster.SpellChannelState.OnExit += SpellChannelState_OnExit;
            On.EntityStates.BrotherMonster.TrueDeathState.FixedUpdate += TrueDeathState_FixedUpdate;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var component = GameObjectPaths.BrotherHurtBody.LoadComponent<CharacterBody>().AddComponent<BrotherHurtBodyComponent>();
            var meteorStormController = GameObjectPaths.MeteorStorm.LoadComponent<MeteorStormController>();
            component.warningEffectPrefab = meteorStormController.warningEffectPrefab;
            component.impactEffectPrefab = meteorStormController.impactEffectPrefab;
            component.travelEffectPrefab = meteorStormController.travelEffectPrefab;
        }

        private void SpellChannelState_OnExit(On.EntityStates.BrotherMonster.SpellChannelState.orig_OnExit orig, SpellChannelState self) {
            orig(self);
            var body = self.characterBody;
            var dotInfo = new InflictDotInfo {
                damageMultiplier = 1,
                dotIndex = DotController.DotIndex.Helfire,
                duration = float.MaxValue,
                attackerObject = body.gameObject,
                victimObject = body.gameObject,
            };
            body.SetBuffCount(DLC1Content.Buffs.ImmuneToDebuffReady.buffIndex, 0);
            DotController.InflictDot(ref dotInfo);
            body.RecalculateStats();
            var healthComponent = body.healthComponent;
            healthComponent.Networkhealth = healthComponent.fullHealth;
            healthComponent.Networkshield = healthComponent.fullShield;
        }

        private void TrueDeathState_FixedUpdate(On.EntityStates.BrotherMonster.TrueDeathState.orig_FixedUpdate orig, TrueDeathState self) {
            orig(self);
            if (!NetworkServer.active || !RunInfo.位于月球) {
                return;
            }
            if ((fireTimer -= Time.fixedDeltaTime) < 0) {
                fireTimer = fireInterval;
                var body = self.characterBody;
                var aimRay = new Ray() {
                    origin = body.footPosition,
                    direction = body.transform.forward
                };
                var projectileInfo = new FireProjectileInfo {
                    projectilePrefab = AssetReferences.lunarMissilePrefab,
                    owner = body.gameObject,
                    damage = self.damageStat * body.level,
                    useSpeedOverride = true,
                    speedOverride = missileSpeed,
                };
                for (int i = 0; i < missileCountPerFire; ++i) {
                    float bonusYaw = (float)(360.0 / missileCountPerFire * i + 360.0 * missileTurretYawFrequency * self.fixedAge);
                    Vector3 forward = Util.ApplySpread(aimRay.direction, 0.0f, 0.0f, 1f, 1f, bonusYaw, Mathf.Sin(6.283185f * missileTurretPitchFrequency * self.fixedAge) * missileTurretPitchMagnitude);
                    projectileInfo.position = aimRay.origin + ((self.fixedAge + 3) * 2 * Vector3.up);
                    projectileInfo.rotation = Util.QuaternionSafeLookRotation(forward);
                    ProjectileManager.instance.FireProjectile(projectileInfo);
                }
            }
        }

        private class BrotherHurtBodyComponent : MonoBehaviour {
            public const float blastDamageCoefficient = 6;
            public const float blastForce = 4000;
            public const float blastRadius = 8;
            public const float impactDelay = 2;
            public const float travelEffectDuration = 2;
            public const float waveInterval = 0.5f;

            public GameObject warningEffectPrefab;
            public GameObject travelEffectPrefab;
            public GameObject impactEffectPrefab;

            private CharacterBody characterBody;
            private List<MeteorStormController.Meteor> meteorList;
            private List<MeteorStormController.MeteorWave> waveList;
            private float waveTimer;

            private void Awake() {
                if (enabled = NetworkServer.active) {
                    characterBody = GetComponent<CharacterBody>();
                    meteorList = [];
                    waveList = [];
                    On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
                    On.RoR2.HealthComponent.RechargeShield += HealthComponent_RechargeShield;
                    On.RoR2.HealthComponent.AddBarrier += HealthComponent_AddBarrier;
                }
            }

            private void Start() {
                characterBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
            }

            private void FixedUpdate() {
                if (RunInfo.位于月球 && (waveTimer -= Time.fixedDeltaTime) <= 0) {
                    waveTimer = waveInterval;
                    waveList.Add(new MeteorStormController.MeteorWave(CharacterBody.readOnlyInstancesList.Where(body => body.bodyIndex != BodyIndexes.BrotherHurt).ToArray(), transform.position) {
                        hitChance = 0.1f,
                    });
                }
                for (int i = waveList.Count - 1; i >= 0; --i) {
                    var meteorWave = waveList[i];
                    meteorWave.timer -= Time.fixedDeltaTime;
                    if (meteorWave.timer <= 0f) {
                        meteorWave.timer = waveInterval;
                        var nextMeteor = meteorWave.GetNextMeteor();
                        if (nextMeteor == null) {
                            waveList.RemoveAt(i);
                        } else if (nextMeteor.valid) {
                            meteorList.Add(nextMeteor);
                            EffectManager.SpawnEffect(warningEffectPrefab, new EffectData {
                                origin = nextMeteor.impactPosition,
                                scale = blastRadius
                            }, true);
                        }
                    }
                }
                float num = Run.instance.time - impactDelay;
                float num2 = num - travelEffectDuration;
                for (int j = meteorList.Count - 1; j >= 0; j--) {
                    var meteor = meteorList[j];
                    if (meteor.startTime < num2 && !meteor.didTravelEffect) {
                        meteor.didTravelEffect = true;
                        if (travelEffectPrefab) {
                            EffectManager.SpawnEffect(travelEffectPrefab, new EffectData {
                                origin = meteor.impactPosition
                            }, true);
                        }
                    }
                    if (meteor.startTime < num) {
                        meteorList.RemoveAt(j);
                        DetonateMeteor(meteor);
                        if (UnityEngine.Random.value > 0.5f) {
                            ProjectileManager.instance.FireProjectile(AssetReferences.brotherUltLineProjectileStatic, meteor.impactPosition, Quaternion.Euler(0, UnityEngine.Random.Range(0f, 180f), 0), gameObject, characterBody.damage * UltChannelState.waveProjectileDamageCoefficient, UltChannelState.waveProjectileForce, characterBody.RollCrit());
                        } else {
                            ProjectileManager.instance.FireProjectile(WeaponSlam.pillarProjectilePrefab, meteor.impactPosition, Quaternion.identity, gameObject, characterBody.damage * WeaponSlam.pillarDamageCoefficient, 0f, characterBody.RollCrit());
                        }
                    }
                }
            }

            private void OnDestroy() {
                if (NetworkServer.active) {
                    On.RoR2.HealthComponent.Heal -= HealthComponent_Heal;
                    On.RoR2.HealthComponent.RechargeShield -= HealthComponent_RechargeShield;
                    On.RoR2.HealthComponent.AddBarrier -= HealthComponent_AddBarrier;
                }
            }

            private void DetonateMeteor(MeteorStormController.Meteor meteor) {
                var effectData = new EffectData {
                    origin = meteor.impactPosition
                };
                EffectManager.SpawnEffect(impactEffectPrefab, effectData, true);
                new BlastAttack {
                    baseDamage = blastDamageCoefficient * characterBody.damage,
                    baseForce = blastForce,
                    attackerFiltering = AttackerFiltering.Default,
                    crit = characterBody.RollCrit(),
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    attacker = gameObject,
                    bonusForce = Vector3.zero,
                    damageColorIndex = DamageColorIndex.Default,
                    position = meteor.impactPosition,
                    procChainMask = default,
                    procCoefficient = 1f,
                    teamIndex = characterBody.teamComponent.teamIndex,
                    radius = blastRadius
                }.Fire();
            }

            private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen) {
                if (self.body.bodyIndex == BodyIndexes.BrotherHurt) {
                    return 0f;
                }
                return orig(self, amount, procChainMask, nonRegen);
            }

            private void HealthComponent_RechargeShield(On.RoR2.HealthComponent.orig_RechargeShield orig, HealthComponent self, float value) {
                if (self.body.bodyIndex == BodyIndexes.BrotherHurt) {
                    return;
                }
                orig(self, value);
            }

            private void HealthComponent_AddBarrier(On.RoR2.HealthComponent.orig_AddBarrier orig, HealthComponent self, float value) {
                if (self.body.bodyIndex == BodyIndexes.BrotherHurt) {
                    return;
                }
                orig(self, value);
            }
        }
    }
}