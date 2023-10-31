using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class HuntressTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            //IL.RoR2.HuntressTracker.SearchForTarget += HuntressTracker_SearchForTarget;
            IL.RoR2.HuntressTracker.FixedUpdate += HuntressTracker_FixedUpdate;
            On.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.FireOrbArrow += FireSeekingArrow_FireOrbArrow;
        }

        public override void Load() {
            base.Load();
            CharacterBody huntressBody = RoR2Content.Survivors.Huntress.bodyPrefab.GetComponent<CharacterBody>();
            huntressBody.baseCrit = 10f;
            huntressBody.levelCrit = 1f;
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
            if (cursor.TryGotoNext(MoveType.Before, x => ILPatternMatchingExt.MatchCall<HuntressTracker>(x, "SearchForTarget"))) {
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.EmitDelegate((HuntressTracker huntressTracker) => {
                    HuntressAutoaimFix.Main.maxTrackingDistance.Value = 60f + 10f * huntressTracker.characterBody.inventory.GetItemCount(DLC1Content.Items.MoveSpeedOnKill.itemIndex);
                });
            } else {
                Main.Logger.LogError("HuntressTracker FixedUpdateHook Failed!");
            }
        }

        //private void HuntressTracker_SearchForTarget(ILContext il) {
        //    ILCursor cursor = new(il);
        //    if (cursor.TryGotoNext(MoveType.Before, x => ILPatternMatchingExt.MatchStfld<BullseyeSearch>(x, "maxDistanceFilter"))) {
        //        cursor.Emit(OpCodes.Ldarg_0);
        //        cursor.EmitDelegate((HuntressTracker huntressTracker) => {
        //            return ModConfig.女猎人射程每级增加距离.Value * (huntressTracker.characterBody.level - 1);
        //        });
        //        cursor.Emit(OpCodes.Add);
        //    } else {
        //        Main.Logger.LogError("HuntressTracker SearchForTargetHook Failed!");
        //    }
        //}
    }
}