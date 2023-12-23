using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class DeathMarkTweak : TweakBase<DeathMarkTweak>, IOnModLoadBehavior {
        public const float BaseDamageCoefficient = 0.4f;
        public const float StackDamageCoefficient = 0.08f;

        public void OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                              x => x.MatchLdsfld(typeof(RoR2Content.Buffs), "DeathMark"),
                              x => x.MatchCallvirt<CharacterBody>("HasBuff"))
                && c.TryGotoNext(MoveType.After,
                                 x => x.MatchLdcR4(1.5f))) {
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldloc_2);
                c.EmitDelegate((TeamIndex attackTeamIndex) => {
                    var teaamItemCount = Util.GetItemCountForTeam(attackTeamIndex, RoR2Content.Items.DeathMark.itemIndex, true, false);
                    return 1f + (teaamItemCount > 0 ? BaseDamageCoefficient + StackDamageCoefficient * (teaamItemCount - 1) : 0);
                });
            } else {
                Main.Logger.LogError("DeathMark Hook Failed!");
            }
        }
    }
}