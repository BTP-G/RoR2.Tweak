using EntityStates.Missions.LunarScavengerEncounter;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks {

    internal class MiscTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.StunState.PlayStunAnimation += StunState_PlayStunAnimation;
            On.EntityStates.BaseState.RollCrit += BaseState_RollCrit;
            On.RoR2.CharacterBody.RollCrit += CharacterBody_RollCrit;
            IL.RoR2.CharacterMaster.GiveMoney += CharacterMaster_GiveMoney;
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster += Util_CheckRoll_float_float_CharacterMaster;
            On.RoR2.LevelUpEffectManager.OnCharacterLevelUp += LevelUpEffectManager_OnCharacterLevelUp;
            On.RoR2.LevelUpEffectManager.OnTeamLevelUp += LevelUpEffectManager_OnTeamLevelUp;
            On.RoR2.LevelUpEffectManager.OnRunAmbientLevelUp += LevelUpEffectManager_OnRunAmbientLevelUp;
            IL.RoR2.Run.RecalculateDifficultyCoefficentInternal += Run_RecalculateDifficultyCoefficentInternal;
            On.RoR2.CharacterMasterNotificationQueue.PushNotification += CharacterMasterNotificationQueue_PushNotification;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            FadeOut.duration = 60f;
            EntityStates.BrotherMonster.SpellChannelState.maxDuration = 180f;
        }

        private void CharacterMaster_GiveMoney(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchAdd())) {
                cursor.Remove();
                cursor.EmitDelegate(HGMath.UintSafeAdd);
            } else {
                LogExtensions.LogError("CharacterMaster_GiveMoney Hook Failed!");
            }
        }

        private void Run_RecalculateDifficultyCoefficentInternal(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(c => c.MatchLdloc(7),
                                   c => c.MatchLdloc(0))) {
                cursor.Index += 1;
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Ldloc, 6);
                cursor.EmitDelegate((Run run, float num6) => RunInfo.已选择造物难度 ? 0.0506f * Settings.每关难度增加量.Value / 50f * run.stageClearCount * num6 : 0f);
                cursor.Emit(OpCodes.Add);
            } else {
                LogExtensions.LogError("Run_RecalculateDifficultyCoefficentInternal Hook Failed!");
            }
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
                var layerIndex = modelAnimator.GetLayerIndex("Body");
                modelAnimator.CrossFadeInFixedTime(Random.Range(0, 2) == 0 ? "Hurt1" : "Hurt2", 0.1f);
                modelAnimator.Update(0f);
                var nextAnimatorStateInfo = modelAnimator.GetNextAnimatorStateInfo(layerIndex);
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
                    var itemCount = inventory.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex);
                    if (itemCount > 0) {
                        inventory.RemoveItem(RoR2Content.Items.TonicAffliction.itemIndex, itemCount);
                    }
                }
            }
        }

        private bool Util_CheckRoll_float_float_CharacterMaster(On.RoR2.Util.orig_CheckRoll_float_float_CharacterMaster orig, float percentChance, float luck, CharacterMaster effectOriginMaster) {
            if (percentChance > 0f) {
                var random = Random.Range(0f, 100f);
                if (random <= percentChance + percentChance * (luck / (luck > 0 ? 3f + luck : 3f - luck))) {
                    if (random > percentChance && effectOriginMaster) {
                        var body = effectOriginMaster.GetBody();
                        if (body != null) {
                            body.wasLucky = true;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private void CharacterMasterNotificationQueue_PushNotification(On.RoR2.CharacterMasterNotificationQueue.orig_PushNotification orig, CharacterMasterNotificationQueue self, CharacterMasterNotificationQueue.NotificationInfo info, float duration) {
            var notification = self.notifications.FirstOrDefault();
            if (notification != null && notification.notification != info) {
                notification.duration = 0f;
            }
            orig(self, info, duration);
        }
    }
}