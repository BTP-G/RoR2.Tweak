using RoR2;

namespace BtpTweak.Tweaks {

    internal partial class SummonTweak : TweakBase<SummonTweak> {
        private ItemDef _weddingRing;

        public override void SetEventHandlers() {
            MasterSummon.onServerMasterSummonGlobal += MasterSummon_onServerMasterSummonGlobal;
        }

        public override void ClearEventHandlers() {
            MasterSummon.onServerMasterSummonGlobal -= MasterSummon_onServerMasterSummonGlobal;
        }

        public void Load() {
            _weddingRing = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("RuinaWeddingRing"));
        }

        private void MasterSummon_onServerMasterSummonGlobal(MasterSummon.MasterSummonReport summonReport) {
            Inventory summonInvertory = summonReport.summonMasterInstance?.inventory;
            if (summonInvertory) {
                Inventory leaderInventory = summonReport.leaderMasterInstance?.inventory;
                if (leaderInventory) {
                    summonInvertory.GiveItem(_weddingRing, leaderInventory.GetItemCount(_weddingRing) - summonInvertory.GetItemCount(_weddingRing));
                }
            }
        }
    }
}