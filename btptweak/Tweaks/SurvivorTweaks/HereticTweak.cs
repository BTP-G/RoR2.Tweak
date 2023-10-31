using BtpTweak.IndexCollections;
using BtpTweak.Utils;
using EntityStates;
using EntityStates.GlobalSkills.LunarNeedle;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class HereticTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            IL.RoR2.LunarDetonatorPassiveAttachment.DamageListener.OnDamageDealtServer += DamageListener_OnDamageDealtServer;
            On.EntityStates.GhostUtilitySkillState.FixedUpdate += GhostUtilitySkillState_FixedUpdate;
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += Detonate_OnEnter;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += FireLunarNeedle_OnEnter;
            On.EntityStates.Heretic.Weapon.Squawk.OnEnter += Squawk_OnEnter;
        }

        public override void Load() {
            base.Load();
            "RoR2/Base/Heretic/HereticDefaultAbility.asset".Load<SkillDef>().baseRechargeInterval = 40;
            var lunarSecondaryProjectile = "RoR2/Base/LunarSkillReplacements/LunarSecondaryProjectile.prefab".Load<GameObject>();
            lunarSecondaryProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;
            lunarSecondaryProjectile.AddComponent<LunarSecondaryProjectileStartAction>();
            var directionalTargetFinder = FireLunarNeedle.projectilePrefab.GetComponent<ProjectileDirectionalTargetFinder>();
            directionalTargetFinder.lookCone = 30f;
            directionalTargetFinder.lookRange = 30f;
            FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSimple>().lifetime *= 2;
        }

        private void DamageListener_OnDamageDealtServer(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(x => x.MatchLdcR4(100f))) {
                cursor.Remove();
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.EmitDelegate((DamageReport damageReport) => {
                    if (damageReport.attackerBodyIndex == BodyIndexCollection.HereticBody) {
                        return 1000f;
                    } else {
                        return 100f;
                    }
                });
            } else {
                Main.Logger.LogError("LunarDetonator Hook Failed!");
            }
        }

        private void Detonate_OnEnter(On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self) {
            if (self.characterBody.bodyIndex == BodyIndexCollection.HereticBody) {
                EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f + 0.6f * self.characterBody.inventory.GetItemCount(RoR2Content.Items.LunarSpecialReplacement.itemIndex);
            } else {
                EntityStates.GlobalSkills.LunarDetonator.Detonate.damageCoefficientPerStack = 1.2f;
            }
            orig(self);
        }

        private void FireLunarNeedle_OnEnter(On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, FireLunarNeedle self) {
            if (self.characterBody.bodyIndex == BodyIndexCollection.HereticBody) {
                FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 1800f;
            } else {
                FireLunarNeedle.projectilePrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 180f;
            }
            orig(self);
        }

        private void GhostUtilitySkillState_FixedUpdate(On.EntityStates.GhostUtilitySkillState.orig_FixedUpdate orig, GhostUtilitySkillState self) {
            orig(self);
            if (self.isAuthority && self.characterBody.bodyIndex == BodyIndexCollection.HereticBody && self.inputBank.skill3.justReleased && self.fixedAge > 1f) {
                self.outer.SetNextStateToMain();
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

            private void Awake() {
                enabled = NetworkServer.active;
            }

            public void Start() {
                Inventory inventory = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>().inventory;
                if (inventory) {
                    GetComponent<ProjectileExplosion>().blastRadius *= 1 + 0.5f * inventory.GetItemCount(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                }
            }
        }
    }
}