using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class AutoCastEquipmentTweak : TweakBase<AutoCastEquipmentTweak> {
        public const float 强制冷却时间 = 0.15f;
        public override void ClearEventHandlers() {
            IL.RoR2.Inventory.UpdateEquipment -= IL_Inventory_UpdateEquipment;
        }

        public override void SetEventHandlers() {
            IL.RoR2.Inventory.UpdateEquipment += IL_Inventory_UpdateEquipment;
        }

        private void IL_Inventory_UpdateEquipment(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchStloc(x, 6))) {
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((float chargeFinishTime, Inventory inventory) => {
                    return Mathf.Max(强制冷却时间 * inventory.GetItemCount(RoR2Content.Items.AutoCastEquipment.itemIndex), chargeFinishTime);
                });
            } else {
                Main.Logger.LogError("AutoCastEquipment Hook Failed!");
            }
        }
    }
}