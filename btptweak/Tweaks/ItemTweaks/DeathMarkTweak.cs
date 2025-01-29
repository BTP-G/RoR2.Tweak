using BtpTweak.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class DeathMarkTweak : TweakBase<DeathMarkTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float BaseDamageCoefficient = 0.4f;
        public const float StackDamageCoefficient = 0.08f;

        public void OnModLoad() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            RoR2Content.Items.DeathMark.TryApplyTag(ItemTag.CannotCopy);
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After,
                              x => x.MatchLdsfld(typeof(RoR2Content.Buffs), "DeathMark"),
                              x => x.MatchCallvirt<CharacterBody>("HasBuff"))
                && cursor.TryGotoNext(MoveType.After, x => x.MatchLdcR4(1.5f))) {
                cursor.Emit(OpCodes.Pop)
                      .Emit(OpCodes.Ldloc_2)
                      .EmitDelegate((TeamIndex attackTeamIndex) => {
                          var teaamItemCount = Util.GetItemCountForTeam(attackTeamIndex, RoR2Content.Items.DeathMark.itemIndex, true, false);
                          return 1f + (teaamItemCount > 0 ? BaseDamageCoefficient + (teaamItemCount - 1) * StackDamageCoefficient : 0f);
                      });
            } else {
                Main.Logger.LogError("DeathMark Hook Failed!");
            }
        }
    }
}