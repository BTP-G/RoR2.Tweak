using EntityStates;
using EntityStates.BrotherMonster;
using EntityStates.Huntress;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BtpTweak.Tweaks.MithrixTweaks.MithrixEntityStates {

    public class AimCrushingLeap : BaseSkillState {
        private static readonly Material awShellExpolsionMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/artifactworld/matArtifactShellExplosionIndicator.mat").WaitForCompletion();
        private static readonly Material tpMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Teleporters/matTeleporterRangeIndicator.mat").WaitForCompletion();
        private float stopwatch;
        private float ultLinetimer;
        private GameObject areaIndicatorInstance;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
        private CharacterBody targetBody;

        public override void OnEnter() {
            base.OnEnter();
            var modelTransform = GetModelTransform();
            if (modelTransform) {
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if (characterModel) {
                ++characterModel.invisibilityCount;
            }
            if (hurtboxGroup) {
                hurtboxGroup.hurtBoxesDeactivatorCounter += 1;
            }
            Util.PlaySound("Play_voidRaid_snipe_shoot_final", gameObject);
            gameObject.layer = LayerIndex.fakeActor.intVal;
            characterMotor.Motor.RebuildCollidableLayers();
            characterMotor.velocity = Vector3.zero;
            var newPosition = characterMotor.transform.position;
            newPosition.y += 25f;
            characterMotor.Motor.SetPosition(newPosition, true);
            PickNextTarget();
            if (!ArrowRain.areaIndicatorPrefab || areaIndicatorInstance) {
                return;
            }
            areaIndicatorInstance = Object.Instantiate(ArrowRain.areaIndicatorPrefab, transform.position, Quaternion.identity);
            //areaIndicatorInstance.AddComponent<NetworkTransform>();
            areaIndicatorInstance.transform.localScale = new Vector3(ArrowRain.arrowRainRadius, ArrowRain.arrowRainRadius, ArrowRain.arrowRainRadius);
            areaIndicatorInstance.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = tpMat;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch < HoldSkyLeap.duration - 1f) {
                UpdateAreaIndicator();
            } else {
                areaIndicatorInstance.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = awShellExpolsionMat;
            }
            if (isAuthority) {
                if ((ultLinetimer += Time.fixedDeltaTime) > HoldSkyLeap.duration * 0.3f) {
                    ultLinetimer = 0;
                    float num = 360f / UltChannelState.totalWaves;
                    Vector3 point = Vector3.ProjectOnPlane(inputBank.aimDirection, Vector3.up);
                    var projectileInfo = new FireProjectileInfo {
                        projectilePrefab = AssetReferences.brotherUltLineProjectileStatic,
                        owner = gameObject,
                        damage = damageStat * UltChannelState.waveProjectileDamageCoefficient,
                        force = UltChannelState.waveProjectileForce,
                    };
                    for (int i = 0; i < 4; ++i) {
                        projectileInfo.position = SkillTweak.p23PizzaPoints[i];
                        for (int j = 0; j < UltChannelState.totalWaves; ++j) {
                            projectileInfo.rotation = Util.QuaternionSafeLookRotation(Quaternion.AngleAxis(num * j, Vector3.up) * point);
                            projectileInfo.crit = characterBody.RollCrit();
                            ProjectileManager.instance.FireProjectile(projectileInfo);
                        }
                    }
                }
                if (fixedAge > HoldSkyLeap.duration) {
                    HandleFollowupAttack();
                }
            }
        }

        public override void OnExit() {
            if (characterModel) {
                --characterModel.invisibilityCount;
            }
            if (hurtboxGroup) {
                var hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
            gameObject.layer = LayerIndex.defaultLayer.intVal;
            characterMotor.Motor.RebuildCollidableLayers();
            if (areaIndicatorInstance) {
                Destroy(areaIndicatorInstance);
            }
            base.OnExit();
        }

        private void HandleFollowupAttack() {
            var targetPosition = areaIndicatorInstance.transform.position + new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            characterMotor.Motor.SetPosition(targetPosition, true);
            outer.SetNextState(new ExitCrushingLeap());
        }

        private void UpdateAreaIndicator() {
            if (areaIndicatorInstance && (targetBody || PickNextTarget())) {
                if (Physics.Raycast(new Ray(targetBody.footPosition, Vector3.down), out var raycastHit, 200f, LayerIndex.world.mask, QueryTriggerInteraction.Ignore)) {
                    areaIndicatorInstance.transform.position = raycastHit.point;
                    return;
                }
                areaIndicatorInstance.transform.position = targetBody.corePosition;
            }
        }

        private bool PickNextTarget() {
            foreach (var target in CharacterBody.readOnlyInstancesList) {
                if (!target.isPlayerControlled) {
                    continue;
                }
                if (!targetBody
                    || (targetBody.corePosition - transform.position).sqrMagnitude > (target.corePosition - transform.position).sqrMagnitude) {
                    targetBody = target;
                }
            }
            return targetBody != null;
        }
    }
}