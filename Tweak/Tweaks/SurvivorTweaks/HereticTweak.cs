using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using EntityStates;
using EntityStates.GlobalSkills.LunarNeedle;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class HereticTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.GhostUtilitySkillState.FixedUpdate += GhostUtilitySkillState_FixedUpdate;
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += Detonate_OnEnter;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += FireLunarNeedle_OnEnter;
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += Squawk_OnEnter;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            SkillDefPaths.HereticDefaultAbility.Load<SkillDef>().baseRechargeInterval = 40;
            var lunarSecondaryProjectile = GameObjectPaths.LunarSecondaryProjectile.Load<GameObject>();
            lunarSecondaryProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            lunarSecondaryProjectile.AddComponent<LunarSecondaryProjectileStartAction>();
            var directionalTargetFinder = FireLunarNeedle.projectilePrefab.GetComponent<ProjectileDirectionalTargetFinder>();
            directionalTargetFinder.lookCone = 30f;
            directionalTargetFinder.lookRange = 30f;
            FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSimple>().lifetime *= 2;
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Heretic, RecalculateHereticStats);
        }

        private void RecalculateHereticStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            args.cooldownReductionAdd += 2 * inventory.GetItemCount(RoR2Content.Items.LunarPrimaryReplacement.itemIndex);
            args.attackSpeedMultAdd += 0.1f * inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
            args.moveSpeedMultAdd += 0.1f * inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement.itemIndex);
            args.healthMultAdd += 0.1f * inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
        }

        private void Detonate_OnEnter(On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self) {
            if (self.characterBody.bodyIndex == BodyIndexes.Heretic) {
                EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f + 0.6f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
            } else {
                EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f;
            }
            orig(self);
        }

        private void FireLunarNeedle_OnEnter(On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, FireLunarNeedle self) {
            if (self.characterBody.bodyIndex == BodyIndexes.Heretic) {
                FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 1800f;
            } else {
                FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 180f;
            }
            orig(self);
        }

        private void GhostUtilitySkillState_FixedUpdate(On.EntityStates.GhostUtilitySkillState.orig_FixedUpdate orig, GhostUtilitySkillState self) {
            orig(self);
            if (self.isAuthority) {
                if (self.characterBody.bodyIndex == BodyIndexes.Heretic && self.inputBank.skill3.justReleased && self.fixedAge > 1f) {
                    self.outer.SetNextStateToMain();
                }
            }
        }

        private void Squawk_OnEnter(On.EntityStates.Heretic.Weapon.Squawk.orig_OnEnter orig, EntityStates.Heretic.Weapon.Squawk self) {
            orig(self);
            if (self.isAuthority) {
                foreach (var body in CharacterBody.readOnlyInstancesList) {
                    if (TeamIndex.Lunar == body.teamComponent.teamIndex) {
                        body.AddTimedBuffAuthority(RoR2Content.Buffs.LunarSecondaryRoot.buffIndex, 30);
                    } else {
                        body.AddTimedBuffAuthority(RoR2Content.Buffs.LunarSecondaryRoot.buffIndex, 10);
                    }
                }
            }
        }

        [RequireComponent(typeof(ProjectileController))]
        private class LunarSecondaryProjectileStartAction : MonoBehaviour {

            private void Awake() {
                enabled = NetworkServer.active;
            }

            private void Start() {
                var owner = GetComponent<ProjectileController>().owner;
                if (owner != null && owner.TryGetComponent<CharacterBody>(out var body) && body.bodyIndex == BodyIndexes.Heretic) {
                    GetComponent<ProjectileExplosion>().blastRadius *= 1 + 0.5f * body.inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                }
            }
        }
    }
}