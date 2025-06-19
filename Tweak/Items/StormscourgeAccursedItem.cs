using BTP.RoR2Plugin.Skills;
using BTP.RoR2Plugin.Tweaks;
using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Items {
    internal class StormscourgeAccursedItem : TweakBase<StormscourgeAccursedItem>, IOnModLoadBehavior {
        public static ItemDef ItemDef { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = "StormscourgeAccursedItem";
            ItemDef.nameToken = "ACCURSEDMITHRIX_ITEM2_NAME";
            ItemDef.pickupToken = "ACCURSEDMITHRIX_ITEM2_PICKUP";
            ItemDef.descriptionToken = "ACCURSEDMITHRIX_ITEM2_DESC";
            ItemDef.loreToken = "ACCURSEDMITHRIX_ITEM2_LORE";
            ItemDef.tier = ItemTier.NoTier;
            ItemDef.deprecatedTier = ItemTier.NoTier;
            ItemDef.TryApplyTag(ItemTag.WorldUnique);
            ItemDef.hidden = true;
            if (!ContentAddition.AddItemDef(ItemDef)) {
                "AddItemDef :: StormscourgeAccursedItem Failed!".LogError();
            }
        }
    }
}
