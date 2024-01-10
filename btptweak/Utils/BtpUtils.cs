using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
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

        public static void Set(this EntityStateConfiguration configuration, Dictionary<string, string> fieldNameToNewFieldValues) {
            var serializedFields = configuration.serializedFieldsCollection.serializedFields;
            for (int i = 0; i < serializedFields.Length; ++i) {
                if (fieldNameToNewFieldValues.TryGetValue(serializedFields[i].fieldName, out var newValue)) {
                    Main.Logger.LogMessage($"set {configuration.targetType.assemblyQualifiedName} field '{serializedFields[i].fieldName}' : oldValue('{serializedFields[i].fieldValue.stringValue}') => newValue('{newValue}')");
                    serializedFields[i].fieldValue.stringValue = newValue;
                    fieldNameToNewFieldValues.Remove(serializedFields[i].fieldName);
                }
            }
            if (fieldNameToNewFieldValues.Count != 0) {
                Main.Logger.LogError($"set fields '{string.Join("|", fieldNameToNewFieldValues.Keys)}' not found!");
                fieldNameToNewFieldValues.Clear();
            }
        }

        public static void Set(this EntityStateConfiguration configuration, string fieldName, string newFieldValue) {
            var serializedFields = configuration.serializedFieldsCollection.serializedFields;
            for (int i = 0; i < serializedFields.Length; ++i) {
                if (serializedFields[i].fieldName == fieldName) {
                    Main.Logger.LogMessage($"set {configuration.targetType.assemblyQualifiedName} field '{serializedFields[i].fieldName}' : oldValue('{serializedFields[i].fieldValue.stringValue}') => newValue('{newFieldValue}')");
                    serializedFields[i].fieldValue.stringValue = newFieldValue;
                    return;
                }
            }
            Main.Logger.LogError($"set field '{fieldName}' not found!");
        }
    }
}