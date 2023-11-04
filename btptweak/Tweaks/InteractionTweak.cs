using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using EntityStates.Scrapper;
using RoR2;
using RoR2.EntityLogic;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class InteractionTweak : TweakBase<InteractionTweak> {
        private bool _位于月球;

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.RoR2.ShopTerminalBehavior.DropPickup += ShopTerminalBehavior_DropPickup;
            On.RoR2.PurchaseInteraction.Awake += PurchaseInteraction_Awake;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.RoR2.ShopTerminalBehavior.DropPickup -= ShopTerminalBehavior_DropPickup;
            On.RoR2.PurchaseInteraction.Awake -= PurchaseInteraction_Awake;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
        }

        public void Load() {
            WaitToBeginScrapping.duration = 0.125f;
            Scrapping.duration = 0.5f;
            ScrappingToIdle.duration = 0.125f;
            EntityStates.Duplicator.Duplicating.initialDelayDuration = 0;
            var purchaseInteraction = "RoR2/Base/LunarCauldrons/LunarCauldron, RedToWhite Variant.prefab".LoadComponent<PurchaseInteraction>();
            purchaseInteraction.costType = CostTypeIndex.GreenItem;
            purchaseInteraction.Networkcost = purchaseInteraction.cost = 2;
            "RoR2/Base/Duplicator/Duplicator.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/DuplicatorLarge/DuplicatorLarge.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/DuplicatorMilitary/DuplicatorMilitary.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/DuplicatorWild/DuplicatorWild.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/Scrapper/iscScrapper.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineBlood/iscShrineBlood.asset".Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = true;
            "RoR2/Base/ShrineBlood/iscShrineBloodSandy.asset".Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = true;
            "RoR2/Base/ShrineBlood/iscShrineBloodSnowy.asset".Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = true;
            "RoR2/Base/ShrineCleanse/iscShrineCleanse.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineCleanse/iscShrineCleanseSandy.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineCleanse/iscShrineCleanseSnowy.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            "RoR2/Base/ShrineGoldshoresAccess/iscShrineGoldshoresAccess.asset".Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
        }

        public void Stage_onStageStartGlobal(Stage stage) {
            var sceneIndex = stage.sceneDef.sceneDefIndex;
            _位于月球 = sceneIndex == SceneIndexes.Moon2 || sceneIndex == SceneIndexes.Moon;
        }

        private float GetTPChanceFromItemTier(ItemTier itemTier) => itemTier switch {
            ItemTier.Tier1 => 5f,
            ItemTier.Tier2 => 10f,
            ItemTier.Tier3 => 25f,
            ItemTier.Lunar => 20f,
            ItemTier.Boss => 20f,
            ItemTier.VoidTier1 => 10f,
            ItemTier.VoidTier2 => 20f,
            ItemTier.VoidTier3 => 50f,
            ItemTier.VoidBoss => 40f,
            _ => 0,
        };

        private void PurchaseInteraction_Awake(On.RoR2.PurchaseInteraction.orig_Awake orig, PurchaseInteraction self) {
            orig(self);
            if (self.name.StartsWith("LunarCauldron, RedToWhite") && self.costType == CostTypeIndex.RedItem) {
                self.costType = CostTypeIndex.GreenItem;
                self.Networkcost = 2;
            }
        }

        private void ShopTerminalBehavior_DropPickup(On.RoR2.ShopTerminalBehavior.orig_DropPickup orig, ShopTerminalBehavior self) {
            orig(self);
            if (!NetworkServer.active) {
                return;
            }
            if (self.name.StartsWith("Duplicator")) {
                static void FireVoidBomb(Vector3 position) {
                    FireProjectileInfo fireProjectileInfo = new() {
                        projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile,
                        position = position,
                        rotation = Quaternion.identity,
                        owner = null,
                        damage = 0,
                        crit = false
                    };
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
                switch (PickupCatalog.GetPickupDef(self.CurrentPickupIndex()).itemTier) {
                    case ItemTier.Lunar:
                        if (!_位于月球 && Util.CheckRoll(20)) {
                            Object.Destroy(self.gameObject);
                            EffectManager.SpawnEffect(AssetReferences.lunarBlink, new EffectData {
                                origin = self.pickupDisplay.transform.position,
                                rotation = default,
                            }, true);
                            return;
                        }
                        break;

                    case ItemTier.VoidTier1:
                        if (Util.CheckRoll(10)) {
                            Object.Destroy(self.gameObject);
                            FireVoidBomb(self.pickupDisplay.transform.position);
                            return;
                        }
                        break;

                    case ItemTier.VoidTier2:
                        if (Util.CheckRoll(30)) {
                            Object.Destroy(self.gameObject);
                            FireVoidBomb(self.pickupDisplay.transform.position);
                            return;
                        }
                        break;

                    case ItemTier.VoidTier3:
                        if (Util.CheckRoll(60)) {
                            Object.Destroy(self.gameObject);
                            FireVoidBomb(self.pickupDisplay.transform.position);
                            return;
                        }
                        break;

                    case ItemTier.VoidBoss:
                        if (Util.CheckRoll(40)) {
                            Object.Destroy(self.gameObject);
                            FireVoidBomb(self.pickupDisplay.transform.position);
                            return;
                        }
                        break;
                }
                self.GetComponent<PurchaseInteraction>().SetAvailable(true);
            } else if (self.name.StartsWith("LunarCauldron") && !_位于月球 && Util.CheckRoll(GetTPChanceFromItemTier(PickupCatalog.GetPickupDef(self.CurrentPickupIndex()).itemTier))) {
                Object.Destroy(self.gameObject);
                EffectManager.SpawnEffect(AssetReferences.lunarBlink, new EffectData {
                    origin = self.pickupDisplay.transform.position,
                    rotation = default,
                }, true);
            }
        }
    }
}