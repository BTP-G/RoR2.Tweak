using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    public static class RunInfo {
        public static bool 位于月球 { get; private set; }
        public static bool 位于隔间 { get; private set; }
        public static bool 位于天文馆 { get; private set; }
        public static bool 位于时之墓 { get; private set; }
        public static bool 位于月球商店 { get; private set; }
        public static bool 是否选择造物难度 { get; private set; }
        public static bool 造物主的试炼 { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            Run.onRunStartGlobal += OnRunStart;
            SceneCatalog.onMostRecentSceneDefChanged += OnMostRecentSceneDefChanged;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += OnBrotherTrueDeath;
        }

        private static void OnBrotherTrueDeath(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (是否选择造物难度 && !造物主的试炼) {
                造物主的试炼 = true;
                if (NetworkServer.active) {
                    ChatMessage.Send("世界不再是你熟悉的那样！！！".ToRainbowWavy());
                }
            }
        }

        private static void OnRunStart(Run run) {
            造物主的试炼 = false;
            是否选择造物难度 = run.selectedDifficulty == BtpContent.Difficulties.造物索引;
        }

        private static void OnMostRecentSceneDefChanged(SceneDef mostRecentSceneDef) {
            var mostRecentSceneIndex = mostRecentSceneDef.sceneDefIndex;
            位于月球 = mostRecentSceneIndex == SceneIndexes.Moon || mostRecentSceneIndex == SceneIndexes.Moon2;
            位于隔间 = mostRecentSceneIndex == SceneIndexes.Arena;
            位于天文馆 = mostRecentSceneIndex == SceneIndexes.VoidRaid;
            位于时之墓 = mostRecentSceneIndex == SceneIndexes.BulwarksHaunt_GhostWave;
            位于月球商店 = mostRecentSceneIndex == SceneIndexes.Bazaar;
        }
    }
}