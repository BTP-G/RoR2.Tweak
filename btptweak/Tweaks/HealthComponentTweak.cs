using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace BtpTweak.Tweaks {

    [Obsolete]
    internal class HealthComponentTweak : TweakBase<HealthComponentTweak>, IOnModLoadBehavior {
        private float _老米爆发伤害限制;
        private float _老米触发伤害限制;
        private float _伤害阈值;
        private float _虚灵爆发伤害限制;
        private float _虚灵触发伤害限制;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamage += IL_HealthComponent_TakeDamage;
            Run.onRunAmbientLevelUp += Run_onRunAmbientLevelUp;
            Stage.onStageStartGlobal += StageStartAction;
        }

        private void IL_HealthComponent_TakeDamage(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdsfld(x, typeof(RoR2Content.Buffs), "LunarShell"))) {  // 901
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.Emit(OpCodes.Ldarg, 1);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate((HealthComponent healthComponent, DamageInfo damageInfo, float damage) => {
                    if (RunInfo.已选择造物难度) {
                        if (RunInfo.位于天文馆) {
                            var victimBody = healthComponent.body;
                            if (victimBody.bodyIndex >= BodyIndexes.MiniVoidRaidCrabBodyPhase1 && victimBody.bodyIndex <= BodyIndexes.MiniVoidRaidCrabBodyPhase3) {  // 虚灵
                                if (damage < _伤害阈值 * healthComponent.fullHealth && damageInfo.procCoefficient <= 1f) {
                                    damage = Mathf.Min(damage, _虚灵触发伤害限制 * healthComponent.fullHealth);
                                } else {
                                    damage = Mathf.Min(damage, _虚灵爆发伤害限制 * healthComponent.fullHealth);
                                    Util.CleanseBody(victimBody, true, false, false, false, true, true);
                                }
                            }
                        } else if (RunInfo.位于月球) {
                            if (healthComponent.body.bodyIndex == BodyIndexes.Brother) {  // 米斯历克斯
                                if (damage < _伤害阈值 * healthComponent.fullCombinedHealth && damageInfo.procCoefficient <= 1f) {
                                    damage = Mathf.Min(damage, _老米触发伤害限制 * healthComponent.fullCombinedHealth);
                                } else {
                                    damage = Mathf.Min(damage, _老米爆发伤害限制 * healthComponent.fullCombinedHealth);
                                    Util.CleanseBody(healthComponent.body, true, false, false, false, true, true);
                                }
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

        private void StageStartAction(Stage stage) {
            _伤害阈值 = BtpUtils.简单逼近(Run.instance.ambientLevel, Run.instance.ambientLevel + 99f + TeamManager.instance.GetTeamLevel(TeamIndex.Player), 0.1f * Run.instance.stageClearCount);
        }

        private void Run_onRunAmbientLevelUp(Run run) {
            int stageCount = run.stageClearCount + 1;
            float 爆发 = Mathf.Max(0.01f, BtpUtils.简单逼近(run.ambientLevel, run.ambientLevel + 99f + TeamManager.instance.GetTeamLevel(TeamIndex.Player), 1f - 0.1f * stageCount));
            _老米爆发伤害限制 = 爆发;
            _虚灵爆发伤害限制 = 爆发 * 0.5f;
            float 触发 = 爆发 * 0.001f;
            _老米触发伤害限制 = 触发;
            _虚灵触发伤害限制 = 触发 * 0.5f;
        }
    }
}