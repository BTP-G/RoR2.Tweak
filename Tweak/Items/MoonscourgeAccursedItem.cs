using BTP.RoR2Plugin.Skills;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Items {
    internal class MoonscourgeAccursedItem : ModComponent, IModLoadMessageHandler {
        public static ItemDef ItemDef { get; private set; }

        void IModLoadMessageHandler.Handle() {
            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = "MoonscourgeAccursedItem";
            ItemDef.nameToken = "ACCURSEDMITHRIX_ITEM1_NAME";
            ItemDef.pickupToken = "ACCURSEDMITHRIX_ITEM1_PICKUP";
            ItemDef.descriptionToken = "ACCURSEDMITHRIX_ITEM1_DESC";
            ItemDef.loreToken = "ACCURSEDMITHRIX_ITEM1_LORE";
            ItemDef.tier = ItemTier.NoTier;
            ItemDef.deprecatedTier = ItemTier.NoTier;
            ItemDef.TryApplyTag(ItemTag.WorldUnique);
            ItemDef.hidden = true;
            if (!ContentAddition.AddItemDef(ItemDef)) {
                "AddItemDef :: MoonscourgeAccursedItem Failed!".LogError();
            }
        }
    }
}
