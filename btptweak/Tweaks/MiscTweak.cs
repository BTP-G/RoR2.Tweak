using BtpTweak.Utils;
using EntityStates.Missions.LunarScavengerEncounter;
using R2API.Utils;
using RoR2;
using TPDespair.ZetArtifacts;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class MiscTweak : TweakBase {
        private ItemIndex _特拉法梅的祝福;

        public override void AddHooks() {
            base.AddHooks();
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter += FadeOut_OnEnter;
            On.EntityStates.StunState.PlayStunAnimation += StunState_PlayStunAnimation;
            On.RoR2.Run.BeginGameOver += Run_BeginGameOver;
        }

        public override void Load() {
            base.Load();
            _特拉法梅的祝福 = "RoR2/DLC1/LunarWings/LunarWings.asset".Load<ItemDef>().itemIndex;
            FadeOut.duration = 60;
        }

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            SetLunarWingsState(false);
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody body) {
            if (body.isPlayerControlled && body.inventory.GetItemCount(_特拉法梅的祝福) == 2) {
                SetLunarWingsState(true);
            }
        }

        private void FadeOut_OnEnter(On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.orig_OnEnter orig, FadeOut self) {
            orig(self);
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances) {
                var inventory = player.master.inventory;
                if (inventory?.GetItemCount(_特拉法梅的祝福) == 1) {
                    if (NetworkServer.active) {
                        inventory.GiveItem(_特拉法梅的祝福);
                        ChatMessage.Send(player.GetDisplayName() + "已转化<style=cIsLunar>特拉法梅的祝福(过去时->完成时)</style>");
                    }
                    SetLunarWingsState(true);
                }
            }
        }

        private void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEndingDef) {
            if (gameEndingDef == BulwarksHaunt.BulwarksHauntContent.GameEndings.BulwarksHaunt_HauntedEnding) {
                if (NetworkServer.active) {
                    foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances) {
                        var inventory = player.master.inventory;
                        if (inventory?.GetItemCount(_特拉法梅的祝福) == 0) {
                            inventory.GiveItem(_特拉法梅的祝福);
                            ChatMessage.Send(player.GetDisplayName() + "已获得<style=cIsLunar>特拉法梅的祝福(过去时)</style>");
                        }
                    }
                }
                if (Run.instance.nextStageScene) {
                    Run.instance.AdvanceStage(Run.instance.nextStageScene);
                } else if (Stage.instance.nextStage) {
                    Run.instance.AdvanceStage(Stage.instance.nextStage);
                } else {
                    Run.instance.AdvanceStage(self.runRNG.NextElementUniform(SceneCatalog.allStageSceneDefs));
                }
                return;
            }
            orig(self, gameEndingDef);
        }

        private void SetLunarWingsState(bool enable) {
            TPDespair.ZetAspects.Configuration.AspectEquipmentAbsorb.Value = enable;
            ZetArtifactsPlugin.DropifactVoidT1.Value = enable;
            ZetArtifactsPlugin.DropifactVoidT2.Value = enable;
            ZetArtifactsPlugin.DropifactVoidT3.Value = enable;
            ZetArtifactsPlugin.DropifactVoid.Value = enable;
            ZetArtifactsPlugin.DropifactLunar.Value = enable;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = enable;
            if (enable) {
                R2API.LanguageAPI.AddOverlay("ITEM_LUNARWINGS_DESC", $"{"工匠·完成时".ToLunar()}：掌握对{"虚空".ToVoid()}和{"月球".ToLunar()}物品的丢弃权。重复获得{"相同象征".ToUtil()}装备可将其{"转化为物品".ToUtil()}。", "zh-CN");
            } else {
                R2API.LanguageAPI.AddOverlay("ITEM_LUNARWINGS_DESC", $"{"工匠·过去时".ToLunar()}：随着{"时间".ToUtil()}流逝已经丧失了全部力量，{"或许在某个地方可以恢复...".ToDeath()}。", "zh-CN");
            }
        }

        private void StunState_PlayStunAnimation(On.EntityStates.StunState.orig_PlayStunAnimation orig, EntityStates.StunState self) {
            Animator modelAnimator = self.GetModelAnimator();
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
            if (GlobalInfo.是否选择造物难度 && GlobalInfo.往日不再 == false) {
                GlobalInfo.往日不再 = true;
                if (NetworkServer.active) {
                    ChatMessage.Send("--世界不再是你熟悉的那样！！！".ToLunar());
                }
            }
            if (NetworkServer.active) {
                foreach (var player in PlayerCharacterMasterController.instances) {
                    Inventory inventory = player.master.inventory;
                    int itemCount = inventory?.GetItemCount(RoR2Content.Items.TonicAffliction) ?? 0;
                    if (itemCount > 0) {
                        inventory.RemoveItem(RoR2Content.Items.TonicAffliction, itemCount);
                        ChatMessage.Send($"已移除{player.GetDisplayName().ToLunar()}强心剂副作用！");
                    }
                }
            }
        }
    }
}