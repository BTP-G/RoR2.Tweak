using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates;
using EntityStates.BrotherMonster;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.MithrixTweaks.MithrixEntityStates {

    public class ExitCrushingLeap : BaseSkillState {
        public const int cloneCount = 3;
        public const int cloneDuration = 10;
        public static readonly CharacterSpawnCard brotherGlassSpawnCard = CharacterSpawnCardPaths.cscBrotherGlass.Load<CharacterSpawnCard>();
        private bool recast = false;
        private float duration;

        public override void OnEnter() {
            base.OnEnter();
            var isAuthority = base.isAuthority;
            duration = ExitSkyLeap.baseDuration / attackSpeedStat;
            if (isAuthority) {
                characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.ArmorBoost.buffIndex, ExitSkyLeap.baseDuration);
            }
            Util.PlaySound(ExitSkyLeap.soundString, gameObject);
            PlayAnimation("Body", "ExitSkyLeap", "SkyLeap.playbackRate", duration);
            PlayAnimation("FullBody Override", "BufferEmpty");
            AimAnimator aimAnimator = GetAimAnimator();
            if (aimAnimator) {
                aimAnimator.enabled = true;
            }
            float num = 360f / ExitSkyLeap.waveProjectileCount;
            Vector3 point = Vector3.ProjectOnPlane(inputBank.aimDirection, Vector3.up);
            Vector3 footPosition = characterBody.footPosition;
            if (isAuthority) {
                for (int i = 0; i < ExitSkyLeap.waveProjectileCount; i++) {
                    Vector3 forward = Quaternion.AngleAxis(num * i, Vector3.up) * point;
                    ProjectileManager.instance.FireProjectile(ExitSkyLeap.waveProjectilePrefab, footPosition, Util.QuaternionSafeLookRotation(forward), gameObject, characterBody.damage * ExitSkyLeap.waveProjectileDamageCoefficient, ExitSkyLeap.waveProjectileForce, RollCrit(), DamageColorIndex.Default, null, -1f);
                }
            }
            if (!PhaseCounter.instance) {
                return;
            }
            if (isAuthority) {
                ProjectileManager.instance.FireProjectile(WeaponSlam.pillarProjectilePrefab, footPosition, Quaternion.identity, gameObject, characterBody.damage * WeaponSlam.pillarDamageCoefficient, 0f, RollCrit(), DamageColorIndex.Default, null, -1f);
            }
            if (PhaseCounter.instance.phase < 2) {
                return;
            }
            for (int i = 0; i < cloneCount; ++i) {
                var directorSpawnRequest = new DirectorSpawnRequest(brotherGlassSpawnCard, new DirectorPlacementRule {
                    placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                    minDistance = 3f,
                    maxDistance = 20f,
                    spawnOnTarget = transform
                }, RoR2Application.rng) {
                    summonerBodyObject = gameObject
                };
                directorSpawnRequest.onSpawnedServer += (spawnResult) => {
                    if (spawnResult.success) {
                        spawnResult.spawnedInstance.GetComponent<Inventory>().GiveItem(RoR2Content.Items.HealthDecay, cloneDuration);
                    }
                };
                DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
            }
            skillLocator?.special.SetSkillOverride(outer, UltChannelState.replacementSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            if (isAuthority) {
                switch (Random.Range(0, 5)) {
                    case 0: {
                        characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.AffixBlue.buffIndex, 20f);
                        ChatMessage.Send("<color=#99CCFF>雷电</color>");
                        break;
                    }
                    case 1: {
                        characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.AffixRed.buffIndex, 20f);
                        ChatMessage.Send("<color=#f25d25>火焰</color>");
                        break;
                    }
                    case 2: {
                        characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.AffixWhite.buffIndex, 20f);
                        ChatMessage.Send("<color=#CCFFFF>冰霜</color>");
                        break;
                    }
                    case 3: {
                        characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.AffixPoison.buffIndex, 20f);
                        ChatMessage.Send("<color=#014421>腐蚀</color>");
                        break;
                    }
                    case 4: {
                        characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.AffixHaunted.buffIndex, 20f);
                        ChatMessage.Send("<color=#20b2aa>无形</color>");
                        break;
                    }
                }
            }
            recast = Random.value < ExitSkyLeap.recastChance;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (!isAuthority) {
                return;
            }
            if (recast && fixedAge > ExitSkyLeap.baseDurationUntilRecastInterrupt) {
                outer.SetNextState(new EnterCrushingLeap());
            }
            if (fixedAge > duration) {
                outer.SetNextStateToMain();
            }
        }
    }
}