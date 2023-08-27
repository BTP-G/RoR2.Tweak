using AncientScepter;
using BtpTweak.Utils;
using BtpTweak.Utils.Paths;
using EntityStates.Missions.LunarScavengerEncounter;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BtpTweak {

    internal class MainMenuHook {

        public static void AddHook() {
            On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuController_Start;
        }

        public static void RemoveHook() {
            On.RoR2.UI.MainMenu.MainMenuController.Start -= MainMenuController_Start;
        }

        private static void MainMenuController_Start(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self) {
            orig(self);
            Localization.基础汉化();
            Localization.圣骑士汉化();
            Localization.探路者汉化();
            物品调整();
            LateInitAll();
            RemoveHook();
        }

        private static void 物品调整() {
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.ShockNearby);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.Thorns);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, AncientScepterItem.instance.ItemDef);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, DLC1Content.Items.ExtraLifeVoid);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.Infusion);
            ItemDef itemDef = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarWings");
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, itemDef);
            itemDef.tier = ItemTier.Lunar;
        }

        private static void LateInitAll() {
            RoR2GameObjects r2GameObjects = new();
            RoR2InteractableSpawnCards r2InteractableSpawnCards = new();
            r2InteractableSpawnCards.iscBrokenDrone1.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscBrokenDrone2.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscBrokenEmergencyDrone.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscBrokenEquipmentDrone.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscBrokenFlameDrone.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscBrokenMegaDrone.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscBrokenMissileDrone.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscGoldshoresPortal.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscScrapper.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineBlood.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineBloodSandy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineBloodSnowy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineCleanse.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineCleanseSandy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineCleanseSnowy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            r2InteractableSpawnCards.iscShrineGoldshoresAccess.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            BuffAndDotHook.LateInit();
            EffectHook.LateInit();
            FadeOut.duration = 60;
            FinalBossHook.LateInit();
            SkillHook.LateInit();
            StatHook.LateInit();
            r2GameObjects.BonusMoneyPack.LoadComponentInChildren<GravitatePickup>().maxSpeed = 50;
            ProjectileImpactExplosion projectileImpactExplosion = r2GameObjects.StickyBomb1.LoadComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.lifetime *= 0.01f;
            projectileImpactExplosion.lifetimeAfterImpact *= 0.01f;
        }
    }
}