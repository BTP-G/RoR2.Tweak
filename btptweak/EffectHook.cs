using RoR2;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BtpTweak {

    public class EffectHook {
        public static float 暴击_timer_ = 0;
        public static float 共生蝎_timer_ = 0;
        public static float 聚合鲁特琴_timer_ = 0;
        public static float 熔融钻机_timer_ = 0;
        public static float 完美巨兽_timer_ = 0;
        public static float 异教徒生成_timer_ = 0;
        public static float 雷电球_timer_ = 0;
        public static float 剃刀_timer_ = 0;
        public static float 黏弹_timer_ = 0;
        public static float 等离子虾_timer_ = 0;

        public static Dictionary<EffectIndex, int> effect_caseLoc_ = new();

        public static void AddHook() {
            On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool += EffectManager_SpawnEffect_GameObject_EffectData_bool;
        }

        public static void RemoveHook() {
            On.RoR2.EffectManager.SpawnEffect_GameObject_EffectData_bool -= EffectManager_SpawnEffect_GameObject_EffectData_bool;
        }

        public static void LateInit() {
            foreach (var entry in EffectCatalog.entries) {
                string name = entry.prefabName;
                if (name.StartsWith("Crits")) {
                    if (name == "Critspark") {
                        effect_caseLoc_.Add(entry.index, 1);
                    } else if (name == "CritsparkHeavy") {
                        effect_caseLoc_.Add(entry.index, 1);
                    }
                } else if (name.StartsWith("FireM")) {
                    effect_caseLoc_.Add(entry.index, 2);
                } else if (name.StartsWith("Here")) {
                    effect_caseLoc_.Add(entry.index, 3);
                } else if (name.StartsWith("Perma")) {
                    effect_caseLoc_.Add(entry.index, 4);
                } else if (name.StartsWith("VoidL")) {
                    if (name == "VoidLightningOrbEffect") {
                        effect_caseLoc_.Add(entry.index, 5);
                    } else if (name == "VoidLightningStrikeImpact") {
                        effect_caseLoc_.Add(entry.index, 5);
                    }
                } else if (name.EndsWith("XQuick")) {
                    effect_caseLoc_.Add(entry.index, 6);
                } else if (name == "LightningStakeNova") {
                    effect_caseLoc_.Add(entry.index, 7);
                } else if (name == "RazorwireOrbEffect" || name == "AffixWhiteImpactEffect") {
                    effect_caseLoc_.Add(entry.index, 8);
                } else if (name == "BehemothVFX") {
                    effect_caseLoc_.Add(entry.index, 9);
                } else if (name == "VoidImpactEffect") {
                    effect_caseLoc_.Add(entry.index, 10);
                }
            }
        }

        private static void EffectManager_SpawnEffect_GameObject_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_GameObject_EffectData_bool orig, GameObject effectPrefab, EffectData effectData, bool transmit) {
            EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);
            if (effectIndex != EffectIndex.Invalid) {
                if (effect_caseLoc_.TryGetValue(effectIndex, out int loc))
                    switch (loc) {
                        case 1: {  // 暴击
                            if ((Time.fixedTime - 暴击_timer_) < 0.1f) {
                                return;
                            } else {
                                暴击_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 2: {  // 熔融钻机
                            if ((Time.fixedTime - 熔融钻机_timer_) < 0) {
                                return;
                            } else {
                                熔融钻机_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 3: {  // 异教徒生成
                            if ((Time.fixedTime - 异教徒生成_timer_) < 10f) {
                                return;
                            } else {
                                异教徒生成_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 4: {  // 共生蝎
                            if ((Time.fixedTime - 共生蝎_timer_) < 0.1f) {
                                return;
                            } else {
                                共生蝎_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 5: {  // 聚合鲁特琴
                            if ((Time.fixedTime - 聚合鲁特琴_timer_) < 0.01f) {
                                return;
                            } else {
                                聚合鲁特琴_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 6: {  // 完美巨兽
                            if ((Time.fixedTime - 完美巨兽_timer_) < 0.01f) {
                                return;
                            } else {
                                完美巨兽_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 7: {  // 雷电球
                            if ((Time.fixedTime - 雷电球_timer_) < 0.01f) {
                                return;
                            } else {
                                雷电球_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 8: {  // 剃刀/冰刀/冰雾
                            if ((Time.fixedTime - 剃刀_timer_) < 0.01f) {
                                return;
                            } else {
                                剃刀_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 9: {  // 黏弹
                            if ((Time.fixedTime - 黏弹_timer_) < 0.05f) {
                                return;
                            } else {
                                黏弹_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 10: {  // 等离子虾
                            if ((Time.fixedTime - 等离子虾_timer_) < 0.05f) {
                                return;
                            } else {
                                等离子虾_timer_ = Time.fixedTime;
                            }
                            break;
                        }
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