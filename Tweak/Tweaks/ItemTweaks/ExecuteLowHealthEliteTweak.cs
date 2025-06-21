using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class ExecuteLowHealthEliteTweak : ModComponent, IModLoadMessageHandler {
        public const float BaseExecuteEliteHealthFraction = 10f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void CharacterBody_OnInventoryChanged(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => x.MatchCall<CharacterBody>("set_executeEliteHealthFraction"))) {
                ilcursor.Emit(OpCodes.Pop)
                        .Emit(OpCodes.Ldarg_0)
                        .EmitDelegate((CharacterBody body) => Util2.CloseTo(body.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite.itemIndex), 4f, 0.5f));
            } else {
                LogExtensions.LogError("ExecuteLowHealthElite Hook Failed!");
            }
        }
    }
}