using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Btp {

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
        public const string PluginName = "Btp";
        public const string PluginVersion = "1.1.1";

        public static bool 是否选择造物难度_ = false;
        public static byte 虚灵战斗阶段计数_;
        public static float 虚空恶鬼二技能充能时间_ = 0;
        public static float 怪物等级生命值系数_ = 1;
        public static float 玩家等级生命值系数_ = 1;
        public static int 盗贼标记_ = 0;
        public static int 玩家等级_ = 1;
        public static int 敌人等级_ = 1;

        public static ConfigEntry<int> 浸剂击杀奖励倍率_;
        public static ConfigEntry<uint> 女猎人射程每级增加距离_;

        public static GameObject electricOrbProjectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricOrbProjectile.prefab").WaitForCompletion();
        public static GameObject mageBody = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageBody.prefab").WaitForCompletion();

        public void Awake() {
            InitConfig();
            Localization.AddHook();
            Infusion.浸剂修改();  // 默认一次5点，无上限
            Skills.技能调整();
            Stats.角色属性调整();
            Damage.伤害调整();
            MiscHook.关卡();
            MiscHook.虚灵战斗消息();
        }

        public void InitConfig() {
            浸剂击杀奖励倍率_ = Config.Bind("Btp - 浸剂", "InfusionCefficient - 浸剂击杀奖励倍率", 5, "击杀一个敌人增加多少的最大生命值。");
            女猎人射程每级增加距离_ = Config.Bind<uint>("Btp - 女猎人射程", "HuntressMaxTrackingDistance - 女猎人每级射程增加量", 5, "默认射程60m（设置为0就不增加）");
        }

        public void RemoveHook() {
        }
    }
}