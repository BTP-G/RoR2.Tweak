using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BleedOnHitVoidTweak : TweakBase<BleedOnHitVoidTweak>, IOnModLoadBehavior {
        public const int PercnetChance = 10;
        public const float BaseDamageCoefficient = 0.44f;
        public const float 每层熵的破裂伤害叠加系数 = 0.11f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items), "BleedOnHitVoid"),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.EmitDelegate((int itemCount, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                    if (itemCount > 0 && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerBody.master)) {
                        var dotInfo = new InflictDotInfo {
                            attackerObject = damageInfo.attacker,
                            dotIndex = DotController.DotIndex.Fracture,
                            duration = 3f,
                            victimObject = victim,
                            damageMultiplier = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, BaseDamageCoefficient + 每层熵的破裂伤害叠加系数 * TPDespair.ZetAspects.Catalog.GetStackMagnitude(attackerBody, DLC1Content.Buffs.EliteVoid))
                                                                    * (damageInfo.crit ? attackerBody.critMultiplier : 1f)
                                                                    / (attackerBody.damage * 4f)
                        };
                        DotController.InflictDot(ref dotInfo);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("BleedOnHitVoid Hook Failed!");
            }
        }
    }
}