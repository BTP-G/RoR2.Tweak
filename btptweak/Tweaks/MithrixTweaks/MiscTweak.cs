using BtpTweak.RoR2Indexes;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.MithrixTweaks {

    internal class MiscTweak : TweakBase<MiscTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            SceneCatalog.onMostRecentSceneDefChanged += SceneCatalog_onMostRecentSceneDefChanged;
        }

        private void SceneCatalog_onMostRecentSceneDefChanged(SceneDef sceneDef) {
            if (sceneDef.sceneDefIndex == SceneIndexes.Moon2) {
                ArenaSetup();
            }
        }

        private void ArenaSetup() {
            // -88.4849 491.488 -0.3325 center orb thing
            GameObject arenaHolder = GameObject.Find("HOLDER: Final Arena");
            if (arenaHolder) {
                // Disable inner arena pillars and boulders
                Transform innerPillars = arenaHolder.transform.GetChild(0);
                innerPillars.gameObject.SetActive(false);
                Transform arenaBoulders = arenaHolder.transform.GetChild(6);
                arenaBoulders.gameObject.SetActive(false);
                // Add outer pillars and arches
                Transform outerPillars = arenaHolder.transform.GetChild(1);
                for (int i = 0; i < 8; ++i) {
                    outerPillars.GetChild(i).gameObject.SetActive(true);
                }
                for (int i = 8; i < 12; ++i) {
                    Transform weakPillar = outerPillars.GetChild(i);
                    for (int idx = 0; idx < 4; ++idx) {
                        weakPillar.GetChild(idx).gameObject.SetActive(true);
                    }
                }
            }
            // Activate the unused throne
            GameObject sceneInfo = GameObject.Find("SceneInfo");
            if (sceneInfo) {
                Transform missionController = sceneInfo.transform.GetChild(0);
                GameObject phase1Throne = missionController.transform.GetChild(3).GetChild(0).GetChild(3).gameObject;
                phase1Throne.SetActive(true);
            }
        }
    }
}