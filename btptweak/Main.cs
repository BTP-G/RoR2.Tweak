using BepInEx;
using BepInEx.Logging;
using BtpTweak.Tweaks;
using ConfigurableDifficulty;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Reflection;

namespace BtpTweak
{

    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("com.groovesalad.GrooveSaladSpikestripContent")]
    [BepInDependency("com.plasmacore.PlasmaCoreSpikestripContent")]
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
    [BepInDependency(TPDespair.ZetAspects.ZetAspectsPlugin.ModGuid)]
    [BepInDependency(vanillaVoid.vanillaVoidPlugin.ModGuid)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin {
        public const string PluginAuthor = "BTP";
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginName = "BtpTweak";
        public const string PluginVersion = "2.3.33";

        public static ManualLogSource logger_;
        public static bool 是否选择造物难度_ = false;
        public static bool 往日不再_ = false;

        protected void Awake() {
            logger_ = Logger;
            ModConfig.InitConfig(Config);
            var TweakTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(TweakBase)));
            TweakTypes.ForEachTry(type => Activator.CreateInstance(type));
            RoR2Application.onLoad = (Action)Delegate.Combine(RoR2Application.onLoad, () => {
                TweakBase.Instances.ForEach((instance) => {
                    instance.Load();
                    instance.AddHooks();
                });
                Localization.象征汉化();
                Localization.基础汉化();
                Localization.权杖技能汉化();
                Localization.圣骑士汉化();
                Localization.探路者汉化();
            });
            Run.onRunStartGlobal += run => {
                往日不再_ = false;
                是否选择造物难度_ = run.selectedDifficulty == ConfigurableDifficultyPlugin.configurableDifficultyIndex;
                ConfigurableDifficultyPlugin.configurableDifficultyDef.scalingValue = 2f + 100f / 50f;
                TweakBase.Instances.ForEach(instance => instance.RunStartAction(run));
            };
            Stage.onStageStartGlobal += stage => {
                ConfigurableDifficultyPlugin.configurableDifficultyDef.scalingValue = 2f + 100f / 50f + 0.2f * Run.instance.stageClearCount;
                TweakBase.Instances.ForEach(instance => instance.StageStartAction(stage));
            };
        }
    }
}