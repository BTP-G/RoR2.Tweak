using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using TPDespair.ZetAspects;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BleedOnHitVoidTweak : TweakBase<BleedOnHitVoidTweak>, IOnModLoadBehavior {
        public const int PercnetChance = 10;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items), "BleedOnHitVoid"),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                ilcursor.Emit(OpCodes.Ldarg_1)
                        .Emit(OpCodes.Ldarg_2)
                        .Emit(OpCodes.Ldloc_0)
                        .EmitDelegate((int itemCount, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody) => {
                            if (attackerBody.TryGetAspectStackMagnitude(DLC1Content.Buffs.EliteVoid.buffIndex, out var stack)) {
                                var damageCoefficient = Configuration.AspectVoidBaseCollapseDamage.Value + Configuration.AspectVoidStackCollapseDamage.Value * (stack - 1f);
                                if (itemCount > 0 && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerBody.master)) {
                                    damageCoefficient *= 2f;
                                }
                                if (attackerBody.teamComponent.teamIndex != TeamIndex.Player) {
                                    damageCoefficient *= Configuration.AspectVoidMonsterDamageMult.Value;
                                }
                                var dotInfo = new InflictDotInfo {
                                    attackerObject = damageInfo.attacker,
                                    dotIndex = DotController.DotIndex.Fracture,
                                    duration = 3f,
                                    victimObject = victim,
                                    totalDamage = Configuration.AspectVoidUseBase.Value
                                        ? damageCoefficient * attackerBody.damage
                                        : damageCoefficient * damageInfo.damage * (damageInfo.crit ? attackerBody.critMultiplier : 1f)
                                };
                                DotController.InflictDot(ref dotInfo);
                            } else if (itemCount > 0 && Util.CheckRoll(PercnetChance * itemCount * damageInfo.procCoefficient, attackerBody.master)) {
                                var dotInfo = new InflictDotInfo {
                                    attackerObject = damageInfo.attacker,
                                    dotIndex = DotController.DotIndex.Fracture,
                                    duration = 3f,
                                    victimObject = victim,
                                    totalDamage = Configuration.AspectVoidUseBase.Value
                                       ? Configuration.AspectVoidBaseCollapseDamage.Value * attackerBody.damage
                                       : Configuration.AspectVoidBaseCollapseDamage.Value * damageInfo.damage * (damageInfo.crit ? attackerBody.critMultiplier : 1f)
                                };
                                DotController.InflictDot(ref dotInfo);
                            }
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                "BleedOnHitVoid Hook Failed!".LogError();
            }
        }
    }
}