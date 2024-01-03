using BtpTweak.Tweaks.MithrixTweaks.MithrixEntityStates;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.BrotherMonster;
using EntityStates.BrotherMonster.Weapon;
using EntityStates.LunarGolem;
using R2API;
using R2API.Utils;

using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BtpTweak.Tweaks.MithrixTweaks {

    internal class SkillTweak : TweakBase<SkillTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        public static readonly Dictionary<int, Vector3> p23PizzaPoints = new() {
            {
                0,
                new Vector3(13.5f, 489.7f, -107f)
            },
            {
                1,
                new Vector3(-189f, 489.7f, 107f)
            },
            {
                2,
                new Vector3(16.7f, 489.7f, 101f)
            },
            {
                3,
                new Vector3(-196f, 489.7f, -101f)
            }
        };

        void IOnModLoadBehavior.OnModLoad() {
            ContentAddition.AddEntityState<LunarBlink>(out _);
            ContentAddition.AddEntityState<EnterCrushingLeap>(out _);
            ContentAddition.AddEntityState<AimCrushingLeap>(out _);
            ContentAddition.AddEntityState<ExitCrushingLeap>(out _);
            On.EntityStates.BrotherMonster.Weapon.FireLunarShards.OnEnter += FireLunarShards_OnEnter;
            On.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate += WeaponSlam_FixedUpdate;
            On.EntityStates.BrotherMonster.SprintBash.OnEnter += SprintBash_OnEnter;
            On.EntityStates.BrotherMonster.UltChannelState.FireWave += UltChannelState_FireWave;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GroundedSkillDefPaths.SkyLeap.Load<SkillDef>().activationState = new EntityStates.SerializableEntityStateType(typeof(EnterCrushingLeap));
            GameObjectPaths.BrotherBody.LoadComponent<SkillLocator>().utility.skillFamily.variants[0].skillDef = BtpContent.Skills.UngroundedDash;
        }

        private void FireLunarShards_OnEnter(On.EntityStates.BrotherMonster.Weapon.FireLunarShards.orig_OnEnter orig, FireLunarShards self) {
            orig(self);
            if (self.isAuthority && PhaseCounter.instance && PhaseCounter.instance.phase > 1) {
                var aimRay = self.GetAimRay();
                var muzzleTransform = self.FindModelChild(FireLunarShards.muzzleString);
                if (muzzleTransform) {
                    aimRay.origin = muzzleTransform.position;
                }
                var body = self.characterBody;
                var projectileInfo = new FireProjectileInfo {
                    crit = body.RollCrit(),
                    damage = self.damageStat * self.damageCoefficient,
                    force = FireTwinShots.force,
                    owner = body.gameObject,
                    position = aimRay.origin,
                    projectilePrefab = AssetReferences.lunarMissilePrefab,
                    useSpeedOverride = true,
                    speedOverride = 50
                };
                aimRay.direction += new Vector3(0f, 0.065f, 0f);
                projectileInfo.crit = body.RollCrit();
                projectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                ProjectileManager.instance.FireProjectile(projectileInfo);
                if (PhaseCounter.instance.phase == 3) {
                    projectileInfo.useSpeedOverride = false;
                    projectileInfo.projectilePrefab = FireTwinShots.projectilePrefab;
                    projectileInfo.damage = self.damageStat * FireTwinShots.damageCoefficient;
                    aimRay.direction += new Vector3(0f, 0.07f, 0f);
                    projectileInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction, Vector3.down);
                    projectileInfo.crit = body.RollCrit();
                    ProjectileManager.instance.FireProjectile(projectileInfo);

                    projectileInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction, Vector3.up);
                    projectileInfo.crit = body.RollCrit();
                    ProjectileManager.instance.FireProjectile(projectileInfo);
                    Util.PlaySound(FireTwinShots.attackSoundString, self.gameObject);
                }
            }
        }

        private void WeaponSlam_FixedUpdate(On.EntityStates.BrotherMonster.WeaponSlam.orig_FixedUpdate orig, WeaponSlam self) {
            if (self.isAuthority && PhaseCounter.instance?.phase > 0 && !self.hasDoneBlastAttack && self.modelAnimator && self.modelAnimator.GetFloat("blast.hitBoxActive") > 0.5f) {
                var waveProjectileCount = WeaponSlam.waveProjectileCount * PhaseCounter.instance.phase;
                var anglePerBall = 360 / waveProjectileCount;
                var xAxis = Vector3.ProjectOnPlane(self.characterDirection.forward, Vector3.up);
                var tmpBody = self.characterBody;
                var projectileInfo = new FireProjectileInfo {
                    damage = FistSlam.waveProjectileDamageCoefficient * self.damageStat,
                    force = FistSlam.waveProjectileForce,
                    owner = tmpBody.gameObject,
                    position = self.FindModelChild(WeaponSlam.muzzleString).position,
                    projectilePrefab = FistSlam.waveProjectilePrefab,
                };
                for (int i = 0, randomOffset = Random.Range(0, anglePerBall); i < waveProjectileCount; ++i) {  // 环绕球
                    projectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(randomOffset + anglePerBall * i, Vector3.up) * xAxis);
                    projectileInfo.crit = tmpBody.RollCrit();
                    ProjectileManager.instance.FireProjectile(projectileInfo);
                }
                foreach (var teamMember in TeamComponent.GetTeamMembers(TeamIndex.Player)) {
                    tmpBody = teamMember.body;
                    if (tmpBody) {
                        if (tmpBody.characterMotor && !tmpBody.characterMotor.isGrounded) {
                            tmpBody.healthComponent.TakeDamageForce(5000f * PhaseCounter.instance.phase * Vector3.down, true, false);
                        } else if (tmpBody.isFlying) {  // drones
                            tmpBody.healthComponent.TakeDamageForce(10000f * PhaseCounter.instance.phase * Vector3.down, true, false);
                        }
                    }
                }
                ChatMessage.Send("<color=#c6d5ff><size=120%>米斯历克斯：Fall!</color></size>");
            }
            orig(self);
        }

        private void SprintBash_OnEnter(On.EntityStates.BrotherMonster.SprintBash.orig_OnEnter orig, SprintBash self) {
            orig(self);
            if (PhaseCounter.instance && self.isAuthority) {
                var aimRay = self.GetAimRay();
                var body = self.characterBody;
                var projectileInfo = new FireProjectileInfo() {
                    projectilePrefab = FireLunarShards.projectilePrefab,
                    position = aimRay.origin,
                    rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                    crit = body.RollCrit(),
                    damage = body.damage * self.damageCoefficient * 0.1f,
                    damageColorIndex = DamageColorIndex.Default,
                    owner = self.gameObject,
                    procChainMask = default,
                    force = 0.0f,
                    useFuseOverride = false,
                    useSpeedOverride = false,
                    target = null
                };
                ProjectileManager.instance.FireProjectile(projectileInfo);
                Util.PlaySound(FireLunarShards.fireSound, self.gameObject);
                projectileInfo.damage *= 0.1f * PhaseCounter.instance.phase;
                for (int i = 0, randoffset = Random.RandomRangeInt(0, 360), n = 3 + 2 * PhaseCounter.instance.phase, angel = 360 / n; i < n; ++i) {
                    projectileInfo.rotation = Quaternion.Euler(0, i * angel + randoffset, 0);
                    projectileInfo.crit = body.RollCrit();
                    ProjectileManager.instance.FireProjectile(projectileInfo);
                }
                if (PhaseCounter.instance.phase > 1) {
                    projectileInfo.projectilePrefab = WeaponSlam.waveProjectilePrefab;
                    projectileInfo.damage = self.damageStat * self.damageCoefficient;
                    projectileInfo.position = body.footPosition;
                    projectileInfo.rotation = Util.QuaternionSafeLookRotation(Vector3.ProjectOnPlane(aimRay.direction, Vector3.up));
                    projectileInfo.crit = body.RollCrit();
                    ProjectileManager.instance.FireProjectile(projectileInfo);
                }
            }
        }

        private void UltChannelState_FireWave(On.EntityStates.BrotherMonster.UltChannelState.orig_FireWave orig, UltChannelState self) {
            orig(self);
            if (PhaseCounter.instance) {
                if (PhaseCounter.instance.phase == 2) {
                    float num = 360f / UltChannelState.totalWaves;
                    Vector3 point = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                    for (int i = 0; i < 4; i++) {
                        for (int j = 0; j < UltChannelState.totalWaves; ++j) {
                            Vector3 forward = Quaternion.AngleAxis(num * j, Vector3.up) * point;
                            ProjectileManager.instance.FireProjectile(AssetReferences.brotherUltLineProjectileStatic, p23PizzaPoints[i], Util.QuaternionSafeLookRotation(forward), self.gameObject, self.characterBody.damage * UltChannelState.waveProjectileDamageCoefficient, UltChannelState.waveProjectileForce, self.RollCrit());
                        }
                    }
                } else if (PhaseCounter.instance.phase == 3) {
                    var livePlayers = CharacterBody.readOnlyInstancesList.Where(body => body.isPlayerControlled);
                    var target = livePlayers.ElementAt(Random.Range(0, livePlayers.Count()));
                    if (Physics.Raycast(new Ray(target.footPosition, Vector3.down), out var raycastHit, float.MaxValue, LayerIndex.world.mask, QueryTriggerInteraction.Ignore)) {
                        Vector3[] hitPositions = [
                            raycastHit.point + new Vector3(Random.Range(-90f, -30f), 0f, Random.Range(-90f, -30f)),
                            raycastHit.point + new Vector3(Random.Range(30f, 90f), 0f, Random.Range(30f, 90f))
                        ];
                        var projectileInfo = new FireProjectileInfo {
                            projectilePrefab = Random.value > 0.5f ? UltChannelState.waveProjectileLeftPrefab : UltChannelState.waveProjectileRightPrefab,
                            owner = self.gameObject,
                            damage = self.damageStat * UltChannelState.waveProjectileDamageCoefficient,
                            force = UltChannelState.waveProjectileForce,
                        };
                        float anglePerLine = 360f / UltChannelState.totalWaves;
                        Vector3 normalized = Vector3.ProjectOnPlane(Random.onUnitSphere, Vector3.up).normalized;
                        for (int k = 0; k < 2; ++k) {
                            for (int l = 0; l < UltChannelState.totalWaves; ++l) {
                                projectileInfo.position = hitPositions[k];
                                projectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(anglePerLine * l, Vector3.up) * normalized);
                                projectileInfo.crit = self.RollCrit();
                                ProjectileManager.instance.FireProjectile(projectileInfo);
                            }
                        }
                    }
                }
            }
        }
    }
}