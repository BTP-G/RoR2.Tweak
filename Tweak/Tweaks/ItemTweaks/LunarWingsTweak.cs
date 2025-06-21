using BTP.RoR2Plugin.Language;
using BTP.RoR2Plugin.Messages;
using BTP.RoR2Plugin.Utils;
using R2API.Utils;
using RoR2;
using System;
using System.Runtime.Serialization;
using TPDespair.ZetArtifacts;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    public enum LunarWingsState : byte {
        Default,
        过去时,
        过去完成时,
        现在时,
        现在完成时,
        将来时,
        将来完成时,
        Count,
    }

    [Obsolete]
    internal class LunarWingsTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const string DescToken = "ITEM_LUNARWINGS_DESC";
        public const string DefaultDesc = "默认：看起来只是一双翅膀。";
        private static LunarWingsState _state = LunarWingsState.Default;
        public static string PastDesc => $"{"过去时".ToStk()}：看起来只是一双翅膀。寄宿在里面的{"亡灵".ToDeath()}在你耳边低语：“{"离开...".ToDeath()}”\n无效果。";
        public static string PastPerfectDesc => $"{"过去完成时".ToLunar()}：看起来这双翅膀对某个敌人有反应。寄宿在里面的{"亡灵".ToDeath()}在你耳边低语：“{"力量...".ToDmg()}”\n{"全属性上升".ToYellow() + 1.ToBaseAndStkPct().ToYellow()}。";
        public static string PresentDesc => $"{"现在时".ToStk()}：看起来这双翅膀十分活跃。寄宿在里面的{"亡灵".ToDeath()}在你耳边低语：“{"时间...".ToUtil()}”\n{"或许它在提示你...".ToStk()}";

        public static string PresentPrefectDesc => $"{"现在完成时".ToGold()}：看起来这双翅膀拥有无尽的知识。寄宿在里面的{"亡灵".ToDeath()}在你耳边低语：“{"知识..."}”\n"
            + (ZetDropifact.Enabled ? $"掌握对{"月球".ToLunar()}和{"虚空".ToVoid()}物品的丢弃权。" : "")
            + $"{"右键点击象征（右下角）".ToUtil()}装备可将其{"转化为物品".ToUtil()}。";

        public static string FutureDesc => $"{"将来时"}：敬请期待。";
        public static string FuturePerfectDesc => $"{"将来完成时"}：敬请期待。";
        public static ItemDef 特拉法梅的祝福 { get; private set; }

        void IModLoadMessageHandler.Handle() {
            Run.onRunStartGlobal += OnRunStartGlobal;
            CharacterBody.onBodyInventoryChangedGlobal += CharacterBody_onBodyInventoryChangedGlobal;
            ProperSaveSupport.AddSaveDataType<SaveData>();
        }

        void IRoR2LoadedMessageHandler.Handle() {
            特拉法梅的祝福 = ItemDefPaths.LunarWings.Load<ItemDef>();
            特拉法梅的祝福.tier = ItemTier.Lunar;
            特拉法梅的祝福.canRemove = false;
            特拉法梅的祝福.tags = [
                ItemTag.CannotCopy,
                ItemTag.CannotDuplicate,
                ItemTag.CannotSteal,
                ItemTag.Utility,
                ItemTag.WorldUnique,
            ];
            var pickupDef = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(特拉法梅的祝福.itemIndex));
            var itemTierDef = ItemTierCatalog.GetItemTierDef(特拉法梅的祝福.tier);
            pickupDef.baseColor = ColorCatalog.GetColor(itemTierDef.colorIndex);
            pickupDef.darkColor = ColorCatalog.GetColor(itemTierDef.darkColorIndex);
            pickupDef.dropletDisplayPrefab = itemTierDef.dropletDisplayPrefab;
            pickupDef.isLunar = 特拉法梅的祝福.tier == ItemTier.Lunar;
            pickupDef.itemTier = 特拉法梅的祝福.tier;
            DescToken.AddOverlay(DefaultDesc);
        }

        internal static void UpgradeLunarWings(LunarWingsState newState) {
            if (_state < newState) {
                UpdateLunarWingsState(newState);
                if (NetworkServer.active) {
                    ChatMessage.Send($"你感觉耳边响起了{"亡灵".ToDeath()}的低语。");
                }
            }
        }

        internal static void UpdateLunarWingsState(LunarWingsState newState) {
            Reset();
            switch (newState) {
                case LunarWingsState.Default:
                    DescToken.AddOverlay(DefaultDesc);
                    break;

                case LunarWingsState.过去时:
                    DescToken.AddOverlay(PastDesc);
                    break;

                case LunarWingsState.过去完成时:
                    DescToken.AddOverlay(PastPerfectDesc);
                    R2API.RecalculateStatsAPI.GetStatCoefficients += 全属性上升Hook;
                    break;

                case LunarWingsState.现在时:
                    DescToken.AddOverlay(PresentDesc);
                    break;

                case LunarWingsState.现在完成时:
                    DescToken.AddOverlay(PresentPrefectDesc);
                    TPDespair.ZetAspects.Configuration.AspectEquipmentConversion.Value = true;
                    ZetArtifactsPlugin.DropifactVoidT1.Value = true;
                    ZetArtifactsPlugin.DropifactVoidT2.Value = true;
                    ZetArtifactsPlugin.DropifactVoidT3.Value = true;
                    ZetArtifactsPlugin.DropifactVoid.Value = true;
                    ZetArtifactsPlugin.DropifactLunar.Value = true;
                    ZetArtifactsPlugin.DropifactVoidLunar.Value = true;
                    break;

                case LunarWingsState.将来时:
                case LunarWingsState.将来完成时:
                    DescToken.AddOverlay(FutureDesc);
                    break;

                default:
                    break;
            }
            LogExtensions.LogMessage($"LunarWingsState changed {_state} => {newState}");
            _state = newState;
        }

        private static void Reset() {
            TPDespair.ZetAspects.Configuration.AspectEquipmentConversion.Value = false;
            ZetArtifactsPlugin.DropifactVoidT1.Value = false;
            ZetArtifactsPlugin.DropifactVoidT2.Value = false;
            ZetArtifactsPlugin.DropifactVoidT3.Value = false;
            ZetArtifactsPlugin.DropifactVoid.Value = false;
            ZetArtifactsPlugin.DropifactLunar.Value = false;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = false;
            R2API.RecalculateStatsAPI.GetStatCoefficients -= 全属性上升Hook;
        }

        private static void 全属性上升Hook(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            if (sender.inventory) {
                var itemCount = sender.inventory.GetItemCount(特拉法梅的祝福);
                if (itemCount > 0) {
                    args.attackSpeedMultAdd += itemCount;
                    args.damageMultAdd += itemCount;
                    args.healthMultAdd += itemCount;
                    args.levelArmorAdd += itemCount;
                    args.moveSpeedMultAdd += itemCount;
                    args.regenMultAdd += itemCount;
                }
            }
        }

        private void CharacterBody_onBodyInventoryChangedGlobal(CharacterBody body) {
            if (body.hasAuthority) {
                body.AddItemBehavior<LunarWingsBehavior>(body.inventory.GetItemCount(特拉法梅的祝福));
            }
        }

        private void OnRunStartGlobal(Run run) {
            UpdateLunarWingsState(LunarWingsState.Default);
        }

        private struct SaveData : ISaveData {

            [DataMember(Name = "s")]
            public byte _state_sd;

            readonly void ISaveData.LoadData() {
                var state = (LunarWingsState)_state_sd;
                UpdateLunarWingsState(state);
                state.同步特拉法梅的祝福();
            }

            void ISaveData.SaveData() {
                _state_sd = (byte)_state;
            }
        }

        public class LunarWingsBehavior : CharacterBody.ItemBehavior {
            public static LunarWingsBehavior Instance { get; private set; }

            private void Start() {
                if (Instance) {
                    LogExtensions.LogError("Singleton class 'LunarWingsBehavior' was instantiated twice!!!");
                }
                Instance = this;
                UpgradeLunarWings(LunarWingsState.过去时);
                if (!RunInfo.位于时之墓) {
                    UpgradeLunarWings(LunarWingsState.现在时);
                }
                if (_state < LunarWingsState.现在完成时) {
                    On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter += FadeOut_OnEnter;
                }
            }

            private void FadeOut_OnEnter(On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.orig_OnEnter orig, EntityStates.Missions.LunarScavengerEncounter.FadeOut self) {
                orig(self);
                On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter -= FadeOut_OnEnter;
                UpgradeLunarWings(LunarWingsState.现在完成时);
            }

            private void OnDestroy() {
                Instance = null;
                On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter -= FadeOut_OnEnter;
            }
        }
    }
}