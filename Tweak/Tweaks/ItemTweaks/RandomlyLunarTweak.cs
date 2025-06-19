using RoR2;
using System.Collections.Generic;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class RandomlyLunarTweak : TweakBase<RandomlyLunarTweak>, IOnModLoadBehavior {
        public const int UsageCount = 1;
        private int _rerolledCount;

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacement += RandomlyLunarUtils_CheckForLunarReplacement;
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacementUniqueArray += RandomlyLunarUtils_CheckForLunarReplacementUniqueArray;
            On.RoR2.PurchaseInteraction.SetAvailable += PurchaseInteraction_SetAvailable;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
        }

        public void Stage_onStageStartGlobal(Stage stage) {
            _rerolledCount = 0;
        }

        private void PurchaseInteraction_SetAvailable(On.RoR2.PurchaseInteraction.orig_SetAvailable orig, PurchaseInteraction self, bool newAvailable) {
            if (self.name.StartsWith("LunarRecycler")) {
                if (!newAvailable) {
                    ++_rerolledCount;
                }
                int itemCountGlobal = Util.GetItemCountGlobal(DLC1Content.Items.RandomlyLunar.itemIndex, false);
                self.Networkcost = _rerolledCount + 1 + _rerolledCount * UsageCount * itemCountGlobal;
                if (_rerolledCount >= 9 + UsageCount * itemCountGlobal) {
                    newAvailable = false;
                }
            }
            orig(self, newAvailable);
        }

        private void RandomlyLunarUtils_CheckForLunarReplacementUniqueArray(On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacementUniqueArray orig, PickupIndex[] pickupIndices, Xoroshiro128Plus rng) {
            if (RunInfo.位于隔间) {
                List<PickupIndex> list = null;
                for (int i = 0; i < pickupIndices.Length; ++i) {
                    PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndices[i]);
                    if (pickupDef != null && Util.CheckRoll(5)) {
                        List<PickupIndex> list3 = null;
                        if (pickupDef.itemIndex != ItemIndex.None) {
                            if (list == null) {
                                switch (pickupDef.itemTier) {
                                    case ItemTier.Tier1: {
                                        list = Run.instance.availableVoidTier1DropList;
                                        break;
                                    }
                                    case ItemTier.Tier2: {
                                        list = Run.instance.availableVoidTier2DropList;
                                        break;
                                    }
                                    case ItemTier.Tier3: {
                                        list = Run.instance.availableVoidTier3DropList;
                                        break;
                                    }
                                }
                                Util.ShuffleList(list, rng);
                            }
                            list3 = list;
                        }
                        if (list3 != null && list3.Count > 0) {
                            pickupIndices[i] = list3[i % list3.Count];
                        }
                    }
                }
            }
        }

        private PickupIndex RandomlyLunarUtils_CheckForLunarReplacement(On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacement orig, PickupIndex pickupIndex, Xoroshiro128Plus rng) {
            if (RunInfo.位于隔间) {
                var pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                if (pickupDef != null && Util.CheckRoll(5)) {
                    switch (pickupDef.itemTier) {
                        case ItemTier.Tier1: {
                            return rng.NextElementUniform(Run.instance.availableVoidTier1DropList);
                        }
                        case ItemTier.Tier2: {
                            return rng.NextElementUniform(Run.instance.availableVoidTier2DropList);
                        }
                        case ItemTier.Tier3: {
                            return rng.NextElementUniform(Run.instance.availableVoidTier3DropList);
                        }
                    }
                }
            }
            return pickupIndex;
        }
    }
}