using BTP.RoR2Plugin.RoR2Indexes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class GlobalEventTweak : TweakBase<GlobalEventTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
        }

        private void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask) {
            EffectManager.SimpleImpactEffect(body.critMultiplier > 2f ? AssetReferences.critsparkHeavy : AssetReferences.critspark, damageInfo?.position ?? body.corePosition, Vector3.up, transmit: true);
            if (procCoefficient > 0f) {
                var inventory = master.inventory;
                if (inventory.GetItemCount(RoR2Content.Items.AttackSpeedOnCrit) > 0) {
                    body.AddTimedBuff(RoR2Content.Buffs.AttackSpeedOnCrit, 3f * procCoefficient);
                }
                int itemCount2 = inventory.GetItemCount(JunkContent.Items.CooldownOnCrit);
                if (itemCount2 > 0 && body.skillLocator) {
                    Util.PlaySound("Play_item_proc_crit_cooldown", body.gameObject);
                    body.skillLocator.DeductCooldownFromAllSkillsServer(itemCount2 * procCoefficient);
                }
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            var victimBody = damageReport.victimBody;
            if (victimBody.bodyIndex == BodyIndexes.EquipmentDrone) {
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(victimBody.inventory.currentEquipmentIndex), victimBody.corePosition, Vector3.up * 15f);
            }
            if (victimBody.TryGetComponent<DeathRewards>(out var deathRewards) && deathRewards.bossDropTable && Util.CheckRoll(Settings.Boss物品掉率.Value)) {
                PickupDropletController.CreatePickupDroplet(deathRewards.bossDropTable.GenerateDrop(Run.instance.treasureRng), victimBody.corePosition, Vector3.up * 15f);
            }
        }

        private void IL_GlobalEventManager_OnCharacterDeath(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => x.MatchLdstr("Prefabs/Effects/ImpactEffects/Bandit2ResetEffect"))) {
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.EmitDelegate((CharacterBody attackerBody) => attackerBody.inventory.DeductActiveEquipmentCooldown(float.MaxValue));
            } else {
                "ResetCooldownsOnKill Hook Error".LogError();
            }
        }
    }
}