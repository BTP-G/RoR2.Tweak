using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Tweaks {

    internal class OrbTweak : TweakBase<OrbTweak> {

        public override void SetEventHandlers() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin += MissileVoidOrb_Begin;
            IL.RoR2.Orbs.SimpleLightningStrikeOrb.OnArrival += SimpleLightningStrikeOrb_OnArrival;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.Orbs.MissileVoidOrb.Begin -= MissileVoidOrb_Begin;
            IL.RoR2.Orbs.SimpleLightningStrikeOrb.OnArrival -= SimpleLightningStrikeOrb_OnArrival;
        }

        private void MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 75f))) {
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_R4, 160f);
            } else {
                Main.Logger.LogError("MissileVoidOrb Hook Failed!");
            }
        }

        private void SimpleLightningStrikeOrb_OnArrival(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<BlastAttack>(x, "procCoefficient"))) {
                cursor.Emit(OpCodes.Pop);
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit<GenericDamageOrb>(OpCodes.Ldfld, "procCoefficient");
            } else {
                Main.Logger.LogError("SimpleLightningStrikeOrb ProcHook Failed!");
            }
        }
    }
}