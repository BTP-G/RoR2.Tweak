using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using TPDespair.ZetAspects;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Utils {

    public static class Util2 {

        public static float GetAspectCount(this Inventory inventory, BuffIndex aspectBuffIndex) {
            var result = 0f;
            if (Catalog.buffToEquip.TryGetValue(aspectBuffIndex, out var equipmentIndex)) {
                if (inventory.currentEquipmentIndex == equipmentIndex) {
                    ++result;
                }
                if (inventory.alternateEquipmentIndex == equipmentIndex) {
                    ++result;
                }
                result *= Mathf.Max(1f, Configuration.AspectEquipmentEffect.Value);
            }
            if (Catalog.buffToItem.TryGetValue(aspectBuffIndex, out var itemIndex)) {
                result += inventory.GetItemCount(itemIndex);
            }
            return result;
        }

        public static bool TryGetAspectStackMagnitude(this CharacterBody body, BuffIndex aspectBuffIndex, out float stack) {
            if (body.HasBuff(aspectBuffIndex)) {
                stack = Mathf.Max(1f, body.inventory.GetAspectCount(aspectBuffIndex));
                return true;
            } else {
                stack = 0f;
                return false;
            }
        }

        public static void SpawnVoidDeathBomb(in Vector3 position) => ProjectileManager.instance.FireProjectile(new FireProjectileInfo() {
            projectilePrefab = EntityStates.NullifierMonster.DeathState.deathBombProjectile,
            position = position,
            rotation = Quaternion.identity,
        });

        public static float CloseTo(float 基数, float 半数, float 目标) => 目标 * 基数 / (基数 + 半数);

        public static float CloseTo1(float 基数, float 半数) => 基数 / (基数 + 半数);

        public static void TryApplyTag(this ItemDef itemDef, ItemTag itemTag) {
            if (itemDef != null && itemDef.DoesNotContainTag(itemTag)) {
                R2API.ItemAPI.ApplyTagToItem(itemTag, itemDef);
            }
        }
        public static void TryRemoveTag(this ItemDef itemDef, ItemTag itemTag) {
            if (itemDef != null && itemDef.ContainsTag(itemTag)) {
                var set = itemDef.tags.ToHashSet();
                set.Remove(itemTag);
                itemDef.tags = [.. set];
            }
        }

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
                    LogExtensions.LogMessage($"set {configuration.targetType.assemblyQualifiedName} field '{serializedFields[i].fieldName}' : oldValue('{serializedFields[i].fieldValue.stringValue}') => newValue('{newValue}')");
                    serializedFields[i].fieldValue.stringValue = newValue;
                    fieldNameToNewFieldValues.Remove(serializedFields[i].fieldName);
                }
            }
            if (fieldNameToNewFieldValues.Count != 0) {
                LogExtensions.LogError($"set {configuration.targetType.assemblyQualifiedName} fields '{string.Join("|", fieldNameToNewFieldValues.Keys)}' not found!");
                fieldNameToNewFieldValues.Clear();
            }
        }

        public static void Set(this EntityStateConfiguration configuration, string fieldName, string newFieldValue) {
            var serializedFields = configuration.serializedFieldsCollection.serializedFields;
            for (int i = 0; i < serializedFields.Length; ++i) {
                if (serializedFields[i].fieldName == fieldName) {
                    LogExtensions.LogMessage($"set {configuration.targetType.assemblyQualifiedName} field '{serializedFields[i].fieldName}' : oldValue('{serializedFields[i].fieldValue.stringValue}') => newValue('{newFieldValue}')");
                    serializedFields[i].fieldValue.stringValue = newFieldValue;
                    return;
                }
            }
            LogExtensions.LogError($"set {configuration.targetType.assemblyQualifiedName} field '{fieldName}' not found!");
        }
    }
}