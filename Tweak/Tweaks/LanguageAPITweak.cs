using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using R2API;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static Mono.Security.X509.X520;

namespace BTP.RoR2Plugin.Tweaks {

    internal static class LanguageAPITweak {

        [ModLoadMessageHandler]
        internal static void Handle() {
            var targetMethod = typeof(LanguageAPI).GetMethod("Language_GetLocalizedStringByToken", BindingFlags.NonPublic | BindingFlags.Static);
            HookEndpointManager.Modify<Action<ILContext>>(targetMethod, (ILContext il) => {
                var cursor = new ILCursor(il);
                cursor.RemoveRange(67);
                cursor.Index += 3;
                cursor.Remove()
                .Emit(OpCodes.Ldsfld, typeof(LanguageAPI).GetField("OverlayLanguage", BindingFlags.NonPublic | BindingFlags.Static))
                .Emit(OpCodes.Ldsfld, typeof(LanguageAPI).GetField("CustomLanguage", BindingFlags.NonPublic | BindingFlags.Static))
                .EmitDelegate((On.RoR2.Language.orig_GetLocalizedStringByToken orig, RoR2.Language self, string token, Dictionary<string, Dictionary<string, string>> overlayLanguage, Dictionary<string, Dictionary<string, string>> customLanguage) => {
                    if (overlayLanguage.TryGetValue(self.name, out var innerDict) && innerDict.TryGetValue(token, out var result)) {
                        return result;
                    }
                    if (overlayLanguage.TryGetValue("generic", out innerDict) && innerDict.TryGetValue(token, out result)) {
                        return result;
                    }
                    if (customLanguage.TryGetValue(self.name, out innerDict) && innerDict.TryGetValue(token, out result)) {
                        return result;
                    }
                    if (customLanguage.TryGetValue("generic", out innerDict) && innerDict.TryGetValue(token, out result)) {
                        return result;
                    }
                    return orig(self, token);
                });
            });
        }
    }
}