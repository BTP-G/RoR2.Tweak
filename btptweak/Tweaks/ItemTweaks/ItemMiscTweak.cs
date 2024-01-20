using BtpTweak.Utils;
using RoR2;
using System.Linq;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ItemMiscTweak : TweakBase<ItemMiscTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            AssetReferences.bonusMoneyPack.GetComponentInChildren<GravitatePickup>().maxSpeed = 50;
            DLC1Content.Items.ExtraLifeVoid.TryApplyTag(ItemTag.CannotSteal);
            DLC1Content.Items.ImmuneToDebuff.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.RegeneratingScrap.TryApplyTag(ItemTag.AIBlacklist);
            DLC1Content.Items.MinorConstructOnKill.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.BeetleGland.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.Behemoth.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.CaptainDefenseMatrix.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.CaptainDefenseMatrix.TryApplyTag(ItemTag.CannotSteal);
            RoR2Content.Items.Dagger.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ExecuteLowHealthElite.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ExplodeOnDeath.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ExtraLife.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.ExtraLife.TryApplyTag(ItemTag.CannotSteal);
            RoR2Content.Items.IgniteOnKill.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.NovaOnHeal.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.NovaOnLowHealth.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.RoboBallBuddy.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.ShockNearby.TryApplyTag(ItemTag.AIBlacklist);
            RoR2Content.Items.ShockNearby.TryApplyTag(ItemTag.BrotherBlacklist);
            RoR2Content.Items.Thorns.TryApplyTag(ItemTag.BrotherBlacklist);
            foreach (var item in ItemCatalog.allItemDefs) {
                item.TryApplyTag(ItemTag.BrotherBlacklist);
            }
            var tags = RoR2Content.Items.TonicAffliction.tags.ToList();
            tags.Remove(ItemTag.CannotSteal);
            RoR2Content.Items.TonicAffliction.tags = [.. tags];
            RoR2Content.Items.FlatHealth.tags = [ItemTag.Healing];
        }
    }
}