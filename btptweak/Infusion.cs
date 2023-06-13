using RoR2;
using RoR2.Orbs;

namespace BtpTweak {

    internal class Infusion {

        public static void 浸剂修改() {
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
        }

        public static void RemoveHook() {
            On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
        }

        private static void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport) {
            if (damageReport.victimBody?.HasBuff(AncientScepter.AncientScepterMain.perishSongDebuff) ?? false) {
                foreach (CharacterMaster characterMaster in CharacterMaster.readOnlyInstancesList) {
                    if (characterMaster.masterIndex == damageReport.victimMaster.masterIndex) {
                        characterMaster.TrueKill(damageReport.attacker, null, DamageType.Generic);
                    }
                }
            }
            orig(self, damageReport);
            Inventory inventory = damageReport.attackerMaster?.inventory;
            if (inventory != null) {
                int item_infusion_count = inventory.GetItemCount(RoR2Content.Items.Infusion);
                if (item_infusion_count > 0) {
                    uint max_hp = (uint)(item_infusion_count * 100);
                    InfusionOrb infusionOrb = new InfusionOrb {
                        origin = damageReport.victim.transform.position,
                        target = Util.FindBodyMainHurtBox(damageReport.attackerBody),
                        maxHpValue = item_infusion_count
                    };
                    if (inventory.infusionBonus >= max_hp) {
                        infusionOrb.maxHpValue *= BtpTweak.浸剂击杀奖励倍率_.Value;
                        OrbManager.instance.AddOrb(infusionOrb);
                    } else {
                        infusionOrb.maxHpValue *= BtpTweak.浸剂击杀奖励倍率_.Value - 1;
                        OrbManager.instance.AddOrb(infusionOrb);
                    }
                }
            }
        }
    }
}