using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class MissileVoidTweak : TweakBase<MissileVoidTweak>, IOnModLoadBehavior {
        public const float DamageCoefficient = 0.6f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin += MissileVoidOrb_Begin;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdcR4(x, 75f))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_R4, 100f);
            } else {
                Main.Logger.LogError("MissileVoidOrb Hook Failed!");
            }
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("MissileVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    if (itemCount < 1 || !victimBody.mainHurtBox) {
                        return;
                    }
                    var shieldFraction = attackerBody.healthComponent.shield / attackerBody.healthComponent.fullShield;
                    if (!Util.CheckRoll(100f * shieldFraction, attackerBody.master)) {
                        return;
                    }
                    var simpleOrbInfo = new SimpleOrbInfo {
                        attacker = damageInfo.attacker,
                        isCrit = damageInfo.crit,
                        procChainMask = damageInfo.procChainMask,
                        target = victimBody.mainHurtBox,
                    };
                    simpleOrbInfo.procChainMask.AddGreenProcs();
                    (attackerBody.GetComponent<MissileVoidOrbPool>()
                        ?? attackerBody.AddComponent<MissileVoidOrbPool>()).AddOrb(simpleOrbInfo,
                                                                                   Util.OnHitProcDamage(damageInfo.damage, 0, shieldFraction * DamageCoefficient * itemCount),
                                                                                   attackerBody.teamComponent.teamIndex);
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("MissileVoid :: Hook Failed!");
            }
        }
    }
}