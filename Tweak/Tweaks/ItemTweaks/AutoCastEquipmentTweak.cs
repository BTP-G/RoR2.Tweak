using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class AutoCastEquipmentTweak : ModComponent, IModLoadMessageHandler {
        public const float 强制冷却时间 = 0.15f;

        public void Handle() {
            IL.RoR2.Inventory.UpdateEquipment += IL_Inventory_UpdateEquipment;
        }

        private void IL_Inventory_UpdateEquipment(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => x.MatchStloc(6))) {
                ilcursor.Emit(OpCodes.Ldarg_0)
                        .EmitDelegate((float chargeFinishTime, Inventory inventory) => {
                            return Mathf.Max(强制冷却时间 * inventory.GetItemCount(RoR2Content.Items.AutoCastEquipment.itemIndex), chargeFinishTime);
                        });
            } else {
                "AutoCastEquipment Hook Failed!".LogError();
            }
        }
    }
}