using BtpTweak.Pools;
using BtpTweak.RoR2Indexes;
using BtpTweak.Tweaks.ItemTweaks;
using BtpTweak.Utils;
using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Newtonsoft.Json.Utilities;
using RoR2;
using System;
using System.Threading.Tasks;
using TPDespair.ZetAspects;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class GlobalEventTweak : TweakBase<GlobalEventTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitAll += GlobalEventManager_OnHitAll;
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.OnHitEnemy += IL_GlobalEventManager_OnHitEnemy;
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

        private void GlobalEventManager_OnHitAll(On.RoR2.GlobalEventManager.orig_OnHitAll orig, GlobalEventManager self, DamageInfo damageInfo, GameObject hitObject) {
            if (damageInfo.procCoefficient == 0f || damageInfo.rejected || !damageInfo.attacker) {
                return;
            }
            if (!damageInfo.attacker.TryGetComponent<CharacterBody>(out var attackerBody)) {
                return;
            }
            var master = attackerBody.master;
            if (!master) {
                return;
            }
            var inventory = master.inventory;
            if (!inventory) {
                return;
            }
            int itemCount = inventory.GetItemCount(RoR2Content.Items.Behemoth);
            if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Behemoth)) {
                var attackInfo = new BlastAttackInfo {
                    crit = damageInfo.crit,
                    damageType = damageInfo.damageType,
                    procCoefficient = damageInfo.procCoefficient,
                    radius = BehemothTweak.Radius * itemCount,
                    attacker = damageInfo.attacker,
                    inflictor = damageInfo.inflictor,
                    procChainMask = damageInfo.procChainMask,
                    teamIndex = master.teamIndex,
                };
                attackInfo.procChainMask.AddWhiteProcs();
                BehemothPool.RentPool(attackInfo.attacker).AddBlastAttack(attackInfo,
                                                                     damageInfo.position,
                                                                     Util.OnHitProcDamage(damageInfo.damage, 0, BehemothTweak.BaseDamageCoefficient));
            }
            if (attackerBody.HasBuff(RoR2Content.Buffs.AffixBlue)) {
                var stack = Catalog.GetStackMagnitude(attackerBody, RoR2Content.Buffs.AffixBlue);
                var damageCoefficient = Configuration.AspectBlueBaseDamage.Value + Configuration.AspectBlueStackDamage.Value * (stack - 1f);
                if (master.teamIndex != TeamIndex.Player) {
                    damageCoefficient *= Configuration.AspectBlueMonsterDamageMult.Value;
                }
                LightningStakePool.RentPool(damageInfo.attacker).AddProjectile(new() { attacker = damageInfo.attacker, isCrit = damageInfo.crit, },
                                                                               damageInfo.position,
                                                                               Util.OnHitProcDamage(damageInfo.damage, 0, damageCoefficient));
            }
        }

        private void GlobalEventManager_OnCrit(On.RoR2.GlobalEventManager.orig_OnCrit orig, GlobalEventManager self, CharacterBody body, DamageInfo damageInfo, CharacterMaster master, float procCoefficient, ProcChainMask procChainMask) {
            EffectManager.SimpleImpactEffect(body.critMultiplier > 2f ? AssetReferences.critsparkHeavy : AssetReferences.critspark, damageInfo?.position ?? body.corePosition, Vector3.up, transmit: true);
            if (procCoefficient > 0f) {
                var inventory = master.inventory;
                if (!procChainMask.HasProc(ProcType.HealOnCrit)) {
                    procChainMask.AddProc(ProcType.HealOnCrit);
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.HealOnCrit);
                    if (itemCount > 0 && (bool)body.healthComponent) {
                        Util.PlaySound("Play_item_proc_crit_heal", body.gameObject);
                        body.healthComponent.Heal((4f + itemCount * 4f) * procCoefficient, procChainMask);
                    }
                }
                if (inventory.GetItemCount(RoR2Content.Items.AttackSpeedOnCrit) > 0) {
                    body.AddTimedBuff(RoR2Content.Buffs.AttackSpeedOnCrit, 3f * procCoefficient);
                }
                int itemCount2 = inventory.GetItemCount(JunkContent.Items.CooldownOnCrit);
                if (itemCount2 > 0) {
                    Util.PlaySound("Play_item_proc_crit_cooldown", body.gameObject);
                    body.skillLocator?.DeductCooldownFromAllSkillsServer(itemCount2 * procCoefficient);
                }
            }
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
            if (victimBody.bodyIndex == BodyIndexes.EquipmentDrone) {
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(victimBody.inventory.currentEquipmentIndex), victimBody.corePosition, Vector3.up * 15f);
            }
            if (victimBody.TryGetComponent<DeathRewards>(out var deathRewards) && deathRewards.bossDropTable && Util.CheckRoll(ModConfig.Boss物品掉率.Value, damageReport.attackerMaster)) {
                PickupDropletController.CreatePickupDroplet(deathRewards.bossDropTable.GenerateDrop(Run.instance.treasureRng), victimBody.corePosition, Vector3.up * 15f);
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