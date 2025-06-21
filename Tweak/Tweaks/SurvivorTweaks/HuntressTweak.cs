using BTP.RoR2Plugin.Utils;
using EntityStates.Huntress;
using EntityStates.Huntress.HuntressWeapon;
using HG;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class HuntressTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float 基础射程 = 60f;
        public const float 猎人的鱼叉叠加射程 = 10f;
        public const float BallistaDamageCoefficient = 9f;
        public const float FlurryDamageCoefficient = 1.2f;
        public const float LaserGlaiveBounceDamageCoefficient = 1.1f;
        public const float LaserGlaiveDamageCoefficient = 3f;
        public const float StrafeDamageCoefficient = 1.8f;
        public const float ArrowRainDamageCoefficient = 3f;
        public const int BallistaBoltCount = 3;
        public const int LaserGlaiveBounceCount = 10;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.HuntressTracker.MyFixedUpdate += HuntressTracker_FixedUpdate;
            On.RoR2.HuntressTracker.Start += HuntressTracker_Start;
            On.RoR2.HuntressTracker.SearchForTarget += HuntressTracker_SearchForTarget;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.FireOrbArrow += FireSeekingArrow_FireOrbArrow;
            EntityStateConfigurationPaths.EntityStatesHuntressHuntressWeaponFireSeekingArrow.Load<EntityStateConfiguration>().Set(new Dictionary<string, string> {
                ["baseDuration"] = "0.5",
                ["orbDamageCoefficient"] = StrafeDamageCoefficient.ToString(),
            });
            EntityStateConfigurationPaths.EntityStatesHuntressHuntressWeaponFireFlurrySeekingArrow.Load<EntityStateConfiguration>().Set(new Dictionary<string, string> {
                ["baseDuration"] = "1",
                ["orbDamageCoefficient"] = FlurryDamageCoefficient.ToString(),
            });
        }

        void IRoR2LoadedMessageHandler.Handle() {
            var huntressBody = RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<CharacterBody>();
            huntressBody.baseCrit = 5f;
            huntressBody.levelCrit = 1f;
            huntressBody.bodyFlags |= CharacterBody.BodyFlags.SprintAnyDirection;
            RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<HuntressTracker>().trackerUpdateFrequency = 5;

            ArrowRain.damageCoefficient = 3f;  // 2.2
            var projectileDotZone = ArrowRain.projectilePrefab.GetComponent<ProjectileDotZone>();
            projectileDotZone.fireFrequency = 10f;
            projectileDotZone.resetFrequency = 2f;
            projectileDotZone.damageCoefficient = 0.5f;

            var skillDef = HuntressTrackingSkillDefPaths.HuntressBodyGlaive.Load<HuntressTrackingSkillDef>();
            skillDef.cancelSprintingOnActivation = false;
            ArrayUtils.ArrayAppend(ref skillDef.keywordTokens, "KEYWORD_AGILE");
            ThrowGlaive.damageCoefficient = LaserGlaiveDamageCoefficient;
            ThrowGlaive.maxBounceCount = LaserGlaiveBounceCount;

            SkillDefPaths.AimArrowSnipe.Load<SkillDef>().baseRechargeInterval = 9;
        }

        private void HuntressTracker_Start(On.RoR2.HuntressTracker.orig_Start orig, HuntressTracker self) {
            orig(self);
            var search = self.search;
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(self.teamComponent.teamIndex);
            search.filterByLoS = true;
            search.sortMode = BullseyeSearch.SortMode.Angle;
            search.maxAngleFilter = self.maxTrackingAngle;
        }

        private void HuntressTracker_SearchForTarget(On.RoR2.HuntressTracker.orig_SearchForTarget orig, HuntressTracker self, UnityEngine.Ray aimRay) {
            var search = self.search;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.maxDistanceFilter = 基础射程 + 猎人的鱼叉叠加射程 * self.characterBody.inventory.GetItemCount(DLC1Content.Items.MoveSpeedOnKill);
            search.RefreshCandidates();
            search.FilterOutGameObject(self.gameObject);
            self.trackingTarget = search.GetResults().FirstOrDefault();
        }

        private void FireSeekingArrow_FireOrbArrow(On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_FireOrbArrow orig, FireSeekingArrow self) {
            if (NetworkServer.active && self.initialOrbTarget) {
                while (self.firedArrowCount < self.maxArrowCount) {
                    var genericDamageOrb = self.CreateArrowOrb();
                    genericDamageOrb.damageValue = self.damageStat * self.orbDamageCoefficient;
                    genericDamageOrb.isCrit = self.isCrit;
                    genericDamageOrb.teamIndex = self.teamComponent.teamIndex;
                    genericDamageOrb.attacker = self.gameObject;
                    genericDamageOrb.procCoefficient = self.orbProcCoefficient;
                    genericDamageOrb.origin = self.childLocator.FindChild(self.muzzleString).position;
                    genericDamageOrb.target = self.initialOrbTarget;
                    if (self.firedArrowCount == 0) {
                        EffectManager.SimpleMuzzleFlash(self.muzzleflashEffectPrefab, self.gameObject, self.muzzleString, true);
                    }
                    OrbManager.instance.AddOrb(genericDamageOrb);
                    ++self.firedArrowCount;
                }
            }
        }

        private void HuntressTracker_FixedUpdate(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   x => x.MatchLdarg(0),
                                   x => x.MatchLdfld<HuntressTracker>("trackingTarget"),
                                   x => x.MatchPop())) {
                cursor.RemoveRange(3);
            } else {
                LogExtensions.LogError("HuntressTracker FixedUpdateHook Failed!");
            }
        }
    }
}