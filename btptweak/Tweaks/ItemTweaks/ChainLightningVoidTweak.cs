using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ChainLightningVoidTweak : TweakBase<ChainLightningVoidTweak> {
        public const float BasePercentChance = 25f;
        public const float DamageCoefficient = 25f;
        public const int BaseTotalStrikes = 3;
        public const int StackTotalStrikes = 1;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("ChainLightningVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) => {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && victimBody.mainHurtBox && Util.CheckRoll(100f * (itemCount / (itemCount + 3f)) * damageInfo.procCoefficient, attackerMaster)) {
                        var voidLightningOrb = new VoidLightningOrb() {
                            origin = damageInfo.position,
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, 0.4f),
                            isCrit = damageInfo.crit,
                            totalStrikes = 2 + itemCount,
                            teamIndex = attackerMaster.teamIndex,
                            attacker = damageInfo.attacker,
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.2f,
                            damageColorIndex = DamageColorIndex.Void,
                            secondsPerStrike = 0.1f,
                            target = victimBody.mainHurtBox
                        };
                        voidLightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                        voidLightningOrb.procChainMask.AddPoolProcs();
                        OrbManager.instance.AddOrb(voidLightningOrb);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightningVoid :: Hook Failed!");
            }
        }
    }
}