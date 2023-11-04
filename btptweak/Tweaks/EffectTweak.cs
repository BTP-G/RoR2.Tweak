using BtpTweak.Utils;
using RoR2;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class EffectTweak : TweakBase<EffectTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
            On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
            On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool -= EffectManager_SpawnEffect_GameObject_EffectData_bool;
        }

        public void Load() {
            Object.Destroy(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/SimpleLightningStrikeImpact").transform.Find("Flash").gameObject);
            Object.Destroy(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact").transform.Find("Flash").gameObject);
            Object.Destroy(AssetReferences.affixWhiteExplosion.transform.Find("Flash, Blue").gameObject);
            Object.Destroy(AssetReferences.affixWhiteExplosion.transform.Find("Flash, White").gameObject);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Cleanse/CleanseEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Clover/CloverEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.5f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/ChainLightning/ChainLightningOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.05f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/GoldOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/CoinImpact.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/Critspark.prefab".LoadComponent<EffectComponent>().effectIndex, 0.5f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/CritsparkHeavy.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/DamageRejected.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Crowbar/ImpactCrowbar.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/EliteIce/AffixWhiteImpactEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/EliteLightning/LightningStakeNova.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/FireballsOnHit/FireMeatBallExplosion.prefab".LoadComponent<EffectComponent>().effectIndex, 0.001f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Heretic/HereticSpawnEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 12f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/NearbyDamageBonus/DiamondDamageBonusEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/StickyBomb/BehemothVFX.prefab".LoadComponent<EffectComponent>().effectIndex, 0.05f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/StunChanceOnHit/ImpactStunGrenade.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Thorns/RazorwireOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/ChainLightningVoid/VoidLightningOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/ChainLightningVoid/VoidLightningStrikeImpact.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/MissileVoid/MissileVoidOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/MissileVoid/VoidImpactEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.02f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/PermanentDebuffOnHit/PermanentDebuffEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect(EntityStates.Merc.Evis.hitEffectPrefab.GetComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect(HealthComponent.AssetReferences.bearEffectPrefab.GetComponent<EffectComponent>().effectIndex, 1f);
            EffectSpawnLimit.AddLimitToEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab.GetComponent<EffectComponent>().effectIndex, 1f);
            GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection.RemoveAll((temporaryVFX) => temporaryVFX.tempEffectPrefab.name == "VoidFogMildEffect");
        }

        private void EffectManager_SpawnEffect_GameObject_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_GameObject_EffectData_bool orig, GameObject effectPrefab, EffectData effectData, bool transmit) {
            EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);
            if (effectIndex != EffectIndex.Invalid) {
                if (!EffectSpawnLimit.IsSkipThisSpawn(effectIndex)) {
                    EffectManager.SpawnEffect(effectIndex, effectData, transmit);
                    if (ModConfig.是否开启特效生成日志.Value) {
                        Main.Logger.LogMessage("EffectName(特效名称) == " + effectPrefab?.name + ", EffectIndex(特效ID) == " + effectIndex);
                    }
                }
                return;
            }
            if (effectPrefab && !string.IsNullOrEmpty(effectPrefab.name)) {
                UnityEngine.Debug.LogError("Unable to SpawnEffect from prefab named '" + effectPrefab.name + "'");
                return;
            }
            UnityEngine.Debug.LogError(string.Format("Unable to SpawnEffect.  Is null? {0}.  Name = '{1}'.\n{2}", effectPrefab == null, effectPrefab?.name, new StackTrace()));
        }

        public class EffectSpawnLimit {
            private static readonly Dictionary<EffectIndex, EffectSpawnLimit> _EffectIndexToEffectSpawnLimit = new();
            private float LastSpawnTime_;
            private float spawnInterval;

            private EffectSpawnLimit(float interval) {
                spawnInterval = interval;
            }

            public static void AddLimitToEffect(EffectIndex effectIndex, float interval) {
                if (effectIndex == EffectIndex.Invalid) {
                    Main.Logger.LogWarning($"EffectSpawnLimit：effectIndex(ID) Invalid!");
                    return;
                }
                if (_EffectIndexToEffectSpawnLimit.TryGetValue(effectIndex, out var spawnLimit)) {
                    if (interval < 0) {
                        Main.Logger.LogInfo($"EffectSpawnLimit：remove {EffectCatalog.GetEffectDef(effectIndex)?.prefabName ?? "null"} spawn interval");
                        _EffectIndexToEffectSpawnLimit.Remove(effectIndex);
                    } else {
                        Main.Logger.LogInfo($"EffectSpawnLimit：{EffectCatalog.GetEffectDef(effectIndex)?.prefabName ?? "null"} 间隔被覆盖 old interval == {spawnLimit.spawnInterval} to new interval == {interval}");
                        spawnLimit.spawnInterval = interval;
                    }
                } else if (interval >= 0) {
                    _EffectIndexToEffectSpawnLimit.Add(effectIndex, new(interval));
                    Main.Logger.LogInfo($"EffectSpawnLimit：{EffectCatalog.GetEffectDef(effectIndex)?.prefabName ?? "null"} 添加生成间隔 interval == {interval}");
                }
            }

            public static bool IsSkipThisSpawn(EffectIndex effectIndex) {
                if (ModConfig.关闭所有特效.Value) {
                    return true;
                }
                if (_EffectIndexToEffectSpawnLimit.TryGetValue(effectIndex, out var spawnLimit)) {
                    if ((Time.fixedTime - spawnLimit.LastSpawnTime_) < spawnLimit.spawnInterval) {
                        return true;
                    }
                    spawnLimit.LastSpawnTime_ = Time.fixedTime;
                }
                return false;
            }
        }
    }
}