using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ChainLightningVoidTweak : TweakBase<ChainLightningVoidTweak>, IOnModLoadBehavior {
        public const int BasePercentChance = 20;
        public const float 半数 = 4;
        public const float DamageCoefficient = 0.3f;
        public const int TotalStrikes = 3;

        void IOnModLoadBehavior.OnModLoad() {
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
                    //if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && victimBody.mainHurtBox && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                    //    var voidLightningOrb = new VoidLightningOrb() {
                    //        attacker = damageInfo.attacker,
                    //        damageColorIndex = DamageColorIndex.Void,
                    //        damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount),
                    //        isCrit = damageInfo.crit,
                    //        origin = damageInfo.position,
                    //        procChainMask = damageInfo.procChainMask,
                    //        procCoefficient = 0.2f,
                    //        secondsPerStrike = 0.1f,
                    //        target = victimBody.mainHurtBox,
                    //        teamIndex = attackerMaster.teamIndex,
                    //        totalStrikes = TotalStrikes,
                    //    };
                    //    voidLightningOrb.procChainMask.AddWhiteProcs();
                    //    voidLightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                    //    OrbManager.instance.AddOrb(voidLightningOrb);
                    //}
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning) && victimBody.mainHurtBox && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerMaster)) {
                        var simpleOrbInfo = new SimpleOrbInfo {
                            attacker = damageInfo.attacker,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                            target = victimBody.mainHurtBox,
                        };
                        simpleOrbInfo.procChainMask.AddGreenProcs();
                        (victimBody.GetComponent<VoidLightningOrbPool>()
                        ?? victimBody.AddComponent<VoidLightningOrbPool>()).AddOrb(simpleOrbInfo,
                                                                                   Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount),
                                                                                   attackerMaster.teamIndex);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightningVoid :: Hook Failed!");
            }
        }
    }
}