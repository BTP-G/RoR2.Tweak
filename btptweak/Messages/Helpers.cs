using BtpTweak.Tweaks.ItemTweaks;
using R2API.Networking.Interfaces;
using RoR2;
using UnityEngine.Networking;

namespace BtpTweak.Messages {

    public static class Helpers {

        public static void GiveMoneyAuthority(this CharacterMaster master, uint amount, bool changeToRemoveMoney = false) {
            if (NetworkServer.active) {
                if (changeToRemoveMoney) {
                    master.money -= amount;
                } else {
                    master.money += amount;
                }
            } else {
                new MoneyMessage(master, amount, changeToRemoveMoney).Send(R2API.Networking.NetworkDestination.Server);
            }
        }

        public static void GiveItemAuthority(this Inventory inventory, ItemIndex itemIndex, int itemCount) {
            if (NetworkServer.active) {
                inventory.GiveItem(itemIndex, itemCount);
            } else {
                new ItemMessage(inventory, itemIndex, itemCount).Send(R2API.Networking.NetworkDestination.Server);
            }
        }

        [Server]
        public static void 同步特拉法梅的祝福(this LunarWingsState state) {
            if (NetworkServer.active) {
                new LunarWingsMessage(state).Send(R2API.Networking.NetworkDestination.Clients);
            }
        }
    }
}