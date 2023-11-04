using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class MissileVoidTweak : TweakBase<MissileVoidTweak> {
        public const float DamageCoefficient = 1;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(MonoMod.Cil.ILContext il) {
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
                    var itemMoreMissileCount = attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile);
                    var damageValue = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, shieldFraction * itemCount) * shieldFraction * (itemMoreMissileCount > 1 ? 0.5f * (1 + itemMoreMissileCount) : 1);
                    var procChainMask = damageInfo.procChainMask;
                    procChainMask.AddProc(ProcType.Missile);
                    procChainMask.AddPoolProcs();
                    for (int i = itemMoreMissileCount > 0 ? 3 : 1; i > 0; --i) {
                        OrbManager.instance.AddOrb(new MissileVoidOrb() {
                            origin = attackerBody.aimOrigin,
                            damageValue = damageValue,
                            isCrit = damageInfo.crit,
                            teamIndex = attackerBody.teamComponent.teamIndex,
                            attacker = damageInfo.attacker,
                            procChainMask = procChainMask,
                            procCoefficient = 0.2f,
                            damageColorIndex = DamageColorIndex.Void,
                            target = victimBody.mainHurtBox
                        });
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("MissileVoid :: Hook Failed!");
            }
        }
    }
}