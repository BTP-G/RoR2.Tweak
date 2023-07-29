using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak {

    public class EffectHook {
        public static float 暴击_上一特效生成时间 = 0;
        public static float 等离子虾_上一特效生成时间 = 0;
        public static float 电能钻机_上一特效生成时间 = 0;
        public static float 共生蝎_上一特效生成时间 = 0;
        public static float 激光瞄准镜_上一特效生成时间 = 0;
        public static float 聚合鲁特琴_上一特效生成时间 = 0;
        public static float 熔融钻机_上一特效生成时间 = 0;
        public static float 完美巨兽_上一特效生成时间 = 0;
        public static float 异教徒生成_上一特效生成时间 = 0;

        public static Dictionary<EffectIndex, int> effect_caseLoc_ = new();

        public static void AddHook() {
            On.RoR2.EffectManager.SpawnEffect_EffectIndex_EffectData_bool += EffectManager_SpawnEffect_EffectIndex_EffectData_bool;
        }

        public static void RemoveHook() {
            On.RoR2.EffectManager.SpawnEffect_EffectIndex_EffectData_bool -= EffectManager_SpawnEffect_EffectIndex_EffectData_bool;
        }

        public static void LateInit() {
            for (int i = 0; i < EffectCatalog.effectCount; ++i) {
                string name = EffectCatalog.entries[i].prefabName;
                EffectIndex index = EffectCatalog.entries[i].index;
                if (name.StartsWith("Crits")) {
                    if (name == "Critspark") {
                        effect_caseLoc_.Add(index, 1);
                    } else if (name == "CritsparkHeavy") {
                        effect_caseLoc_.Add(index, 1);
                    }
                } else if (name.StartsWith("FireM")) {
                    effect_caseLoc_.Add(index, 4);
                } else if (name.EndsWith("XQuick")) {
                    effect_caseLoc_.Add(index, 5);
                } else if (name.StartsWith("Perma")) {
                    effect_caseLoc_.Add(index, 2);
                } else if (name.StartsWith("VoidL")) {
                    if (name == "VoidLightningOrbEffect") {
                        effect_caseLoc_.Add(index, 3);
                    } else if (name == "VoidLightningStrikeImpact") {
                        effect_caseLoc_.Add(index, 3);
                    }
                } else if (name.StartsWith("Here")) {
                    effect_caseLoc_.Add(index, 6);
                }
            }
        }

        private static void EffectManager_SpawnEffect_EffectIndex_EffectData_bool(On.RoR2.EffectManager.orig_SpawnEffect_EffectIndex_EffectData_bool orig, EffectIndex effectIndex, EffectData effectData, bool transmit) {
            if (effect_caseLoc_.TryGetValue(effectIndex, out int loc))
                switch (loc) {
                    case 1: {  // 暴击
                        if ((Time.time - 暴击_上一特效生成时间) < 0.1f) {
                            return;
                        } else {
                            暴击_上一特效生成时间 = Time.time;
                        }
                        break;
                    }
                    case 2: {  // 共生蝎
                        if ((Time.time - 共生蝎_上一特效生成时间) < 0.15f) {
                            return;
                        } else {
                            共生蝎_上一特效生成时间 = Time.time;
                        }
                        break;
                    }
                    case 3: {  // 聚合鲁特琴
                        if ((Time.time - 聚合鲁特琴_上一特效生成时间) < 0.025f) {
                            return;
                        } else {
                            聚合鲁特琴_上一特效生成时间 = Time.time;
                        }
                        break;
                    }
                    case 4: {  // 熔融钻机
                        if ((Time.time - 熔融钻机_上一特效生成时间) < 0.075f) {
                            return;
                        } else {
                            熔融钻机_上一特效生成时间 = Time.time;
                        }
                        break;
                    }
                    case 5: {  // 完美巨兽
                        if ((Time.time - 完美巨兽_上一特效生成时间) < 0.15f) {
                            return;
                        } else {
                            完美巨兽_上一特效生成时间 = Time.time;
                        }
                        break;
                    }
                    case 6: {  // 异教徒生成
                        if ((Time.time - 异教徒生成_上一特效生成时间) < 10f) {
                            return;
                        } else {
                            异教徒生成_上一特效生成时间 = Time.time;
                        }
                        break;
                    }
                }
            orig(effectIndex, effectData, transmit);
        }
    }
}