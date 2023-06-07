using BepInEx;
using BepInEx.Configuration;

namespace BtpTweak {

    //This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(R2API.R2API.PluginGUID)]
    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class BtpTweak : BaseUnityPlugin {

        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "BTP";
        public const string PluginName = "BtpTweak";
        public const string PluginVersion = "1.1.1";

        public static float 玩家角色等级_ = 1;
        public static float 玩家角色等级生命值系数_ = 1;
        public static bool 是否选择造物难度_ = false;
        public static float holdTime = 0;
        public static ushort 虚灵战斗阶段计数_;
        public static ushort fireSeekingArrowCount = 0;

        public static ConfigEntry<float> 造物难度最大修正难度缩放_;
        public static ConfigEntry<int> 浸剂击杀奖励倍率_;
        public static ConfigEntry<uint> 女猎人射程每级增加距离_;

        public void Awake() {
            InitConfig();
            Localization.汉化();
            InfusionTweak.浸剂修改();  // 一次5点，无上限
            LevelStatsTweak.角色修改();
            DamageTweak.虚灵伤害修改();
            DamageTweak.毒狗被动伤害修改();
            StageDifficultyTweak.关卡难度缩放修改();
        }

        public void InitConfig() {
            造物难度最大修正难度缩放_ = Config.Bind<float>("BtpTweak - 难度缩放", "MaxDifficultScale -  造物难度最大修正难度缩放", 24, "此项只影响造物难度。其中造物难度缩放 = 3 + 关卡数。原版季风难度缩放为 3。");
            浸剂击杀奖励倍率_ = Config.Bind<int>("BtpTweak - 浸剂", "InfusionCefficient - 浸剂击杀奖励倍率", 5, "击杀一个敌人增加多少的最大生命值。");
            女猎人射程每级增加距离_ = Config.Bind<uint>("BtpTweak - 女猎人射程", "HuntressMaxTrackingDistance - 女猎人每级射程增加量", 5, "默认射程60m（设置为0就不增加）");
        }

        public void RemoveHook() {
        }
    }
}