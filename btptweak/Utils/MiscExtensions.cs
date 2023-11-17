using RoR2;

namespace BtpTweak.Utils {

    public static class MiscExtensions {
        private static readonly uint whiteItemProcsMask;
        private static readonly uint greenItemProcsMask;
        private static readonly uint redItemProcsMask;
        private static readonly uint yellowItemProcsMask;

        static MiscExtensions() {
            var procChainMask = new ProcChainMask();
            procChainMask.AddProc(ProcType.LightningStrikeOnHit);
            procChainMask.AddProc(ProcType.Meatball);
            yellowItemProcsMask = procChainMask.mask;
            procChainMask.AddProc(ProcType.BounceNearby);
            redItemProcsMask = procChainMask.mask;
            procChainMask.AddProc(ProcType.Missile);
            procChainMask.AddProc(ProcType.ChainLightning);
            procChainMask.AddProc(ProcType.Rings);
            greenItemProcsMask = procChainMask.mask;
            procChainMask.AddProc(ProcType.Count);
            whiteItemProcsMask = procChainMask.mask;
        }

        public static void AddWhiteProcs(this ref ProcChainMask procChainMask) {
            if (ModConfig.ÆôÓÃ½×ÌÝ´¥·¢Á´.Value) {
                procChainMask.mask |= whiteItemProcsMask;
            }
        }

        public static void AddGreenProcs(this ref ProcChainMask procChainMask) {
            if (ModConfig.ÆôÓÃ½×ÌÝ´¥·¢Á´.Value) {
                procChainMask.mask |= greenItemProcsMask;
            }
        }

        public static void AddRedProcs(this ref ProcChainMask procChainMask) {
            if (ModConfig.ÆôÓÃ½×ÌÝ´¥·¢Á´.Value) {
                procChainMask.mask |= redItemProcsMask;
            }
        }

        public static void AddYellowProcs(this ref ProcChainMask procChainMask) {
            if (ModConfig.ÆôÓÃ½×ÌÝ´¥·¢Á´.Value) {
                procChainMask.mask |= yellowItemProcsMask;
            }
        }

        public static void Qlog(this object loginfo, string prefix = "QQQlog: ") => Main.Logger.LogInfo(prefix + loginfo);
    }
}