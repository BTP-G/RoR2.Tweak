using BtpTweak.Utils;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using System.Linq;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class HuntressTweak : TweakBase<HuntressTweak> {
        public const float 基础射程 = 60f;
        public const float 猎人的鱼叉叠加射程 = 10f;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            IL.RoR2.HuntressTracker.FixedUpdate += HuntressTracker_FixedUpdate;
            On.RoR2.HuntressTracker.Start += HuntressTracker_Start;
            On.RoR2.HuntressTracker.SearchForTarget += HuntressTracker_SearchForTarget;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.FireOrbArrow += FireSeekingArrow_FireOrbArrow;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            IL.RoR2.HuntressTracker.FixedUpdate -= HuntressTracker_FixedUpdate;
            On.RoR2.HuntressTracker.Start -= HuntressTracker_Start;
            On.RoR2.HuntressTracker.SearchForTarget -= HuntressTracker_SearchForTarget;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.FireOrbArrow -= FireSeekingArrow_FireOrbArrow;
        }

        public void Load() {
            CharacterBody huntressBody = RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<CharacterBody>();
            huntressBody.baseCrit = 5f;
            huntressBody.levelCrit = 1f;
            RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<HuntressTracker>().trackerUpdateFrequency = 5;
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

        private void FireSeekingArrow_FireOrbArrow(On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.orig_FireOrbArrow orig, EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self) {
            if (NetworkServer.active && self.initialOrbTarget != null) {
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
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   x => x.MatchLdarg(0),
                                   x => x.MatchLdfld<HuntressTracker>("trackingTarget"),
                                   x => x.MatchPop())) {
                cursor.RemoveRange(3);
            } else {
                Main.Logger.LogError("HuntressTracker FixedUpdateHook Failed!");
            }
        }
    }
}