using RoR2;
using System.Collections.Generic;

namespace BtpTweak.RoR2Indexes
{

    public sealed class BodyIndexes : IEventHandlers {
        private static BodyIndex _arbiterBody = BodyIndex.None;
        private static BodyIndex _bandit2Body = BodyIndex.None;
        private static BodyIndex _brotherBody = BodyIndex.None;
        private static BodyIndex _brotherHurtBody = BodyIndex.None;
        private static BodyIndex _captainBody = BodyIndex.None;
        private static BodyIndex _chef = BodyIndex.None;
        private static BodyIndex _commandoBody = BodyIndex.None;
        private static BodyIndex _crocoBody = BodyIndex.None;
        private static BodyIndex _engiBody = BodyIndex.None;
        private static BodyIndex _engiTurretBody = BodyIndex.None;
        private static BodyIndex _engiWalkerTurretBody = BodyIndex.None;
        private static BodyIndex _equipmentDroneBody = BodyIndex.None;
        private static BodyIndex _hereticBody = BodyIndex.None;
        private static BodyIndex _huntressBody = BodyIndex.None;
        private static BodyIndex _loaderBody = BodyIndex.None;
        private static BodyIndex _mageBody = BodyIndex.None;
        private static BodyIndex _mercBody = BodyIndex.None;
        private static BodyIndex _pathfinderBody = BodyIndex.None;
        private static BodyIndex _railgunnerBody = BodyIndex.None;
        private static BodyIndex _redMistBody = BodyIndex.None;
        private static BodyIndex _robPaladinBody = BodyIndex.None;
        private static BodyIndex _sniperClassicBody = BodyIndex.None;
        private static BodyIndex _toolbotBody = BodyIndex.None;
        private static BodyIndex _treebotBody = BodyIndex.None;
        private static BodyIndex _voidSurvivorBody = BodyIndex.None;

        public enum BodyNameIndex : byte {
            None = 0,
            ArbiterBody,
            Bandit2Body,
            BrotherBody,
            BrotherHurtBody,
            CaptainBody,
            CHEF,
            CommandoBody,
            CrocoBody,
            EngiBody,
            EngiTurretBody,
            EngiWalkerTurretBody,
            HereticBody,
            HuntressBody,
            LoaderBody,
            MageBody,
            MercBody,
            MiniVoidRaidCrabBodyPhase1,
            MiniVoidRaidCrabBodyPhase2,
            MiniVoidRaidCrabBodyPhase3,
            PathfinderBody,
            RailgunnerBody,
            RedMistBody,
            RobPaladinBody,
            SniperClassicBody,
            ToolbotBody,
            TreebotBody,
            VoidSurvivorBody,
            Count,
        }

        public static BodyIndex ArbiterBody {
            get {
                if (_arbiterBody != BodyIndex.None) {
                    return _arbiterBody;
                } else {
                    return _arbiterBody = BodyCatalog.FindBodyIndex(BodyNameIndex.ArbiterBody.ToString());
                }
            }
        }

        public static BodyIndex Bandit2Body {
            get {
                if (_bandit2Body != BodyIndex.None) {
                    return _bandit2Body;
                } else {
                    return _bandit2Body = BodyCatalog.FindBodyIndex(BodyNameIndex.Bandit2Body.ToString());
                }
            }
        }

        public static BodyIndex BrotherBody {
            get {
                if (_brotherBody != BodyIndex.None) {
                    return _brotherBody;
                } else {
                    return _brotherBody = BodyCatalog.FindBodyIndex(BodyNameIndex.BrotherBody.ToString());
                }
            }
        }

        public static BodyIndex BrotherHurtBody {
            get {
                if (_brotherHurtBody != BodyIndex.None) {
                    return _brotherHurtBody;
                } else {
                    return _brotherHurtBody = BodyCatalog.FindBodyIndex(BodyNameIndex.BrotherHurtBody.ToString());
                }
            }
        }

        public static BodyIndex CaptainBody {
            get {
                if (_captainBody != BodyIndex.None) {
                    return _captainBody;
                } else {
                    return _captainBody = BodyCatalog.FindBodyIndex(BodyNameIndex.CaptainBody.ToString());
                }
            }
        }

        public static BodyIndex CHEF {
            get {
                if (_chef != BodyIndex.None) {
                    return _chef;
                } else {
                    return _chef = BodyCatalog.FindBodyIndex(BodyNameIndex.CHEF.ToString());
                }
            }
        }

        public static BodyIndex CommandoBody {
            get {
                if (_commandoBody != BodyIndex.None) {
                    return _commandoBody;
                } else {
                    return _commandoBody = BodyCatalog.FindBodyIndex(BodyNameIndex.CommandoBody.ToString());
                }
            }
        }

        public static BodyIndex CrocoBody {
            get {
                if (_crocoBody != BodyIndex.None) {
                    return _crocoBody;
                } else {
                    return _crocoBody = BodyCatalog.FindBodyIndex(BodyNameIndex.CrocoBody.ToString());
                }
            }
        }

        public static BodyIndex EngiBody {
            get {
                if (_engiBody != BodyIndex.None) {
                    return _engiBody;
                } else {
                    return _engiBody = BodyCatalog.FindBodyIndex(BodyNameIndex.EngiBody.ToString());
                }
            }
        }

        public static BodyIndex EngiTurretBody {
            get {
                if (_engiTurretBody != BodyIndex.None) {
                    return _engiTurretBody;
                } else {
                    return _engiTurretBody = BodyCatalog.FindBodyIndex(BodyNameIndex.EngiTurretBody.ToString());
                }
            }
        }

        public static BodyIndex EngiWalkerTurretBody {
            get {
                if (_engiWalkerTurretBody != BodyIndex.None) {
                    return _engiWalkerTurretBody;
                } else {
                    return _engiWalkerTurretBody = BodyCatalog.FindBodyIndex(BodyNameIndex.EngiWalkerTurretBody.ToString());
                }
            }
        }

        public static BodyIndex EquipmentDroneBody {
            get {
                if (_equipmentDroneBody != BodyIndex.None) {
                    return _equipmentDroneBody;
                } else {
                    return _equipmentDroneBody = BodyCatalog.FindBodyIndex("EquipmentDroneBody");
                }
            }
        }

        public static BodyIndex HereticBody {
            get {
                if (_hereticBody != BodyIndex.None) {
                    return _hereticBody;
                } else {
                    return _hereticBody = BodyCatalog.FindBodyIndex(BodyNameIndex.HereticBody.ToString());
                }
            }
        }

        public static BodyIndex HuntressBody {
            get {
                if (_huntressBody != BodyIndex.None) {
                    return _huntressBody;
                } else {
                    return _huntressBody = BodyCatalog.FindBodyIndex(BodyNameIndex.HuntressBody.ToString());
                }
            }
        }

        public static BodyIndex LoaderBody {
            get {
                if (_loaderBody != BodyIndex.None) {
                    return _loaderBody;
                } else {
                    return _loaderBody = BodyCatalog.FindBodyIndex(BodyNameIndex.LoaderBody.ToString());
                }
            }
        }

        public static BodyIndex MageBody {
            get {
                if (_mageBody != BodyIndex.None) {
                    return _mageBody;
                } else {
                    return _mageBody = BodyCatalog.FindBodyIndex(BodyNameIndex.MageBody.ToString());
                }
            }
        }

        public static BodyIndex MercBody {
            get {
                if (_mercBody != BodyIndex.None) {
                    return _mercBody;
                } else {
                    return _mercBody = BodyCatalog.FindBodyIndex(BodyNameIndex.MercBody.ToString());
                }
            }
        }

        public static BodyIndex PathfinderBody {
            get {
                if (_pathfinderBody != BodyIndex.None) {
                    return _pathfinderBody;
                } else {
                    return _pathfinderBody = BodyCatalog.FindBodyIndex(BodyNameIndex.PathfinderBody.ToString());
                }
            }
        }

        public static BodyIndex RailgunnerBody {
            get {
                if (_railgunnerBody != BodyIndex.None) {
                    return _railgunnerBody;
                } else {
                    return _railgunnerBody = BodyCatalog.FindBodyIndex(BodyNameIndex.RailgunnerBody.ToString());
                }
            }
        }

        public static BodyIndex RedMistBody {
            get {
                if (_redMistBody != BodyIndex.None) {
                    return _redMistBody;
                } else {
                    return _redMistBody = BodyCatalog.FindBodyIndex(BodyNameIndex.RedMistBody.ToString());
                }
            }
        }

        public static BodyIndex RobPaladinBody {
            get {
                if (_robPaladinBody != BodyIndex.None) {
                    return _robPaladinBody;
                } else {
                    return _robPaladinBody = BodyCatalog.FindBodyIndex(BodyNameIndex.RobPaladinBody.ToString());
                }
            }
        }

        public static BodyIndex SniperClassicBody {
            get {
                if (_sniperClassicBody != BodyIndex.None) {
                    return _sniperClassicBody;
                } else {
                    return _sniperClassicBody = BodyCatalog.FindBodyIndex(BodyNameIndex.SniperClassicBody.ToString());
                }
            }
        }

        public static BodyIndex ToolbotBody {
            get {
                if (_toolbotBody != BodyIndex.None) {
                    return _toolbotBody;
                } else {
                    return _toolbotBody = BodyCatalog.FindBodyIndex(BodyNameIndex.ToolbotBody.ToString());
                }
            }
        }

        public static BodyIndex TreebotBody {
            get {
                if (_treebotBody != BodyIndex.None) {
                    return _treebotBody;
                } else {
                    return _treebotBody = BodyCatalog.FindBodyIndex(BodyNameIndex.TreebotBody.ToString());
                }
            }
        }

        public static BodyIndex VoidSurvivorBody {
            get {
                if (_voidSurvivorBody != BodyIndex.None) {
                    return _voidSurvivorBody;
                } else {
                    return _voidSurvivorBody = BodyCatalog.FindBodyIndex(BodyNameIndex.VoidSurvivorBody.ToString());
                }
            }
        }

        internal static Dictionary<int, BodyNameIndex> BodyIndexToNameIndex { get; } = new();

        public void ClearEventHandlers() {
        }

        public void SetEventHandlers() {
            RoR2Application.onLoad += Load;
        }

        private void Load() {
            RoR2Application.onLoad -= Load;
            BodyIndexToNameIndex.Clear();
            for (BodyNameIndex bodyNameIndex = BodyNameIndex.None + 1; bodyNameIndex < BodyNameIndex.Count; ++bodyNameIndex) {
                BodyIndexToNameIndex.Add((int)BodyCatalog.FindBodyIndex(bodyNameIndex.ToString()), bodyNameIndex);
            }
        }
    }
}