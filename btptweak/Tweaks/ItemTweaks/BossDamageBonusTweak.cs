using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class BossDamageBonusTweak : TweakBase<BossDamageBonusTweak> {

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("BossDamageBonus")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {  // 665
                ilcursor.GotoPrev(MoveType.After, x => x.MatchCallvirt<CharacterBody>("get_isBoss"));
                ilcursor.Emit(OpCodes.Pop);
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((HealthComponent healthComponent) => healthComponent.barrier + healthComponent.shield > 0);
            } else {
                Main.Logger.LogError("BossDamageBonus Hook Failed!");
            }
        }
    }
}