using RoR2;

namespace BtpTweak.IndexCollections {

    public static class BodyIndexCollection {
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
        }
    }
}