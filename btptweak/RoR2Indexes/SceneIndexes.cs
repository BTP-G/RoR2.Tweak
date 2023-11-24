using RoR2;
using UnityEngine;

namespace BtpTweak.RoR2Indexes {

    public static class SceneIndexes {

        static SceneIndexes() {
            if (Moon2 == SceneIndex.Invalid) {
                Debug.LogWarning("SceneIndexs not found!");
            }
            Debug.Log($"class {typeof(SceneIndexes).FullName} has been initialized.");
        }

        public static SceneIndex AncientLoft { get; } = SceneCatalog.FindSceneIndex("ancientloft");
        public static SceneIndex Arena { get; } = SceneCatalog.FindSceneIndex("arena");
        public static SceneIndex ArtifactWorld { get; } = SceneCatalog.FindSceneIndex("artifactworld");
        public static SceneIndex Bazaar { get; } = SceneCatalog.FindSceneIndex("bazaar");
        public static SceneIndex BlackBeach { get; } = SceneCatalog.FindSceneIndex("blackbeach");
        public static SceneIndex BlackBeach2 { get; } = SceneCatalog.FindSceneIndex("blackbeach2");
        public static SceneIndex BulwarksHaunt_GhostWave { get; } = SceneCatalog.FindSceneIndex("BulwarksHaunt_GhostWave");
        public static SceneIndex CrystalWorld { get; } = SceneCatalog.FindSceneIndex("crystalworld");
        public static SceneIndex DampCaveSimple { get; } = SceneCatalog.FindSceneIndex("dampcavesimple");
        public static SceneIndex DryBasin { get; } = SceneCatalog.FindSceneIndex("drybasin");
        public static SceneIndex EclipseWorld { get; } = SceneCatalog.FindSceneIndex("eclipseworld");
        public static SceneIndex FBLScene { get; } = SceneCatalog.FindSceneIndex("FBLScene");
        public static SceneIndex FoggySwamp { get; } = SceneCatalog.FindSceneIndex("foggyswamp");
        public static SceneIndex ForgottenHaven { get; } = SceneCatalog.FindSceneIndex("forgottenhaven");
        public static SceneIndex FrozenWall { get; } = SceneCatalog.FindSceneIndex("frozenwall");
        public static SceneIndex GoldShores { get; } = SceneCatalog.FindSceneIndex("goldshores");
        public static SceneIndex GolemPlains { get; } = SceneCatalog.FindSceneIndex("golemplains");
        public static SceneIndex GolemPlains2 { get; } = SceneCatalog.FindSceneIndex("golemplains2");
        public static SceneIndex GooLake { get; } = SceneCatalog.FindSceneIndex("goolake");
        public static SceneIndex InfiniteTowerWorld { get; } = SceneCatalog.FindSceneIndex("infinitetowerworld");
        public static SceneIndex Limbo { get; } = SceneCatalog.FindSceneIndex("limbo");
        public static SceneIndex Moon { get; } = SceneCatalog.FindSceneIndex("moon");
        public static SceneIndex Moon2 { get; } = SceneCatalog.FindSceneIndex("moon2");
        public static SceneIndex Mysteryspace { get; } = SceneCatalog.FindSceneIndex("mysteryspace");
        public static SceneIndex PromoRailGunner { get; } = SceneCatalog.FindSceneIndex("PromoRailGunner");
        public static SceneIndex PromoVoidSurvivor { get; } = SceneCatalog.FindSceneIndex("PromoVoidSurvivor");
        public static SceneIndex RootJungle { get; } = SceneCatalog.FindSceneIndex("rootjungle");
        public static SceneIndex Satellitescene { get; } = SceneCatalog.FindSceneIndex("satellitescene");
        public static SceneIndex Shipgraveyard { get; } = SceneCatalog.FindSceneIndex("shipgraveyard");
        public static SceneIndex Skymeadow { get; } = SceneCatalog.FindSceneIndex("skymeadow");
        public static SceneIndex Slumberingsatellite { get; } = SceneCatalog.FindSceneIndex("slumberingsatellite");
        public static SceneIndex SnowyForest { get; } = SceneCatalog.FindSceneIndex("snowyforest");
        public static SceneIndex Splash { get; } = SceneCatalog.FindSceneIndex("splash");
        public static SceneIndex SulfurPools { get; } = SceneCatalog.FindSceneIndex("sulfurpools");
        public static SceneIndex VoidRaid { get; } = SceneCatalog.FindSceneIndex("voidraid");
        public static SceneIndex VoidStage { get; } = SceneCatalog.FindSceneIndex("voidstage");
        public static SceneIndex WispGraveYard { get; } = SceneCatalog.FindSceneIndex("wispgraveyard");
    }
}