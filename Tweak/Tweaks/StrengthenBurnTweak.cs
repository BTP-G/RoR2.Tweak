using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal static class StrengthenBurnTweak {

        [ModLoadMessageHandler]
        private static void Init() {
            On.RoR2.StrengthenBurnUtils.CheckDotForUpgrade += StrengthenBurnUtils_CheckDotForUpgrade;
        }

        private static void StrengthenBurnUtils_CheckDotForUpgrade(On.RoR2.StrengthenBurnUtils.orig_CheckDotForUpgrade orig, Inventory inventory, ref InflictDotInfo dotInfo) {
            if (dotInfo.dotIndex == DotController.DotIndex.Burn || dotInfo.dotIndex == DotController.DotIndex.Helfire) {
                var count = inventory.GetItemCount(DLC1Content.Items.StrengthenBurn);
                if (dotInfo.victimObject.TryGetComponent<CharacterBody>(out var body)) {
                    count += body.GetBuffCount(DLC2Content.Buffs.Oiled);
                }
                if (count > 0) {
                    dotInfo.preUpgradeDotIndex = new DotController.DotIndex?(dotInfo.dotIndex);
                    dotInfo.dotIndex = DotController.DotIndex.StrongerBurn;
                    var num = 1 + (3 * (1 + count));
                    dotInfo.damageMultiplier *= num;
                    dotInfo.totalDamage *= num;
                }
            }
        }

        public static void TryUpgrade(ref this InflictDotInfo dotInfo, Inventory inventory, CharacterBody victimBody) {
            if (dotInfo.dotIndex == DotController.DotIndex.Burn || dotInfo.dotIndex == DotController.DotIndex.Helfire) {
                var count = inventory.GetItemCount(DLC1Content.Items.StrengthenBurn)
                    + victimBody.GetBuffCount(DLC2Content.Buffs.Oiled);
                if (count > 0) {
                    dotInfo.preUpgradeDotIndex = new DotController.DotIndex?(dotInfo.dotIndex);
                    dotInfo.dotIndex = DotController.DotIndex.StrongerBurn;
                    var num = 1 + (3 * (1 + count));
                    dotInfo.damageMultiplier *= num;
                    dotInfo.totalDamage *= num;
                }
            }
        }
    }
}
