using RoR2;
using System.Collections.Generic;

namespace BtpTweak.IndexCollections {

    public static class BodyIndexCollection {
        public static readonly Dictionary<BodyIndex, BodyNameIndex> BodyIndexToNameIndex = new();
        public static BodyIndex ArbiterBody;
        public static BodyIndex Bandit2Body;
        public static BodyIndex BrotherBody;
        public static BodyIndex BrotherHurtBody;
        public static BodyIndex CaptainBody;
        public static BodyIndex CHEF;
        public static BodyIndex CommandoBody;
        public static BodyIndex CrocoBody;
        public static BodyIndex EngiBody;
        public static BodyIndex EngiTurretBody;
        public static BodyIndex EngiWalkerTurretBody;
        public static BodyIndex EquipmentDroneBody;
        public static BodyIndex HereticBody;
        public static BodyIndex HuntressBody;
        public static BodyIndex LoaderBody;
        public static BodyIndex MageBody;
        public static BodyIndex MercBody;
        public static BodyIndex PathfinderBody;
        public static BodyIndex RailgunnerBody;
        public static BodyIndex RedMistBody;
        public static BodyIndex RobPaladinBody;
        public static BodyIndex SniperClassicBody;
        public static BodyIndex ToolbotBody;
        public static BodyIndex TreebotBody;
        public static BodyIndex VoidSurvivorBody;

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

        public static void LoadAllBodyIndexes() {
            ArbiterBody = BodyCatalog.FindBodyIndex("ArbiterBody");
            Bandit2Body = BodyCatalog.FindBodyIndex("Bandit2Body");
            BrotherBody = BodyCatalog.FindBodyIndex("BrotherBody");
            BrotherHurtBody = BodyCatalog.FindBodyIndex("BrotherHurtBody");
            CaptainBody = BodyCatalog.FindBodyIndex("CaptainBody");
            CHEF = BodyCatalog.FindBodyIndex("CHEF");
            CommandoBody = BodyCatalog.FindBodyIndex("CommandoBody");
            CrocoBody = BodyCatalog.FindBodyIndex("CrocoBody");
            EngiBody = BodyCatalog.FindBodyIndex("EngiBody");
            EngiTurretBody = BodyCatalog.FindBodyIndex("EngiTurretBody");
            EngiWalkerTurretBody = BodyCatalog.FindBodyIndex("EngiWalkerTurretBody");
            EquipmentDroneBody = BodyCatalog.FindBodyIndex("EquipmentDroneBody");
            HereticBody = BodyCatalog.FindBodyIndex("HereticBody");
            HuntressBody = BodyCatalog.FindBodyIndex("HuntressBody");
            LoaderBody = BodyCatalog.FindBodyIndex("LoaderBody");
            MageBody = BodyCatalog.FindBodyIndex("MageBody");
            MercBody = BodyCatalog.FindBodyIndex("MercBody");
            PathfinderBody = BodyCatalog.FindBodyIndex("PathfinderBody");
            RailgunnerBody = BodyCatalog.FindBodyIndex("RailgunnerBody");
            RedMistBody = BodyCatalog.FindBodyIndex("RedMistBody");
            RobPaladinBody = BodyCatalog.FindBodyIndex("RobPaladinBody");
            SniperClassicBody = BodyCatalog.FindBodyIndex("SniperClassicBody");
            ToolbotBody = BodyCatalog.FindBodyIndex("ToolbotBody");
            TreebotBody = BodyCatalog.FindBodyIndex("TreebotBody");
            VoidSurvivorBody = BodyCatalog.FindBodyIndex("VoidSurvivorBody");
            for (BodyNameIndex bodyNameIndex = BodyNameIndex.None + 1; bodyNameIndex < BodyNameIndex.Count; ++bodyNameIndex) {
                BodyIndexToNameIndex.Add(BodyCatalog.FindBodyIndex(bodyNameIndex.ToString()), bodyNameIndex);
            }
        }
    }
}