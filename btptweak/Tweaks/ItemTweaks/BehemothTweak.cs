using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BehemothTweak : TweakBase<BehemothTweak> {
        public const int Radius = 3;
        public const float BaseDamageCoefficient = 0.6f;

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitAll += GlobalEventManager_OnHitAll;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitAll -= GlobalEventManager_OnHitAll;
        }

        private void GlobalEventManager_OnHitAll(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items), "Behemoth"),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc_0);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody) => {
                    if (itemCount > 0) {
                        float radius = Radius * itemCount;
                        var procChainMask = damageInfo.procChainMask;
                        procChainMask.AddProc(ProcType.Behemoth);
                        new BlastAttack {
                            attacker = damageInfo.attacker,
                            baseDamage = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, BaseDamageCoefficient),
                            baseForce = 0,
                            crit = damageInfo.crit,
                            damageColorIndex = damageInfo.damageColorIndex,
                            damageType = damageInfo.damageType,
                            falloffModel = BlastAttack.FalloffModel.Linear,
                            inflictor = damageInfo.inflictor,
                            position = damageInfo.position,
                            procChainMask = procChainMask,
                            procCoefficient = damageInfo.procCoefficient,
                            radius = radius,
                            teamIndex = attackerBody.teamComponent.teamIndex,
                        }.Fire();
                        EffectManager.SpawnEffect(AssetReferences.omniExplosionVFXQuick, new EffectData {
                            origin = damageInfo.position,
                            scale = radius,
                            rotation = Quaternion.identity,
                        }, true);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Behemoth :: Hook Failed!");
            }
        }
    }
}