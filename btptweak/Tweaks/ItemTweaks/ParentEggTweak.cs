using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Audio;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ParentEggTweak : TweakBase<ParentEggTweak> {
        public const float HealFractionFromDamage = 0.01f;
        public const float HealAmount = 20f;

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdarg(0),
                                     x => x.MatchLdflda<HealthComponent>("itemCounts"),
                                     x => x.MatchLdfld(typeof(HealthComponent.ItemCounts), "parentEgg"))) {
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate((int itemCount, HealthComponent healthComponent, float damage) => {
                    healthComponent.Heal(itemCount * (damage * HealFractionFromDamage + HealAmount), default, true);
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("ParentEgg hook error");
            }
        }
    }
}