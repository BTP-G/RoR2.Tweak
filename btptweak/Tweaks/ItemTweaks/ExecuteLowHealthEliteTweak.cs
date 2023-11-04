using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ExecuteLowHealthEliteTweak : TweakBase<ExecuteLowHealthEliteTweak> {
        public const float BaseExecuteEliteHealthFraction = 10f;

        public override void ClearEventHandlers() {
            IL.RoR2.CharacterBody.OnInventoryChanged -= CharacterBody_OnInventoryChanged;
        }

        public override void SetEventHandlers() {
            IL.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void CharacterBody_OnInventoryChanged(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => x.MatchCall<CharacterBody>("set_executeEliteHealthFraction"))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((CharacterBody body) => {
                    float num = body.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite.itemIndex);
                    return 0.5f * (num / (num + 4f));
                });
            } else {
                Main.Logger.LogError("ExecuteLowHealthElite Hook Failed!");
            }
        }
    }
}