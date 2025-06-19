using BTP.RoR2Plugin.RoR2Indexes;
using EntityStates;
using EntityStates.BrotherMonster;
using EntityStates.Mage.Weapon;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.MithrixTweaks.MithrixEntityStates {

    public class LunarBlink : BaseState {
        public const float speedCoefficient = 7.5f;
        public const float duration = 0.15f;
        public const float damageCoefficient = 3f;
        public const int cloneDuration = 10;
        public const int orbCount = 3;
        public const int cloneCount = 1;
        private Transform modelTransform;
        private float stopwatch;
        private Vector3 blinkVector = Vector3.zero;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private CharacterBody body;

        public override void OnEnter() {
            base.OnEnter();
            Util.PlaySound(Flamethrower.startAttackSoundString, gameObject);
            modelTransform = GetModelTransform();
            body = characterBody;
            blinkVector = GetBlinkVector();
            if (modelTransform) {
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if (characterModel) {
                characterModel.transform.GetChild(1).gameObject.SetActive(false);
                ++characterModel.invisibilityCount;
            }
            if (hurtboxGroup) {
                ++hurtboxGroup.hurtBoxesDeactivatorCounter;
            }
            CreateBlinkEffect(body.corePosition);
            if (NetworkServer.active && PhaseCounter.instance?.phase == 3) {
                if (body.HasBuff(RoR2Content.Buffs.AffixBlue.buffIndex)) {
                    FireBalls(AssetReferences.electricOrbProjectile, body.corePosition, orbCount, 0, WeaponSlam.weaponForce);
                }
                if (body.HasBuff(RoR2Content.Buffs.AffixRed.buffIndex)) {
                    FireBalls(AssetReferences.magmaOrbProjectile, body.corePosition, orbCount, 0, WeaponSlam.weaponForce);
                }
            }
            if (characterBody.bodyIndex == BodyIndexes.Brother && Random.value > 0.5f) {
                for (int i = 0; i < cloneCount; ++i) {
                    var directorSpawnRequest = new DirectorSpawnRequest(ExitCrushingLeap.brotherGlassSpawnCard, new DirectorPlacementRule {
                        placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                        minDistance = 3f,
                        maxDistance = 20f,
                        spawnOnTarget = transform
                    }, RoR2Application.rng) {
                        summonerBodyObject = gameObject
                    };
                    directorSpawnRequest.onSpawnedServer += (spawnResult) => {
                        if (spawnResult.success) {
                            spawnResult.spawnedInstance.GetComponent<Inventory>().GiveItem(RoR2Content.Items.HealthDecay.itemIndex, cloneDuration);
                        }
                    };
                    DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                }
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (characterMotor && characterDirection) {
                characterMotor.velocity = Vector3.zero;
                characterMotor.rootMotion += blinkVector * (moveSpeedStat * speedCoefficient * Time.fixedDeltaTime);
            }
            if (stopwatch > (double)duration && isAuthority) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            if (!outer.destroying) {
                Util.PlaySound(Flamethrower.endAttackSoundString, gameObject);
                CreateBlinkEffect(Util.GetCorePosition(gameObject));
                modelTransform = GetModelTransform();
                if ((bool)modelTransform) {
                    TemporaryOverlay temporaryOverlay1 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay1.duration = 0.6f;
                    temporaryOverlay1.animateShaderAlpha = true;
                    temporaryOverlay1.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.0f);
                    temporaryOverlay1.destroyComponentOnEnd = true;
                    temporaryOverlay1.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                    temporaryOverlay1.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                    TemporaryOverlay temporaryOverlay2 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay2.duration = 0.7f;
                    temporaryOverlay2.animateShaderAlpha = true;
                    temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0.0f, 1f, 1f, 0.0f);
                    temporaryOverlay2.destroyComponentOnEnd = true;
                    temporaryOverlay2.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                    temporaryOverlay2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                }
            }
            if ((bool)characterModel) {
                characterModel.transform.GetChild(1).gameObject.SetActive(true);
                --characterModel.invisibilityCount;
            }
            if ((bool)hurtboxGroup)
                --hurtboxGroup.hurtBoxesDeactivatorCounter;
            if ((bool)characterMotor)
                characterMotor.disableAirControlUntilCollision = false;
            base.OnExit();
        }

        protected virtual Vector3 GetBlinkVector() => (inputBank.moveVector == Vector3.zero ? characterDirection.forward : inputBank.moveVector).normalized;

        private void FireBalls(GameObject prefab, Vector3 impactPosition, int ballCount, float ballYAngle, float ballForce) {
            var projectileInfo = new FireProjectileInfo {
                crit = body.RollCrit(),
                damage = damageStat * damageCoefficient,
                force = ballForce,
                owner = gameObject,
                position = impactPosition,
                projectilePrefab = prefab,
                rotation = Quaternion.Euler(-90, 0, 0),
            };
            ProjectileManager.instance.FireProjectile(projectileInfo);
            for (int i = 0, anglePerBall = 360 / ballCount, 随机偏移角 = Random.Range(0, 360); i < ballCount; ++i) {
                projectileInfo.rotation = Quaternion.Euler(ballYAngle, i * anglePerBall + 随机偏移角, 0);
                projectileInfo.crit = body.RollCrit();
                ProjectileManager.instance.FireProjectile(projectileInfo);
            }
        }

        private void CreateBlinkEffect(Vector3 origin) => EffectManager.SpawnEffect(AssetReferences.moonExitArenaOrbEffect, new EffectData() {
            rotation = Util.QuaternionSafeLookRotation(blinkVector),
            origin = origin
        }, false);
    }
}