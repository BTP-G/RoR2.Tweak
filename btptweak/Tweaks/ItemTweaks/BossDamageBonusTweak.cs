using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class BossDamageBonusTweak : TweakBase<BossDamageBonusTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var ilcursor = new ILCursor(il);
            if (ilcursor.TryGotoNext(x => x.MatchLdsfld(typeof(RoR2Content.Items).GetField("BossDamageBonus")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {  // 665
                ilcursor.GotoPrev(MoveType.After, x => x.MatchCallvirt<CharacterBody>("get_isBoss"))
                        .Emit(OpCodes.Pop)
                        .Emit(OpCodes.Ldarg_0)
                        .EmitDelegate((HealthComponent healthComponent) => healthComponent.barrier + healthComponent.shield > 0);
            } else {
                "BossDamageBonus Hook Failed!".LogError();
            }
        }
    }
}