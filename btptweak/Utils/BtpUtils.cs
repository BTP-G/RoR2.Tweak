using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Utils {

    public static class BtpUtils {

        public static void SpawnVoidDeathBomb(in Vector3 position) => ProjectileManager.instance.FireProjectile(new FireProjectileInfo() {
            projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile,
            position = position,
            rotation = Quaternion.identity,
        });

        public static float 简单逼近(float 基数, float 半数, float 目标) => 目标 * 基数 / (基数 + 半数);

        public static bool SpawnLunarPortal(Vector3 position) {
            var directorPlacementRule = new DirectorPlacementRule {
                minDistance = 0,
                maxDistance = float.MaxValue,
                placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                position = position
            };
            var directorSpawnRequest = new DirectorSpawnRequest(InteractableSpawnCardPaths.iscShopPortal.Load<SpawnCard>(), directorPlacementRule, Run.instance.runRNG);
            var gameObject = DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
            if (gameObject) {
                NetworkServer.Spawn(gameObject);
                return true;
            }
            return false;
        }
    }
}