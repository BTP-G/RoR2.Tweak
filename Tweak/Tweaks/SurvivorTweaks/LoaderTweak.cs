using BTP.RoR2Plugin.Utils;
using EntityStates.Loader;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class LoaderTweak : TweakBase<LoaderTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float ChargeZapFistLightningDamageCoefficient = 0.5f;
        public const float ChargeZapFistProcCoefficient = 2f;
        public const float ChargeZapFistLungeSpeed = 70f;
        public const float ChargeFistMinChargeProcCoefficient = 0.3f;
        public const float ChargeFistMaxChargeProcCoefficient = 3f;
        public const float ChargeFistMinLungeSpeed = 20f;
        public const float ChargeFistMaxLungeSpeed = 90f;
        public const float GroundSlamBaseDamageCoefficient = 10f;
        public const float GroundSlamYVelocityDamageCoefficient = 0.05f;
        public const float SwingChargedFistVelocityDamageCoefficient = 0.05f;
        public const float pow = 1.5f;
        public const float PylonDamageCoefficient = 1f;
        public const float PylonRange = 25f;
        public const int PylonFireCount = 3;
        public const int PylonBounces = 1;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter += BaseSwingChargedFist_OnEnter;
            IL.EntityStates.Loader.BaseSwingChargedFist.OnEnter += IL_BaseSwingChargedFist_OnEnter;
            IL.EntityStates.Loader.SwingZapFist.OnMeleeHitAuthority += IL_SwingZapFist_OnMeleeHitAuthority;
            On.EntityStates.Loader.SwingZapFist.OnExit += SwingZapFist_OnExit;
            On.EntityStates.Loader.GroundSlam.OnEnter += GroundSlam_OnEnter;
            On.EntityStates.Loader.GroundSlam.DetonateAuthority += GroundSlam_DetonateAuthority;
            EntityStateConfigurationPaths.EntityStatesLoaderSwingZapFist.Load<EntityStateConfiguration>().Set("procCoefficient", ChargeZapFistProcCoefficient.ToString());
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            //===双钩===//
            var loaderBody = RoR2Content.Survivors.Loader.bodyPrefab.GetComponent<CharacterBody>();
            loaderBody.baseAcceleration = 100f;
            var loaderHook = GameObjectPaths.LoaderHook.Load<GameObject>();  // 抓钩1
            loaderHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 2f;
            var loaderYankHook = GameObjectPaths.LoaderYankHook.Load<GameObject>();  // 抓钩2
            loaderYankHook.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 2f;
            loaderYankHook.GetComponent<ProjectileGrappleController>().maxTravelDistance *= 2f;
            //===充能铁手套===//
            BaseSwingChargedFist.velocityDamageCoefficient = 0.03f;
            var steppedSkillDef2 = SteppedSkillDefPaths.ChargeFist.Load<SteppedSkillDef>();
            LanguageAPI.Add("SKILL_FIST_MINCHARGE_NAME", $"拳击(无充能)");
            LanguageAPI.Add("SKILL_FIST_MAXCHARGE_NAME", $"拳击(满充能)");
            //BetterUI.ProcCoefficientCatalog.AddSkill(steppedSkillDef2.skillName, "SKILL_FIST_MINCHARGE_NAME", ChargeFistMinChargeProcCoefficient);
            //BetterUI.ProcCoefficientCatalog.AddToSkill(steppedSkillDef2.skillName, "SKILL_FIST_MAXCHARGE_NAME", ChargeFistMaxChargeProcCoefficient);
            //===雷冲===//
            var steppedSkillDef = SteppedSkillDefPaths.ChargeZapFist.Load<SteppedSkillDef>();
            steppedSkillDef.baseRechargeInterval = 3f;
            var proximityBeamController = GameObjectPaths.LoaderZapCone.LoadComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange *= 2;
            proximityBeamController.bounces = 1;
            proximityBeamController.damageCoefficient = 0.5f;
            //BetterUI.ProcCoefficientCatalog.AddSkill(steppedSkillDef.skillName, "SKILL_FIST_NAME", ChargeZapFistProcCoefficient);
            //===电塔===//
            var pylon = GameObjectPaths.LoaderPylon.Load<GameObject>();
            pylon.AddComponent<M551PylonStartAction>();
            proximityBeamController = pylon.GetComponent<ProjectileProximityBeamController>();
            proximityBeamController.attackRange = PylonRange;
            proximityBeamController.attackFireCount = PylonFireCount;
            proximityBeamController.bounces = PylonBounces;
            proximityBeamController.damageCoefficient = PylonDamageCoefficient;
            //===雷霆一击===//
            ArrayUtils.ArrayAppend(ref SteppedSkillDefPaths.GroundSlam.Load<SteppedSkillDef>().keywordTokens, "KEYWORD_HEAVY");
            GroundSlam.blastRadius = 20f;
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

        private void IL_BaseSwingChargedFist_OnEnter(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchStfld<BaseSwingChargedFist>("bonusDamage"))) {
                cursor.Index -= 7;
                cursor.RemoveRange(7);
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((BaseSwingChargedFist state) => Mathf.Pow(state.punchSpeed * SwingChargedFistVelocityDamageCoefficient * (state.moveSpeedStat / state.characterBody.baseMoveSpeed), pow) * state.damageStat);
            } else {
                LogExtensions.LogError(GetType().FullName + " add hook 'IL_BaseSwingChargedFist_OnEnter' failed!");
            }
        }

        private void IL_SwingZapFist_OnMeleeHitAuthority(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchStfld<FireProjectileInfo>("damage"))) {
                cursor.Index -= 4;
                cursor.RemoveRange(4);
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((SwingZapFist swingZapFistState) => swingZapFistState.overlapAttack.damage);
            } else {
                LogExtensions.LogError(GetType().FullName + " add hook 'SwingZapFist_OnMeleeHitAuthority' failed!");
            }
        }

        private void SwingZapFist_OnExit(On.EntityStates.Loader.SwingZapFist.orig_OnExit orig, SwingZapFist self) {
            SwingZapFist.selfKnockback = 100f * self.punchSpeed;
            orig(self);
        }

        private void GroundSlam_OnEnter(On.EntityStates.Loader.GroundSlam.orig_OnEnter orig, GroundSlam self) {
            orig(self);
            GroundSlam.verticalAcceleration = -self.characterBody.acceleration;
        }

        private BlastAttack.Result GroundSlam_DetonateAuthority(On.EntityStates.Loader.GroundSlam.orig_DetonateAuthority orig, GroundSlam self) {
            GroundSlam.blastDamageCoefficient = GroundSlamBaseDamageCoefficient + Mathf.Pow(-self.characterMotor.lastVelocity.y * GroundSlamYVelocityDamageCoefficient * (self.moveSpeedStat / self.characterBody.baseMoveSpeed), pow);
            return orig(self);
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