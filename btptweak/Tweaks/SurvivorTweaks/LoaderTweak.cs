using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using HIFULoaderTweaks.Skills;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class LoaderTweak : TweakBase<LoaderTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.EntityStates.Loader.SwingZapFist.OnExit += SwingZapFist_OnExit;
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter += BaseSwingChargedFist_OnEnter;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.EntityStates.Loader.SwingZapFist.OnExit -= SwingZapFist_OnExit;
            On.EntityStates.Loader.BaseSwingChargedFist.OnEnter -= BaseSwingChargedFist_OnEnter;
        }

        public void Load() {
            var loaderBody = RoR2Content.Survivors.Loader.bodyPrefab.GetComponent<CharacterBody>();
            loaderBody.baseAcceleration *= 2f;
            GameObjectPaths.LoaderPylon.Load<GameObject>().AddComponent<M551PylonStartAction>();  // 电塔
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
        }

        private void BaseSwingChargedFist_OnEnter(On.EntityStates.Loader.BaseSwingChargedFist.orig_OnEnter orig, EntityStates.Loader.BaseSwingChargedFist self) {
            if (self is EntityStates.Loader.SwingChargedFist) {
                self.procCoefficient = Mathf.Lerp(0.6f, 2.7f, self.charge);
            } else {
                self.procCoefficient = 2.1f;
            }
            orig(self);
        }

        private void SwingZapFist_OnExit(On.EntityStates.Loader.SwingZapFist.orig_OnExit orig, EntityStates.Loader.SwingZapFist self) {
            EntityStates.Loader.SwingZapFist.selfKnockback = 100 * self.punchSpeed;
            orig(self);
        }

        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileProximityBeamController))]
        private class M551PylonStartAction : MonoBehaviour {

            public void Start() {
                var inventory = GetComponent<ProjectileController>().owner?.GetComponent<CharacterBody>().inventory;
                if (inventory) {
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.ShockNearby.itemIndex);
                    var proximityBeamController = GetComponent<ProjectileProximityBeamController>();
                    proximityBeamController.attackFireCount += itemCount;
                    proximityBeamController.attackRange += M551Pylon.aoe * itemCount;
                    proximityBeamController.bounces += itemCount;
                }
            }

            private void Awake() {
                enabled = NetworkServer.active;
            }
        }
    }
}