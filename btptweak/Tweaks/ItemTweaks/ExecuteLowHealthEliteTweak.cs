using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ExecuteLowHealthEliteTweak : TweakBase<ExecuteLowHealthEliteTweak>, IOnModLoadBehavior {
        public const float BaseExecuteEliteHealthFraction = 10f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void CharacterBody_OnInventoryChanged(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => x.MatchCall<CharacterBody>("set_executeEliteHealthFraction"))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((CharacterBody body) => BtpUtils.简单逼近(body.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite.itemIndex), 4f, 0.5f));
            } else {
                Main.Logger.LogError("ExecuteLowHealthElite Hook Failed!");
            }
        }
    }
}