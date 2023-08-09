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
        public static BodyIndex 虚灵_;

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
                if (self.shield > 0 && damageInfo.damageType == DamageType.DoT) {
                    damageInfo.damage *= 0.5f;
                }
                orig(self, damageInfo);
                if (Main.是否选择造物难度_) {
                    CharacterBody body = self.body;
                    if (TeamIndex.Monster == body.teamComponent.teamIndex && (body.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) == 0) && self.isHealthLow) {
                        if (PhaseCounter.instance && self.name.StartsWith("Bro")) {
                            body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                            body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 3);
                            body.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                            self.ospTimer = 0.5f;
                        } else {
                            body.AddTimedBuff(RoR2Content.Buffs.TonicBuff, 10 * (1 + Run.instance.stageClearCount));
                            body.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex, 1 + Run.instance.stageClearCount);
                            self.ospTimer = 0.25f;
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
                        EffectData effectData2 = new() {
                            origin = damageInfo.position,
                            rotation = Util.QuaternionSafeLookRotation((damageInfo.force != Vector3.zero) ? damageInfo.force : UnityEngine.Random.onUnitSphere)
                        };
                        EffectManager.SpawnEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab, effectData2, true);
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
                    if (healthComponent.body.HasBuff(RoR2Content.Buffs.OnFire.buffIndex)) {
                        return;
                    }
                    CharacterBody attacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    Vector3 corePosition = Util.GetCorePosition(healthComponent.body);
                    float baseDamage = Util.OnKillProcDamage(attacterBody.damage, 2.6f + 1.6f * (itemCount - 1));
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(HealthComponent.AssetReferences.explodeOnDeathVoidExplosionPrefab, corePosition, Quaternion.identity);
                    DelayBlast component = gameObject.GetComponent<DelayBlast>();
                    component.position = corePosition;
                    component.baseDamage = baseDamage;
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
                    InflictDotInfo dotInfo = new() {
                        attackerObject = damageInfo.attacker,
                        damageMultiplier = 1,
                        dotIndex = DotController.DotIndex.PercentBurn,
                        duration = 100,
                        totalDamage = healthComponent.fullHealth,
                        victimObject = healthComponent.gameObject,
                    };
                    DotController.InflictDot(ref dotInfo);
                });
            } else {
                Main.logger_.LogError("ExplodeOnDeathVoid Hook Error");
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
                    healthComponent.Heal(healthComponent.itemCounts.parentEgg * damage * 0.01f, default, true);
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
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, float damage) {
                    if (Main.是否选择造物难度_) {
                        if (FinalBossHook.处于天文馆_ && healthComponent.body.bodyIndex == 虚灵_) {
                            if (damage < 伤害阈值_ * healthComponent.fullHealth) {  // 虚灵
                                damage = Mathf.Min(damage, 虚灵触发伤害保护_ * healthComponent.fullHealth);
                            } else {
                                damage *= 虚灵爆发伤害保护_;
                                damage = Mathf.Min(damage, 0.1f * healthComponent.health);
                                Util.CleanseBody(healthComponent.body, true, false, false, true, true, true);
                            }
                        } else if (PhaseCounter.instance) {
                            BodyIndex selfIndex = healthComponent.body.bodyIndex;
                            if (selfIndex == 老米_) {  // 米斯历克斯
                                if (damage < 伤害阈值_ * healthComponent.fullCombinedHealth) {
                                    damage = Mathf.Min(damage, 老米触发伤害保护_ * healthComponent.fullCombinedHealth);
                                } else {
                                    damage *= 老米爆发伤害保护_;
                                    damage = Mathf.Min(damage, 0.2f * healthComponent.combinedHealth);
                                    Util.CleanseBody(healthComponent.body, true, false, false, true, true, true);
                                }
                            } else if (selfIndex == 负伤老米_) {
                                damage = Mathf.Max(damage, healthComponent.fullCombinedHealth / healthComponent.body.level * 0.1f);
                            }
                        }
                        if (!healthComponent.body.hasOneShotProtection && healthComponent.shield > 0) {
                            float protectedHealth = healthComponent.shield + healthComponent.barrier;
                            if (damage > protectedHealth) {
                                healthComponent.body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 3 * protectedHealth / healthComponent.fullShield);
                                return protectedHealth;
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