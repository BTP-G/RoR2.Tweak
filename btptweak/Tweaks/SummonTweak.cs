using RoR2;
using System.Collections.Generic;

namespace BtpTweak.Tweaks
{

    internal class SummonTweak : TweakBase {
        public static readonly Dictionary<MasterCatalog.MasterIndex, MasterName> MasterIndexToName_ = new();

        public enum MasterName : byte {
            None = 0,
            BeetleGuardAllyMaster,
            Drone1Master,
            Drone2Master,
            DroneBackupMaster,
            DroneCommanderMaster,
            DroneMissileMaster,
            EmergencyDroneMaster,
            EquipmentDroneMaster,
            FlameDroneMaster,
            MegaDroneMaster,
            MinorConstructOnKillMaster,
            RoboBallGreenBuddyMaster,
            RoboBallRedBuddyMaster,
            SquidTurretMaster,
            Turret1Master,
            Count,
        }

        public override void AddHooks() {
            base.AddHooks();
            MasterSummon.onServerMasterSummonGlobal += MasterSummon_onServerMasterSummonGlobal;
        }

        public override void Load() {
            base.Load();
            for (MasterName masterName = MasterName.None + 1; masterName < MasterName.Count; ++masterName) {
                MasterIndexToName_.Add(MasterCatalog.FindMasterIndex(masterName.ToString()), masterName);
            }
        }

        private void MasterSummon_onServerMasterSummonGlobal(MasterSummon.MasterSummonReport summonReport) {
            //if (MasterIndexToName_.TryGetValue(summonReport.summonMasterInstance.masterIndex, out var masterName)) {
            //    Inventory summonInvertory = summonReport.summonMasterInstance.inventory;
            //    Inventory leaderInventory = summonReport.leaderMasterInstance.inventory;
            //    switch (masterName) {
            //        case MasterName.BeetleGuardAllyMaster:
            //            summonInvertory.AddItemsFrom(leaderInventory, ItemAddFilterDelegates.FilterDamageAndHealing);
            //            break;

            // case MasterName.Drone1Master:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.Drone2Master:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterHealing); break;

            // case MasterName.DroneBackupMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.DroneCommanderMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.DroneMissileMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.EmergencyDroneMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterHealing); break;

            // case MasterName.EquipmentDroneMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterUtility); break;

            // case MasterName.FlameDroneMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.MegaDroneMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.MinorConstructOnKillMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.RoboBallGreenBuddyMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.RoboBallRedBuddyMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamage); break;

            // case MasterName.SquidTurretMaster:
            // summonInvertory.AddItemsFrom(leaderInventory,
            // ItemAddFilterDelegates.FilterDamageAndHealing); break;

            //        case MasterName.Turret1Master:
            //            summonInvertory.AddItemsFrom(leaderInventory, ItemAddFilterDelegates.FilterDamage);
            //            break;
            //    }
            //}
        }

        private class ItemAddFilterDelegates {

            public static bool FilterDamage(ItemIndex itemIndex) {
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                return itemDef.DoesNotContainTag(ItemTag.CannotCopy) && itemDef.DoesNotContainTag(ItemTag.AIBlacklist) && itemDef.ContainsTag(ItemTag.Damage);
            }

            public static bool FilterHealing(ItemIndex itemIndex) {
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                return itemDef.DoesNotContainTag(ItemTag.CannotCopy) && itemDef.DoesNotContainTag(ItemTag.AIBlacklist) && itemDef.ContainsTag(ItemTag.Healing);
            }

            public static bool FilterUtility(ItemIndex itemIndex) {
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                return itemDef.DoesNotContainTag(ItemTag.CannotCopy) && itemDef.DoesNotContainTag(ItemTag.AIBlacklist) && itemDef.ContainsTag(ItemTag.Utility);
            }

            public static bool FilterDamageAndHealing(ItemIndex itemIndex) {
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                return itemDef.DoesNotContainTag(ItemTag.CannotCopy) && itemDef.DoesNotContainTag(ItemTag.AIBlacklist) && (itemDef.ContainsTag(ItemTag.Damage) || itemDef.ContainsTag(ItemTag.Healing));
            }
        }
    }
}