using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates;
using EntityStates.GlobalSkills.LunarNeedle;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class HereticTweak : TweakBase<HereticTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.EntityStates.GhostUtilitySkillState.FixedUpdate += GhostUtilitySkillState_FixedUpdate;
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += Detonate_OnEnter;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += FireLunarNeedle_OnEnter;
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += Squawk_OnEnter;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.EntityStates.GhostUtilitySkillState.FixedUpdate -= GhostUtilitySkillState_FixedUpdate;
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter -= Detonate_OnEnter;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter -= FireLunarNeedle_OnEnter;
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter -= Squawk_OnEnter;
        }

        public void Load() {
            SkillDefPaths.HereticDefaultAbility.Load<SkillDef>().baseRechargeInterval = 40;
            var lunarSecondaryProjectile = GameObjectPaths.LunarSecondaryProjectile.Load<GameObject>();
            lunarSecondaryProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            lunarSecondaryProjectile.AddComponent<LunarSecondaryProjectileStartAction>();
            var directionalTargetFinder = FireLunarNeedle.projectilePrefab.GetComponent<ProjectileDirectionalTargetFinder>();
            directionalTargetFinder.lookCone = 30f;
            directionalTargetFinder.lookRange = 30f;
            FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSimple>().lifetime *= 2;
        }

        private void Detonate_OnEnter(On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self) {
            var commonComponents = self.outer.commonComponents;
            if (commonComponents.characterBody.bodyIndex == BodyIndexes.HereticBody) {
                EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f + 0.6f * commonComponents.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
            } else {
                EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f;
            }
            orig(self);
        }

        private void FireLunarNeedle_OnEnter(On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, FireLunarNeedle self) {
            if (self.characterBody.bodyIndex == BodyIndexes.HereticBody) {
                FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 1800f;
            } else {
                FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 180f;
            }
            orig(self);
        }

        private void GhostUtilitySkillState_FixedUpdate(On.EntityStates.GhostUtilitySkillState.orig_FixedUpdate orig, GhostUtilitySkillState self) {
            orig(self);
            if (self.isAuthority) {
                var commonComponents = self.outer.commonComponents;
                if (commonComponents.characterBody.bodyIndex == BodyIndexes.HereticBody && commonComponents.inputBank.skill3.justReleased && self.fixedAge > 1f) {
                    self.outer.SetNextStateToMain();
                }
            }
        }

        private void Squawk_OnEnter(On.EntityStates.Heretic.Weapon.Squawk.orig_OnEnter orig, EntityStates.Heretic.Weapon.Squawk self) {
            orig(self);
            if (NetworkServer.active) {
                foreach (var body in CharacterBody.readOnlyInstancesList) {
                    if (TeamIndex.Lunar == body.teamComponent.teamIndex) {
                        body.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 30);
                    } else {
                        body.AddTimedBuff(RoR2Content.Buffs.LunarSecondaryRoot, 10);
                    }
                }
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        private class LunarSecondaryProjectileStartAction : MonoBehaviour {

            public void Start() {
                var body = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>();
                if (body?.bodyIndex == BodyIndexes.HereticBody) {
                    GetComponent<ProjectileExplosion>().blastRadius *= 1 + 0.5f * body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                }
            }

            private void Awake() {
                enabled = NetworkServer.active;
            }
        }
    }
}