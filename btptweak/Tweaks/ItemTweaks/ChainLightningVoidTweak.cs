using BtpTweak.Pools.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ChainLightningVoidTweak : TweakBase<ChainLightningVoidTweak>, IOnModLoadBehavior {
        public const float 半数 = 4;
        public const float DamageCoefficient = 0.15f;
        public const int TotalStrikes = 3;
        public const float Interval = 0.2f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("ChainLightningVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc_1);
                ilcursor.Emit(OpCodes.Ldloc_2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    if (itemCount > 0
                    && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning)
                    && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerBody.master)
                    && victimBody.mainHurtBox) {
                        var simpleOrbInfo = new OrbPoolKey {
                            attackerBody = attackerBody,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                            target = victimBody.mainHurtBox,
                        };
                        simpleOrbInfo.procChainMask.AddRYProcs();
                        VoidLightningOrbPool.RentPool(simpleOrbInfo.target.gameObject).AddOrb(simpleOrbInfo,
                                                                                    Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount));
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ChainLightningVoid :: Hook Failed!");
            }
        }
    }
}