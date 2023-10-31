using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class GlobalEventTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            var victimBody = damageReport.victimBody;
            if (!victimBody) {
                return;
            }
            if (victimBody.bodyIndex == IndexCollections.BodyIndexCollection.EquipmentDroneBody) {
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(victimBody.inventory.currentEquipmentIndex), victimBody.corePosition, Vector3.up * 15f);
            }
            var bossDropTable = victimBody.gameObject.GetComponent<DeathRewards>()?.bossDropTable;
            if (bossDropTable && Util.CheckRoll(ModConfig.测试用1.Value, damageReport.attackerMaster)) {
                PickupDropletController.CreatePickupDroplet(bossDropTable.GenerateDrop(Run.instance.treasureRng), victimBody.corePosition, Vector3.up * 15f);
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (victimBody?.master == null) {
                GoldenCoastPlus.GoldenCoastPlus.EnableGoldElites.Value = false;
            }
            orig(self, damageInfo, victim);
            GoldenCoastPlus.GoldenCoastPlus.EnableGoldElites.Value = true;
            if (NetworkServer.active) {
                if (victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[1].buffIndex) >= 3 * (1 + victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[0].buffIndex))) {
                    DotController.InflictDot(victim, damageInfo.attacker, DeepRot.deepRotDOT, 20f);
                }
            }
        }

        private void IL_GlobalEventManager_OnCharacterDeath(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/Effects/ImpactEffects/Bandit2ResetEffect"))) {
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.EmitDelegate((CharacterBody attackerBody) => attackerBody.inventory.DeductActiveEquipmentCooldown(float.MaxValue));
            } else {
                Main.Logger.LogError("ResetCooldownsOnKill Hook Error");
            }
        }
    }
}