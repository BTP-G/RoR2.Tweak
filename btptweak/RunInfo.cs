using BtpTweak.Utils;
using ConfigurableDifficulty;
using R2API.Utils;
using RoR2;
using UnityEngine.Networking;

namespace BtpTweak {

    public sealed class RunInfo : IEventHandlers {
        public static SceneIndex CurrentSceneIndex { get; private set; }
        public static bool 是否选择造物难度 { get; private set; }
        public static bool 往日不再 { get; private set; }

        public void SetEventHandlers() {
            Run.onRunStartGlobal += UpdateInfo;
            Stage.onStageStartGlobal += UpdateInfo;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += UpdateInfo;
        }

        public void ClearEventHandlers() {
            Run.onRunStartGlobal -= UpdateInfo;
            Stage.onStageStartGlobal -= UpdateInfo;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= UpdateInfo;
        }

        private void UpdateInfo(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (是否选择造物难度 && !往日不再) {
                往日不再 = true;
                if (NetworkServer.active) {
                    ChatMessage.Send("世界不再是你熟悉的那样！！！".ToLunar());
                }
            }
        }

        private void UpdateInfo(Run run) {
            往日不再 = false;
            是否选择造物难度 = run.selectedDifficulty == ConfigurableDifficultyPlugin.configurableDifficultyIndex;
        }

        private void UpdateInfo(Stage currentStage) {
            CurrentSceneIndex = currentStage.sceneDef.sceneDefIndex;
        }
    }
}