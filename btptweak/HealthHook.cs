using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class HealthHook {
        public static float 老米爆发伤害保护_ = 1;
        public static float 老米触发伤害保护_ = 1;
        public static float 伤害阈值_ = 0.01f;
        public static float 虚灵爆发伤害保护_ = 1;
        public static float 虚灵触发伤害保护_ = 1;
        public static BodyIndex 老米_;
        public static BodyIndex 负伤老米_;

        public static void AddHook() {
            IL.RoR2.HealthComponent.TakeDamage += IL_HealthComponent_TakeDamage;
            IL.RoR2.HealthComponent.TriggerOneShotProtection += IL_HealthComponent_TriggerOneShotProtection;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public static void RemoveHook() {
            IL.RoR2.HealthComponent.TakeDamage -= IL_HealthComponent_TakeDamage;
            IL.RoR2.HealthComponent.TriggerOneShotProtection -= IL_HealthComponent_TriggerOneShotProtection;
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            if (NetworkServer.active) {
                CharacterBody victimBody = self.body;
                if (self.shield > 0 && damageInfo.damageType == DamageType.DoT) {
                    damageInfo.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.25f : 0.5f;
                }
                orig(self, damageInfo);
                if (Main.是否选择造物难度_) {
                    if (TeamIndex.Monster == victimBody.teamComponent.teamIndex && (victimBody.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) == 0) && self.isHealthLow) {
                        if (PhaseCounter.instance && victimBody.bodyIndex == 老米_) {
                            victimBody.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                            victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 10);
                            victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                            Util.CleanseBody(self.body, true, false, false, false, true, true);
                        } else {
                            victimBody.AddTimedBuff(RoR2Content.Buffs.TonicBuff, 10 * (1 + Run.instance.stageClearCount));
                            victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex, 1 + Run.instance.stageClearCount);
                            Util.CleanseBody(self.body, true, false, false, false, true, false);
                        }
                    }
                }
            }
        }

        private static void IL_HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array1 = new Func<Instruction, bool>[4];
            array1[0] = (Instruction x) => ILPatternMatchingExt.Match(x, OpCodes.Brtrue);
            array1[1] = (Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0);
            array1[2] = (Instruction x) => ILPatternMatchingExt.MatchLdfld(x, typeof(HealthComponent).GetFieldCached("body"));
            array1[3] = (Instruction x) => ILPatternMatchingExt.MatchLdsfld(x, typeof(DLC1Content.Buffs).GetFieldCached("BearVoidReady"));
            if (ilcursor.TryGotoNext(array1)) {  // 113
                ilcursor.Index += 10;
                ilcursor.RemoveRange(45);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo) {
                    if (healthComponent.body.HasBuff(DLC1Content.Buffs.EliteVoid.buffIndex) || Util.CheckRoll(50f)) {
                        EffectData effectData = new() {
                            origin = damageInfo.position,
                            rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                        };
                        EffectManager.SpawnEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab, effectData, true);
                        damageInfo.rejected = true;
                    }
                    healthComponent.body.RemoveBuff(DLC1Content.Buffs.BearVoidReady.buffIndex);
                    int itemCount = healthComponent.body.inventory.GetItemCount(DLC1Content.Items.BearVoid);
                    healthComponent.body.AddTimedBuff(DLC1Content.Buffs.BearVoidCooldown, 15f * Mathf.Pow(0.9f, itemCount));
                });
            } else {
                Main.logger_.LogError("BearVoid Hook Error");
            }
            //======
            Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[3];
            array2[0] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 20);
            array2[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcI4(x, 0);
            array2[2] = (Instruction x) => ILPatternMatchingExt.Match(x, OpCodes.Ble);
            if (ilcursor.TryGotoNext(array2)) {  //378
                ilcursor.Index += 3;
                ilcursor.RemoveRange(71);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 20);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, int itemCount) {
                    CharacterBody victimBody = healthComponent.body;
                    if (victimBody.HasBuff(DLC1Content.Buffs.StrongerBurn.buffIndex) || victimBody.HasBuff(RoR2Content.Buffs.OnFire.buffIndex)) {
                        return;
                    }
                    CharacterBody attacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    Vector3 corePosition = victimBody.corePosition;
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab, corePosition, Quaternion.identity);
                    DelayBlast component = gameObject.GetComponent<DelayBlast>();
                    component.position = corePosition;
                    component.baseDamage = Util.OnKillProcDamage(attacterBody.damage, 2.6f + 1.6f * (itemCount - 1));
                    component.baseForce = 1000f;
                    component.radius = 16f;
                    component.attacker = damageInfo.attacker;
                    component.inflictor = null;
                    component.crit = Util.CheckRoll(attacterBody.crit, attacterBody.master);
                    component.maxTimer = 0.2f;
                    component.damageColorIndex = DamageColorIndex.Void;
                    component.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                    gameObject.GetComponent<TeamFilter>().teamIndex = attacterBody.teamComponent.teamIndex;
                    NetworkServer.Spawn(gameObject);
                    InflictDotInfo inflictDotInfo = new() {
                        attackerObject = damageInfo.attacker,
                        damageMultiplier = 1,
                        dotIndex = DotController.DotIndex.Burn,
                        duration = 100,
                        totalDamage = healthComponent.fullHealth,
                        victimObject = healthComponent.gameObject,
                    };
                    if (attacterBody.inventory) {
                        StrengthenBurnUtils.CheckDotForUpgrade(attacterBody.inventory, ref inflictDotInfo);
                    }
                    DotController.InflictDot(ref inflictDotInfo);
                });
            } else {
                Main.logger_.LogError("ExplodeOnDeathVoid Hook Error");
            }
            //======
            array2[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 100f);
            array2[1] = (Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 1);
            array2[2] = (Instruction x) => ILPatternMatchingExt.MatchLdfld<DamageInfo>(x, "procCoefficient");
            if (ilcursor.TryGotoNext(array2)) {  // 566
                ++ilcursor.Index;
                ilcursor.Emit(OpCodes.Ldc_R4, 80f);
                ilcursor.Emit(OpCodes.Sub);
            } else {
                Main.logger_.LogError("PermanentDebuffOnHit Hook Error");
            }
            //======
            Func<Instruction, bool>[] array3 = new Func<Instruction, bool>[1];
            array3[0] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 32);
            if (ilcursor.TryGotoNext(array3)) {  // 665
                ilcursor.Index -= 7;
                ilcursor.RemoveRange(2);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent) {
                    return (healthComponent.barrier + healthComponent.shield) > 0;
                });
            } else {
                Main.logger_.LogError("BossDamageBonus Hook Error");
            }
            //======
            Func<Instruction, bool>[] array4 = new Func<Instruction, bool>[4];
            array4[0] = ((Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0));
            array4[1] = ((Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0));
            array4[2] = ((Instruction x) => ILPatternMatchingExt.MatchLdflda<HealthComponent>(x, "itemCounts"));
            array4[3] = ((Instruction x) => ILPatternMatchingExt.MatchLdfld(x, "RoR2.HealthComponent/ItemCounts", "parentEgg"));
            if (ilcursor.TryGotoNext(array4)) {
                ilcursor.RemoveRange(19);
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, float damage) {
                    healthComponent.Heal(healthComponent.itemCounts.parentEgg * (damage * 0.01f + 20f), default, true);
                    EntitySoundManager.EmitSoundServer(LegacyResourcesAPI.Load<NetworkSoundEventDef>("NetworkSoundEventDefs/nseParentEggHeal").index, healthComponent.gameObject);
                });
            } else {
                Main.logger_.LogError("parentEgg hook error");
            }
            //======
            array3[0] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 41);
            if (ilcursor.TryGotoNext(array3)) {  // 901
                ilcursor.Index += 11;
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, TeamIndex teamIndex, float damage) {
                    if (Main.是否选择造物难度_) {
                        CharacterBody victimBody = healthComponent.body;
                        if (FinalBossHook.处于天文馆_ && victimBody.isBoss) {
                            if (damage < 伤害阈值_ * healthComponent.fullHealth) {  // 虚灵
                                damage = Mathf.Min(damage, 虚灵触发伤害保护_ * healthComponent.fullHealth);
                            } else {
                                damage *= 虚灵爆发伤害保护_;
                                damage = Mathf.Min(damage, 0.1f * healthComponent.health);
                                Util.CleanseBody(victimBody, true, false, false, true, true, true);
                            }
                        } else if (PhaseCounter.instance) {
                            BodyIndex selfIndex = victimBody.bodyIndex;
                            if (selfIndex == 老米_) {  // 米斯历克斯
                                if (damage < 伤害阈值_ * healthComponent.fullCombinedHealth) {
                                    damage = Mathf.Min(damage, 老米触发伤害保护_ * healthComponent.fullCombinedHealth);
                                } else {
                                    damage *= 老米爆发伤害保护_;
                                    damage = Mathf.Min(damage, 0.2f * healthComponent.combinedHealth);
                                    Util.CleanseBody(victimBody, true, false, false, true, true, true);
                                }
                            } else if (selfIndex == 负伤老米_) {
                                damage = Mathf.Max(damage, healthComponent.fullCombinedHealth / victimBody.level * 0.1f);
                            }
                        }
                        if (!victimBody.hasOneShotProtection && healthComponent.shield > 0) {
                            float protectedHealth = healthComponent.shield + healthComponent.barrier;
                            if (damage > protectedHealth) {
                                victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 3 * protectedHealth / healthComponent.fullShield);
                                return damage - protectedHealth;
                            }
                        }
                    }
                    return damage;
                });
                ilcursor.Emit(OpCodes.Stloc, 6);
            } else {
                Main.logger_.LogError("Enemy TakeDamage Hook Error");
            }
            //======
            array3[0] = (Instruction x) => ILPatternMatchingExt.MatchLdsfld(x, typeof(HealthComponent.AssetReferences).GetFieldCached("critGlassesVoidExecuteEffectPrefab"));
            if (ilcursor.TryGotoNext(array3)) {  // 1359
                ilcursor.Index -= 2;
                ilcursor.RemoveRange(10);
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent) {
                    healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse.buffIndex);
                });
            } else {
                Main.logger_.LogError("critGlassesVoid Hook Error");
            }
        }

        private static void IL_HealthComponent_TriggerOneShotProtection(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 0.1f);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 0.5f);
            } else {
                Main.logger_.LogError("ospTimer Hook Error");
            }
        }
    }
}