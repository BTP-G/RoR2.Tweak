using BepInEx;
using BepInEx.Logging;
using BtpTweak.Tweaks;
using ConfigurableDifficulty;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BtpTweak {

    [BepInDependency("com.groovesalad.GrooveSaladSpikestripContent")]
    [BepInDependency("com.plasmacore.PlasmaCoreSpikestripContent")]
    [BepInDependency("com.rune580.riskofoptions")]
    [BepInDependency("com.Skell.GoldenCoastPlus")]
    [BepInDependency(ConfigurableDifficultyPlugin.PluginGUID)]
    [BepInDependency(HIFUArtificerTweaks.Main.PluginGUID)]
    [BepInDependency(HIFUCommandoTweaks.Main.PluginGUID)]
    [BepInDependency(HIFUEngineerTweaks.Main.PluginGUID)]
    [BepInDependency(HIFUHuntressTweaks.Main.PluginGUID)]
    [BepInDependency(HIFULoaderTweaks.Main.PluginGUID)]
    [BepInDependency(HIFUMercenaryTweaks.Main.PluginGUID)]
    [BepInDependency(HIFURailgunnerTweaks.Main.PluginGUID)]
    [BepInDependency(HIFURexTweaks.Main.PluginGUID)]
    [BepInDependency(HuntressAutoaimFix.Main.PluginGUID)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(TPDespair.ZetAspects.ZetAspectsPlugin.ModGuid)]
    [BepInDependency(vanillaVoid.vanillaVoidPlugin.ModGuid)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin {
        public const string PluginAuthor = "BTP";
        public const string PluginGUID = "com." + PluginAuthor + "." + PluginName;
        public const string PluginName = "BtpTweak";
        public const string PluginVersion = "2.3.4";
        private readonly List<IOnModLoadBehavior> onModLoadBehaviors = [];
        private readonly List<IOnModUnloadBehavior> onModUnloadBehaviors = [];

        internal new static ManualLogSource Logger { get; private set; }

        private void Awake() {
            Logger = base.Logger;
            ModConfig.InitConfig(Config);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(TweakBase)) && !type.IsDefined(typeof(ObsoleteAttribute))) {
                    var tweakBase = Activator.CreateInstance(type);
                    if (tweakBase is IOnModLoadBehavior enableBehavior) {
                        onModLoadBehaviors.Add(enableBehavior);
                    }
                    if (tweakBase is IOnModUnloadBehavior disableBehavior) {
                        onModUnloadBehaviors.Add(disableBehavior);
                    }
                    if (tweakBase is IOnRoR2LoadedBehavior ror2LoadedBehavior) {
                        RoR2Application.onLoad += ror2LoadedBehavior.OnRoR2Loaded;
                    }
                }
                var staticMethodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (staticMethodInfos.Length > 0) {
                    foreach (var staticMethod in staticMethodInfos) {
                        if (staticMethod.IsDefined(typeof(RuntimeInitializeOnLoadMethodAttribute), true)) {
                            try {
                                staticMethod.Invoke(null, null);
                                Logger.LogMessage($"RuntimeInitializeOnLoadMethod: {type.FullName}.{staticMethod.Name} has been called.");
                            } catch (Exception e) {
                                Logger.LogError(e);
                            }
                        }
                    }
                }
            }
        }

        private void OnEnable() {
            for (int i = 0; i < onModLoadBehaviors.Count; ++i) {
                try {
                    var onModLoadBehavior = onModLoadBehaviors[i];
                    onModLoadBehavior.OnModLoad();
                    Logger.LogMessage(onModLoadBehavior.GetType().FullName + " :: has set event handlers.");
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }

        private void OnDisable() {
            for (int i = 0; i < onModUnloadBehaviors.Count; ++i) {
                try {
                    var onModUnloadBehavior = onModUnloadBehaviors[i];
                    onModUnloadBehavior.OnModUnload();
                    Logger.LogMessage(onModUnloadBehavior.GetType().FullName + " :: has cleared event handlers.");
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }
    }
}