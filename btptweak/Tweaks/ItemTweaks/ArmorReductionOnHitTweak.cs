using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ArmorReductionOnHitTweak : TweakBase<ArmorReductionOnHitTweak> {

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   cursor => cursor.MatchLdloc(33),
                                   cursor => cursor.MatchLdcR4(0f))) {
                cursor.Index += 1;
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Ldloc_1);
                cursor.EmitDelegate((float armor, HealthComponent healthComponent, CharacterBody attackerBody) => {
                    if (healthComponent.body.HasBuff(RoR2Content.Buffs.Pulverized.buffIndex) && attackerBody) {
                        var itemCount = attackerBody.inventory?.GetItemCount(RoR2Content.Items.ArmorReductionOnHit.itemIndex) ?? 0f;
                        armor -= Mathf.Abs(armor * itemCount / (itemCount + 1f));
                    }
                    return armor;
                });
                cursor.Emit(OpCodes.Stloc, 33);
                cursor.Emit(OpCodes.Ldloc, 33);
            } else {
                Main.Logger.LogError("ArmorReductionOnHit Hook Failed!");
            }
        }
    }
}