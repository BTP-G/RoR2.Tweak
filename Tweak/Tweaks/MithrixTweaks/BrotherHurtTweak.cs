using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using EntityStates.BrotherMonster;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.MithrixTweaks {

    internal class BrotherHurtTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const int missileCountPerFire = 5;
        public const float missileSpeed = 60f;
        public const float missileFuse = 20f;
        public const float missileTurretPitchFrequency = 0.5f;
        public const float missileTurretPitchMagnitude = 20;
        public const float missileTurretYawFrequency = 0.5f;
        public const float fireInterval = 0.3f;
        private float fireTimer;

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.BrotherMonster.SpellChannelExitState.OnEnter += SpellChannelExitState_OnEnter;
            On.EntityStates.BrotherMonster.TrueDeathState.FixedUpdate += TrueDeathState_FixedUpdate;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            var body = GameObjectPaths.BrotherHurtBody.LoadComponent<CharacterBody>();
            body.GetComponent<SetStateOnHurt>().canBeFrozen = false;
            var component = body.gameObject.AddComponent<BrotherHurtBodyComponent>();
            var meteorStormController = GameObjectPaths.MeteorStorm.LoadComponent<MeteorStormController>();
            component.warningEffectPrefab = meteorStormController.warningEffectPrefab;
            component.impactEffectPrefab = meteorStormController.impactEffectPrefab;
            component.travelEffectPrefab = meteorStormController.travelEffectPrefab;
            FistSlam.healthCostFraction = 0;
        }

        private void SpellChannelExitState_OnEnter(On.EntityStates.BrotherMonster.SpellChannelExitState.orig_OnEnter orig, SpellChannelExitState self) {
            orig(self);
            if (NetworkServer.active) {
                var body = self.outer.commonComponents.characterBody;
                body.RecalculateStats();
                foreach (var c in CharacterBody.readOnlyInstancesList) {
                    c.healthComponent.Networkhealth = c.healthComponent.fullHealth;
                    c.healthComponent.RechargeShieldFull();
                }
                var dotInfo = new InflictDotInfo {
                    attackerObject = body.gameObject,
                    damageMultiplier = 1f,
                    dotIndex = DotController.DotIndex.Helfire,
                    duration = float.MaxValue,
                    victimObject = body.gameObject,
                };
                body.SetBuffCount(DLC1Content.Buffs.ImmuneToDebuffReady.buffIndex, 0);
                DotController.InflictDot(ref dotInfo);
                var dotStack = DotController.FindDotController(body.gameObject).dotStackList.Last(dotStack => dotStack.dotIndex == dotInfo.dotIndex);
                dotStack.damageType |= DamageType.BypassArmor | DamageType.NonLethal | DamageType.Silent;
                dotStack.damage = body.healthComponent.fullCombinedHealth * 0.001f;
            }
        }

        private void TrueDeathState_FixedUpdate(On.EntityStates.BrotherMonster.TrueDeathState.orig_FixedUpdate orig, TrueDeathState self) {
            orig(self);
            if (!NetworkServer.active || !RunInfo.位于月球 || !self.dissolving || (fireTimer -= Time.fixedDeltaTime) > 0) {
                return;
            }
            fireTimer = Mathf.Lerp(fireInterval, 0.06f, self.fixedAge / 5f);
            var body = self.characterBody;
            var aimRay = new Ray() {
                origin = body.corePosition,
                direction = new Vector3(1f, Random.Range(-0.5f, 1.5f), 0),
            };
            var projectileInfo = new FireProjectileInfo {
                projectilePrefab = AssetReferences.lunarMissilePrefab,
                owner = body.gameObject,
                damage = self.damageStat * (1 + Run.instance.stageClearCount),
                useSpeedOverride = true,
                speedOverride = missileSpeed,
                useFuseOverride = true,
                fuseOverride = missileFuse,
            };
            for (var i = 0; i < missileCountPerFire; ++i) {
                var bonusYaw = (360 / missileCountPerFire * i) + (360 * missileTurretYawFrequency * self.fixedAge);
                var forward = Util.ApplySpread(aimRay.direction, 0.0f, 0.0f, 1f, 1f, bonusYaw, Mathf.Sin(6.283185f * missileTurretPitchFrequency * self.fixedAge) * missileTurretPitchMagnitude);
                projectileInfo.position = aimRay.origin + new Vector3(0, self.fixedAge, 0);
                projectileInfo.rotation = Util.QuaternionSafeLookRotation(forward);
                ProjectileManager.instance.FireProjectile(projectileInfo);
            }
        }

        private class BrotherHurtBodyComponent : MonoBehaviour {
            public const float blastDamageCoefficient = 3;
            public const float blastForce = 4000;
            public const float blastRadius = 8;
            public const float impactDelay = 2;
            public const float travelEffectDuration = 2;
            public const float waveInterval = 6f;

            public GameObject warningEffectPrefab;
            public GameObject travelEffectPrefab;
            public GameObject impactEffectPrefab;

            private BlastAttack _blastAttack;

            private CharacterBody characterBody;
            private List<MeteorStormController.Meteor> meteorList;
            private List<MeteorStormController.MeteorWave> waveList;
            private float waveTimer;

            private void Awake() {
                if (enabled = NetworkServer.active) {
                    characterBody = GetComponent<CharacterBody>();
                    characterBody.healthComponent.ospTimer += 3f;
                    meteorList = [];
                    waveList = [];
                    On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
                    On.RoR2.HealthComponent.RechargeShield += HealthComponent_RechargeShield;
                    On.RoR2.HealthComponent.AddBarrier += HealthComponent_AddBarrier;
                    On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
                }
                //if (RunInfo.位于时之墓 && LunarWingsTweak.LunarWingsBehavior.Instance) {
                //    LunarWingsTweak.UpgradeLunarWings(LunarWingsState.过去完成时);
                //}
            }

            private void Start() {
                characterBody.master.teamIndex = TeamIndex.Lunar;
                characterBody.teamComponent.teamIndex = TeamIndex.Lunar;
                _blastAttack = new BlastAttack {
                    attacker = gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = blastDamageCoefficient * characterBody.damage,
                    baseForce = blastForce,
                    bonusForce = Vector3.zero,
                    crit = characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = blastRadius,
                    teamIndex = characterBody.teamComponent.teamIndex
                };
                Util.CleanseBody(characterBody, true, false, true, true, true, true);
            }

            private void FixedUpdate() {
                if (!RunInfo.位于月球) {
                    return;
                }
                if ((waveTimer -= Time.fixedDeltaTime) <= 0) {
                    waveTimer = waveInterval / (1 + Run.instance.stageClearCount);
                    waveList.Add(new MeteorStormController.MeteorWave(CharacterBody.readOnlyInstancesList.Where(body => body.isPlayerControlled).ToArray(), transform.position) {
                        hitChance = 0.1f,
                    });
                }
                for (var i = waveList.Count - 1; i >= 0; --i) {
                    var meteorWave = waveList[i];
                    meteorWave.timer -= Time.fixedDeltaTime;
                    if (meteorWave.timer <= 0f) {
                        meteorWave.timer = waveInterval / (1 + Run.instance.stageClearCount);
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
                var num = Run.instance.time - impactDelay;
                var num2 = num - travelEffectDuration;
                for (var j = meteorList.Count - 1; j >= 0; --j) {
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
                        if (Random.value < 0.5f) {
                            ProjectileManager.instance.FireProjectile(AssetReferences.brotherUltLineProjectileStatic, meteor.impactPosition, Quaternion.Euler(0, Random.Range(0f, 180f), 0), gameObject, characterBody.damage * UltChannelState.waveProjectileDamageCoefficient * 0.5f, UltChannelState.waveProjectileForce, characterBody.RollCrit());
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
                    On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
                }
            }

            private void DetonateMeteor(MeteorStormController.Meteor meteor) {
                _blastAttack.position = meteor.impactPosition;
                _blastAttack.Fire();
                EffectManager.SpawnEffect(impactEffectPrefab, new EffectData {
                    origin = meteor.impactPosition
                }, true);
            }

            private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen) {
                if (self.body.bodyIndex == BodyIndexes.BrotherHurt || RunInfo.位于月球) {
                    return 0f;
                }
                return orig(self, amount, procChainMask, nonRegen);
            }

            private void HealthComponent_RechargeShield(On.RoR2.HealthComponent.orig_RechargeShield orig, HealthComponent self, float value) {
                if (self.body.bodyIndex == BodyIndexes.BrotherHurt || RunInfo.位于月球) {
                    return;
                }
                orig(self, value);
            }

            private void HealthComponent_AddBarrier(On.RoR2.HealthComponent.orig_AddBarrier orig, HealthComponent self, float value) {
                if (self.body.bodyIndex == BodyIndexes.BrotherHurt || RunInfo.位于月球) {
                    return;
                }
                orig(self, value);
            }

            private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
                if (damageInfo.attacker) {
                    if (self.gameObject == gameObject) {
                        if (damageInfo.attacker != gameObject && damageInfo.attacker.TryGetComponent<CharacterBody>(out var attackerBody)) {
                            damageInfo.damage = Mathf.Max(self.combinedHealth * 0.01f * (damageInfo.damage / attackerBody.damage) * damageInfo.procCoefficient / (1 + Run.instance.stageClearCount), damageInfo.damage);
                        }
                    } else if (damageInfo.attacker == gameObject) {
                        damageInfo.damage = Mathf.Max((self.fullCombinedHealth + self.barrier) * 0.0075f * (damageInfo.damage / characterBody.damage) * damageInfo.procCoefficient * (1 + Run.instance.stageClearCount), damageInfo.damage);
                    }
                }
                orig(self, damageInfo);
            }
        }
    }
}