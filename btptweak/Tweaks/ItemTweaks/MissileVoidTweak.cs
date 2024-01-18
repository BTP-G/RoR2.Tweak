using BtpTweak.Pools.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class MissileVoidTweak : TweakBase<MissileVoidTweak>, IOnModLoadBehavior {
        public const float DamageCoefficient = 0.6f;
        public const float Interval = 0.1f;

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
                ilcursor.Emit(OpCodes.Ldloc_1);
                ilcursor.Emit(OpCodes.Ldloc_2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    if (itemCount < 1 || !victimBody.mainHurtBox) {
                        return;
                    }
                    var shieldFraction = attackerBody.healthComponent.shield / attackerBody.healthComponent.fullShield;
                    if (!Util.CheckRoll(100f * shieldFraction, attackerBody.master)) {
                        return;
                    }
                    var simpleOrbInfo = new OrbPoolKey {
                        attackerBody = attackerBody,
                        isCrit = damageInfo.crit,
                        procChainMask = damageInfo.procChainMask,
                        target = victimBody.mainHurtBox,
                        通用浮点数 = attackerBody.inventory.GetItemCount(DLC1Content.Items.MoreMissile.itemIndex),
                    };
                    simpleOrbInfo.procChainMask.AddRYProcs();
                    MissileVoidOrbPool.RentPool(simpleOrbInfo.target.gameObject).AddOrb(simpleOrbInfo,
                                                                              Util.OnHitProcDamage(damageInfo.damage, 0, shieldFraction * DamageCoefficient * itemCount));
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("MissileVoid :: Hook Failed!");
            }
        }
    }
}