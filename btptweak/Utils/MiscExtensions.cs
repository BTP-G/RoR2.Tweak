using RoR2;

namespace BtpTweak.Utils {

    public static class MiscExtensions {

        public static void AddPoolProcs(this ref ProcChainMask procChainMask) {
            if (ModConfig.ÖÐ¶Ï´¥·¢Á´.Value) {
                procChainMask.AddProc(ProcType.BounceNearby);
                procChainMask.AddProc(ProcType.ChainLightning);
                procChainMask.AddProc(ProcType.Count);
                procChainMask.AddProc(ProcType.LightningStrikeOnHit);
                procChainMask.AddProc(ProcType.Meatball);
                procChainMask.AddProc(ProcType.Missile);
                procChainMask.AddProc(ProcType.Rings);
            }
        }

        public static void Qlog(this object loginfo, string prefix = "QQQlog: ") => Main.Logger.LogInfo(prefix + loginfo);
    }
}