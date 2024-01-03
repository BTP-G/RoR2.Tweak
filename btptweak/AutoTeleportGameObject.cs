using RoR2;
using RoR2.Navigation;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    public class AutoTeleportGameObject : NetworkBehaviour {

        [SerializeField]
        private float waitingTime;

        public void SetTeleportWaitingTime(float newWaitingTime) => waitingTime = newWaitingTime;

        private void TeleportDroplet() {
            var spawnCard = ScriptableObject.CreateInstance<SpawnCard>();
            spawnCard.hullSize = HullClassification.Human;
            spawnCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
            spawnCard.prefab = AssetReferences.helperPrefab;
            var placementRule = new DirectorPlacementRule() {
                placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                position = transform.position,
            };
            var targetObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, placementRule, RoR2Application.rng));
            if (targetObject) {
                TeleportHelper.TeleportGameObject(gameObject, targetObject.transform.position + (5 * Vector3.up));
                Debug.Log("tp item back");
                Destroy(targetObject);
            }
            Destroy(spawnCard);
        }

        private void Update() {
            if ((waitingTime -= Time.deltaTime) < 0) {
                TeleportDroplet();
                enabled = false;
            }
        }
    }
}