using RoR2;
using UnityEngine;

namespace BtpTweak.RoR2Indexes {

    public static class SceneIndexes {
        public static SceneIndex AncientLoft { get; private set; }

        public static SceneIndex Arena { get; private set; }

        public static SceneIndex ArtifactWorld { get; private set; }

        public static SceneIndex Bazaar { get; private set; }

        public static SceneIndex BlackBeach { get; private set; }

        public static SceneIndex BlackBeach2 { get; private set; }

        public static SceneIndex BulwarksHaunt_GhostWave { get; private set; }

        public static SceneIndex CrystalWorld { get; private set; }

        public static SceneIndex DampCaveSimple { get; private set; }

        public static SceneIndex DryBasin { get; private set; }

        public static SceneIndex EclipseWorld { get; private set; }

        public static SceneIndex FBLScene { get; private set; }

        public static SceneIndex FoggySwamp { get; private set; }

        public static SceneIndex ForgottenHaven { get; private set; }

        public static SceneIndex FrozenWall { get; private set; }

        public static SceneIndex GoldShores { get; private set; }

        public static SceneIndex GolemPlains { get; private set; }

        public static SceneIndex GolemPlains2 { get; private set; }

        public static SceneIndex GooLake { get; private set; }

        public static SceneIndex InfiniteTowerWorld { get; private set; }

        public static SceneIndex Limbo { get; private set; }

        public static SceneIndex Moon { get; private set; }

        public static SceneIndex Moon2 { get; private set; }

        public static SceneIndex Mysteryspace { get; private set; }

        public static SceneIndex PromoRailGunner { get; private set; }

        public static SceneIndex PromoVoidSurvivor { get; private set; }

        public static SceneIndex RootJungle { get; private set; }

        public static SceneIndex Satellitescene { get; private set; }

        public static SceneIndex Shipgraveyard { get; private set; }

        public static SceneIndex Skymeadow { get; private set; }

        public static SceneIndex Slumberingsatellite { get; private set; }

        public static SceneIndex SnowyForest { get; private set; }

        public static SceneIndex Splash { get; private set; }

        public static SceneIndex SulfurPools { get; private set; }

        public static SceneIndex VoidRaid { get; private set; }

        public static SceneIndex VoidStage { get; private set; }

        public static SceneIndex WispGraveYard { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            On.RoR2.SceneCatalog.SetSceneDefs += SceneCatalog_SetSceneDefs;
        }

        private static void SceneCatalog_SetSceneDefs(On.RoR2.SceneCatalog.orig_SetSceneDefs orig, SceneDef[] newSceneDefs) {
            orig(newSceneDefs);
            AncientLoft = SceneCatalog.FindSceneIndex("ancientloft");
            Arena = SceneCatalog.FindSceneIndex("arena");
            ArtifactWorld = SceneCatalog.FindSceneIndex("artifactworld");
            Bazaar = SceneCatalog.FindSceneIndex("bazaar");
            BlackBeach = SceneCatalog.FindSceneIndex("blackbeach");
            BlackBeach2 = SceneCatalog.FindSceneIndex("blackbeach2");
            BulwarksHaunt_GhostWave = SceneCatalog.FindSceneIndex("BulwarksHaunt_GhostWave");
            CrystalWorld = SceneCatalog.FindSceneIndex("crystalworld");
            DampCaveSimple = SceneCatalog.FindSceneIndex("dampcavesimple");
            DryBasin = SceneCatalog.FindSceneIndex("drybasin");
            EclipseWorld = SceneCatalog.FindSceneIndex("eclipseworld");
            FBLScene = SceneCatalog.FindSceneIndex("FBLScene");
            FoggySwamp = SceneCatalog.FindSceneIndex("foggyswamp");
            ForgottenHaven = SceneCatalog.FindSceneIndex("forgottenhaven");
            FrozenWall = SceneCatalog.FindSceneIndex("frozenwall");
            GoldShores = SceneCatalog.FindSceneIndex("goldshores");
            GolemPlains = SceneCatalog.FindSceneIndex("golemplains");
            GolemPlains2 = SceneCatalog.FindSceneIndex("golemplains2");
            GooLake = SceneCatalog.FindSceneIndex("goolake");
            InfiniteTowerWorld = SceneCatalog.FindSceneIndex("infinitetowerworld");
            Limbo = SceneCatalog.FindSceneIndex("limbo");
            Moon = SceneCatalog.FindSceneIndex("moon");
            Moon2 = SceneCatalog.FindSceneIndex("moon2");
            Mysteryspace = SceneCatalog.FindSceneIndex("mysteryspace");
            PromoRailGunner = SceneCatalog.FindSceneIndex("PromoRailGunner");
            PromoVoidSurvivor = SceneCatalog.FindSceneIndex("PromoVoidSurvivor");
            RootJungle = SceneCatalog.FindSceneIndex("rootjungle");
            Satellitescene = SceneCatalog.FindSceneIndex("satellitescene");
            Shipgraveyard = SceneCatalog.FindSceneIndex("shipgraveyard");
            Skymeadow = SceneCatalog.FindSceneIndex("skymeadow");
            Slumberingsatellite = SceneCatalog.FindSceneIndex("slumberingsatellite");
            SnowyForest = SceneCatalog.FindSceneIndex("snowyforest");
            Splash = SceneCatalog.FindSceneIndex("splash");
            SulfurPools = SceneCatalog.FindSceneIndex("sulfurpools");
            VoidRaid = SceneCatalog.FindSceneIndex("voidraid");
            VoidStage = SceneCatalog.FindSceneIndex("voidstage");
            WispGraveYard = SceneCatalog.FindSceneIndex("wispgraveyard");
        }
    }
}