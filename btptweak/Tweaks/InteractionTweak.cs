using BtpTweak.Utils;
using EntityStates.Scrapper;
using RoR2;
using RoR2.EntityLogic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks
{

    internal class InteractionTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            On.RoR2.ShopTerminalBehavior.DropPickup += ShopTerminalBehavior_DropPickup;
            On.RoR2.PurchaseInteraction.Awake += PurchaseInteraction_Awake;
        }

        public override void Load() {
            base.Load();
            WaitToBeginScrapping.duration = 0.125f;
            Scrapping.duration = 0.5f;
            ScrappingToIdle.duration = 0.125f;
            EntityStates.Duplicator.Duplicating.initialDelayDuration = 0.5f;
            var purchaseInteraction = "RoR2/Base/LunarCauldrons/LunarCauldron, RedToWhite Variant.prefab".LoadComponent<PurchaseInteraction>();
            purchaseInteraction.costType = CostTypeIndex.GreenItem;
            purchaseInteraction.Networkcost = purchaseInteraction.cost = 2;
            "RoR2/Base/Duplicator/Duplicator.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/DuplicatorLarge/DuplicatorLarge.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/DuplicatorMilitary/DuplicatorMilitary.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
            "RoR2/Base/DuplicatorWild/DuplicatorWild.prefab".Load<GameObject>().RemoveComponent<DelayedEvent>();
        }

        private float GetBreakChance(ItemTier itemTier) => itemTier switch {
            ItemTier.Tier1 => 5f,
            ItemTier.Tier2 => 15f,
            ItemTier.Tier3 => 30f,
            ItemTier.Lunar => 20f,
            ItemTier.Boss => 20f,
            ItemTier.VoidTier1 => 10f,
            ItemTier.VoidTier2 => 30f,
            ItemTier.VoidTier3 => 60f,
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
                if (Util.CheckRoll(GetBreakChance(self.itemTier))) {
                    self.SetNoPickup();
                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXQuick"), new EffectData {
                        origin = self.transform.position + (Vector3.up * 2),
                        scale = (float)(4 + self.itemTier),
                        rotation = default,
                    }, true);
                } else {
                    self.GetComponent<PurchaseInteraction>().SetAvailable(true);
                }
            } else if (self.name.StartsWith("LunarCauldron") && Util.CheckRoll(GetBreakChance(self.itemTier))) {
                Object.Destroy(self.gameObject);
                EffectManager.SpawnEffect("RoR2/Base/moon/MoonExitArenaOrbEffect.prefab".Load<GameObject>(), new EffectData {
                    origin = self.transform.position + (Vector3.up * 2),
                    rotation = default,
                }, true);
            }
        }
    }
}