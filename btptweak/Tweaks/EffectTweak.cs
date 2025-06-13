using BepInEx;
using BTP.RoR2Plugin.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace BTP.RoR2Plugin.Tweaks {

    internal class EffectTweak : TweakBase<EffectTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.CharacterBody.OnClientBuffsChanged += CharacterBody_OnClientBuffsChanged;
            On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;
            IL.EntityStates.TitanMonster.DeathState.OnEnter += DeathState_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            Object.Destroy(GameObjectPaths.SimpleLightningStrikeImpact.Load<GameObject>().transform.Find("Flash").gameObject);
            Object.Destroy(GameObjectPaths.LightningStrikeImpact.Load<GameObject>().transform.Find("Flash").gameObject);
            Object.Destroy(AssetReferences.affixWhiteExplosion.Asset.transform.Find("Flash, Blue").gameObject);
            Object.Destroy(AssetReferences.affixWhiteExplosion.Asset.transform.Find("Flash, White").gameObject);
            Object.Destroy(GameObjectPaths.TitanRechargeRocksEffect.LoadComponent<EffectComponent>());
            Object.Destroy(GameObjectPaths.ClayBossPreDeath.LoadComponent<EffectComponent>());
            Object.Destroy(GameObjectPaths.RoboBallBossPreDeath.LoadComponent<EffectComponent>());
            GameObjectPaths.EvisProjectile.LoadComponent<ProjectileImpactExplosion>().impactEffect = null;
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.CleanseEffect.LoadComponent<EffectComponent>().effectIndex, 1f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.CloverEffect.LoadComponent<EffectComponent>().effectIndex, 0.5f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.ChainLightningOrbEffect.LoadComponent<EffectComponent>().effectIndex, 0.05f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.GoldOrbEffect.LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.CoinImpact.LoadComponent<EffectComponent>().effectIndex, 0);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.Critspark.LoadComponent<EffectComponent>().effectIndex, 0.5f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.CritsparkHeavy.LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.DamageRejected.LoadComponent<EffectComponent>().effectIndex, 0.1f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.OmniExplosionVFXQuick.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.ImpactCrowbar.LoadComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.AffixWhiteImpactEffect.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.LightningStakeNova.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.FireMeatBallExplosion.LoadComponent<EffectComponent>().effectIndex, 0.001f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.HereticSpawnEffect.LoadComponent<EffectComponent>().effectIndex, 12f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.DiamondDamageBonusEffect.LoadComponent<EffectComponent>().effectIndex, 0.1f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.BehemothVFX.LoadComponent<EffectComponent>().effectIndex, 0.05f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.ImpactStunGrenade.LoadComponent<EffectComponent>().effectIndex, 0.1f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.RazorwireOrbEffect.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.VoidLightningOrbEffect.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.VoidLightningStrikeImpact.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.MissileVoidOrbEffect.LoadComponent<EffectComponent>().effectIndex, 0.01f);
            //EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.VoidImpactEffect.LoadComponent<EffectComponent>().effectIndex, 0.02f);
            EffectSpawnLimit.AddLimitToEffect(GameObjectPaths.PermanentDebuffEffect.LoadComponent<EffectComponent>().effectIndex, 0.1f);
            //EffectSpawnLimit.AddLimitToEffect(EntityStates.Merc.Evis.hitEffectPrefab.GetComponent<EffectComponent>().effectIndex, 0.1f);
            EffectSpawnLimit.AddLimitToEffect(HealthComponent.AssetReferences.bearEffectPrefab.GetComponent<EffectComponent>().effectIndex, 1f);
            EffectSpawnLimit.AddLimitToEffect(HealthComponent.AssetReferences.bearVoidEffectPrefab.GetComponent<EffectComponent>().effectIndex, 1f);
        }

        private void DeathState_OnEnter(ILContext il) {
            var cur = new ILCursor(il);
            if (cur.TryGotoNext(c => c.MatchLdfld<EntityStates.TitanMonster.DeathState>("centerTransform"),
                                c => c.MatchCallvirt<Transform>("set_parent"))) {
                cur.Index += 1;
                cur.Emit(OpCodes.Pop);
                cur.Emit(OpCodes.Ldarg_0);
                cur.EmitDelegate((EntityStates.TitanMonster.DeathState deathState) => deathState.transform);
            }
        }

        private void CharacterBody_OnClientBuffsChanged(On.RoR2.CharacterBody.orig_OnClientBuffsChanged orig, CharacterBody self) {
            if (!NetworkClient.active) {
                UnityEngine.Debug.LogWarning("[Client] function 'System.Void RoR2.CharacterBody::OnClientBuffsChanged()' called on server");
                return;
            }
        }

        private void EffectManager_SpawnEffect_GameObject_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_GameObject_EffectData_bool orig, GameObject effectPrefab, EffectData effectData, bool transmit) {
            var effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);
            if (effectIndex != EffectIndex.Invalid) {
                if (Settings.关闭所有特效.Value) {
                    return;
                }
                if (EffectSpawnLimit.TrySpawnEffect(effectIndex)) {
                    EffectManager.SpawnEffect(effectIndex, effectData, transmit);
                    if (Settings.开启特效生成日志.Value) {
                        ("EffectName == " + effectPrefab?.name + ", EffectIndex(ID) == " + effectIndex).LogMessage();
                    }
                }
                return;
            }
            if (effectPrefab && !string.IsNullOrEmpty(effectPrefab.name)) {
                UnityEngine.Debug.LogWarning("Unable to SpawnEffect from prefab named '" + effectPrefab.name + "'");
                return;
            }
            UnityEngine.Debug.LogError(string.Format("Unable to SpawnEffect.  Is null? {0}.  Name = '{1}'.\n{2}", effectPrefab == null, effectPrefab?.name, new StackTrace()));
        }

        public class EffectSpawnLimit {
            private static readonly Dictionary<int, EffectSpawnLimit> _effectIndexToEffectSpawnLimit = [];
            private float _lastSpawnTime;
            private float _spawnInterval;

            private EffectSpawnLimit(float interval) {
                _spawnInterval = interval;
            }

            public static void AddLimitToEffect(EffectIndex effectIndex, float interval) {
                if (effectIndex == EffectIndex.Invalid) {
                    ($"EffectSpawnLimit：effectIndex(ID) Invalid!").LogWarning();
                    return;
                }
                if (_effectIndexToEffectSpawnLimit.TryGetValue((int)effectIndex, out var spawnLimit)) {
                    if (interval < 0) {
                        ($"EffectSpawnLimit：已移除 {EffectCatalog.GetEffectDef(effectIndex)?.prefabName ?? "null"} 间隔").LogInfo();
                        _effectIndexToEffectSpawnLimit.Remove((int)effectIndex);
                    } else {
                        $"EffectSpawnLimit：{EffectCatalog.GetEffectDef(effectIndex)?.prefabName ?? "null"} 修改间隔: {spawnLimit._spawnInterval}s -> {interval}s".LogInfo();
                        spawnLimit._spawnInterval = interval;
                    }
                } else if (interval >= 0) {
                    _effectIndexToEffectSpawnLimit.Add((int)effectIndex, new(interval));
                    $"EffectSpawnLimit：{EffectCatalog.GetEffectDef(effectIndex)?.prefabName ?? "null"} 添加间隔: {interval}s".LogInfo();
                }
            }

            /// <summary>一次性给多个特效分别添加指定的生成间隔(单位:s), 间隔小于0则移除间隔。</summary>
            /// <param name="id_interval_s">字符串格式为: "特效ID:间隔;特效ID2:间隔2;..."</param>
            public static void AddAddLimitToEffects(string id_interval_s) {
                if (id_interval_s.IsNullOrWhiteSpace()) {
                    return;
                }
                foreach (string text in id_interval_s.Trim().Split(';')) {
                    string[] Index_Interval = text.Split(':');
                    if (Index_Interval.Length != 2) {
                        $"{text}特效ID:间隔 格式错误！".LogWarning();
                        continue;
                    }
                    if (int.TryParse(Index_Interval[0].Trim(), out int index)) {
                        if (float.TryParse(Index_Interval[1].Trim(), out float interval)) {
                            AddLimitToEffect((EffectIndex)index, interval);
                        } else {
                            $"特效ID {Index_Interval[0]} 所设置的间隔 {Index_Interval[1]} 无效！".LogWarning();
                            continue;
                        }
                    } else {
                        $"特效ID {Index_Interval[0]} 无效！".LogWarning();
                        continue;
                    }
                }
            }

            internal static bool TrySpawnEffect(EffectIndex effectIndex) {
                if (_effectIndexToEffectSpawnLimit.TryGetValue((int)effectIndex, out var spawnLimit)) {
                    if (Time.time - spawnLimit._lastSpawnTime < spawnLimit._spawnInterval) {
                        return false;
                    }
                    spawnLimit._lastSpawnTime = Time.time;
                }
                return true;
            }
        }
    }
}