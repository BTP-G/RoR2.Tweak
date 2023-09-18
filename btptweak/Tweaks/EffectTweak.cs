using BtpTweak.Utils;
using RoR2;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class EffectTweak : TweakBase {

        public class EffectSpawnLimit {
            private static readonly Dictionary<EffectIndex, EffectSpawnLimit> EffectIndexToEffectSpawnLimit_ = new();
            private float spawnInterval;
            private float LastSpawnTime_;

            private EffectSpawnLimit(float interval) {
                spawnInterval = interval;
            }

            public static void AddLimitToEffect(EffectIndex effectIndex, float interval) {
                if (EffectIndexToEffectSpawnLimit_.TryGetValue(effectIndex, out var spawnLimit)) {
                    Main.logger_.LogWarning($"EffectSpawnLimit：{effectIndex}被覆盖 old interval == {spawnLimit.spawnInterval} to new interval == {interval}");
                    spawnLimit.spawnInterval = interval;
                } else {
                    EffectIndexToEffectSpawnLimit_.Add(effectIndex, new(interval));
                }
            }

            public static bool IsSkipThisSpawn(EffectIndex effectIndex) {
                if (EffectIndexToEffectSpawnLimit_.TryGetValue(effectIndex, out var spawnLimit)) {
                    if ((Time.fixedTime - spawnLimit.LastSpawnTime_) < spawnLimit.spawnInterval) {
                        return true;
                    }
                    spawnLimit.LastSpawnTime_ = Time.fixedTime;
                }
                return false;
            }
        }

        public override void Load() {
            //======
            //EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Lightning/LightningStrikeImpact.prefab".LoadComponent<EffectComponent>().effectIndex, 0.02f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Cleanse/CleanseEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Clover/CloverEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.5f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/GoldOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/CoinImpact.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/Critspark.prefab".LoadComponent<EffectComponent>().effectIndex, 0.5f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/CritsparkHeavy.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/DamageRejected.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Crowbar/ImpactCrowbar.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/EliteIce/AffixWhiteImpactEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/EliteLightning/LightningStakeNova.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/FireballsOnHit/FireMeatBallExplosion.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Heretic/HereticSpawnEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 12f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/NearbyDamageBonus/DiamondDamageBonusEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/StickyBomb/BehemothVFX.prefab".LoadComponent<EffectComponent>().effectIndex, 0.05f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/Base/Thorns/RazorwireOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/ChainLightningVoid/VoidLightningOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/ChainLightningVoid/VoidLightningStrikeImpact.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/MissileVoid/MissileVoidOrbEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/MissileVoid/VoidImpactEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect("RoR2/DLC1/PermanentDebuffOnHit/PermanentDebuffEffect.prefab".LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect(EntityStates.Merc.Evis.hitEffectPrefab.GetComponent<EffectComponent>().effectIndex, 0.1f);
            for (int i = GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection.Count - 1; i >= 0; --i) {
                if (GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection[i].tempEffectPrefab.name == "VoidFogMildEffect") {
                    GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection.RemoveAt(i);
                }
            }
        }

        public override void AddHooks() => On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;

        public override void RemoveHooks() => On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool -= EffectManager_SpawnEffect_GameObject_EffectData_bool;

        private void EffectManager_SpawnEffect_GameObject_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_GameObject_EffectData_bool orig, GameObject effectPrefab, EffectData effectData, bool transmit) {
            EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);
            if (effectIndex != EffectIndex.Invalid) {
                if (EffectSpawnLimit.IsSkipThisSpawn(effectIndex)) {
                    return;
                }
                EffectManager.SpawnEffect(effectIndex, effectData, transmit);
                return;
            }
            if (effectPrefab && !string.IsNullOrEmpty(effectPrefab.name)) {
                UnityEngine.Debug.LogError("Unable to SpawnEffect from prefab named '" + effectPrefab?.name + "'");
                return;
            }
            UnityEngine.Debug.LogError(string.Format("Unable to SpawnEffect.  Is null? {0}.  Name = '{1}'.\n{2}", effectPrefab == null, effectPrefab?.name, new StackTrace()));
        }
    }
}