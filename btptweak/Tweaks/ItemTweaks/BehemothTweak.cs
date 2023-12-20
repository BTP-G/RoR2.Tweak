using BtpTweak.Pools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BehemothTweak : TweakBase<BehemothTweak>, IOnModLoadBehavior {
        public const int Radius = 3;
        public const float BaseDamageCoefficient = 0.6f;

        public void OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitAll += GlobalEventManager_OnHitAll;
        }

        private void GlobalEventManager_OnHitAll(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items), "Behemoth"),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc_0);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody) => {
                    if (itemCount > 0) {
                        var attackInfo = new BlastAttackInfo {
                            crit = damageInfo.crit,
                            damageType = damageInfo.damageType,
                            procCoefficient = damageInfo.procCoefficient,
                            radius = Radius * itemCount,
                            attacker = damageInfo.attacker,
                            inflictor = damageInfo.inflictor,
                            procChainMask = damageInfo.procChainMask,
                            teamIndex = attackerBody.teamComponent.teamIndex,
                        };
                        attackInfo.procChainMask.AddWhiteProcs();
                        (attackerBody.gameObject.GetComponent<BehemothPool>()
                        ?? attackerBody.gameObject.AddComponent<BehemothPool>()).AddAttack(attackInfo,
                                                                               damageInfo.position,
                                                                               Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient));
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Behemoth :: Hook Failed!");
            }
        }
    }
}