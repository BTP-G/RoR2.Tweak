using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.Loader;
using HG;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class LoaderTweak : TweakBase<LoaderTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float ChargeZapFistProcCoefficient = 2.1f;
        public const float ChargeFistMinChargeProcCoefficient = 0.6f;
        public const float ChargeFistMaxChargeProcCoefficient = 2.7f;
        public const float GroundSlamBaseDamageCoefficient = 10f;
        public const float GroundSlamYVelocityCoefficient = 0.3f;
        public const float GroundSlamBaseVerticalAcceleration = -100f;
        public const float PylonDamageCoefficient = 1f;
        public const float PylonRange = 25f;
        public const int PylonFireCount = 3;
        public const int PylonBounces = 1;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Loader.SwingZapFist.OnExit += SwingZapFist_OnExit;
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter += BaseSwingChargedFist_OnEnter;
            On.EntityStates.Loader.GroundSlam.OnEnter += GroundSlam_OnEnter;
            On.EntityStates.Loader.GroundSlam.DetonateAuthority += GroundSlam_DetonateAuthority;
            EntityStateConfigurationPaths.EntityStatesLoaderChargeZapFist.Load<EntityStateConfiguration>().Set("procCoefficient", ChargeZapFistProcCoefficient.ToString());
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var loaderBody = RoR2Content.Survivors.Loader.bodyPrefab.GetComponent<CharacterBody>();
            loaderBody.baseAcceleration = 100f;
            var loaderHook = GameObjectPaths.LoaderHook.Load<GameObject>();  // 抓钩1
            loaderHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 2f;
            var loaderYankHook = GameObjectPaths.LoaderYankHook.Load<GameObject>();  // 抓钩2
            loaderYankHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderYankHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 2f;
            //====== 雷冲
            var steppedSkillDef = SteppedSkillDefPaths.ChargeZapFist.Load<SteppedSkillDef>();
            steppedSkillDef.baseRechargeInterval = 3f;
            var proximityBeamController = GameObjectPaths.LoaderZapCone.LoadComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange *= 2;
            proximityBeamController.bounces = 1;
            BetterUI.ProcCoefficientCatalog.AddSkill(steppedSkillDef.skillName, "SKILL_FIST_NAME", 2.1f);
            // 充能铁手套
            var steppedSkillDef2 = SteppedSkillDefPaths.ChargeFist.Load<SteppedSkillDef>();
            LanguageAPI.Add("SKILL_FIST_MINCHARGE_NAME", $"拳击(无充能)");
            LanguageAPI.Add("SKILL_FIST_MAXCHARGE_NAME", $"拳击(满充能)");
            BetterUI.ProcCoefficientCatalog.AddSkill(steppedSkillDef2.skillName, "SKILL_FIST_MINCHARGE_NAME", ChargeFistMinChargeProcCoefficient);
            BetterUI.ProcCoefficientCatalog.AddToSkill(steppedSkillDef2.skillName, "SKILL_FIST_MAXCHARGE_NAME", ChargeFistMaxChargeProcCoefficient);

            var pylon = GameObjectPaths.LoaderPylon.Load<GameObject>();  // 电塔
            pylon.AddComponent<M551PylonStartAction>();
            proximityBeamController = pylon.GetComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange = PylonRange;
            proximityBeamController.attackFireCount = PylonFireCount;
            proximityBeamController.bounces = PylonBounces;
            proximityBeamController.damageCoefficient = PylonDamageCoefficient;

            ArrayUtils.ArrayAppend(ref SteppedSkillDefPaths.GroundSlam.Load<SteppedSkillDef>().keywordTokens, "KEYWORD_HEAVY");
            GroundSlam.blastRadius = 20f;
        }

        private void GroundSlam_OnEnter(On.EntityStates.Loader.GroundSlam.orig_OnEnter orig, GroundSlam self) {
            orig(self);
            GroundSlam.verticalAcceleration = -self.characterBody.acceleration;
        }

        private BlastAttack.Result GroundSlam_DetonateAuthority(On.EntityStates.Loader.GroundSlam.orig_DetonateAuthority orig, GroundSlam self) {
            GroundSlam.blastDamageCoefficient = GroundSlamBaseDamageCoefficient + (-self.characterMotor.lastVelocity.y * GroundSlamYVelocityCoefficient);
            return orig(self);
        }

        private void BaseSwingChargedFist_OnEnter(On.EntityStates.Loader.BaseSwingChargedFist.orig_OnEnter orig, BaseSwingChargedFist self) {
            if (self is SwingChargedFist) {
                self.procCoefficient = Mathf.Lerp(ChargeFistMinChargeProcCoefficient, ChargeFistMaxChargeProcCoefficient, self.charge);
            }
            orig(self);
            if (self.isAuthority) {
                self.characterBody.AddTimedBuffAuthority(RoR2Content.Buffs.HiddenInvincibility.buffIndex, self.duration);
            }
        }

        private void SwingZapFist_OnExit(On.EntityStates.Loader.SwingZapFist.orig_OnExit orig, SwingZapFist self) {
            SwingZapFist.selfKnockback = 100f * self.punchSpeed;
            orig(self);
        }

        private class M551PylonStartAction : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void Start() {
                var owner = GetComponent<ProjectileController>().owner;
                if (owner) {
                    int itemCount = owner.GetComponent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.ShockNearby.itemIndex);
                    var proximityBeamController = GetComponent<ProjectileProximityBeamController>();
                    proximityBeamController.attackFireCount += itemCount * PylonFireCount;
                    proximityBeamController.attackRange += itemCount * PylonRange;
                    proximityBeamController.bounces += itemCount * PylonBounces;
                }
            }
        }
    }
}