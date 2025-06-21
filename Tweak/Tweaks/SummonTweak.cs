using RoR2;

namespace BTP.RoR2Plugin.Tweaks {

    internal partial class SummonTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private ItemDef _weddingRing;

        void IModLoadMessageHandler.Handle() {
            MasterSummon.onServerMasterSummonGlobal += MasterSummon_onServerMasterSummonGlobal;
        }

        void IRoR2LoadedMessageHandler.Handle() {
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