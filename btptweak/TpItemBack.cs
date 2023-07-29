using RoR2;
using RoR2.Navigation;
using UnityEngine;

namespace BtpTweak {

    internal class TpItemBack {

        public static void AddHook() {
            Physics.IgnoreLayerCollision(15, 13, false);
            On.RoR2.MapZone.TryZoneStart += MapZone_TryZoneStart;
        }

        private static bool ColliderPickup(Collider collider) {
            if (collider.GetComponent<PickupDropletController>()
                || collider.GetComponent<GenericPickupController>()
                || collider.GetComponent<PickupPickerController>()) {
                return true;
            } else {
                return false;
            }
        }

        private static void MapZone_TryZoneStart(On.RoR2.MapZone.orig_TryZoneStart orig, MapZone self, Collider other) {
            orig(self, other);
            if (self.zoneType == MapZone.ZoneType.OutOfBounds) {
                if (ColliderPickup(other)) {
                    SpawnCard spawnCard = ScriptableObject.CreateInstance<SpawnCard>();
                    spawnCard.hullSize = HullClassification.Human;
                    spawnCard.nodeGraphType = MapNodeGroup.GraphType.Ground;
                    spawnCard.prefab = LegacyResourcesAPI.Load<GameObject>("SpawnCards/HelperPrefab");
                    DirectorPlacementRule placementRule = new() {
                        placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                        position = other.transform.position
                    };
                    GameObject gameObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, placementRule, RoR2Application.rng));
                    if (gameObject) {
                        TeleportHelper.TeleportGameObject(other.gameObject, gameObject.transform.position);
                        Debug.Log("tp item back");
                        Object.Destroy(gameObject);
                    }
                    Object.Destroy(spawnCard);
                }
            }
        }
    }
}