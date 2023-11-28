using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates.Scrapper;
using MonoMod.Cil;
using RoR2;
using RoR2.EntityLogic;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class InteractionTweak : TweakBase<InteractionTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            IL.EntityStates.Duplicator.Duplicating.DropDroplet += Duplicating_DropDroplet;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            IL.EntityStates.Duplicator.Duplicating.DropDroplet -= Duplicating_DropDroplet;
        }

        public void Load() {
            WaitToBeginScrapping.duration = 0.125f;
            Scrapping.duration = 0.5f;
            ScrappingToIdle.duration = 0.125f;
            EntityStates.Duplicator.Duplicating.initialDelayDuration = 0;
            var gameObject = GameObjectPaths.LunarCauldronRedToWhiteVariant.Load<GameObject>().AddComponent<LunarCauldronRedToWhiteAwakeAction>().gameObject;
            var purchaseInteraction = gameObject.GetComponent<PurchaseInteraction>();
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
        }

        private void Duplicating_DropDroplet(ILContext il) {
            var curscor = new ILCursor(il);
            if (curscor.TryGotoNext(c => c.MatchCallvirt<ShopTerminalBehavior>("DropPickup"))) {
                curscor.EmitDelegate((ShopTerminalBehavior shopTerminal) => {
                    shopTerminal.GetComponent<PurchaseInteraction>().SetAvailable(true);
                    var itemTier = PickupCatalog.GetPickupDef(shopTerminal.CurrentPickupIndex()).itemTier;
                    switch (itemTier) {
                        case ItemTier.Lunar:
                            if (RunInfo.CurrentSceneIndex != SceneIndexes.Moon
                            && RunInfo.CurrentSceneIndex != SceneIndexes.Moon2
                            && Util.CheckRoll(GetPercentChance(itemTier))) {
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
                            if (Util.CheckRoll(GetPercentChance(itemTier))) {
                                Object.Destroy(shopTerminal.gameObject);
                                BtpUtils.SpawnVoidDeathBomb(shopTerminal.pickupDisplay.transform.position);
                            }
                            break;
                    }
                    return shopTerminal;
                });
            } else {
                Main.Logger.LogError("Duplicating_DropDroplet hook failed!");
            }
        }

        private float GetPercentChance(ItemTier itemTier) => itemTier switch {
            ItemTier.Tier1 => 5f,
            ItemTier.Tier2 => 10f,
            ItemTier.Tier3 => 20f,
            ItemTier.Lunar => 20f,
            ItemTier.Boss => 20f,
            ItemTier.VoidTier1 => 10f,
            ItemTier.VoidTier2 => 20f,
            ItemTier.VoidTier3 => 40f,
            ItemTier.VoidBoss => 20f,
            _ => 0,
        };

        private class LunarCauldronRedToWhiteAwakeAction : MonoBehaviour {

            private void Awake() {
                var purchaseInteraction = GetComponent<PurchaseInteraction>();
                purchaseInteraction.costType = CostTypeIndex.GreenItem;
                purchaseInteraction.Networkcost = purchaseInteraction.cost = 2;
            }
        }
    }
}