using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ItemMiscTweak : TweakBase<ItemMiscTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private int itemCount = 0;

        void IOnModLoadBehavior.OnModLoad() {
            PlayerCharacterMasterController.onLinkedToNetworkUserLocal += PlayerCharacterMasterController_onLinkedToNetworkUserLocal;
             
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
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

        private void PlayerCharacterMasterController_onLinkedToNetworkUserLocal(PlayerCharacterMasterController player) {
            Debug.Log($"PlayerCharacterMasterController.onLinkedToNetworkUserLocal: player.localPlayerAuthority: {player.localPlayerAuthority}");
            if(!player.localPlayerAuthority) {
                return;
            }
            var inventory = player.master.inventory;
            inventory.onInventoryChanged += () => {
                if (itemCount < inventory.itemAcquisitionOrder.Count) {
                    inventory.itemAcquisitionOrder.Sort(ItemIndexComparer.Instance);
                }
                itemCount = inventory.itemAcquisitionOrder.Count;
            };
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