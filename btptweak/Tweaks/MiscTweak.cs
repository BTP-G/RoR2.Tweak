using EntityStates.Missions.LunarScavengerEncounter;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class MiscTweak : TweakBase<MiscTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.StunState.PlayStunAnimation += StunState_PlayStunAnimation;
            On.EntityStates.BaseState.RollCrit += BaseState_RollCrit;
            On.RoR2.CharacterBody.RollCrit += CharacterBody_RollCrit;
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster += Util_CheckRoll_float_float_CharacterMaster;
            On.RoR2.LevelUpEffectManager.OnCharacterLevelUp += LevelUpEffectManager_OnCharacterLevelUp;
            On.RoR2.LevelUpEffectManager.OnTeamLevelUp += LevelUpEffectManager_OnTeamLevelUp;
            On.RoR2.LevelUpEffectManager.OnRunAmbientLevelUp += LevelUpEffectManager_OnRunAmbientLevelUp;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            PickupDropletController.pickupDropletPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(5f);
            GenericPickupController.pickupPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(100f);
            FadeOut.duration = 60f;
            EntityStates.BrotherMonster.SpellChannelState.maxDuration = 180f;
        }

        public void Stage_onStageStartGlobal(Stage stage) {
            BtpContent.Difficulties.造物.scalingValue = 3f + Run.instance.stageClearCount * ModConfig.每关难度增加量.Value / 50f;
        }

        private void LevelUpEffectManager_OnCharacterLevelUp(On.RoR2.LevelUpEffectManager.orig_OnCharacterLevelUp orig, CharacterBody characterBody) {
            if (characterBody.level > 99) {
                return;
            }
            orig(characterBody);
        }

        private void LevelUpEffectManager_OnTeamLevelUp(On.RoR2.LevelUpEffectManager.orig_OnTeamLevelUp orig, TeamIndex teamIndex) {
            if (TeamManager.instance.GetTeamLevel(teamIndex) > 99) {
                return;
            }
            orig(teamIndex);
        }

        private void LevelUpEffectManager_OnRunAmbientLevelUp(On.RoR2.LevelUpEffectManager.orig_OnRunAmbientLevelUp orig, Run run) {
            if (run.ambientLevel > 99) {
                return;
            }
            orig(run);
        }

        private bool CharacterBody_RollCrit(On.RoR2.CharacterBody.orig_RollCrit orig, CharacterBody self) {
            return Util.CheckRoll(self.crit, self.master);
        }

        private bool BaseState_RollCrit(On.EntityStates.BaseState.orig_RollCrit orig, EntityStates.BaseState self) {
            return self.outer.commonComponents.characterBody?.RollCrit() ?? false;
        }

        private void StunState_PlayStunAnimation(On.EntityStates.StunState.orig_PlayStunAnimation orig, EntityStates.StunState self) {
            var modelAnimator = self.GetModelAnimator();
            if (modelAnimator) {
                int layerIndex = modelAnimator.GetLayerIndex("Body");
                modelAnimator.CrossFadeInFixedTime((Random.Range(0, 2) == 0) ? "Hurt1" : "Hurt2", 0.1f);
                modelAnimator.Update(0f);
                AnimatorStateInfo nextAnimatorStateInfo = modelAnimator.GetNextAnimatorStateInfo(layerIndex);
                self.duration = Mathf.Max(self.stunDuration, nextAnimatorStateInfo.length);
                if (self.stunDuration >= 0f && self.stunVfxInstance == null) {
                    self.stunVfxInstance = Object.Instantiate(EntityStates.StunState.stunVfxPrefab, self.transform);
                    self.stunVfxInstance.GetComponent<ScaleParticleSystemDuration>().newDuration = self.duration;
                }
            }
        }

        private void TrueDeathState_OnEnter(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (NetworkServer.active) {
                foreach (var player in PlayerCharacterMasterController.instances) {
                    var inventory = player.master.inventory;
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex);
                    if (itemCount > 0) {
                        inventory.RemoveItem(RoR2Content.Items.TonicAffliction.itemIndex, itemCount);
                    }
                }
            }
        }

        private bool Util_CheckRoll_float_float_CharacterMaster(On.RoR2.Util.orig_CheckRoll_float_float_CharacterMaster orig, float percentChance, float luck, CharacterMaster effectOriginMaster) {
            if (percentChance > 0f) {
                float random = Random.Range(0f, 100f);
                if (random <= percentChance + percentChance * (luck / (luck > 0 ? 3f + luck : 3f - luck))) {
                    if (random > percentChance && effectOriginMaster) {
                        var body = effectOriginMaster.GetBody();
                        if (body) {
                            body.wasLucky = true;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}