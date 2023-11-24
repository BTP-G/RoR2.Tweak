using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
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
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            On.RoR2.ShopTerminalBehavior.DropPickup += ShopTerminalBehavior_DropPickup;
            On.RoR2.PurchaseInteraction.Awake += PurchaseInteraction_Awake;
            On.RoR2.ShrineBloodBehavior.AddShrineStack += ShrineBloodBehavior_AddShrineStack;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
            On.RoR2.ShopTerminalBehavior.DropPickup -= ShopTerminalBehavior_DropPickup;
            On.RoR2.PurchaseInteraction.Awake -= PurchaseInteraction_Awake;
        }

        public void Load() {
            WaitToBeginScrapping.duration = 0.125f;
            Scrapping.duration = 0.5f;
            ScrappingToIdle.duration = 0.125f;
            EntityStates.Duplicator.Duplicating.initialDelayDuration = 0;
            var purchaseInteraction = GameObjectPaths.LunarCauldronRedToWhiteVariant.LoadComponent<PurchaseInteraction>();
            purchaseInteraction.costType = CostTypeIndex.GreenItem;
            purchaseInteraction.Networkcost = purchaseInteraction.cost = 2;
            GameObjectPaths.Duplicator.Load<GameObject>().RemoveComponent<DelayedEvent>();
            GameObjectPaths.DuplicatorLarge.Load<GameObject>().RemoveComponent<DelayedEvent>();
            GameObjectPaths.DuplicatorMilitary.Load<GameObject>().RemoveComponent<DelayedEvent>();
            GameObjectPaths.DuplicatorWild.Load<GameObject>().RemoveComponent<DelayedEvent>();
            InteractableSpawnCardPaths.iscScrapper.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineGoldshoresAccess.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineCleanse.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineCleanseSandy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineCleanseSnowy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineChance.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = false;
            InteractableSpawnCardPaths.iscShrineChanceSandy.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = false;
            InteractableSpawnCardPaths.iscShrineChanceSnowy.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = false;
            InteractableSpawnCardPaths.iscBarrel1.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = true;
            var card = InteractableSpawnCardPaths.iscLunarChest.Load<InteractableSpawnCard>();
            card.skipSpawnWhenSacrificeArtifactEnabled = false;
            card.weightScalarWhenSacrificeArtifactEnabled *= 0.5f;
            card = InteractableSpawnCardPaths.iscVoidChest.Load<InteractableSpawnCard>();
            card.skipSpawnWhenSacrificeArtifactEnabled = false;
            card.weightScalarWhenSacrificeArtifactEnabled *= 0.5f;
        }

        public void Stage_onStageStartGlobal(Stage stage) {
            var sceneIndex = stage.sceneDef.sceneDefIndex;
            _位于月球 = sceneIndex == SceneIndexes.Moon2 || sceneIndex == SceneIndexes.Moon;
        }

        private void ShrineBloodBehavior_AddShrineStack(On.RoR2.ShrineBloodBehavior.orig_AddShrineStack orig, ShrineBloodBehavior self, Interactor interactor) {
            if (!NetworkServer.active) {
                Debug.LogWarning("[Server] function 'System.Void RoR2.ShrineBloodBehavior::AddShrineStack(RoR2.Interactor)' called on client");
                return;
            }
            self.waitingForRefresh = true;
            CharacterBody component = interactor.GetComponent<CharacterBody>();
            if (component) {
                uint amount = (uint)(component.healthComponent.fullCombinedHealth * self.purchaseInteraction.cost / 100f * self.goldToPaidHpRatio);
                if (component.master) {
                    component.master.GiveMoney(amount);
                    Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage {
                        subjectAsCharacterBody = component,
                        baseToken = "SHRINE_BLOOD_USE_MESSAGE",
                        paramTokens = new string[]
                        {
                            amount.ToString()
                        }
                    });
                }
            }
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData {
                origin = self.transform.position,
                rotation = Quaternion.identity,
                scale = 1f,
                color = Color.red
            }, true);
            self.purchaseCount++;
            self.refreshTimer = 2f;
            if (self.purchaseCount >= self.maxPurchaseCount) {
                self.symbolTransform.gameObject.SetActive(false);
            }
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
                            EffectManager.SpawnEffect(AssetReferences.moonExitArenaOrbEffect, new EffectData {
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
                EffectManager.SpawnEffect(AssetReferences.moonExitArenaOrbEffect, new EffectData {
                    origin = self.pickupDisplay.transform.position,
                    rotation = default,
                }, true);
            }
        }
    }
}