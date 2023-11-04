using RoR2;

namespace BtpTweak.Utils {

    public static class MiscExtensions {

        public static void AddPoolProcs(this ref ProcChainMask procChainMask) {
            if (ModConfig.²âÊÔÓÃ3.Value > 100) {
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