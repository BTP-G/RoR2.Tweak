using BtpTweak.ProjectileFountains;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class FireballsOnHitTweak : TweakBase<FireballsOnHitTweak> {
        public const int BasePercentChance = 10;
        public const int DamageCoefficient = 3;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("FireballsOnHit")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Meatball) && Util.CheckRoll(100f * (itemCount / (itemCount + 9f)) * damageInfo.procCoefficient, attackerMaster)) {
                        (victim.GetComponent<FireFountain>() ?? victim.AddComponent<FireFountain>()).AddProjectile(
                            AssetReferences.fireMeatBallProjectile,
                            damageInfo.attacker,
                            Util.OnHitProcDamage(damageInfo.damage, 0, DamageCoefficient * itemCount),
                            damageInfo.crit,
                            damageInfo.procChainMask);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("FireballsOnHit :: Hook Failed!");
            }
        }
    }
}