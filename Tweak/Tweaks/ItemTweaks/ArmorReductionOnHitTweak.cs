using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ArmorReductionOnHitTweak : ModComponent, IModLoadMessageHandler {
        public const float 基础破甲率 = 0.5f;
        public const float 半数 = 1f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.Before,
                i => i.MatchLdarg(0),
                i => i.MatchLdfld<HealthComponent>("adaptiveArmorValue"),
                i => i.MatchAdd())) {
                cursor.Emit(OpCodes.Ldarg_0)
                      .Emit(OpCodes.Ldloc_1)
                      .EmitDelegate((float armor, HealthComponent healthComponent, CharacterMaster master) => {
                          if (healthComponent.body.HasBuff(RoR2Content.Buffs.Pulverized.buffIndex) && master is not null && master.inventory is not null) {
                              return armor - Util2.CloseTo(master.inventory.GetItemCount(RoR2Content.Items.ArmorReductionOnHit.itemIndex), 半数, armor > 0 ? armor : -armor);
                          }
                          return armor;
                      });
            } else {
                "ArmorReductionOnHit Hook Failed!".LogError();
            }
        }
    }
}