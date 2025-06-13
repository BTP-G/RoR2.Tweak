using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BearTweak : TweakBase<BearTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdarg(0),
                                     x => x.MatchLdflda<HealthComponent>("itemCounts"),
                                     x => x.MatchLdfld(typeof(HealthComponent.ItemCounts), "bear"))) {
                ilcursor.GotoPrev(MoveType.After, i => i.MatchLdloc(6));
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldc_I4_1);
            } else {
                LogExtensions.LogError("Bear hook error");
            }
        }
    }
}