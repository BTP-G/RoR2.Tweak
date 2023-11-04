using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class GlobalEventTweak : TweakBase<GlobalEventTweak> {

        public override void SetEventHandlers() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.OnHitEnemy += IL_GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            GlobalEventManager.onCharacterDeathGlobal -= GlobalEventManager_onCharacterDeathGlobal;
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= IL_GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void IL_GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("GoldOnHit")))) {
                iLCursor.Emit(OpCodes.Ldarg_1);
                iLCursor.Emit(OpCodes.Ldarg_2);
                iLCursor.Emit(OpCodes.Ldloc_1);
                iLCursor.EmitDelegate((DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                    if (attackerBody.HasBuff(GoldenCoastPlus.GoldenCoastPlus.affixGoldDef)) {
                        if (victim.TryGetComponent<DeathRewards>(out var deathRewards)) {
                            attackerBody.master.GiveMoney((uint)(deathRewards.goldReward * damageInfo.procCoefficient));
                        }
                    }
                });
            } else {
                Main.Logger.LogError("AspectGold Hook Failed!");
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            var victimBody = damageReport.victimBody;
            if (!victimBody) {
                return;
            }
            if (victimBody.bodyIndex == RoR2Indexes.BodyIndexes.EquipmentDroneBody) {
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