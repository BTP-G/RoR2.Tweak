using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using System.Threading.Tasks;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class MissileVoidTweak : TweakBase<MissileVoidTweak> {
        public const float DamageCoefficient = 0.6f;

        public override void SetEventHandlers() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin += MissileVoidOrb_Begin;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin -= MissileVoidOrb_Begin;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
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
                ilcursor.EmitDelegate(async (int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    MissileVoidOrb[] missileVoidOrbs = null;
                    await Task.Run(() => {
                        if (itemCount < 1 || !victimBody.mainHurtBox) {
                            return;
                        }
                        var shieldFraction = attackerBody.healthComponent.shield / attackerBody.healthComponent.fullShield;
                        if (!Util.CheckRoll(100f * shieldFraction, attackerBody.master)) {
                            return;
                        }
                        var itemMoreMissileCount = attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile);
                        var damageValue = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, shieldFraction * DamageCoefficient * itemCount) * shieldFraction * (itemMoreMissileCount > 1 ? 0.5f * (1 + itemMoreMissileCount) : 1);
                        var procChainMask = damageInfo.procChainMask;
                        procChainMask.AddGreenProcs();
                        procChainMask.AddProc(ProcType.Missile);
                        missileVoidOrbs = new MissileVoidOrb[(itemMoreMissileCount > 0 ? 3 : 1)];
                        for (int i = 0; i < missileVoidOrbs.Length; ++i) {
                            missileVoidOrbs[i] = new MissileVoidOrb() {
                                origin = attackerBody.aimOrigin,
                                damageValue = damageValue,
                                isCrit = damageInfo.crit,
                                teamIndex = attackerBody.teamComponent.teamIndex,
                                attacker = damageInfo.attacker,
                                procChainMask = procChainMask,
                                procCoefficient = 0.2f,
                                damageColorIndex = DamageColorIndex.Void,
                                target = victimBody.mainHurtBox
                            };
                        }
                    });
                    if (missileVoidOrbs != null) {
                        for (int i = 0; i < missileVoidOrbs.Length; ++i) {
                            OrbManager.instance.AddOrb(missileVoidOrbs[i]);
                        }
                    }
                });
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("MissileVoid :: Hook Failed!");
            }
        }
    }
}