using BtpTweak.Utils;
using EntityStates.Missions.LunarScavengerEncounter;
using R2API.Utils;
using RoR2;
using TPDespair.ZetArtifacts;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class MiscTweak : TweakBase<MiscTweak> {
        private ItemDef _特拉法梅的祝福;

        public override void SetEventHandlers() {
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter += FadeOut_OnEnter;
            On.EntityStates.StunState.PlayStunAnimation += StunState_PlayStunAnimation;
            On.RoR2.Run.BeginGameOver += Run_BeginGameOver;
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster += Util_CheckRoll_float_float_CharacterMaster;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            RoR2Application.onLoad += Load;
        }

        public override void ClearEventHandlers() {
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter -= FadeOut_OnEnter;
            On.EntityStates.StunState.PlayStunAnimation -= StunState_PlayStunAnimation;
            On.RoR2.Run.BeginGameOver -= Run_BeginGameOver;
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster -= Util_CheckRoll_float_float_CharacterMaster;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
            RoR2Application.onLoad -= Load;
        }

        public void Load() {
            _特拉法梅的祝福 = "RoR2/DLC1/LunarWings/LunarWings.asset".Load<ItemDef>();
            _特拉法梅的祝福.deprecatedTier = ItemTier.Lunar;
            _特拉法梅的祝福.tier = ItemTier.Lunar;
            _特拉法梅的祝福.canRemove = false;
            _特拉法梅的祝福.tags = new ItemTag[] {
                ItemTag.CannotCopy,
                ItemTag.CannotDuplicate,
                ItemTag.CannotSteal,
                ItemTag.Utility,
                ItemTag.WorldUnique,
            };
            var pickupDef = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(_特拉法梅的祝福.itemIndex));
            var itemTierDef = ItemTierCatalog.GetItemTierDef(_特拉法梅的祝福.tier);
            pickupDef.baseColor = ColorCatalog.GetColor(itemTierDef.colorIndex);
            pickupDef.darkColor = ColorCatalog.GetColor(itemTierDef.darkColorIndex);
            pickupDef.dropletDisplayPrefab = itemTierDef.dropletDisplayPrefab;
            pickupDef.isLunar = _特拉法梅的祝福.tier == ItemTier.Lunar;
            pickupDef.itemTier = _特拉法梅的祝福.tier;
            PickupDropletController.pickupDropletPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(5f);
            GenericPickupController.pickupPrefab.AddComponent<AutoTeleportGameObject>().SetTeleportWaitingTime(100f);
            FadeOut.duration = 60f;
            EntityStates.BrotherMonster.SpellChannelState.maxDuration = 180f;
        }

        public void Run_onRunStartGlobal(Run run) {
            SetLunarWingsState(false);
        }

        public void Stage_onStageStartGlobal(Stage stage) {
            BulwarksHaunt.GhostWave.maxWaves = Run.instance.stageClearCount + 1;
        }

        private void FadeOut_OnEnter(On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.orig_OnEnter orig, FadeOut self) {
            orig(self);
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances) {
                if (player.master.inventory.GetItemCount(_特拉法梅的祝福) > 0) {
                    SetLunarWingsState(true);
                    if (NetworkServer.active) {
                        ChatMessage.Send(player.GetDisplayName().ToLunar() + "已转化<style=cIsLunar>特拉法梅的祝福(过去时->完成时)</style>");
                    }
                }
            }
        }

        private void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEndingDef) {
            if (gameEndingDef == BulwarksHaunt.BulwarksHauntContent.GameEndings.BulwarksHaunt_HauntedEnding) {
                if (NetworkServer.active) {
                    foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances) {
                        player.master.inventory.GiveItem(_特拉法梅的祝福);
                        ChatMessage.Send(player.GetDisplayName() + "已获得<style=cIsLunar>特拉法梅的祝福(过去时)</style>");
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
            TPDespair.ZetAspects.Configuration.AspectEquipmentConversion.Value = enable;
            ZetArtifactsPlugin.DropifactVoidT1.Value = enable;
            ZetArtifactsPlugin.DropifactVoidT2.Value = enable;
            ZetArtifactsPlugin.DropifactVoidT3.Value = enable;
            ZetArtifactsPlugin.DropifactVoid.Value = enable;
            ZetArtifactsPlugin.DropifactLunar.Value = enable;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = enable;
            if (enable) {
                R2API.LanguageAPI.AddOverlay("ITEM_LUNARWINGS_DESC", $"{"工匠·完成时".ToLunar()}：" + (ZetDropifact.Enabled ? $"掌握对{"月球".ToLunar()}和{"虚空".ToVoid()}物品的丢弃权。" : "") + $"{"右键点击象征（右下角）".ToUtil()}装备可将其{"转化为物品".ToUtil()}。", "zh-CN");
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
            if (NetworkServer.active) {
                foreach (var player in PlayerCharacterMasterController.instances) {
                    Inventory inventory = player.master.inventory;
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.TonicAffliction);
                    if (itemCount > 0) {
                        inventory.RemoveItem(RoR2Content.Items.TonicAffliction, itemCount);
                        ChatMessage.Send($"已移除{player.GetDisplayName().ToLunar()}强心剂副作用！");
                    }
                }
            }
        }

        private bool Util_CheckRoll_float_float_CharacterMaster(On.RoR2.Util.orig_CheckRoll_float_float_CharacterMaster orig, float percentChance, float luck, CharacterMaster effectOriginMaster) {
            if (percentChance <= 0f) {
                return false;
            }
            float random = Random.Range(0f, 100f);
            if (random <= percentChance + percentChance * (luck / (Mathf.Abs(luck) + 3f))) {
                if (random > percentChance && effectOriginMaster) {
                    var body = effectOriginMaster.GetBody();
                    if (body) {
                        body.wasLucky = true;
                    }
                }
                return true;
            } else {
                return false;
            }
        }
    }
}