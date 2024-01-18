using BtpTweak.Pools.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BounceNearbyTweak : TweakBase<BounceNearbyTweak>, IOnModLoadBehavior {
        public const float BaseDamageCoefficient = 1;
        public const float BasePercentChance = 33f;
        public const float BaseRadius = 33f;
        public const float StackPercentChance = 16.5f;
        public const int BaseMaxTargets = 6;
        public const float Interval = 0.33f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("BounceNearby")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc_1);
                ilcursor.Emit(OpCodes.Ldloc_2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    if (itemCount > 0
                    && !damageInfo.procChainMask.HasProc(ProcType.BounceNearby)
                    && Util.CheckRoll((BasePercentChance + StackPercentChance * (itemCount - 1)) * damageInfo.procCoefficient, attackerBody.master)
                    && victimBody.mainHurtBox) {
                        var simpleOrbInfo = new OrbPoolKey {
                            attackerBody = attackerBody,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                            target = victimBody.mainHurtBox,
                        };
                        simpleOrbInfo.procChainMask.AddYellowProcs();
                        BounceOrbPool.RentPool(simpleOrbInfo.target.gameObject).AddOrb(simpleOrbInfo,
                                                                            Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient));
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BounceNearby :: Hook Failed!");
            }
        }
    }
}