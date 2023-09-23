using RoR2;
using RoR2.Navigation;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    public class AutoTeleportGameObject : NetworkBehaviour {
        public float waitingTime;

        public void SetTeleportWaitingTime(float newWaitingTime) => waitingTime = newWaitingTime;

        public void Start() {
            enabled = SceneCatalog.GetSceneDefForCurrentScene().sceneType != SceneType.Intermission;
        }

        public void TeleportDroplet() {
            SpawnCard spawnCard = ScriptableObject.CreateInstance<SpawnCard>();
            spawnCard.hullSize = HullClassification.Human;
            spawnCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
            spawnCard.prefab = LegacyResourcesAPI.Load<GameObject>("SpawnCards/HelperPrefab");
            DirectorPlacementRule placementRule = new() {
                placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                position = transform.position
            };
            GameObject targetObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, placementRule, RoR2Application.rng));
            if (targetObject) {
                TeleportHelper.TeleportGameObject(gameObject, targetObject.transform.position + (5 * Vector3.up));
                Debug.Log("tp item");
                Destroy(targetObject);
            }
            Destroy(spawnCard);
        }

        public void Update() {
            if ((waitingTime -= Time.deltaTime) < 0) {
                TeleportDroplet();
                enabled = false;
            }
        }
    }
}