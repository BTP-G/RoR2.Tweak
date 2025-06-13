using BepInEx.Logging;

namespace BTP.RoR2Plugin {

    public static class LogExtensions {
        internal static ManualLogSource logger;

        public static void LogInfo<T>(this T data) => logger.LogInfo(data);

        public static void LogWarning<T>(this T data) => logger.LogWarning(data);

        public static void LogError<T>(this T data) => logger.LogError(data);

        public static void LogMessage<T>(this T data) => logger.LogMessage(data);

        public static void LogFormat<T>(this T format, LogLevel level = LogLevel.None, params object[] args) => logger.Log(level, string.Format(format.ToString(), args));
    }
}