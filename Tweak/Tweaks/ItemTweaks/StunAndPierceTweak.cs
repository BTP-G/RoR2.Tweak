using BTP.RoR2Plugin.Pools;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class StunAndPierceTweak : TweakBase<StunAndPierceTweak>, IOnModLoadBehavior {
        public const float Interval = 0.34f;
        public const float DamageCoefficient = 0.4f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC2Content.Items).GetField("StunAndPierce")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ldloc_1)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody, CharacterBody victimBody) => {
                            if (itemCount > 0
                            && !damageInfo.procChainMask.HasProc(ProcType.StunAndPierceDamage)
                            && Util.CheckRoll(15f * damageInfo.procCoefficient, attackerBody.master)
                            && victimBody.mainHurtBox) {
                                var key = new StunAndPiercePoolKey {
                                    isCrit = damageInfo.crit,
                                    procChainMask = damageInfo.procChainMask,
                                };
                                key.procChainMask.AddProc(ProcType.StunAndPierceDamage);
                                StunAndPiercePool.RentPool(damageInfo.attacker)
                                .AddProjectile(key, attackerBody, attackerBody.damage * DamageCoefficient * itemCount);
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("StunAndPierce :: Hook Failed!");
            }
        }
    }
}