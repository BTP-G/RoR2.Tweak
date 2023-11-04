using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class FireworkTweak : TweakBase<FireworkTweak> {
        public const float BaseDamageCoefficent = 1f;
        public const int PercentChance = 5;

        public override void SetEventHandlers() {
            IL.RoR2.GlobalEventManager.OnInteractionBegin += GlobalEventManager_OnInteractionBegin;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.GlobalEventManager.OnInteractionBegin -= GlobalEventManager_OnInteractionBegin;
        }

        private void GlobalEventManager_OnInteractionBegin(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(MoveType.After,
                                   x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("Firework")),
                                   x => x.MatchCallvirt<Inventory>("GetItemCount"))) {
                cursor.Emit(OpCodes.Pop);
                cursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("Firework :: DisableOriginalActionHook Failed!");
            }
        }
    }
}