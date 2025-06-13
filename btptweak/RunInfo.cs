using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using R2API.Utils;
using RoR2;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin {

    public static class RunInfo {
        public static bool 位于月球 { get; private set; }
        public static bool 位于隔间 { get; private set; }
        public static bool 位于天文馆 { get; private set; }
        public static bool 位于时之墓 { get; private set; }
        public static bool 位于月球商店 { get; private set; }
        public static bool 已选择造物难度 { get; private set; }
        public static bool 造物主的试炼 { get; private set; }

        [RuntimeInitializeOnLoadMethod]
        private static void Init() {
            Run.onRunSetRuleBookGlobal += OnRunSetRuleBookGlobal;
            SceneCatalog.onMostRecentSceneDefChanged += OnMostRecentSceneDefChanged;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += OnBrotherTrueDeath;
            ProperSaveSupport.AddSaveDataType<SaveData>();
        }

        private static void OnRunSetRuleBookGlobal(Run run, RuleBook ruleBook) {
            造物主的试炼 = false;
            已选择造物难度 = run.selectedDifficulty == Content.Difficulties.造物索引;
        }

        private static void OnBrotherTrueDeath(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (已选择造物难度 && !造物主的试炼) {
                造物主的试炼 = true;
                if (NetworkServer.active) {
                    ChatMessage.Send("世界不再是你熟悉的那样！！！".ToRainbowWavy());
                }
            }
        }

        private static void OnMostRecentSceneDefChanged(SceneDef mostRecentSceneDef) {
            var mostRecentSceneIndex = mostRecentSceneDef.sceneDefIndex;
            位于月球 = mostRecentSceneIndex == SceneIndexes.Moon || mostRecentSceneIndex == SceneIndexes.Moon2;
            位于隔间 = mostRecentSceneIndex == SceneIndexes.Arena;
            位于天文馆 = mostRecentSceneIndex == SceneIndexes.VoidRaid;
            位于时之墓 = mostRecentSceneIndex == SceneIndexes.BulwarksHaunt_GhostWave;
            位于月球商店 = mostRecentSceneIndex == SceneIndexes.Bazaar;
        }

        private struct SaveData : ISaveData {

            [DataMember(Name = "m")]
            public bool 位于月球;

            [DataMember(Name = "a")]
            public bool 位于隔间;

            [DataMember(Name = "v")]
            public bool 位于天文馆;

            [DataMember(Name = "t")]
            public bool 位于时之墓;

            [DataMember(Name = "b")]
            public bool 位于月球商店;

            [DataMember(Name = "c")]
            public bool 是否选择造物难度;

            [DataMember(Name = "d")]
            public bool 造物主的试炼;

            readonly void ISaveData.LoadData() {
                RunInfo.位于月球 = 位于月球;
                RunInfo.位于隔间 = 位于隔间;
                RunInfo.位于天文馆 = 位于天文馆;
                RunInfo.位于时之墓 = 位于时之墓;
                RunInfo.位于月球商店 = 位于月球商店;
                已选择造物难度 = 是否选择造物难度;
                RunInfo.造物主的试炼 = 造物主的试炼;
            }

            void ISaveData.SaveData() {
                位于月球 = RunInfo.位于月球;
                位于隔间 = RunInfo.位于隔间;
                位于天文馆 = RunInfo.位于天文馆;
                位于时之墓 = RunInfo.位于时之墓;
                位于月球商店 = RunInfo.位于月球商店;
                是否选择造物难度 = 已选择造物难度;
                造物主的试炼 = RunInfo.造物主的试炼;
            }
        }
    }
}