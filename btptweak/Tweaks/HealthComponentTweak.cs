using BtpTweak.RoR2Indexes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class HealthComponentTweak : TweakBase<HealthComponentTweak> {
        private float _老米爆发伤害限制;
        private float _老米触发伤害限制;
        private float _伤害阈值;
        private float _虚灵爆发伤害限制;
        private float _虚灵触发伤害限制;

        public override void SetEventHandlers() {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            IL.RoR2.HealthComponent.TakeDamage += IL_HealthComponent_TakeDamage;
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
            On.RoR2.HealthComponent.RechargeShield += HealthComponent_RechargeShield;
            Run.onRunAmbientLevelUp += Run_onRunAmbientLevelUp;
            Stage.onStageStartGlobal += StageStartAction;
        }

        public override void ClearEventHandlers() {
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            IL.RoR2.HealthComponent.TakeDamage -= IL_HealthComponent_TakeDamage;
            On.RoR2.HealthComponent.Heal -= HealthComponent_Heal;
            On.RoR2.HealthComponent.RechargeShield -= HealthComponent_RechargeShield;
            Run.onRunAmbientLevelUp -= Run_onRunAmbientLevelUp;
            Stage.onStageStartGlobal -= StageStartAction;
        }

        public void StageStartAction(Stage stage) {
            _伤害阈值 = 0.1f * Run.instance.stageClearCount * (Run.instance.ambientLevel / (Run.instance.ambientLevel + 100f));
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            if (RunInfo.是否选择造物难度 && damageReport.hitLowHealth && damageReport.victimTeamIndex == TeamIndex.Monster) {
                CharacterBody victimBody = damageReport.victimBody;
                if (victimBody.inventory?.GetItemCount(RoR2Content.Items.TonicAffliction.itemIndex) == 0) {
                    if (PhaseCounter.instance && victimBody.bodyIndex == BodyIndexes.BrotherBody) {
                        victimBody.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                        victimBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 10);
                        victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                        Util.CleanseBody(victimBody, true, false, false, false, true, true);
                    } else {
                        victimBody.AddTimedBuff(RoR2Content.Buffs.TonicBuff, 20);
                        victimBody.inventory.GiveItem(RoR2Content.Items.TonicAffliction.itemIndex);
                        Util.CleanseBody(victimBody, true, false, false, false, true, false);
                    }
                }
            }
        }

        private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen) {
            if (self.body.bodyIndex == BodyIndexes.BrotherHurtBody) {
                return 0f;
            }
            return orig(self, amount, procChainMask, nonRegen);
        }

        private void HealthComponent_RechargeShield(On.RoR2.HealthComponent.orig_RechargeShield orig, HealthComponent self, float value) {
            if (self.body.bodyIndex == BodyIndexes.BrotherHurtBody) {
                return;
            }
            orig(self, value);
        }

        private void IL_HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Buffs), "LunarShell"))) {  // 901
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (HealthComponent healthComponent, DamageInfo damageInfo, float damage) {
                    if (RunInfo.是否选择造物难度) {
                        CharacterBody victimBody = healthComponent.body;
                        if (RunInfo.CurrentSceneIndex == SceneIndexes.VoidRaid && victimBody.isBoss) {  // 虚灵
                            if (damage < _伤害阈值 * healthComponent.fullHealth && damageInfo.procCoefficient <= 1f) {
                                damage = Mathf.Min(damage, _虚灵触发伤害限制 * healthComponent.fullHealth);
                            } else {
                                damage = Mathf.Min(damage, _虚灵爆发伤害限制 * healthComponent.fullHealth);
                                Util.CleanseBody(victimBody, true, false, false, false, true, true);
                            }
                        } else if (PhaseCounter.instance) {
                            BodyIndex selfIndex = victimBody.bodyIndex;
                            if (selfIndex == BodyIndexes.BrotherBody) {  // 米斯历克斯
                                if (damage < _伤害阈值 * healthComponent.fullCombinedHealth && damageInfo.procCoefficient <= 1f) {
                                    damage = Mathf.Min(damage, _老米触发伤害限制 * healthComponent.fullCombinedHealth);
                                } else {
                                    damage = Mathf.Min(damage, _老米爆发伤害限制 * healthComponent.fullCombinedHealth);
                                    Util.CleanseBody(victimBody, true, false, false, false, true, true);
                                }
                            } else if (selfIndex == BodyIndexes.BrotherHurtBody) {
                                damage = Mathf.Max(healthComponent.combinedHealth * 0.01f, Mathf.Min(healthComponent.combinedHealth * 0.99f, damage));
                                Util.CleanseBody(victimBody, true, true, true, true, true, true);
                                victimBody.AddTimedBuff(RoR2Content.Buffs.Immune, healthComponent.combinedHealthFraction);
                            }
                        }
                    }
                    return damage;
                });
                ilcursor.Emit(OpCodes.Stloc, 6);
            } else {
                Main.Logger.LogError("Enemy TakeDamage Hook Error");
            }
        }

        private void Run_onRunAmbientLevelUp(Run run) {
            int stageCount = run.stageClearCount + 1;
            float 爆发 = Mathf.Max(0.01f, 1f - 0.1f * stageCount * (run.ambientLevel / (run.ambientLevel + 99f + TeamManager.instance.GetTeamLevel(TeamIndex.Player))));
            _老米爆发伤害限制 = 爆发;
            _虚灵爆发伤害限制 = 爆发 * 0.5f;
            float 触发 = 爆发 * 0.001f;
            _老米触发伤害限制 = 触发;
            _虚灵触发伤害限制 = 触发 * 0.5f;
        }
    }
}