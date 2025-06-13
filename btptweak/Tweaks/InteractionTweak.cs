using BTP.RoR2Plugin.Utils;
using EntityStates.Scrapper;
using GuestUnion;
using HG;
using MonoMod.Cil;
using RoR2;
using RoR2.EntityLogic;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class InteractionTweak : TweakBase<InteractionTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float origInitialDelayDuration = 1.5f;
        public const float origTimeBetweenStartAndDropDroplet = 1.333f;

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.OnInteractionsGlobal += GlobalEventManager_OnInteractionsGlobal;
            IL.EntityStates.Duplicator.Duplicating.DropDroplet += Duplicating_DropDroplet;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            WaitToBeginScrapping.duration = 0.125f;
            Scrapping.duration = 0.5f;
            ScrappingToIdle.duration = 0.125f;
            GameObjectPaths.Duplicator.Load<GameObject>().RemoveComponent<DelayedEvent>();
            GameObjectPaths.DuplicatorLarge.Load<GameObject>().RemoveComponent<DelayedEvent>();
            GameObjectPaths.DuplicatorMilitary.Load<GameObject>().RemoveComponent<DelayedEvent>();
            GameObjectPaths.DuplicatorWild.Load<GameObject>().RemoveComponent<DelayedEvent>();
            ArrayUtils.ArrayAppend(ref (GameObjectPaths.LunarCauldronWhiteToGreen.LoadComponent<ShopTerminalBehavior>().dropTable as BasicPickupDropTable).bannedItemTags, ItemTag.PriorityScrap);
            InteractableSpawnCardPaths.iscScrapper.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineGoldshoresAccess.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineCleanse.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineCleanseSandy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineCleanseSnowy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 1;
            InteractableSpawnCardPaths.iscShrineChance.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = false;
            InteractableSpawnCardPaths.iscShrineChanceSandy.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = false;
            InteractableSpawnCardPaths.iscShrineChanceSnowy.Load<InteractableSpawnCard>().skipSpawnWhenSacrificeArtifactEnabled = false;
            InteractableSpawnCardPaths.iscShrineBoss.Load<InteractableSpawnCard>().maxSpawnsPerStage = 3;
            InteractableSpawnCardPaths.iscShrineBossSandy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 3;
            InteractableSpawnCardPaths.iscShrineBossSnowy.Load<InteractableSpawnCard>().maxSpawnsPerStage = 3;
        }

        private void GlobalEventManager_OnInteractionsGlobal(Interactor interactor, IInteractable interactable, GameObject interaction) {
            if (interaction.TryGetComponent<PurchaseInteraction>(out var purchaseInteraction)) {
                switch (purchaseInteraction.costType) {
                    case CostTypeIndex.WhiteItem:
                        if (interactor.TryGetComponent<CharacterBody>(out var characterBody) && characterBody.inventory) {
                            var count = characterBody.inventory.GetItemCount(RoR2Content.Items.ScrapWhite.itemIndex);
                            EntityStates.Duplicator.Duplicating.initialDelayDuration = count > 0f ? 0 : origInitialDelayDuration;
                            EntityStates.Duplicator.Duplicating.timeBetweenStartAndDropDroplet = origTimeBetweenStartAndDropDroplet / (count + 1);
                        }
                        break;

                    case CostTypeIndex.GreenItem:
                        if (interactor.TryGetComponent(out characterBody) && characterBody.inventory) {
                            var count = characterBody.inventory.GetItemCount(RoR2Content.Items.ScrapGreen.itemIndex)
                                + characterBody.inventory.GetItemCount(DLC1Content.Items.RegeneratingScrap.itemIndex);
                            EntityStates.Duplicator.Duplicating.initialDelayDuration = count > 0f ? 0 : origInitialDelayDuration;
                            EntityStates.Duplicator.Duplicating.timeBetweenStartAndDropDroplet = origTimeBetweenStartAndDropDroplet / (count + 1);
                        }
                        break;

                    case CostTypeIndex.RedItem:
                        if (interactor.TryGetComponent(out characterBody) && characterBody.inventory) {
                            var count = characterBody.inventory.GetItemCount(RoR2Content.Items.ScrapRed.itemIndex);
                            EntityStates.Duplicator.Duplicating.initialDelayDuration = count > 0f ? 0 : origInitialDelayDuration;
                            EntityStates.Duplicator.Duplicating.timeBetweenStartAndDropDroplet = origTimeBetweenStartAndDropDroplet / (count + 1);
                        }
                        break;

                    case CostTypeIndex.BossItem:
                        if (interactor.TryGetComponent(out characterBody) && characterBody.inventory) {
                            var count = characterBody.inventory.GetItemCount(RoR2Content.Items.ScrapYellow.itemIndex);
                            EntityStates.Duplicator.Duplicating.initialDelayDuration = count > 0 ? 0f : origInitialDelayDuration;
                            EntityStates.Duplicator.Duplicating.timeBetweenStartAndDropDroplet = origTimeBetweenStartAndDropDroplet / (count + 1);
                        }
                        break;
                }
            }
        }

        private void Duplicating_DropDroplet(ILContext il) {
            var curscor = new ILCursor(il);
            if (curscor.TryGotoNext(c => c.MatchCallvirt<ShopTerminalBehavior>("DropPickup"))) {
                curscor.EmitDelegate((ShopTerminalBehavior shopTerminal) => {
                    shopTerminal.GetComponent<PurchaseInteraction>().SetAvailable(true);
                    var itemTier = PickupCatalog.GetPickupDef(shopTerminal.CurrentPickupIndex()).itemTier;
                    switch (itemTier) {
                        case ItemTier.Lunar:
                            if (!RunInfo.位于月球 && RollItemDuplicatorDestroy(itemTier)) {
                                Object.Destroy(shopTerminal.gameObject);
                                EffectManager.SpawnEffect(AssetReferences.moonExitArenaOrbEffect, new EffectData {
                                    origin = shopTerminal.pickupDisplay.transform.position,
                                }, true);
                            }
                            break;

                        case ItemTier.VoidTier1:
                        case ItemTier.VoidTier2:
                        case ItemTier.VoidTier3:
                        case ItemTier.VoidBoss:
                            if (RollItemDuplicatorDestroy(itemTier)) {
                                Object.Destroy(shopTerminal.gameObject);
                                BtpUtils.SpawnVoidDeathBomb(shopTerminal.pickupDisplay.transform.position);
                            }
                            break;
                    }
                    return shopTerminal;
                });
            } else {
                "Duplicating_DropDroplet hook failed!".LogError();
            }
        }

        private bool RollItemDuplicatorDestroy(ItemTier itemTier) {
            return Util.CheckRoll(itemTier switch {
                ItemTier.Lunar => 20f,
                ItemTier.VoidTier1 => 10f,
                ItemTier.VoidTier2 => 20f,
                ItemTier.VoidTier3 => 40f,
                ItemTier.VoidBoss => 20f,
                _ => 0f,
            });
        }
    }
}