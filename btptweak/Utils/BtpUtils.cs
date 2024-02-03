using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using TPDespair.ZetAspects;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Utils {

    public static class BtpUtils {

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

        public static float 简单逼近(float 基数, float 半数, float 目标) => 目标 * 基数 / (基数 + 半数);

        public static float 简单逼近1(float 基数, float 半数) => 基数 / (基数 + 半数);

        public static void TryApplyTag(this ItemDef itemDef, ItemTag itemTag) {
            if (itemDef && itemDef.DoesNotContainTag(itemTag)) {
                R2API.ItemAPI.ApplyTagToItem(itemTag, itemDef);
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
                    Main.Logger.LogMessage($"set {configuration.targetType.assemblyQualifiedName} field '{serializedFields[i].fieldName}' : oldValue('{serializedFields[i].fieldValue.stringValue}') => newValue('{newValue}')");
                    serializedFields[i].fieldValue.stringValue = newValue;
                    fieldNameToNewFieldValues.Remove(serializedFields[i].fieldName);
                }
            }
            if (fieldNameToNewFieldValues.Count != 0) {
                Main.Logger.LogError($"set {configuration.targetType.assemblyQualifiedName} fields '{string.Join("|", fieldNameToNewFieldValues.Keys)}' not found!");
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
            Main.Logger.LogError($"set {configuration.targetType.assemblyQualifiedName} field '{fieldName}' not found!");
        }

        public static void InflictTotalDamageWithinDuration(this ref InflictDotInfo dotInfo, CharacterBody attackerBody = null) {
            if (dotInfo.totalDamage.HasValue) {
                var dotDef = DotController.GetDotDef(dotInfo.dotIndex);
                if (dotInfo.duration > 0 && (attackerBody || dotInfo.attackerObject.TryGetComponent(out attackerBody))) {
                    dotInfo.damageMultiplier = dotInfo.totalDamage.Value / (attackerBody.damage * dotDef.damageCoefficient * Mathf.Ceil(dotInfo.duration / dotDef.interval));
                    dotInfo.totalDamage = null;
                }
            }
        }
    }
}