using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace BtpTweak.Tweaks {

    internal class OrbTweak : TweakBase<OrbTweak> {

        public override void SetEventHandlers() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin += MissileVoidOrb_Begin;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin -= MissileVoidOrb_Begin;
        }

        private void MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdcR4(x, 75f))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_R4, 100f);
            } else {
                Main.Logger.LogError("MissileVoidOrb Hook Failed!");
            }
        }
    }
}