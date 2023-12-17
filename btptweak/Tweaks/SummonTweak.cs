using RoR2;

namespace BtpTweak.Tweaks {

    internal partial class SummonTweak : TweakBase<SummonTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private ItemDef _weddingRing;

        void IOnModLoadBehavior.OnModLoad() {
            MasterSummon.onServerMasterSummonGlobal += MasterSummon_onServerMasterSummonGlobal;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            _weddingRing = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("RuinaWeddingRing"));
        }

        private void MasterSummon_onServerMasterSummonGlobal(MasterSummon.MasterSummonReport summonReport) {
            var summonInvertory = summonReport.summonMasterInstance?.inventory;
            if (summonInvertory) {
                var leaderInventory = summonReport.leaderMasterInstance?.inventory;
                if (leaderInventory) {
                    summonInvertory.GiveItem(_weddingRing, leaderInventory.GetItemCount(_weddingRing) - summonInvertory.GetItemCount(_weddingRing));
                }
            }
        }
    }
}