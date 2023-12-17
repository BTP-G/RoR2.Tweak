using BtpTweak.OrbPools;
using BtpTweak.Utils;
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

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("BounceNearby")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldloc, 2);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) => {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.BounceNearby) && Util.CheckRoll((BasePercentChance + StackPercentChance * (itemCount - 1)) * damageInfo.procCoefficient, attackerMaster)) {
                        var simpleOrbInfo = new SimpleOrbInfo {
                            attacker = damageInfo.attacker,
                            isCrit = damageInfo.crit,
                            procChainMask = damageInfo.procChainMask,
                        };
                        simpleOrbInfo.procChainMask.AddRedProcs();
                        (victimBody.GetComponent<BounceOrbPool>()
                        ?? victimBody.AddComponent<BounceOrbPool>()).AddOrb(simpleOrbInfo,
                                                                            Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient),
                                                                            attackerMaster.teamIndex);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BounceNearby :: Hook Failed!");
            }
        }
    }
}