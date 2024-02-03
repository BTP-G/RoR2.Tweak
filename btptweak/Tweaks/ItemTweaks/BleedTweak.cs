using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BleedTweak : TweakBase<BleedTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float BleedDamageCoefficient = 0.1f;
        public const float BleedDuration = 5f;
        public const float SurperBleedDamageCoefficient = 0.2f;
        public const float SurperBleedDuration = 10f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy2;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var dotDef = DotController.GetDotDef(DotController.DotIndex.Bleed);
            dotDef.damageCoefficient = BleedDamageCoefficient * dotDef.interval;
            dotDef = DotController.GetDotDef(DotController.DotIndex.SuperBleed);
            dotDef.damageCoefficient = SurperBleedDamageCoefficient * dotDef.interval;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdcI4(5),
                                     x => x.MatchCall<ProcChainMask>("HasProc"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.Emit(OpCodes.Ldloc, 0);
                ilcursor.EmitDelegate((bool hasProc, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody, uint? maxStacksFromAttacker) => {
                    if (hasProc) {
                        return;
                    }
                    if (damageInfo.crit) {
                        var superBleedCount = 0f;
                        if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal)) {
                            ++superBleedCount;
                        }
                        if (Util.CheckRoll(attackerBody.bleedChance * damageInfo.procCoefficient, attackerBody.master)) {
                            ++superBleedCount;
                        }
                        if (damageInfo.damageType.HasFlag(DamageType.SuperBleedOnCrit)) {
                            ++superBleedCount;
                        }
                        if (superBleedCount > 0) {
                            var dotInfo = new InflictDotInfo {
                                attackerObject = damageInfo.attacker,
                                dotIndex = DotController.DotIndex.SuperBleed,
                                duration = SurperBleedDuration,
                                maxStacksFromAttacker = maxStacksFromAttacker,
                                totalDamage = superBleedCount * damageInfo.damage * SurperBleedDamageCoefficient * attackerBody.critMultiplier,
                                victimObject = victim,
                            };
                            dotInfo.InflictTotalDamageWithinDuration(attackerBody);
                            DotController.InflictDot(ref dotInfo);
                        }
                        if (damageInfo.damageType.HasFlag(DamageType.BleedOnHit)) {
                            var dotInfo = new InflictDotInfo {
                                attackerObject = damageInfo.attacker,
                                dotIndex = DotController.DotIndex.Bleed,
                                duration = BleedDuration,
                                maxStacksFromAttacker = maxStacksFromAttacker,
                                totalDamage = damageInfo.damage * BleedDamageCoefficient * attackerBody.critMultiplier,
                                victimObject = victim,
                            };
                            dotInfo.InflictTotalDamageWithinDuration(attackerBody);
                            DotController.InflictDot(ref dotInfo);
                        }
                    } else {
                        var bleedCount = 0;
                        if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal)) {
                            ++bleedCount;
                        }
                        if (Util.CheckRoll(attackerBody.bleedChance * damageInfo.procCoefficient, attackerBody.master)) {
                            ++bleedCount;
                        }
                        if (damageInfo.damageType.HasFlag(DamageType.BleedOnHit)) {
                            ++bleedCount;
                        }
                        if (bleedCount > 0) {
                            var dotInfo = new InflictDotInfo {
                                attackerObject = damageInfo.attacker,
                                dotIndex = DotController.DotIndex.Bleed,
                                duration = BleedDuration,
                                maxStacksFromAttacker = maxStacksFromAttacker,
                                totalDamage = bleedCount * damageInfo.damage * BleedDamageCoefficient,
                                victimObject = victim,
                            };
                            dotInfo.InflictTotalDamageWithinDuration(attackerBody);
                            DotController.InflictDot(ref dotInfo);
                        }
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_1);
            } else {
                Main.Logger.LogError("BleedOnHit :: Hook Failed!");
            }
        }

        private void GlobalEventManager_OnHitEnemy2(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => x.MatchLdarg(1),
                                     x => x.Match(OpCodes.Brfalse),
                                     x => x.MatchLdarg(1),
                                     x => x.MatchLdfld<DamageInfo>("inflictor"),
                                     x => x.MatchLdnull())) {
                ilcursor.Index += 1;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Bleed :: Hook2 Failed!");
            }
            if (ilcursor.TryGotoNext(x => x.MatchCall<DotController>("InflictDot"))) {
                ilcursor.Index += 3;
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Bleed :: Hook3 Failed!");
            }
        }
    }
}