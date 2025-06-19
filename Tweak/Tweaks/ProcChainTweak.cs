using RoR2;

namespace BTP.RoR2Plugin.Tweaks {

    internal static class ProcChainTweak {
        public const ProcType StickyBombOnHit = ProcType.Count;
        private static readonly uint green_red_yellowProcsMask;
        private static readonly uint red_yellowProcsMask;
        private static readonly uint white_green_red_yellowProcsMask;
        private static readonly uint yellowProcsMask;

        static ProcChainTweak() {
            var procChainMask = default(ProcChainMask);
            procChainMask.AddProc(ProcType.LightningStrikeOnHit);
            procChainMask.AddProc(ProcType.Meatball);
            yellowProcsMask = procChainMask.mask;
            procChainMask.AddProc(ProcType.BounceNearby);
            red_yellowProcsMask = procChainMask.mask;
            procChainMask.AddProc(ProcType.Missile);
            procChainMask.AddProc(ProcType.ChainLightning);
            procChainMask.AddProc(ProcType.Rings);
            green_red_yellowProcsMask = procChainMask.mask;
            procChainMask.AddProc(StickyBombOnHit);
            white_green_red_yellowProcsMask = procChainMask.mask;
        }

        public static void AddWGRYProcs(this ref ProcChainMask procChainMask) {
            if (Settings.启用阶梯触发链.Value) {
                procChainMask.mask |= white_green_red_yellowProcsMask;
            }
        }

        public static void AddGRYProcs(this ref ProcChainMask procChainMask) {
            if (Settings.启用阶梯触发链.Value) {
                procChainMask.mask |= green_red_yellowProcsMask;
            }
        }

        public static void AddRYProcs(this ref ProcChainMask procChainMask) {
            if (Settings.启用阶梯触发链.Value) {
                procChainMask.mask |= red_yellowProcsMask;
            }
        }

        public static void AddYellowProcs(this ref ProcChainMask procChainMask) {
            if (Settings.启用阶梯触发链.Value) {
                procChainMask.mask |= yellowProcsMask;
            }
        }
    }
}