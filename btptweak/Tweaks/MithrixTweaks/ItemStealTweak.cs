using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BtpTweak.Tweaks.MithrixTweaks {

    internal class ItemStealTweak : TweakBase<ItemStealTweak>, IOnModLoadBehavior {
        private readonly Dictionary<ItemStealController.StolenInventoryInfo, List<ItemIndex>> StolenInventoryInfoToItemIndexes = [];

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.ReturnStolenItemsOnGettingHit.OnTakeDamageServer += ReturnStolenItemsOnGettingHit_OnTakeDamageServer;
            //On.RoR2.ItemStealController.StolenInventoryInfo.StealNewestItem += StolenInventoryInfo_StealNewestItem;
            //SceneCatalog.onMostRecentSceneDefChanged += SceneCatalog_onMostRecentSceneDefChanged;
        }

        private bool StolenInventoryInfo_StealNewestItem(On.RoR2.ItemStealController.StolenInventoryInfo.orig_StealNewestItem orig, object self, int maxStackToSteal, bool? useOrbOverride) {
            if (self is ItemStealController.StolenInventoryInfo stolenInventoryInfo && stolenInventoryInfo.victimInventory) {
                var victimInventory = stolenInventoryInfo.victimInventory;
                if (!StolenInventoryInfoToItemIndexes.TryGetValue(stolenInventoryInfo, out var itemIndexes)) {
                    itemIndexes = [.. stolenInventoryInfo.itemAcquisitionOrder];
                    StolenInventoryInfoToItemIndexes.Add(stolenInventoryInfo, itemIndexes);
                }
                for (int i = itemIndexes.Count - 1; i >= 0; --i) {
                    var itemIndex = itemIndexes[i];
                    itemIndexes.Remove(itemIndex);
                    var maxStackToStel = Mathf.CeilToInt(victimInventory.GetItemCount(itemIndex) * 0.5f);
                    if (maxStackToStel > 0 && stolenInventoryInfo.StealItem(itemIndex, maxStackToStel, useOrbOverride) > 0) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ReturnStolenItemsOnGettingHit_OnTakeDamageServer(On.RoR2.ReturnStolenItemsOnGettingHit.orig_OnTakeDamageServer orig, ReturnStolenItemsOnGettingHit self, DamageReport damageReport) {
        }

        private void SceneCatalog_onMostRecentSceneDefChanged(SceneDef sceneDef) {
            if (StolenInventoryInfoToItemIndexes.Count > 0) {
                foreach (var itemIndexes in StolenInventoryInfoToItemIndexes.Values.Where(l => l.Count > 0)) {
                    itemIndexes.Clear();
                }
                StolenInventoryInfoToItemIndexes.Clear();
            }
        }
    }
}