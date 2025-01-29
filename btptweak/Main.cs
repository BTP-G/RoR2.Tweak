using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BtpTweak.Tweaks;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BtpTweak {

    [BepInDependency("com.groovesalad.GrooveSaladSpikestripContent")]
    [BepInDependency("com.plasmacore.PlasmaCoreSpikestripContent")]
    [BepInDependency("com.rune580.riskofoptions")]
    [BepInDependency("com.Phreel.GoldenCoastPlusRevived")]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(TPDespair.ZetAspects.ZetAspectsPlugin.ModGuid)]
    [BepInDependency(vanillaVoid.vanillaVoidPlugin.ModGuid)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin {
        public const string PluginAuthor = "BTP";
        public const string PluginGUID = "com." + PluginAuthor + "." + PluginName;
        public const string PluginName = "BtpTweak";
        public const string PluginVersion = "3.0.0";
        private readonly List<IOnModLoadBehavior> onModLoadBehaviors = [];
        private readonly List<IOnModUnloadBehavior> onModUnloadBehaviors = [];
        internal new static ManualLogSource Logger { get; private set; }
        internal new static ConfigFile Config { get; private set; }

        private void Awake() {
            Config = base.Config;
            Logger = base.Logger;
            ModConfig.InitConfig(base.Config);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (!type.IsAbstract && !type.IsDefined(typeof(ObsoleteAttribute))) {
                    if (type.IsSubclassOf(typeof(TweakBase))) {
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