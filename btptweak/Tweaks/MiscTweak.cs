using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
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
            On.EntityStates.BaseState.RollCrit += BaseState_RollCrit;
            On.RoR2.CharacterBody.RollCrit += CharacterBody_RollCrit;
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster += Util_CheckRoll_float_float_CharacterMaster;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            RoR2Application.onLoad += Load;
        }

        public override void ClearEventHandlers() {
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter -= FadeOut_OnEnter;
            On.EntityStates.StunState.PlayStunAnimation -= StunState_PlayStunAnimation;
            On.EntityStates.BaseState.RollCrit -= BaseState_RollCrit;
            On.RoR2.CharacterBody.RollCrit -= CharacterBody_RollCrit;
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster -= Util_CheckRoll_float_float_CharacterMaster;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
            RoR2Application.onLoad -= Load;
            SetLunarWingsState(false);
        }

        public void Load() {
            _特拉法梅的祝福 = ItemDefPaths.LunarWings.Load<ItemDef>();
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

        internal static void SetLunarWingsState(bool enable) {
            if (TPDespair.ZetAspects.Configuration.AspectEquipmentConversion == null
                || ZetArtifactsPlugin.DropifactVoidT1 == null) {
                return;
            }
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

        private bool CharacterBody_RollCrit(On.RoR2.CharacterBody.orig_RollCrit orig, CharacterBody self) {
            return Util.CheckRoll(self.crit, self.master);
        }

        private bool BaseState_RollCrit(On.EntityStates.BaseState.orig_RollCrit orig, EntityStates.BaseState self) {
            return self.outer.commonComponents.characterBody?.RollCrit() ?? false;
        }

        private void FadeOut_OnEnter(On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.orig_OnEnter orig, FadeOut self) {
            orig(self);
            foreach (var player in PlayerCharacterMasterController.instances) {
                if (player.master.inventory.GetItemCount(_特拉法梅的祝福) > 0) {
                    SetLunarWingsState(true);
                    if (NetworkServer.active) {
                        ChatMessage.Send(player.GetDisplayName().ToLunar() + "已转化<style=cIsLunar>特拉法梅的祝福(过去时->完成时)</style>");
                    }
                }
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
                var flag = false;
                foreach (var player in PlayerCharacterMasterController.instances) {
                    var inventory = player.master.inventory;
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.TonicAffliction);
                    if (itemCount > 0) {
                        inventory.RemoveItem(RoR2Content.Items.TonicAffliction, itemCount);
                        ChatMessage.Send($"已移除{player.GetDisplayName().ToLunar()}强心剂副作用！");
                    }
                    if (RunInfo.CurrentSceneIndex == SceneIndexes.BulwarksHaunt_GhostWave) {
                        inventory.GiveItem(_特拉法梅的祝福);
                        ChatMessage.Send(player.GetDisplayName() + "已获得<style=cIsLunar>特拉法梅的祝福(过去时)</style>");
                        flag = true;
                    }
                }
                if (flag) {
                    BtpUtils.SpawnLunarPortal(self.transform.position);
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