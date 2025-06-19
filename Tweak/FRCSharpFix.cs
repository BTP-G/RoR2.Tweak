using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using UnityEngine;

namespace BTP.RoR2Plugin {

    internal class FRCSharpFix {

        [RuntimeInitializeOnLoadMethod]
        public static void Fix() {
            var targetMethod = typeof(FRCSharp.VF2ContentPackProvider).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Static);
            HookEndpointManager.Modify<Action<ILContext>>(targetMethod, (ILContext il) => {
                var cursor = new ILCursor(il) {
                    Index = 483
                };
                cursor.RemoveRange(4);
            });
            On.RoR2.ItemDisplayRuleSet.Init += ItemDisplayRuleSet_Init;
        }

        private static System.Collections.IEnumerator ItemDisplayRuleSet_Init(On.RoR2.ItemDisplayRuleSet.orig_Init orig) {
            try {
                FRCSharp.FRItemIDRS.ItemDisplayRuleSet_Init(null);
            } catch (Exception) {
            }
            return orig.Invoke();
        }
    }
}
