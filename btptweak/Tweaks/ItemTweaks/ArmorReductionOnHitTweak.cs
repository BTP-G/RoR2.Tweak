using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ArmorReductionOnHitTweak : TweakBase<ArmorReductionOnHitTweak>, IOnModLoadBehavior {
        public const float 基础破甲率 = 0.5f;
        public const float 半数 = 1f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before,
                                   cursor => cursor.MatchLdloc(37),
                                   cursor => cursor.MatchLdcR4(0f))) {
                cursor.Index += 1;
                cursor.Emit(OpCodes.Ldarg_0)
                      .Emit(OpCodes.Ldloc_1)
                      .EmitDelegate((float armor, HealthComponent healthComponent, CharacterBody attackerBody) => {
                          if (healthComponent.body.HasBuff(RoR2Content.Buffs.Pulverized.buffIndex) && attackerBody && attackerBody.inventory) {
                              return armor - BtpUtils.简单逼近(attackerBody.inventory.GetItemCount(RoR2Content.Items.ArmorReductionOnHit.itemIndex), 半数, armor > 0 ? armor : -armor);
                          }
                          return armor;
                      });
                cursor.Emit(OpCodes.Stloc, 37)
                      .Emit(OpCodes.Ldloc, 37);
            } else {
                Main.Logger.LogError("ArmorReductionOnHit Hook Failed!");
            }
        }
    }
}