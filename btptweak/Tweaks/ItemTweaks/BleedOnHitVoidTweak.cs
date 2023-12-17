using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BleedOnHitVoidTweak : TweakBase<BleedOnHitVoidTweak>, IOnModLoadBehavior {
        public const int PercnetChance = 10;
        public const float DamageCoefficient = 0.44f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items), "BleedOnHitVoid"),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                    if (itemCount > 0 && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerBody.master)) {
                        DotController.InflictDot(victim, attackerBody.gameObject, DotController.DotIndex.Fracture, 3,
                            Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, DamageCoefficient)
                            * (damageInfo.crit ? attackerBody.critMultiplier : 1f)
                            / (attackerBody.damage * 4));
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BleedOnHitVoid Hook Failed!");
            }
        }
    }
}