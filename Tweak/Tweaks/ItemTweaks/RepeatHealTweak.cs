﻿using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    [System.Obsolete]
    internal class RepeatHealTweak : ModComponent, IModLoadMessageHandler {

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }

        private void HealthComponent_Heal(ILContext il) {
            ILCursor iLCursor = new(il);
            if (iLCursor.TryGotoNext(x => x.MatchStfld<HealthComponent.RepeatHealComponent>("healthFractionToRestorePerSecond"))) {
                iLCursor.GotoPrev(MoveType.Before, x => x.MatchLdcR4(0.1f))
                        .Remove()
                        .Emit(OpCodes.Ldc_R4, 0.5f);
            } else {
                LogExtensions.LogError("RepeatHeal Hook Failed!");
            }
        }
    }
}