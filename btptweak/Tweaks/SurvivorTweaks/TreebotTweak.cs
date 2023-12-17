using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class TreebotTweak : TweakBase<TreebotTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }

        private void HealthComponent_Heal(ILContext il) {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdfld<HealthComponent.ItemCounts>(x, "increaseHealing"))) {
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.EmitDelegate((HealthComponent healthComponent, float amount) => {
                    var inventory = healthComponent.body.inventory;
                    if (inventory) {
                        return amount * (1 + 0.2f * inventory.GetItemCount(RoR2Content.Items.TPHealingNova.itemIndex));
                    } else {
                        return amount;
                    }
                });
                cursor.Emit(OpCodes.Starg, 1);
            } else {
                Main.Logger.LogError("Treebot HealHook Failed!");
            }
        }
    }
}