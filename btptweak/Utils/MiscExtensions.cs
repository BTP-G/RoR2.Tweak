namespace BtpTweak.Utils {

    public static class MiscExtensions {

        public static void Qlog(this object loginfo, string prefix = "QQQlog: ") => Main.Logger.LogInfo(prefix + loginfo);
    }
}