using BTP.RoR2Plugin.Pools.OrbPools;
using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ChainLightningTweak : TweakBase<ChainLightningTweak>, IOnModLoadBehavior {
        public const float DamageCoefficient = 0.3f;
        public const float 半数 = 4;
        public const int BaseRadius = 18;
        public const int Bounces = 2;
        public const int StackRadius = 3;
        public const float Interval = 0.2f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("ChainLightning")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ldloc_1)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                            if (itemCount > 0
                            && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning)
                            && Util.CheckRoll(BtpUtils.简单逼近(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerBody.master)
                            && victimBody.mainHurtBox) {
                                var simpleOrbInfo = new OrbPoolKey {
                                    attackerBody = attackerBody,
                                    isCrit = damageInfo.crit,
                                    procChainMask = damageInfo.procChainMask,
                                    target = victimBody.mainHurtBox,
                                    通用浮点数 = BaseRadius + StackRadius * (itemCount - 1),
                                };
                                simpleOrbInfo.procChainMask.AddRYProcs();
                                LightningOrbPool.RentPool(simpleOrbInfo.target.gameObject).AddOrb(simpleOrbInfo,
                                                                         Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount));
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("ChainLightning :: Hook Failed!");
            }
        }
    }
}