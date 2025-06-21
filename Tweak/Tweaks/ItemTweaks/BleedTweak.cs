using BTP.RoR2Plugin.Utils;
using HarmonyLib;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Linq;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BleedTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float BleedDamageCoefficient = 0.1f;
        public const float BleedDuration = 5f;
        public const float SurperBleedDamageCoefficient = 0.2f;
        public const float SurperBleedDuration = 10f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy;
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_OnHitEnemy2;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            RoR2Content.Items.BleedOnHit.TryApplyTag(ItemTag.AIBlacklist);
            foreach (var item in ItemCatalog.allItemDefs) {
                if (item.tier == ItemTier.Tier1 || item.tier == ItemTier.Tier2 || item.tier == ItemTier.Tier3)
                    $"BleedTweak :: ItemCatalog :: {RoR2.Language.GetString(item.nameToken)} :: {string.Join(", ", item.tags)}".LogInfo();
            }
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdcI4(5),
                                     x => x.MatchCall<ProcChainMask>("HasProc"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc_0);
                ilcursor.Emit(OpCodes.Ldloc_3);
                ilcursor.EmitDelegate((bool hasProc, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody, uint? maxStacksFromAttacker) => {
                    if (hasProc) return;
                    if (damageInfo.crit) {
                        var superBleedCount = 0f;
                        if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal)) {
                            ++superBleedCount;
                        }
                        if (Util.CheckRoll(attackerBody.bleedChance * damageInfo.procCoefficient, attackerBody.master)) {
                            ++superBleedCount;
                        }
                        if (damageInfo.damageType.damageType.HasFlag(DamageType.SuperBleedOnCrit)) {
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
                            DotController.InflictDot(ref dotInfo);
                        }
                        if (damageInfo.damageType.damageType.HasFlag(DamageType.BleedOnHit)) {
                            var dotInfo = new InflictDotInfo {
                                attackerObject = damageInfo.attacker,
                                dotIndex = DotController.DotIndex.Bleed,
                                duration = BleedDuration,
                                maxStacksFromAttacker = maxStacksFromAttacker,
                                totalDamage = damageInfo.damage * BleedDamageCoefficient * attackerBody.critMultiplier,
                                victimObject = victim,
                            };
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
                        if (damageInfo.damageType.damageType.HasFlag(DamageType.BleedOnHit)) {
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
                            DotController.InflictDot(ref dotInfo);
                        }
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_1);
            } else {
                "BleedOnHit :: Hook Failed!".LogError();
            }
        }

        private void GlobalEventManager_OnHitEnemy2(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(i => i.MatchCall<GlobalEventManager>("ProcDeathMark"),
                x => x.MatchLdarg(1),
                x => x.MatchLdfld<DamageInfo>("inflictor"),
                x => x.MatchLdnull())) {
                ilcursor.Index += 3;
                ilcursor.Emit(OpCodes.Pop)
                        .Emit(OpCodes.Ldnull);
            } else {
                "Bleed :: Hook2 Failed!".LogError();
            }
            if (ilcursor.TryGotoNext(x => x.MatchCall<DotController>("InflictDot"))) {
                ilcursor.Index += 3;
                ilcursor.Emit(OpCodes.Pop)
                        .Emit(OpCodes.Ldc_I4_0);
            } else {
                "Bleed :: Hook3 Failed!".LogError();
            }
        }
    }
}