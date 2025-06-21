using BTP.RoR2Plugin.Utils;
using GuestUnion;
using RoR2;
using System;
using System.Collections.Generic;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ItemMiscTweak : ModComponent, IComparer<ItemIndex>, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private static int[] ItemIndexToOrderIndex;
        private int itemCount = 0;

        void IModLoadMessageHandler.Handle() {
            PlayerCharacterMasterController.onLinkedToNetworkUserLocal += PlayerCharacterMasterController_onLinkedToNetworkUserLocal;
            On.RoR2.ItemCatalog.SetItemDefs += OnSetItemDefs;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            AssetReferences.bonusMoneyPack.Asset.GetComponentInChildren<GravitatePickup>().maxSpeed = 50;
            DLC1Content.Items.ExtraLifeVoid.TryApplyTag(ItemTag.CannotSteal);
            DLC1Content.Items.FreeChest.TryApplyTag(ItemTag.CannotCopy);
            DLC1Content.Items.ImmuneToDebuff.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.RegeneratingScrap.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.RegeneratingScrap.TryApplyTag(ItemTag.Scrap);
            DLC1Content.Items.TreasureCacheVoid.TryApplyTag(ItemTag.CannotCopy);
            RoR2Content.Items.CaptainDefenseMatrix.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.CaptainDefenseMatrix.TryApplyTag(ItemTag.CannotSteal);
            RoR2Content.Items.ExtraLife.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.ExtraLife.TryApplyTag(ItemTag.CannotSteal);
            RoR2Content.Items.ShockNearby.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.TreasureCache.TryApplyTag(ItemTag.CannotCopy);
            vanillaVoid.Items.VoidShell.instance.ItemDef.TryApplyTag(ItemTag.CannotCopy);
            RoR2Content.Items.TonicAffliction.TryRemoveTag(ItemTag.CannotSteal);
            RoR2Content.Items.JumpBoost.TryApplyTag(ItemTag.AIBlacklist);
            foreach (var item in ItemCatalog.allItemDefs) {
                item.TryApplyTag(ItemTag.BrotherBlacklist);
                item.pickupToken = item.descriptionToken;
                if (item.ContainsTag(ItemTag.OnKillEffect) || item.ContainsTag(ItemTag.Healing)) {
                    item.TryApplyTag(ItemTag.AIBlacklist);
                }
            }
        }

        public int Compare(ItemIndex x, ItemIndex y) => ItemIndexToOrderIndex[(int)x] - ItemIndexToOrderIndex[(int)y];

        private void OnSetItemDefs(On.RoR2.ItemCatalog.orig_SetItemDefs orig, ItemDef[] newItemDefs) {
            orig(newItemDefs);
            var orderedItemIndexes = new int[ItemCatalog.itemCount];
            for (var i = 0; i < orderedItemIndexes.Length; ++i) {
                orderedItemIndexes[i] = i;
            }
            Array.Sort(orderedItemIndexes, (x, y) => {
                var xDef = ItemCatalog.GetItemDef((ItemIndex)x);
                var yDef = ItemCatalog.GetItemDef((ItemIndex)y);
                var result = (xDef.ContainsTag(ItemTag.PriorityScrap) ? 10000 : 0) - (yDef.ContainsTag(ItemTag.PriorityScrap) ? 10000 : 0);
                result += (xDef.ContainsTag(ItemTag.Scrap) ? 1000 : 0) - (yDef.ContainsTag(ItemTag.Scrap) ? 1000 : 0);
                result += 10 * (yDef.tier - xDef.tier);
                result += xDef.name.CompareTo(yDef.name);
                return result;
            });
            ItemIndexToOrderIndex = new int[ItemCatalog.itemCount];
            for (var i = 0; i < ItemIndexToOrderIndex.Length; ++i) {
                ItemIndexToOrderIndex[i] = orderedItemIndexes.IndexOf(i);
            }
        }

        private void PlayerCharacterMasterController_onLinkedToNetworkUserLocal(PlayerCharacterMasterController player) {
            var inventory = player.master.inventory;
            inventory.onInventoryChanged += () => {
                if (itemCount < inventory.itemAcquisitionOrder.Count) {
                    inventory.itemAcquisitionOrder.Sort(this);
                }
                itemCount = inventory.itemAcquisitionOrder.Count;
            };
        }
    }
}