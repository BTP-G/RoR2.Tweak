using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BleedTweak : TweakBase<BleedTweak> {
        public const int LifeSteal_SeedStackBleedChance = 10;
        public const int LifeStealBaseBleedChance = 10;

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdcI4(5),
                                     x => x.MatchCall<ProcChainMask>("HasProc"))) {
                ilcursor.Emit(OpCodes.Ldarg_1);
                ilcursor.Emit(OpCodes.Ldarg_2);
                ilcursor.Emit(OpCodes.Ldloc, 1);
                ilcursor.Emit(OpCodes.Ldloc, 5);
                ilcursor.Emit(OpCodes.Ldloc, 0);
                ilcursor.EmitDelegate((bool hasProc, DamageInfo damageInfo, GameObject victim, CharacterBody attackerBody, Inventory inventory, uint? maxStacksFromAttacker) => {
                    if (attackerBody.HasBuff(RoR2Content.Buffs.LifeSteal) && Util.CheckRoll((LifeStealBaseBleedChance + LifeSteal_SeedStackBleedChance * inventory.GetItemCount(RoR2Content.Items.Seed)) * damageInfo.procCoefficient, attackerBody.master)) {
                        DotController.InflictDot(victim, damageInfo.attacker, damageInfo.crit ? DotController.DotIndex.SuperBleed : DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                    }
                    if (hasProc) {
                        return true;
                    }
                    int itemCount = inventory.GetItemCount(RoR2Content.Items.BleedOnHitAndExplode);
                    if (itemCount > 0 && damageInfo.crit) {
                        DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                    }
                    if ((damageInfo.damageType & DamageType.BleedOnHit) != DamageType.Generic) {
                        DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                    }
                    if (Util.CheckRoll(attackerBody.bleedChance * damageInfo.procCoefficient, attackerBody.master)) {
                        DotController.InflictDot(victim, damageInfo.attacker, DotController.DotIndex.Bleed, 4f, 1f, maxStacksFromAttacker);
                    }
                    return true;
                });
            } else {
                Main.Logger.LogError("BleedOnHit :: Hook Failed!");
            }
        }
    }
}