using BtpTweak.Utils;
using ConfigurableDifficulty;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    public static class RunInfo {

        static RunInfo() {
            Run.onRunStartGlobal += UpdateInfo;
            UpdateInfo(Run.instance);
            Stage.onStageStartGlobal += UpdateInfo;
            UpdateInfo(Stage.instance);
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += UpdateInfo;
            Debug.Log($"class {typeof(RunInfo).FullName} has been initialized.");
        }

        public static SceneIndex CurrentSceneIndex { get; private set; }
        public static bool 是否选择造物难度 { get; private set; }
        public static bool 往日不再 { get; private set; }

        private static void UpdateInfo(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (是否选择造物难度 && !往日不再) {
                往日不再 = true;
                if (NetworkServer.active) {
                    ChatMessage.Send("世界不再是你熟悉的那样！！！".ToLunar());
                }
            }
        }

        private static void UpdateInfo(Run run) {
            往日不再 = false;
            是否选择造物难度 = run?.selectedDifficulty == ConfigurableDifficultyPlugin.configurableDifficultyIndex;
        }

        private static void UpdateInfo(Stage currentStage) {
            CurrentSceneIndex = currentStage?.sceneDef.sceneDefIndex ?? SceneIndex.Invalid;
        }
    }
}