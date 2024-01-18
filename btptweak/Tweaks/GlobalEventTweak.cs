using BtpTweak.Pools;
using BtpTweak.Pools.ProjectilePools;
using BtpTweak.RoR2Indexes;
using BtpTweak.Tweaks.ItemTweaks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using TPDespair.ZetAspects;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class GlobalEventTweak : TweakBase<GlobalEventTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            BetterEvents.OnHitAll += BetterEvents_OnHitAll;
            BetterEvents.OnHitEnemy += BetterEvents_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.GlobalEventManager.OnCrit += GlobalEventManager_OnCrit;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
        }

        private void BetterEvents_OnHitEnemy(DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) {
            if (attackerBody.HasBuff(GoldenCoastPlus.GoldenCoastPlus.affixGoldDef) && victimBody.TryGetComponent<DeathRewards>(out var deathRewards)) {
                attackerBody.master.GiveMoney((uint)(deathRewards.goldReward * damageInfo.procCoefficient));
            }
        }

        private void BetterEvents_OnHitAll(DamageInfo damageInfo, CharacterBody attackerBody) {
            int itemCount = attackerBody.inventory.GetItemCount(RoR2Content.Items.Behemoth);
            if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Behemoth)) {
                var attackInfo = new BehemothPoolKey {
                    crit = damageInfo.crit,
                    damageType = damageInfo.damageType,
                    procCoefficient = damageInfo.procCoefficient,
                    radius = BehemothTweak.Radius * itemCount,
                    attacker = damageInfo.attacker,
                    procChainMask = damageInfo.procChainMask,
                    teamIndex = attackerBody.teamComponent.teamIndex,
                };
                attackInfo.procChainMask.AddWGRYProcs();
                BehemothPool.RentPool(attackInfo.attacker).AddBlastAttack(attackInfo,
                                                                     damageInfo.position,
                                                                     Util.OnHitProcDamage(damageInfo.damage, 0, BehemothTweak.BaseDamageCoefficient));
            }
            if (attackerBody.HasBuff(RoR2Content.Buffs.AffixBlue)) {
                var stack = Catalog.GetStackMagnitude(attackerBody, RoR2Content.Buffs.AffixBlue);
                var damageCoefficient = Configuration.AspectBlueBaseDamage.Value + Configuration.AspectBlueStackDamage.Value * (stack - 1f);
                if (attackerBody.teamComponent.teamIndex != TeamIndex.Player) {
                    damageCoefficient *= Configuration.AspectBlueMonsterDamageMult.Value;
                }
                LightningStakePool.RentPool(damageInfo.attacker).AddProjectile(
                    new() { attacker = damageInfo.attacker, isCrit = damageInfo.crit, procChainMask = damageInfo.procChainMask },
                    damageInfo.position,
                    Util.OnHitProcDamage(damageInfo.damage, 0, damageCoefficient));
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

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            if (RunInfo.是否选择造物难度 && damageReport.hitLowHealth && damageReport.victim.alive && damageReport.victimTeamIndex != TeamIndex.Player) {
                var victimBody = damageReport.victimBody;
                if (!victimBody.inventory) {
                    return;
                }
                if (damageReport.victimTeamIndex == TeamIndex.Monster
                    && victimBody.inventory.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) == 0) {
                    victimBody.AddTimedBuff(RoR2Content.Buffs.TonicBuff, victimBody.isBoss ? 40f : 20f);
                    victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                    Util.CleanseBody(victimBody, true, false, false, false, true, false);
                } else if (damageReport.victimTeamIndex == TeamIndex.Void
                    && !victimBody.HasBuff(DLC1Content.Buffs.VoidSurvivorCorruptMode.buffIndex)) {
                    victimBody.AddBuff(DLC1Content.Buffs.VoidSurvivorCorruptMode.buffIndex);
                    victimBody.AddBuff(DLC1Content.Buffs.KillMoveSpeed.buffIndex);
                }
            }
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            var victimBody = damageReport.victimBody;
            if (victimBody.bodyIndex == BodyIndexes.EquipmentDrone) {
                PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(victimBody.inventory.currentEquipmentIndex), victimBody.corePosition, Vector3.up * 15f);
            }
            if (victimBody.TryGetComponent<DeathRewards>(out var deathRewards) && deathRewards.bossDropTable && Util.CheckRoll(ModConfig.Boss物品掉率.Value)) {
                PickupDropletController.CreatePickupDroplet(deathRewards.bossDropTable.GenerateDrop(Run.instance.treasureRng), victimBody.corePosition, Vector3.up * 15f);
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