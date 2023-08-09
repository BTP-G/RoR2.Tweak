using BepInEx;
using BepInEx.Logging;

namespace BtpTweak {

    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("com.DestroyedClone.AncientScepter")]
    [BepInDependency("com.groovesalad.GrooveSaladSpikestripContent")]
    [BepInDependency("com.plasmacore.PlasmaCoreSpikestripContent")]
    [BepInDependency("com.Skell.GoldenCoastPlus")]
    [BepInDependency(ConfigurableDifficulty.ConfigurableDifficultyPlugin.PluginGUID)]
    [BepInDependency(HIFUArtificerTweaks.Main.PluginGUID)]
    [BepInDependency(HIFUCaptainTweaks.Main.PluginGUID)]
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
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "BTP";
        public const string PluginName = "BtpTweak";
        public const string PluginVersion = "2.3.33";

        public static bool 是否选择造物难度_ = false;

        public static ManualLogSource logger_;

        public void Awake() {
            logger_ = Logger;
            ModConfig.InitConfig(Config);
            BuffAndDotHook.AddHook();
            CombatHook.AddHook();
            EffectHook.AddHook();
            FinalBossHook.AddHook();
            GlobalEventHook.AddHook();
            HealthHook.AddHook();
            MainMenuHook.AddHook();
            MiscHook.AddHook();
            RunHook.AddHook();
            SkillHook.AddHook();
            StatHook.AddHook();
        }
    }
}