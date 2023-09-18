using BepInEx.Configuration;

namespace BtpTweak {

    public class ModConfig {
        public static ConfigFile config_;
        public static ConfigEntry<uint> 女猎人射程每级增加距离_;
        public static ConfigEntry<bool> 开启战斗日志_;
        public static ConfigEntry<bool> 测试用_;
        public static ConfigEntry<float> 测试用2_;
        public static ConfigEntry<float> 测试用3_;
        public static ConfigEntry<float> 测试用4_;
        public static ConfigEntry<float> 测试用5_;
        public static ConfigEntry<float> 测试用6_;
        public static ConfigEntry<float> 测试用7_;

        public static void InitConfig(ConfigFile config) {
            Main.logger_.LogInfo("InitConfig");
            config_ = config;
            女猎人射程每级增加距离_ = config.Bind<uint>("女猎人", "女猎人每级射程增加量", 5, "默认射程60m（设置为0就不增加）");
            开启战斗日志_ = config.Bind("日志", "是否开启战斗日志", false, "原版默认不开启");
            测试用_ = config.Bind("测试", "-", true, "-");
            测试用2_ = config.Bind("测试2", "-", 0f, "-");
            测试用3_ = config.Bind("测试3", "-", 0f, "-");
            测试用4_ = config.Bind("测试4", "-", 0f, "-");
            测试用5_ = config.Bind("测试5", "-", 0f, "-");
            测试用6_ = config.Bind("测试6", "-", 0f, "-");
            测试用7_ = config.Bind("测试7", "-", 0f, "-");
        }
    }
}