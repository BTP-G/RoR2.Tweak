using RoR2;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BtpTweak {

    public class EffectHook {
        public static Dictionary<EffectIndex, int> EffectIndexToCaseIndex_ = new();
        private static float 暴击_timer_;
        private static float 等离子虾_timer_;
        private static float 共生蝎_timer_;
        private static float 金钱_timer_;
        private static float 聚合鲁特琴_timer_;
        private static float 雷电球_timer_;
        private static float 黏弹_timer_;
        private static float 凝神水晶_timer_;
        private static float 撬棍_timer_;
        private static float 熔融钻机_timer_;
        private static float 剃刀_timer_;
        private static float 完美巨兽_timer_;
        private static float 幸运草_timer_;
        private static float 异教徒生成_timer_;
        private static float 硬币_timer_;

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
                        EffectIndexToCaseIndex_.Add(entry.index, 1);
                    } else if (name == "CritsparkHeavy") {
                        EffectIndexToCaseIndex_.Add(entry.index, 1);
                    }
                } else if (name.StartsWith("FireM")) {
                    EffectIndexToCaseIndex_.Add(entry.index, 2);
                } else if (name.StartsWith("Here")) {
                    EffectIndexToCaseIndex_.Add(entry.index, 3);
                } else if (name.StartsWith("Perma")) {
                    EffectIndexToCaseIndex_.Add(entry.index, 4);
                } else if (name.StartsWith("VoidL")) {
                    if (name == "VoidLightningOrbEffect") {
                        EffectIndexToCaseIndex_.Add(entry.index, 5);
                    } else if (name == "VoidLightningStrikeImpact") {
                        EffectIndexToCaseIndex_.Add(entry.index, 5);
                    }
                } else if (name.EndsWith("XQuick")) {
                    EffectIndexToCaseIndex_.Add(entry.index, 6);
                } else if (name == "LightningStakeNova") {
                    EffectIndexToCaseIndex_.Add(entry.index, 7);
                } else if (name == "RazorwireOrbEffect" || name == "AffixWhiteImpactEffect") {
                    EffectIndexToCaseIndex_.Add(entry.index, 8);
                } else if (name == "BehemothVFX") {
                    EffectIndexToCaseIndex_.Add(entry.index, 9);
                } else if (name == "VoidImpactEffect" || name == "MissileVoidOrbEffect") {
                    EffectIndexToCaseIndex_.Add(entry.index, 10);
                } else if (name == "ImpactCrowbar") {
                    EffectIndexToCaseIndex_.Add(entry.index, 11);
                } else if (name == "DiamondDamageBonusEffect") {
                    EffectIndexToCaseIndex_.Add(entry.index, 12);
                } else if (name == "GoldOrbEffect") {
                    EffectIndexToCaseIndex_.Add(entry.index, 13);
                } else if (name == "CoinImpact") {
                    EffectIndexToCaseIndex_.Add(entry.index, 14);
                } else if (name == "CloverEffect") {
                    EffectIndexToCaseIndex_.Add(entry.index, 15);
                } else if (name == "CleanseEffect") {
                    EffectIndexToCaseIndex_.Add(entry.index, 16);
                }
            }
            for (int i = 0; i < GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection.Count; ++i) {
                if (GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection[i].tempEffectPrefab.name == "VoidFogMildEffect") {
                    GrooveSaladSpikestripContent.SpikestripVisuals.temporaryVFXCollection.RemoveAt(i--);
                }
            }
        }

        private static void EffectManager_SpawnEffect_GameObject_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_GameObject_EffectData_bool orig, GameObject effectPrefab, EffectData effectData, bool transmit) {
            EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);
            if (effectIndex != EffectIndex.Invalid) {
                if (EffectIndexToCaseIndex_.TryGetValue(effectIndex, out int loc)) {
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
                            if ((Time.fixedTime - 等离子虾_timer_) < 0.02f) {
                                return;
                            } else {
                                等离子虾_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 11: {  // 撬棍
                            if ((Time.fixedTime - 撬棍_timer_) < 0.1f) {
                                return;
                            } else {
                                撬棍_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 12: {  //
                            if ((Time.fixedTime - 凝神水晶_timer_) < 0.1f) {
                                return;
                            } else {
                                凝神水晶_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 13: {  //
                            if ((Time.fixedTime - 金钱_timer_) < 0.02f) {
                                return;
                            } else {
                                金钱_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 14: {  //
                            if ((Time.fixedTime - 硬币_timer_) < 0.02f) {
                                return;
                            } else {
                                硬币_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 15: {  //
                            if ((Time.fixedTime - 幸运草_timer_) < 0.1f) {
                                return;
                            } else {
                                幸运草_timer_ = Time.fixedTime;
                            }
                            break;
                        }
                        case 16: {
                            return;
                        }
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