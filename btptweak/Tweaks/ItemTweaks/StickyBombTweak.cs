using BtpTweak.ProjectileFountains;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class StickyBombTweak : TweakBase<StickyBombTweak> {
        public const int PercnetChance = 5;
        public const int BaseDamageCoefficient = 1;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("StickyBomb")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldloc, 4);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.EmitDelegate(delegate (int itemCount, DamageInfo damageInfo, CharacterMaster attackerMaster, GameObject victim) {
                    if (itemCount > 0 && !damageInfo.procChainMask.HasProc(ProcType.Count) && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerMaster)) {
                        (victim.GetComponent<StickyBombFountain>() ?? victim.AddComponent<StickyBombFountain>()).AddProjectile(
                            AssetReferences.stickyBombProjectile,
                            damageInfo.attacker,
                            Util.OnHitProcDamage(damageInfo.damage, 0, BaseDamageCoefficient),
                            damageInfo.crit,
                            damageInfo.procChainMask);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("StickyBomb :: Hook Failed!");
            }
        }
    }
}