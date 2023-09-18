using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

    internal class HealthComponentTweak : TweakBase {
        private bool 位于天文馆;
        private float 老米爆发伤害限制_;
        private float 老米触发伤害限制_;
        private float 伤害阈值_;
        private float 虚灵爆发伤害限制_;
        private float 虚灵触发伤害限制_;
        private BodyIndex 老米_;
        private BodyIndex 负伤老米_;

        public override void Load() {
            老米_ = "RoR2/Base/Brother/BrotherBody.prefab".LoadComponent<CharacterBody>().bodyIndex;
            负伤老米_ = "RoR2/Base/Brother/BrotherHurtBody.prefab".LoadComponent<CharacterBody>().bodyIndex;
        }

        public override void AddHooks() {
            IL.RoR2.HealthComponent.TakeDamage += IL_HealthComponent_TakeDamage;
            IL.RoR2.HealthComponent.TriggerOneShotProtection += IL_HealthComponent_TriggerOneShotProtection;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            Run.onRunAmbientLevelUp += Run_onRunAmbientLevelUp;
        }

        public override void StageStartAction(Stage stage) {
            伤害阈值_ = 0.01f * (Run.instance.stageClearCount + 1);
            位于天文馆 = stage.sceneDef.cachedName == "voidraid";
        }

        private void Run_onRunAmbientLevelUp(Run run) {
            float 爆发 = Mathf.Max(0.1f, 1f - 0.1f * (run.stageClearCount + 1) * (run.ambientLevel / (run.ambientLevel + 100f)));
            老米爆发伤害限制_ = 爆发;
            虚灵爆发伤害限制_ = 爆发 * 0.5f;
            float 触发 = 爆发 * 0.001f;
            老米触发伤害限制_ = 触发;
            虚灵触发伤害限制_ = 触发 * 0.5f;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
            if (NetworkServer.active) {
                CharacterBody victimBody = self.body;
                if (self.shield > 0 && (damageInfo.damageType & DamageType.DoT) > DamageType.Generic) {
                    damageInfo.damage *= victimBody.HasBuff(RoR2Content.Buffs.AffixLunar.buffIndex) ? 0.25f : 0.5f;
                }
                orig(self, damageInfo);
                if (Main.是否选择造物难度_) {
                    if (TeamIndex.Monster == victimBody.teamComponent.teamIndex && (victimBody.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) == 0) && self.isHealthLow) {
                        if (PhaseCounter.instance && victimBody.bodyIndex == 老米_) {
                            victimBody.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                            victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 10);
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

        private void IL_HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Buffs), "LunarShell"))) {  // 901
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, float damage) {
                    if (Main.是否选择造物难度_) {
                        CharacterBody victimBody = healthComponent.body;
                        if (位于天文馆 && victimBody.isBoss) {
                            if (damage < 伤害阈值_ * healthComponent.fullHealth) {  // 虚灵
                                damage = Mathf.Min(damage, 虚灵触发伤害限制_ * healthComponent.fullHealth);
                            } else {
                                damage = Mathf.Min(damage, 虚灵爆发伤害限制_ * healthComponent.health);
                                Util.CleanseBody(victimBody, true, false, false, true, true, true);
                            }
                        } else if (PhaseCounter.instance) {
                            BodyIndex selfIndex = victimBody.bodyIndex;
                            if (selfIndex == 老米_) {  // 米斯历克斯
                                if (damage < 伤害阈值_ * healthComponent.fullCombinedHealth) {
                                    damage = Mathf.Min(damage, 老米触发伤害限制_ * healthComponent.fullCombinedHealth);
                                } else {
                                    damage = Mathf.Min(damage, 老米爆发伤害限制_ * healthComponent.combinedHealth);
                                    Util.CleanseBody(victimBody, true, false, false, true, true, true);
                                }
                            } else if (selfIndex == 负伤老米_) {
                                damage = Mathf.Max(damage, healthComponent.fullCombinedHealth / victimBody.level * 0.1f);
                            }
                        }
                    }
                    return damage;
                });
                ilcursor.Emit(OpCodes.Stloc, 6);
            } else {
                Main.logger_.LogError("Enemy TakeDamage Hook Error");
            }
        }

        private void IL_HealthComponent_TriggerOneShotProtection(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 0.1f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 0.75f);
            } else {
                Main.logger_.LogError("ospTimer Hook Error");
            }
        }
    }
}