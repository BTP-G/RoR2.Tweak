using BTP.RoR2Plugin.Pools.OrbPools;
using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ChainLightningVoidTweak : ModComponent, IModLoadMessageHandler {
        public const float 半数 = 4;
        public const float DamageCoefficient = 0.15f;
        public const int TotalStrikes = 3;
        public const float Interval = 0.2f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("ChainLightningVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ldloc_1)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                            if (itemCount > 0
                            && !damageInfo.procChainMask.HasProc(ProcType.ChainLightning)
                            && Util.CheckRoll(Util2.CloseTo(itemCount, 半数, 100f * damageInfo.procCoefficient), attackerBody.master)
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
                LogExtensions.LogError("ChainLightningVoid :: Hook Failed!");
            }
        }
    }
}