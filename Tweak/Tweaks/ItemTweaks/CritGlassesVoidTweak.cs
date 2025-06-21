using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class CritGlassesVoidTweak : ModComponent, IModLoadMessageHandler {
        public const float CritDamageMultAdd = 0.0666f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamage;
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void HealthComponent_TakeDamage(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(x => x.MatchLdsfld(typeof(DLC1Content.Items).GetField("CritGlassesVoid")),
                                     x => x.MatchCallvirt<Inventory>("GetItemCount"))) {  // 1359
                cursor.GotoPrev(c => c.MatchLdarg(0),
                                c => c.MatchLdfld<HealthComponent>("body"));
                cursor.Index += 1;
                cursor.RemoveRange(2)
                      .Emit(OpCodes.Pop)
                      .Emit(OpCodes.Ldc_I4_1);
            } else {
                LogExtensions.LogError("CritGlassesVoid Hook Failed!");
            }
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
            if (sender.inventory) {
                args.critDamageMultAdd += CritDamageMultAdd * sender.inventory.GetItemCount(DLC1Content.Items.CritGlassesVoid.itemIndex);
            }
        }
    }
}