using BtpTweak.Pools.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class LightningStrikeOnHitTweak : TweakBase<LightningStrikeOnHitTweak>, IOnModLoadBehavior {
        public const int BasePercentChance = 10;
        public const float 半数 = 9f;
        public const int DamageCoefficient = 3;
        public const float Interval = 0.5f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("LightningStrikeOnHit")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc_1);
                ilcursor.Emit(OpCodes.Ldloc_2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                    if (itemCount > 0
                    && !damageInfo.procChainMask.HasProc(ProcType.LightningStrikeOnHit)
                    && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerBody.master)
                    && victimBody.mainHurtBox) {
                        var simpleOrbInfo = new OrbPoolKey {
                            attackerBody = attackerBody,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                            target = victimBody.mainHurtBox,
                        };
                        SimpleLightningStrikeOrbPool.RentPool(simpleOrbInfo.target.gameObject).AddOrb(simpleOrbInfo,
                                                                                           Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount));
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("LightningStrikeOnHit :: Hook Failed!");
            }
        }
    }
}