using RoR2;
using RoR2.Navigation;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Components {

    public class AutoTeleportGameObject : MonoBehaviour {
        private float _teleportTime;

        private void Awake() {
            enabled = NetworkServer.active;
        }

        private void OnEnable() {
            if(RunInfo.位于月球商店) {
                _teleportTime = Time.time + Settings.商店物品传送时间.Value;
            } else {
                _teleportTime = Time.time + Settings.物品传送时间.Value;
            }
        }

        private void Update() {
            if (Time.time > _teleportTime) {
                TeleportDroplet();
                enabled = false;
            }
        }

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
                TeleportHelper.TeleportGameObject(gameObject, targetObject.transform.position + new Vector3(0, 5, 0));
                Debug.Log("tp item back");
                Destroy(targetObject);
            }
            Destroy(spawnCard);
        }
    }
}