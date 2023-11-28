using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Threading.Tasks;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BounceNearbyTweak : TweakBase<BounceNearbyTweak> {
        public const float BaseDamageCoefficient = 1;
        public const float BasePercentChance = 33f;
        public const float StackPercentChance = 16.5f;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
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
                ilcursor.EmitDelegate(async (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) => {
                    if (itemCount == 0) {
                        return;
                    }
                    var simpleOrbInfo = default(SimpleOrbInfo);
                    var result = 0f;
                    await Task.Run(() => {
                        if (!damageInfo.procChainMask.HasProc(ProcType.BounceNearby) && Util.CheckRoll((BasePercentChance + StackPercentChance * (itemCount - 1)) * damageInfo.procCoefficient, attackerMaster)) {
                            simpleOrbInfo = new SimpleOrbInfo {
                                attacker = damageInfo.attacker,
                                isCrit = damageInfo.crit,
                                procChainMask = damageInfo.procChainMask,
                            };
                            simpleOrbInfo.procChainMask.AddRedProcs();
                            result = Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient);
                        }
                    });
                    if (result > 0f) {
                        (victimBody.GetComponent<BounceOrbPool>()
                        ?? victimBody.AddComponent<BounceOrbPool>()).AddOrb(simpleOrbInfo,
                                                                            result,
                                                                            attackerMaster.teamIndex);
                    }
                });
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BounceNearby :: Hook Failed!");
            }
        }
    }
}