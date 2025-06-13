using BepInEx;
using BTP.RoR2Plugin.Tweaks;
using RoR2;
using RoR2.UI.MainMenu;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BTP.RoR2Plugin {

    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency(TPDespair.ZetAspects.ZetAspectsPlugin.ModGuid)]
    [BepInDependency(vanillaVoid.vanillaVoidPlugin.ModGuid)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin {
        public const string PluginAuthor = "BTP";
        public const string PluginGUID = "com.BTP.Tweak";
        public const string PluginName = "BTP.Tweak";
        public const string PluginVersion = "3.0.0";
        private readonly List<IOnModLoadBehavior> onModLoadBehaviors = [];
        private readonly List<IOnModUnloadBehavior> onModUnloadBehaviors = [];
        private readonly List<IOnRoR2LoadedBehavior> onRoR2LoadedBehaviors = [];

        private void Awake() {
            LogExtensions.logger = base.Logger;
            Settings.Initialize(base.Config);
            MainMenuController.OnMainMenuInitialised += OnRoR2Loaded;
            AssetReferences.Init();
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
                            onRoR2LoadedBehaviors.Add(ror2LoadedBehavior);
                        }
                    }
                }
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).AsSpan()) {
                    if (method.IsDefined(typeof(RuntimeInitializeOnLoadMethodAttribute), true)) {
                        try {
                            method.Invoke(null, null);
                            Logger.LogMessage($"RuntimeInitializeOnLoadMethod: {type.FullName}.{method.Name} has been called.");
                        } catch (Exception e) {
                            Logger.LogError(e);
                        }
                    }
                }
            }
        }

        private void OnRoR2Loaded() {
            MainMenuController.OnMainMenuInitialised -= OnRoR2Loaded;
            foreach (var behavior in onRoR2LoadedBehaviors) {
                try {
                    var roR2LoadedBehavior = behavior;
                    roR2LoadedBehavior.OnRoR2Loaded();
                    Logger.LogMessage(roR2LoadedBehavior.GetType().FullName + " :: has called.");
                } catch (Exception e) {
                    Debug.LogException(e, this);
                }
            }
        }

        private void OnEnable() {
            foreach (var behavior in onModLoadBehaviors) {
                try {
                    var onModLoadBehavior = behavior;
                    onModLoadBehavior.OnModLoad();
                    Logger.LogMessage(onModLoadBehavior.GetType().FullName + " :: has set event handlers.");
                } catch (Exception e) {
                    Debug.LogException(e, this);
                }
            }
        }

        private void OnDisable() {
            foreach (var behavior in onModUnloadBehaviors) {
                try {
                    var onModUnloadBehavior = behavior;
                    onModUnloadBehavior.OnModUnload();
                    Logger.LogMessage(onModUnloadBehavior.GetType().FullName + " :: has cleared event handlers.");
                } catch (Exception e) {
                    Debug.LogException(e, this);
                }
            }
        }
    }
}