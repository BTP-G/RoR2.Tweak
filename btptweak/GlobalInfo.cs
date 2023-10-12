using RoR2;

namespace BtpTweak {

    public static class GlobalInfo {
        public static bool 是否选择造物难度;
        public static bool 往日不再;
        public static SceneIndex CurrentSceneIndex { get; private set; }

        public static void UpdateCurrentSceneIndex(SceneDef sceneDef) {
            CurrentSceneIndex = sceneDef.sceneDefIndex;
        }
    }
}