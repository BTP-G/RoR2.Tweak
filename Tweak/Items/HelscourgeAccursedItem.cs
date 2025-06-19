using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Skills;
using BTP.RoR2Plugin.Tweaks;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Items {
    internal class HelscourgeAccursedItem : TweakBase<HelscourgeAccursedItem>, IOnModLoadBehavior {
        public static ItemDef ItemDef { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = "HelscourgeAccursedItem";
            ItemDef.nameToken = "ACCURSEDMITHRIX_ITEM3_NAME";
            ItemDef.pickupToken = "ACCURSEDMITHRIX_ITEM3_PICKUP";
            ItemDef.descriptionToken = "ACCURSEDMITHRIX_ITEM3_DESC";
            ItemDef.loreToken = "ACCURSEDMITHRIX_ITEM3_LORE";
            ItemDef.tier = ItemTier.NoTier;
            ItemDef.deprecatedTier = ItemTier.NoTier;
            ItemDef.TryApplyTag(ItemTag.WorldUnique);
            ItemDef.hidden = true;
            if (!ContentAddition.AddItemDef(ItemDef)) {
                "AddItemDef :: HelscourgeAccursedItemDef Failed!".LogError();
            }
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
        }

        private void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self) {
            orig(self);
            if (self.body?.bodyIndex == BodyIndexes.Brother && self.activeOverlayCount < CharacterModel.maxOverlays) {
                var inventory = self.body.inventory;
                if (inventory.GetItemCount(ItemDef._itemIndex) > 0) {
                    self.currentOverlays[self.activeOverlayCount++] = AssetReferences.helscourgeMat;
                } else if (inventory.GetItemCount(StormscourgeAccursedItem.ItemDef) > 0) {
                    self.currentOverlays[self.activeOverlayCount++] = AssetReferences.stormscourgeMat;
                } else if (inventory.GetItemCount(MoonscourgeAccursedItem.ItemDef) > 0) {
                    self.currentOverlays[self.activeOverlayCount++] = AssetReferences.moonscourgeMat;
                }
            }
        }
    }
}
