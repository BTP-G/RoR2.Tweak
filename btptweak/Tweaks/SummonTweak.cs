using RoR2;

namespace BtpTweak.Tweaks {

    internal partial class SummonTweak : TweakBase {
        private ItemIndex _weddingRingIndex;

        public override void AddHooks() {
            base.AddHooks();
            MasterSummon.onServerMasterSummonGlobal += MasterSummon_onServerMasterSummonGlobal;
        }

        public override void Load() {
            base.Load();
            _weddingRingIndex = ItemCatalog.FindItemIndex("RuinaWeddingRing");
        }

        private void MasterSummon_onServerMasterSummonGlobal(MasterSummon.MasterSummonReport summonReport) {
            Inventory summonInvertory = summonReport.summonMasterInstance?.inventory;
            if (summonInvertory) {
                Inventory leaderInventory = summonReport.leaderMasterInstance?.inventory;
                if (leaderInventory) {
                    summonInvertory.GiveItem(_weddingRingIndex, leaderInventory.GetItemCount(_weddingRingIndex) - summonInvertory.GetItemCount(_weddingRingIndex));
                }
            }
        }
    }
}