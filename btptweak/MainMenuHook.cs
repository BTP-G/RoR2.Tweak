using AncientScepter;
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
            Helpers.基础汉化();
            Helpers.圣骑士汉化();
            Helpers.探路者汉化();
            物品调整();
            其他调整();
            RemoveHook();
        }

        private static void 物品调整() {
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.Infusion);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            R2API.ItemAPI.ApplyTagToItem(ItemTag.CannotSteal, AncientScepterItem.instance.ItemDef);
        }

        private static void 其他调整() {
            Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/Scrapper/iscScrapper.asset").WaitForCompletion().maxSpawnsPerStage = 1;
            BuffAndDotHook.LateInit();
            EffectHook.LateInit();
            FinalBossHook.LateInit();
            LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BonusMoneyPack").GetComponentInChildren<GravitatePickup>().maxSpeed = 50;
            SkillHook.LateInit();
            StatHook.LateInit();
            ProjectileImpactExplosion projectileImpactExplosion = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/StickyBomb").GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.lifetime *= 0.3f;
            projectileImpactExplosion.lifetimeAfterImpact *= 0.3f;
        }
    }
}