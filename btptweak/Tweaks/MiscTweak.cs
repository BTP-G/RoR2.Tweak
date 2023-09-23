using BtpTweak.Utils;
using EntityStates.Missions.LunarScavengerEncounter;
using R2API.Utils;
using RoR2;
using TPDespair.ZetArtifacts;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks
{

    internal class MiscTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter += FadeOut_OnEnter;
        }

        public override void Load() {
            base.Load();
            FadeOut.duration = 60;
            "RoR2/Base/Scrapper/iscScrapper.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineCleanse/iscShrineCleanse.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineCleanse/iscShrineCleanseSandy.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineCleanse/iscShrineCleanseSnowy.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineGoldshoresAccess/iscShrineGoldshoresAccess.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
        }

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            TPDespair.ZetAspects.Configuration.AspectEquipmentConversion.Value = false;
            TPDespair.ZetAspects.Configuration.AspectEquipmentAbsorb.Value = false;
            ZetArtifactsPlugin.DropifactVoidT1.Value = false;
            ZetArtifactsPlugin.DropifactVoidT2.Value = false;
            ZetArtifactsPlugin.DropifactVoidT3.Value = false;
            ZetArtifactsPlugin.DropifactVoid.Value = false;
            ZetArtifactsPlugin.DropifactLunar.Value = false;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = false;
        }

        private void FadeOut_OnEnter(On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.orig_OnEnter orig, FadeOut self) {
            orig(self);
            if (NetworkServer.active) {
                foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances) {
                    ChatMessage.Send(player.GetDisplayName() + "已获得<style=cIsLunar>特拉法梅的祝福</style>");
                    player.master.inventory.GiveItem(LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarWings").itemIndex);
                }
            }
            TPDespair.ZetAspects.Configuration.AspectEquipmentConversion.Value = true;
            TPDespair.ZetAspects.Configuration.AspectEquipmentAbsorb.Value = true;
            ZetArtifactsPlugin.DropifactVoidT1.Value = true;
            ZetArtifactsPlugin.DropifactVoidT2.Value = true;
            ZetArtifactsPlugin.DropifactVoidT3.Value = true;
            ZetArtifactsPlugin.DropifactVoid.Value = true;
            ZetArtifactsPlugin.DropifactLunar.Value = true;
            ZetArtifactsPlugin.DropifactVoidLunar.Value = true;
        }

        private void TrueDeathState_OnEnter(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (Main.是否选择造物难度_ && Main.往日不再_ == false) {
                if (NetworkServer.active) {
                    ChatMessage.SendColored("--世界不再是你熟悉的那样！！！", Color.red);
                }
                Main.往日不再_ = true;
            }
            if (NetworkServer.active) {
                foreach (var player in PlayerCharacterMasterController.instances) {
                    Inventory inventory = player.master.inventory;
                    inventory.RemoveItem(RoR2Content.Items.TonicAffliction, inventory.GetItemCount(RoR2Content.Items.TonicAffliction));
                    ChatMessage.SendColored($"已移除{player.body?.GetUserName()}强心剂副作用！", Color.blue);
                }
            }
        }
    }
}