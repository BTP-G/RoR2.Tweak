using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class CritGlassesVoidTweak : TweakBase<CritGlassesVoidTweak> {
        public const float PercentChance = 1f;

        public override void SetEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        public override void ClearEventHandlers() {
            IL.RoR2.HealthComponent.TakeDamage -= HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            ILCursor ilcursor = new(il);
            if (ilcursor.TryGotoNext(MoveType.After,
                                     x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("CritGlassesVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {  // 1359
                ilcursor.Emit(OpCodes.Ldarg_0);
                ilcursor.EmitDelegate((int itemCount, HealthComponent healthComponent) => {
                    if (Util.CheckRoll(itemCount)) {
                        healthComponent.body.AddBuff(RoR2Content.Buffs.PermanentCurse.buffIndex);
                    }
                });
                ilcursor.Emit(OpCodes.Ldc_I4_0);
            } else {
                Main.Logger.LogError("CritGlassesVoid Hook Failed!");
            }
        }
    }
}