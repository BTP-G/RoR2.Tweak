using BepInEx.Configuration;

namespace BtpTweak {

    public class ModConfig {
        public static ConfigEntry<uint> 女猎人射程每级增加距离_;
        public static ConfigEntry<bool> 开启日志_;

        public static void InitConfig(ConfigFile config) {
            Main.logger_.LogInfo("InitConfig");
            女猎人射程每级增加距离_ = config.Bind<uint>("女猎人", "女猎人每级射程增加量", 5, "默认射程60m（设置为0就不增加）");
            开启日志_ = config.Bind<bool>("日志", "是否开启战斗日志", false, "原版默认不开启");
        }
    }
}