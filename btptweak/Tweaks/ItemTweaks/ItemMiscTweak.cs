using BtpTweak.Utils;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ItemMiscTweak : TweakBase<ItemMiscTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private static readonly Dictionary<Inventory, int> 背包物品数量缓存 = [];

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.UI.ItemInventoryDisplay.OnInventoryChanged += ItemInventoryDisplay_OnInventoryChanged;
            On.RoR2.UI.ItemInventoryDisplay.OnDestroy += ItemInventoryDisplay_OnDestroy;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            AssetReferences.bonusMoneyPack.GetComponentInChildren<GravitatePickup>().maxSpeed = 50;
            DLC1Content.Items.ElementalRingVoid.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.ExtraLifeVoid.TryApplyTag(ItemTag.CannotSteal);
            DLC1Content.Items.FreeChest.TryApplyTag(ItemTag.CannotCopy);
            DLC1Content.Items.ImmuneToDebuff.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.MinorConstructOnKill.TryApplyTag(ItemTag.BrotherBlacklist);
            DLC1Content.Items.RegeneratingScrap.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.RegeneratingScrap.TryApplyTag(ItemTag.Scrap);
            DLC1Content.Items.TreasureCacheVoid.TryApplyTag(ItemTag.CannotCopy);
            RoR2Content.Items.BeetleGland.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.Behemoth.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.CaptainDefenseMatrix.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.CaptainDefenseMatrix.TryApplyTag(ItemTag.CannotSteal);
            RoR2Content.Items.Dagger.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ExecuteLowHealthElite.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ExplodeOnDeath.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ExtraLife.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.ExtraLife.TryApplyTag(ItemTag.CannotSteal);
            RoR2Content.Items.FireRing.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.IceRing.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.IgniteOnKill.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.NovaOnHeal.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.NovaOnLowHealth.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.RoboBallBuddy.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ShockNearby.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.ShockNearby.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.Thorns.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.TreasureCache.TryApplyTag(ItemTag.CannotCopy);
            vanillaVoid.Items.VoidShell.instance.ItemDef.TryApplyTag(ItemTag.CannotCopy);
            var tags = RoR2Content.Items.TonicAffliction.tags.ToList();
            tags.Remove(ItemTag.CannotSteal);
            RoR2Content.Items.TonicAffliction.tags = [.. tags];
            RoR2Content.Items.FlatHealth.tags = [ItemTag.Healing];
            foreach (var item in ItemCatalog.allItemDefs) {
                item.TryApplyTag(ItemTag.BrotherBlacklist);
                item.pickupToken = item.descriptionToken;
            }
        }

        private void ItemInventoryDisplay_OnInventoryChanged(On.RoR2.UI.ItemInventoryDisplay.orig_OnInventoryChanged orig, ItemInventoryDisplay self) {
            if (self.inventoryWasValid) {
                var inventory = self.inventory;
                if (!背包物品数量缓存.TryGetValue(inventory, out var count)) {
                    背包物品数量缓存.Add(inventory, inventory.itemAcquisitionOrder.Count);
                }
                if (count != inventory.itemAcquisitionOrder.Count) {
                    if (count < inventory.itemAcquisitionOrder.Count) {
                        inventory.itemAcquisitionOrder.Sort(ItemIndexComparer.Instance);
                    }
                    背包物品数量缓存[inventory] = inventory.itemAcquisitionOrder.Count;
                }
            }
            orig(self);
        }

        private void ItemInventoryDisplay_OnDestroy(On.RoR2.UI.ItemInventoryDisplay.orig_OnDestroy orig, ItemInventoryDisplay self) {
            orig(self);
            for (int i = 背包物品数量缓存.Count - 1; i > -1; --i) {
                var e = 背包物品数量缓存.ElementAt(i);
                if (!e.Key) {
                    背包物品数量缓存.Remove(e.Key);
                }
            }
        }

        private class ItemIndexComparer : IComparer<ItemIndex> {
            private static int[] ItemIndexToOrderIndex;
            public static ItemIndexComparer Instance { get; } = new ItemIndexComparer();

            public int Compare(ItemIndex x, ItemIndex y) {
                return ItemIndexToOrderIndex[(int)x] - ItemIndexToOrderIndex[(int)y];
            }

            [RuntimeInitializeOnLoadMethod]
            private static void Init() {
                On.RoR2.ItemCatalog.SetItemDefs += ItemCatalog_SetItemDefs;
            }

            private static void ItemCatalog_SetItemDefs(On.RoR2.ItemCatalog.orig_SetItemDefs orig, ItemDef[] newItemDefs) {
                orig(newItemDefs);
                var OrderedItemIndexes = new int[ItemCatalog.itemCount];
                for (int i = 0; i < OrderedItemIndexes.Length; ++i) {
                    OrderedItemIndexes[i] = i;
                }
                Array.Sort(OrderedItemIndexes, (x, y) => {
                    var xDef = ItemCatalog.GetItemDef((ItemIndex)x);
                    var yDef = ItemCatalog.GetItemDef((ItemIndex)y);
                    var result = (xDef.ContainsTag(ItemTag.PriorityScrap) ? 10000 : 0) - (yDef.ContainsTag(ItemTag.PriorityScrap) ? 10000 : 0);
                    result += (xDef.ContainsTag(ItemTag.Scrap) ? 1000 : 0) - (yDef.ContainsTag(ItemTag.Scrap) ? 1000 : 0);
                    result += 10 * (yDef.tier - xDef.tier);
                    result += xDef.name.CompareTo(yDef.name);
                    return result;
                });
                ItemIndexToOrderIndex = new int[ItemCatalog.itemCount];
                for (int i = 0; i < ItemIndexToOrderIndex.Length; ++i) {
                    ItemIndexToOrderIndex[i] = Array.IndexOf(OrderedItemIndexes, i);
                }
            }
        }
    }
}