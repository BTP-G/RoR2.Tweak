using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BleedTweak : TweakBase<BleedTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const int LifeSteal_SeedStackBleedChance = 10;
        public const int LifeStealBaseBleedChance = 10;
        public const float BleedDamageCoefficient = 0.5f;
        public const float BleedDuration = 5f;
        public const float SurperBleedDamageCoefficient = 1.5f;
        public const float SurperBleedDuration = 15f;
        public const float BoostDamageMultiplierPerBleedOnHitAndExplode = 1f;

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
                ilcursor.Emit(OpCodes.Ldloc, 5);
                ilcursor.Emit(OpCodes.Ldloc, 0);
                ilcursor.EmitDelegate((bool hasProc, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody, Inventory inventory, uint? maxStacksFromAttacker) => {
                    if (hasProc) {
                        return;
                    }
                    var itemCount = inventory.GetItemCount(RoR2Content.Items.BleedOnHitAndExplode);
                    var bleedCount = 0;
                    var superBleedCount = 0;
                    if (damageInfo.crit) {
                        if (itemCount > 0) {
                            ++superBleedCount;
                        }
                        if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal) && Util.CheckRoll((LifeStealBaseBleedChance + LifeSteal_SeedStackBleedChance * inventory.GetItemCount(RoR2Content.Items.Seed)) * damageInfo.procCoefficient, attackerBody.master)) {
                            ++superBleedCount;
                        }
                        if (damageInfo.damageType.HasFlag(DamageType.SuperBleedOnCrit)) {
                            ++superBleedCount;
                        }
                    } else {
                        if (itemCount > 0) {
                            ++bleedCount;
                        }
                        if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal) && Util.CheckRoll((LifeStealBaseBleedChance + LifeSteal_SeedStackBleedChance * inventory.GetItemCount(RoR2Content.Items.Seed)) * damageInfo.procCoefficient, attackerBody.master)) {
                            ++bleedCount;
                        }
                    }
                    if (damageInfo.damageType.HasFlag(DamageType.BleedOnHit)) {
                        ++bleedCount;
                    }
                    if (Util.CheckRoll(attackerBody.bleedChance * damageInfo.procCoefficient, attackerBody.master)) {
                        if (Util.CheckRoll(attackerBody.bleedChance * 0.2f * damageInfo.procCoefficient, attackerBody.master)) {
                            ++superBleedCount;
                        }
                        ++bleedCount;
                    }
                    if (bleedCount > 0) {
                        var bleedDotInfo = new InflictDotInfo {
                            attackerObject = damageInfo.attacker,
                            damageMultiplier = 1 + itemCount,
                            dotIndex = DotController.DotIndex.Bleed,
                            duration = BleedDuration,
                            maxStacksFromAttacker = maxStacksFromAttacker,
                            victimObject = victim,
                        };
                        while (bleedCount-- > 0) {
                            var d = bleedDotInfo;
                            DotController.InflictDot(ref d);
                        }
                    }
                    if (superBleedCount > 0) {
                        var superBleedDotInfo = new InflictDotInfo {
                            attackerObject = damageInfo.attacker,
                            damageMultiplier = 1 + itemCount,
                            dotIndex = DotController.DotIndex.SuperBleed,
                            duration = SurperBleedDuration,
                            maxStacksFromAttacker = maxStacksFromAttacker,
                            victimObject = victim,
                        };
                        while (superBleedCount-- > 0) {
                            var d = superBleedDotInfo;
                            DotController.InflictDot(ref d);
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