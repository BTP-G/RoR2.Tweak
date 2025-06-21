using BepInEx;
using RoR2.UI.MainMenu;
using System;
using System.Buffers;
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
        public const string PluginVersion = "3.0.1";
        internal static HashSet<IFixedTickable> fixedTickableSet = [];
        private readonly List<IModLoadMessageHandler> modLoadMessageHandlers = [];
        private readonly List<IModUnloadMessageHandler> modUnloadMessageHandlers = [];
        private readonly List<IRoR2LoadedMessageHandler> ror2LoadedMessageHandlers = [];

        private void Awake() {
            LogExtensions.logger = Logger;
            Settings.Initialize(Config);
            MainMenuController.OnMainMenuInitialised += OnMainMenuFirstInitialised;
            //RoR2Application.onLoadFinished
            AssetReferences.Init();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (!type.IsAbstract && !type.IsDefined(typeof(ObsoleteAttribute))) {
                    if (type.IsSubclassOf(typeof(ModComponent))) {
                        var component = Activator.CreateInstance(type);
                        if (component is IModLoadMessageHandler loadMessageHandler) {
                            modLoadMessageHandlers.Add(loadMessageHandler);
                        }
                        if (component is IModUnloadMessageHandler unloadMessageHandler) {
                            modUnloadMessageHandlers.Add(unloadMessageHandler);
                        }
                        if (component is IRoR2LoadedMessageHandler ror2LoadedMessageHandler) {
                            ror2LoadedMessageHandlers.Add(ror2LoadedMessageHandler);
                        }
                    }
                }
                foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).AsSpan()) {
                    if (method.IsDefined(typeof(ModLoadMessageHandlerAttribute), true)) {
                        try {
                            method.Invoke(null, null);
                            Logger.LogMessage($"{type.FullName}.{method.Name} has been called.");
                        } catch (Exception e) {
                            Logger.LogError(e);
                        }
                    }
                }
            }
        }

        private void OnEnable() {
            foreach (var handler in modLoadMessageHandlers) {
                try {
                    var handler2 = handler;
                    handler2.Handle();
                    Logger.LogMessage($"{handler2.GetType().FullName} :: {nameof(handler2.Handle)} has been called.");
                } catch (Exception e) {
                    Debug.LogException(e, this);
                }
            }
        }

        private void OnMainMenuFirstInitialised() {
            MainMenuController.OnMainMenuInitialised -= OnMainMenuFirstInitialised;
            foreach (var handler in ror2LoadedMessageHandlers) {
                try {
                    var handler2 = handler;
                    handler2.Handle();
                    Logger.LogMessage($"{handler2.GetType().FullName} :: {nameof(handler2.Handle)} has been called.");
                } catch (Exception e) {
                    Debug.LogException(e, this);
                }
            }
        }

        private void FixedUpdate() {
            var array = ArrayPool<IFixedTickable>.Shared.Rent(fixedTickableSet.Count);
            fixedTickableSet.CopyTo(array);
            foreach (ref var tickable in array.AsSpan()) {
                if (tickable == null) break;
                tickable.FixedTick();
                tickable = null;
            }
            ArrayPool<IFixedTickable>.Shared.Return(array);
        }

        private void OnDisable() {
            foreach (var handler in modUnloadMessageHandlers) {
                try {
                    var handler2 = handler;
                    handler2.Handle();
                    Logger.LogMessage($"{handler2.GetType().FullName} :: {nameof(handler2.Handle)} has been called.");
                } catch (Exception e) {
                    Debug.LogException(e, this);
                }
            }
        }
    }
}