using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ParentEggTweak : TweakBase<ParentEggTweak>, IOnModLoadBehavior {
        public const float HealFractionFromDamage = 0.01f;
        public const float HealAmount = 20f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdarg(0),
                                     x => x.MatchLdflda<HealthComponent>("itemCounts"),
                                     x => x.MatchLdfld(typeof(HealthComponent.ItemCounts), "parentEgg"))) {
                ilcursor.Emit(OpCodes.Ldarg_0)
                        .Emit(OpCodes.Ldloc, 7)
                        .EmitDelegate((int itemCount, HealthComponent healthComponent, float damage) => {
                            healthComponent.Heal(itemCount * (damage * HealFractionFromDamage + HealAmount), default, true);
                        });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ParentEgg hook error");
            }
        }
    }
}