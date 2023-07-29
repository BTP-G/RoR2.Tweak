using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class HealthHook {
        public static float 虚灵爆发伤害保护 = 1;
        public static float 虚灵触发伤害保护 = 1;
        public static float 老米爆发伤害保护 = 1;
        public static float 老米触发伤害保护 = 1;
        public static float 伤害阈值 = 0.01f;

        public static void AddHook() {
            IL.RoR2.HealthComponent.TakeDamage += IL_HealthComponent_TakeDamage;
            IL.RoR2.HealthComponent.TriggerOneShotProtection += IL_HealthComponent_TriggerOneShotProtection;
            On.RoR2.HealthComponent.Awake += HealthComponent_Awake;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public static void RemoveHook() {
            IL.RoR2.HealthComponent.TakeDamage -= IL_HealthComponent_TakeDamage;
            IL.RoR2.HealthComponent.TriggerOneShotProtection -= IL_HealthComponent_TriggerOneShotProtection;
            On.RoR2.HealthComponent.Awake -= HealthComponent_Awake;
            On.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
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
                    healthComponent.body.RemoveBuff(DLC1Content.Buffs.BearVoidReady);
                    int itemCount = healthComponent.body.inventory.GetItemCount(DLC1Content.Items.BearVoid);
                    healthComponent.body.AddTimedBuff(DLC1Content.Buffs.BearVoidCooldown, 15f * Mathf.Pow(0.9f, itemCount));
                });
            } else {
                BtpTweak.logger_.LogError("BearVoid Hook Error");
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
                        duration = 666,
                        totalDamage = healthComponent.fullHealth,
                        victimObject = healthComponent.gameObject,
                    };
                    DotController.InflictDot(ref dotInfo);
                });
            } else {
                BtpTweak.logger_.LogError("ExplodeOnDeathVoid Hook Error");
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
                BtpTweak.logger_.LogError("BossDamageBonus Hook Error");
            }
            //======
            array3[0] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 41);
            if (ilcursor.TryGotoNext(array3)) {  // 901
                ilcursor.Index += 11;
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, float damage) {
                    if (BtpTweak.是否选择造物难度_) {
                        if (BtpTweak.虚灵战斗阶段计数_ > 0 && TeamIndex.Void == healthComponent.body.teamComponent.teamIndex) {
                            if (damage < 伤害阈值 * healthComponent.fullHealth || damageInfo.damageType == DamageType.DoT) {  // 虚灵
                                damage = Mathf.Min(damage, 虚灵触发伤害保护 * healthComponent.fullHealth);
                            } else {
                                damage *= 虚灵爆发伤害保护;
                                healthComponent.ospTimer = 1;
                                damage = Mathf.Min(damage, 0.5f * healthComponent.health);
                            }
                        } else if (PhaseCounter.instance && TeamIndex.Monster == healthComponent.body.teamComponent.teamIndex && healthComponent.name.StartsWith("Bro")) {  // 米斯历克斯
                            if (PhaseCounter.instance.phase == 4) {
                                damage = Mathf.Max(damage, healthComponent.fullCombinedHealth / healthComponent.body.level * 0.1f);
                            } else {
                                if (damage < 伤害阈值 * healthComponent.fullCombinedHealth || damageInfo.damageType == DamageType.DoT) {
                                    damage = Mathf.Min(damage, 老米触发伤害保护 * healthComponent.fullCombinedHealth);
                                } else {
                                    damage *= 老米爆发伤害保护;
                                    healthComponent.ospTimer = 1;
                                    damage = Mathf.Min(damage, 0.5f * healthComponent.combinedHealth);
                                }
                            }
                        }
                        if (!healthComponent.body.hasOneShotProtection && healthComponent.shield > 0) {
                            float protectedHealth = healthComponent.shield + healthComponent.barrier;
                            if (damage > protectedHealth) {
                                healthComponent.body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost.buffIndex, 3 * protectedHealth / healthComponent.fullShield);
                                return protectedHealth;
                            }
                        }
                    }
                    return damage;
                });
                ilcursor.Emit(OpCodes.Stloc, 6);
            } else {
                BtpTweak.logger_.LogError("Enemy TakeDamage Hook Error");
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
                BtpTweak.logger_.LogError("critGlassesVoid Hook Error");
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
                BtpTweak.logger_.LogError("ospTimer Hook Error");
            }
        }

        private static void HealthComponent_Awake(On.RoR2.HealthComponent.orig_Awake orig, HealthComponent self) {
            orig(self);
            self.ospTimer = 0.5f;
        }

        private static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            if (NetworkServer.active) {
                if (BtpTweak.虚灵战斗阶段计数_ > 0) {
                    if (TeamIndex.Player == self.body.teamComponent.teamIndex && damageInfo.attacker) {
                        damageInfo.damage = Mathf.Max(0.15f * BtpTweak.虚灵战斗阶段计数_ * self.fullCombinedHealth, damageInfo.damage);
                    }
                }
                if (self.shield > 0 && damageInfo.damageType == DamageType.DoT) {
                    damageInfo.damage *= 0.5f;
                }
                orig(self, damageInfo);
                if (BtpTweak.是否选择造物难度_
                    && TeamIndex.Monster == self.body.teamComponent.teamIndex
                    && self.isHealthLow
                    && !self.body.HasBuff(RoR2Content.Buffs.TonicBuff.buffIndex)
                    && (self.body.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction) == 0)) {
                    if (PhaseCounter.instance && self.name.StartsWith("Bro")) {
                        self.body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                        self.body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost.buffIndex, 3);
                        self.ospTimer = 0.5f;
                    } else {
                        self.body.AddTimedBuff(RoR2Content.Buffs.TonicBuff.buffIndex, 10 * (1 + Run.instance.stageClearCount));
                        self.body.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex, 1 + Run.instance.stageClearCount);
                        self.ospTimer = 0.25f;
                    }
                }
            }
        }
    }
}