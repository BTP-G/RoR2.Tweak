using BtpTweak.RoR2Indexes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class GlobalEventTweak : TweakBase<GlobalEventTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.OnHitEnemy += IL_GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            if (RunInfo.是否选择造物难度 && damageReport.hitLowHealth && damageReport.victim.alive && damageReport.victimTeamIndex == TeamIndex.Monster) {
                var victimBody = damageReport.victimBody;
                if (!victimBody.inventory || victimBody.inventory.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) > 0) {
                    return;
                }
                if (PhaseCounter.instance && victimBody.bodyIndex == BodyIndexes.Brother) {
                    victimBody.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                    victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 20);
                    victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                    Util.CleanseBody(victimBody, true, false, false, false, true, true);
                } else {
                    victimBody.AddTimedBuff(RoR2Content.Buffs.TonicBuff, 20);
                    victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                    Util.CleanseBody(victimBody, true, false, false, false, true, false);
                }
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            var victimBody = damageReport.victimBody;
            if (victimBody.bodyIndex == RoR2Indexes.BodyIndexes.EquipmentDrone) {
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(victimBody.inventory.currentEquipmentIndex), victimBody.corePosition, Vector3.up * 15f);
            }
            if (victimBody.TryGetComponent<DeathRewards>(out var deathRewards) && deathRewards.bossDropTable && Util.CheckRoll(ModConfig.Boss物品掉率.Value, damageReport.attackerMaster)) {
                PickupDropletController.CreatePickupDroplet(deathRewards.bossDropTable.GenerateDrop(Run.instance.treasureRng), victimBody.corePosition, Vector3.up * 15f);
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
            if (victim.TryGetComponent<CharacterBody>(out var victimBody) && !victimBody.master) {
                GoldenCoastPlus.GoldenCoastPlus.EnableGoldElites.Value = false;
                orig(self, damageInfo, victim);
                GoldenCoastPlus.GoldenCoastPlus.EnableGoldElites.Value = true;
            } else {
                orig(self, damageInfo, victim);
            }
        }

        private void IL_GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("GoldOnHit")))) {
                iLCursor.Emit(OpCodes.Ldarg_1);
                iLCursor.Emit(OpCodes.Ldarg_2);
                iLCursor.Emit(OpCodes.Ldloc_1);
                iLCursor.EmitDelegate((DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                    if (attackerBody.HasBuff(GoldenCoastPlus.GoldenCoastPlus.affixGoldDef) && victim.TryGetComponent<DeathRewards>(out var deathRewards)) {
                        attackerBody.master.GiveMoney((uint)(deathRewards.goldReward * damageInfo.procCoefficient));
                    }
                });
            } else {
                Main.Logger.LogError("AspectGold Hook Failed!");
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