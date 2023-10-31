using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Tweaks {

    internal class OrbTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            IL.RoR2.Orbs.LightningOrb.Begin += LightningOrb_Begin;
            IL.RoR2.Orbs.MissileVoidOrb.Begin += MissileVoidOrb_Begin;
            IL.RoR2.Orbs.SimpleLightningStrikeOrb.OnArrival += SimpleLightningStrikeOrb_OnArrival;
        }

        private void LightningOrb_Begin(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(MoveType.After, x => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/Effects/OrbEffects/RazorwireOrbEffect"))) {
                iLCursor.Emit(OpCodes.Ldarg_0);
                iLCursor.EmitDelegate((LightningOrb lightningOrb) => {
                    lightningOrb.procCoefficient = 0.2f;
                });
            } else {
                Main.Logger.LogError("Razorwire ProcHook Failed!");
            }
        }

        private void MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdcR4(x, 75f))) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 150f);
            } else {
                Main.Logger.LogError("MissileVoidOrb Hook Failed!");
            }
        }

        private void SimpleLightningStrikeOrb_OnArrival(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchStfld<BlastAttack>(x, "procCoefficient"))) {
                --cursor.Index;
                cursor.Remove();
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit<GenericDamageOrb>(OpCodes.Ldfld, "procCoefficient");
            } else {
                Main.Logger.LogError("SimpleLightningStrikeOrb ProcHook Failed!");
            }
        }
    }
}