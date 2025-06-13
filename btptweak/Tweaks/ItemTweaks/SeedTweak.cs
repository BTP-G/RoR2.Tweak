using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class SeedTweak : TweakBase<SeedTweak>, IOnModLoadBehavior {
        public const float Leech = 0.01f;
        public const float 指数 = 0.5f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Seed")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldloc_0)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, CharacterBody attackerBody) => {
                            if (itemCount > 0) {
                                var procChainMask = damageInfo.procChainMask;
                                procChainMask.AddProc(ProcType.HealOnHit);
                                attackerBody.healthComponent.Heal(Mathf.Sqrt(damageInfo.damage * damageInfo.procCoefficient * Leech * itemCount), procChainMask, true);
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                LogExtensions.LogError("Seed :: Hook Failed!");
            }
        }
    }
}