using BtpTweak.OrbPools;
using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BounceNearbyTweak : TweakBase<BounceNearbyTweak> {
        public const float BaseDamageCoefficient = 1f;
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
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, CharacterBody victimBody) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.BounceNearby) && Util.CheckRoll((16.5f + StackPercentChance * itemCount) * damageInfo.procCoefficient, attackerMaster)) {
                        (victimBody.GetComponent<BounceOrbPool>() ?? victimBody.AddComponent<BounceOrbPool>()).AddOrb(damageInfo.attacker, damageInfo.procChainMask, new() {
                            damageValue = Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient),
                            isCrit = damageInfo.crit,
                            teamIndex = attackerMaster.teamIndex,
                            attacker = damageInfo.attacker,
                            procChainMask = damageInfo.procChainMask,
                            procCoefficient = 0.33f,
                            damageColorIndex = DamageColorIndex.Item,
                        });
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BounceNearby :: Hook Failed!");
            }
        }
    }
}