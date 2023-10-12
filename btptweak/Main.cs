using BepInEx;
using BepInEx.Logging;
using BtpTweak.IndexCollections;
using BtpTweak.Tweaks;
using ConfigurableDifficulty;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Reflection;

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
        public const string PluginVersion = "2.3.33";

        public Main() {
            Instance = this;
            Logger = base.Logger;
        }

        public static Main Instance { get; private set; }
        public new static ManualLogSource Logger { get; private set; }

        protected void Awake() {
            ModConfig.InitConfig();
            var TweakTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(TweakBase)));
            TweakTypes.ForEachTry(type => Activator.CreateInstance(type));
            RoR2Application.onLoad = (Action)Delegate.Combine(RoR2Application.onLoad, () => {
                BodyIndexCollection.LoadAllBodyIndexes();
                SceneIndexCollection.LoadAllSceneIndexes();
                TweakBase.Instances.ForEach((instance) => {
                    instance.Load();
                    instance.AddHooks();
                });
                Localization.基础汉化();
                Localization.权杖技能汉化();
                Localization.圣骑士汉化();
                Localization.探路者汉化();
                Localization.象征汉化();
            });
            Run.onRunStartGlobal += run => {
                GlobalInfo.往日不再 = false;
                GlobalInfo.是否选择造物难度 = run.selectedDifficulty == ConfigurableDifficultyPlugin.configurableDifficultyIndex;
                ConfigurableDifficultyPlugin.configurableDifficultyDef.scalingValue = 2f + ConfigurableDifficultyPlugin.difficultyScaling.Value / 50f;
                TweakBase.Instances.ForEach(instance => instance.RunStartAction(run));
            };
            Stage.onStageStartGlobal += stage => {
                GlobalInfo.UpdateCurrentSceneIndex(stage.sceneDef);
                ConfigurableDifficultyPlugin.configurableDifficultyDef.scalingValue = 2f + (ConfigurableDifficultyPlugin.difficultyScaling.Value + ModConfig.每关难度增加量.Value) / 50f;
                TweakBase.Instances.ForEach(instance => instance.StageStartAction(stage));
            };
        }
    }
}