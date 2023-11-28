using BepInEx;
using BepInEx.Logging;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using ConfigurableDifficulty;
using Newtonsoft.Json.Utilities;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<IEventHandlers> eventHandlers = new();
        public static BuffDef VoidFire { get; private set; }
        internal new static ManualLogSource Logger { get; private set; }

        private void Awake() {
            Logger = base.Logger;
            ModConfig.InitConfig(Config);
            Localizer.Init();
            SetUpBuffs();
            InitEventHandlers();
        }

        private void InitEventHandlers() {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.ImplementInterface(typeof(IEventHandlers)) && type.GetCustomAttribute<ObsoleteAttribute>() == null)) {
                var obj = Activator.CreateInstance(type);
                eventHandlers.Add(obj as IEventHandlers);
            }
        }

        private void OnEnable() {
            eventHandlers.ForEach(i => {
                try {
                    i.SetEventHandlers();
                    Logger.LogMessage(i.GetType().FullName + ": has set event handlers.");
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            });
        }

        private void OnDisable() {
            eventHandlers.ForEach(i => {
                try {
                    i.ClearEventHandlers();
                    Logger.LogMessage(i.GetType().FullName + ": has cleared event handlers.");
                } catch (Exception e) {
                    Debug.LogException(e);
                }
            });
        }

        private void SetUpBuffs() {
            VoidFire = ScriptableObject.CreateInstance<BuffDef>();
            VoidFire.name = "Void Fire";
            VoidFire.iconSprite = Texture2DPaths.texBuffOnFireIcon.Load<Sprite>();
            VoidFire.buffColor = new Color(174, 108, 209);
            VoidFire.canStack = false;
            VoidFire.isHidden = true;
            VoidFire.isDebuff = false;
            VoidFire.isCooldown = false;
            if (!R2API.ContentAddition.AddBuffDef(VoidFire)) {
                Logger.LogError("Buff '" + VoidFire.name + "' failed to be added!");
            }
        }
    }
}