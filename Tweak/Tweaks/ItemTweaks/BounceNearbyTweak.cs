using BTP.RoR2Plugin.Pools.OrbPools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BounceNearbyTweak : ModComponent, IModLoadMessageHandler {
        public const float BaseDamageCoefficient = 1;
        public const float BasePercentChance = 33f;
        public const float BaseRadius = 33f;
        public const float StackPercentChance = 16.5f;
        public const int BaseMaxTargets = 6;
        public const float Interval = 0.33f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("BounceNearby")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ldloc_1)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
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
                "BounceNearby :: Hook Failed!".LogError();
            }
        }
    }
}