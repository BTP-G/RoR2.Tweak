using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class NovaOnHealTweak : TweakBase<NovaOnHealTweak> {

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.ServerFixedUpdate += HealthComponent_ServerFixedUpdate;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.ServerFixedUpdate -= HealthComponent_ServerFixedUpdate;
        }

        private void HealthComponent_ServerFixedUpdate(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(MoveType.Before,
                x => x.MatchLdcR4(0.1f),
                x => x.MatchAdd(),
                x => x.MatchStfld<HealthComponent>("devilOrbTimer"))) {
                iLCursor.Remove();
                iLCursor.Emit(OpCodes.Ldc_R4, 0.666f);
            } else {
                Main.Logger.LogError("NovaOnHeal Hook Failed!");
            }
        }
    }
}