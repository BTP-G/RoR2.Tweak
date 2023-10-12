using BepInEx;
using BtpTweak.Tweaks;
using HIFUHuntressTweaks.Skills;
using MysticsRisky2Utils;
using RoR2;

namespace BtpTweak {

    public static class ModConfig {
        public static ConfigOptions.ConfigurableValue<bool> 开启战斗日志;
        public static ConfigOptions.ConfigurableValue<bool> 是否开启特效生成日志;
        public static ConfigOptions.ConfigurableValue<bool> 关闭所有特效;
        public static ConfigOptions.ConfigurableValue<float> 女猎人射程每级增加距离;
        public static ConfigOptions.ConfigurableValue<float> 导弹发射间隔;
        public static ConfigOptions.ConfigurableValue<float> 喷泉喷射间隔;
        public static ConfigOptions.ConfigurableValue<float> 每关难度增加量;
        public static ConfigOptions.ConfigurableValue<string> 特效_间隔;
        public static ConfigOptions.ConfigurableValue<float> 测试用1;
        public static ConfigOptions.ConfigurableValue<float> 测试用2;
        public static ConfigOptions.ConfigurableValue<float> 测试用3;

        public static void InitConfig() {
            开启战斗日志 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "日志", "是否开启战斗日志", false, "原版默认不开启，仅供测试用", onChanged: (newValue) => CombatDirector.cvDirectorCombatEnableInternalLogs.value = newValue);
            是否开启特效生成日志 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "日志", "是否开启特效生成日志", false, "用于获取特效的ID", onChanged: (newValue) => {
            });
            每关难度增加量 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "难度", "每完成一关增加的难度（%）", 20f, 0, 1000f, "基础为100%，计算公式：基础难度 + 当前关卡数 x ~%");
            女猎人射程每级增加距离 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "女猎人", "每级射程增加量（单位：米）", 5f, 0, 10f, "默认射程60m（设置为0就不增加）", onChanged: (newValue) => {
                R2API.LanguageAPI.AddOverlay("HUNTRESS_PRIMARY_ALT_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。瞄准60米<style=cStack>（每升一级+{newValue}米）</style>内的敌人，拉弓射出<style=cIsDamage>{Flurry.minArrows}枚</style>跟踪箭，每枚造成{(Flurry.damage).ToDmgPct()}的伤害。如果暴击则发射<style=cIsDamage>{Flurry.maxArrows}</style>枚跟踪箭。", "zh-CN");
                R2API.LanguageAPI.AddOverlay("HUNTRESS_PRIMARY_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。瞄准60米<style=cStack>（每升一级+{newValue}米）</style>内的敌人，快速射出一枚能够造成{(Strafe.damage).ToDmgPct()}伤害的跟踪箭。", "zh-CN");
            });
            导弹发射间隔 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "导弹发射池", "导弹池发射间隔（单位：秒）", 1f, 0, 10f, "每隔~发射一次导弹，间隔随池中导弹数量自动缩短，间隔添加的相同触发链的导弹将会整合为一枚。", onChanged: (newValue) => {
                R2API.LanguageAPI.AddOverlay("ITEM_FIREWORK_DESC", $"有{"5%".ToDmg() + "（每层+5%）".ToStack()}机率向{"烟花发射池".ToUtil()}里添加一枚烟花{newValue.ToStack("（发射间隔：", "s）")}。每枚合计造成{1.5f.ToDmgPct()}的伤害。", "zh-CN");
                R2API.LanguageAPI.AddOverlay("ITEM_MISSILE_DESC", $"有{"25%".ToDmg()}机率向{"导弹发射池".ToUtil()}里添加一枚导弹{newValue.ToStack("（发射间隔：", "s）")}。每枚合计造成{"250%".ToDmg() + "（每层增加250%）".ToStack()}的伤害。", "zh-CN");
            });
            喷泉喷射间隔 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "喷泉发射池", "发射池喷射间隔（单位：秒）", 1f, 0, 10f, "每隔~喷射一次投射物，间隔随池中投射物数量自动缩短，间隔添加的相同触发链的投射物将会整合为一枚。", onChanged: (newValue) => {
                R2API.LanguageAPI.AddOverlay("ITEM_FIREBALLSONHIT_DESC", $"命中时有<style=cIsDamage>30%</style>的几率向敌人的{"岩浆球喷泉".ToFire()}中添加{"1颗岩浆球".ToFire() + newValue.ToStack("（喷射间隔：", "s）")}，造成<style=cIsDamage>300%</style><style=cStack>（每层+300%）</style>的伤害，并<style=cIsDamage>点燃</style>所有敌人。", "zh-CN");
                R2API.LanguageAPI.AddOverlay("ITEM_LIGHTNINGSTRIKEONHIT_DESC", $"命中时有<style=cIsDamage>30%</style>的几率向敌人的{"雷电召唤池".ToLightning()}中添加{"1次雷击".ToLightning() + newValue.ToStack("（基础召唤间隔：", "s）")}，造成<style=cIsDamage>300%</style><style=cStack>（每层+300%）</style>的伤害。<color=#FFFF00>装卸工特殊效果：关于“雷霆拳套”（详情看 权杖 技能介绍）。</color>", "zh-CN");
                R2API.LanguageAPI.AddOverlay("ITEM_STICKYBOMB_DESC", $"命中时有<style=cIsDamage>5%</style><style=cStack>（每层增加5%）</style>的机率向敌人的{"黏弹喷泉".ToDmg()}中添加{"1颗黏弹".ToDmg() + newValue.ToStack("（喷射间隔：", "s）")}，爆炸时合计造成<style=cIsDamage>180%</style>伤害。", "zh-CN");
            });
            特效_间隔 = ConfigOptions.ConfigurableValue.CreateString(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "特效-间隔", "通过特效ID指定某些特效的生成间隔", "", "默认已含有某些会导致游戏卡顿的特效。\neg：假如共生蝎的特效ID为1，我想让其有0.1秒的生成间隔，那么应该填入：1:0.1，如果间隔为负数则表示取消间隔。\n每个设置之间用“;”隔开，所有标点均用英文标点！", onChanged: (newString) => {
                if (!newString.IsNullOrWhiteSpace()) {
                    foreach (string text in newString.Trim().Split(';')) {
                        string[] Index_Interval = text.Split(':');
                        if (Index_Interval.Length != 2) {
                            Main.Logger.LogWarning($"{text}特效ID:间隔 格式无效！");
                            continue;
                        }
                        if (int.TryParse(Index_Interval[0].Trim(), out int index)) {
                            if (float.TryParse(Index_Interval[1].Trim(), out float interval)) {
                                EffectTweak.EffectSpawnLimit.AddLimitToEffect((EffectIndex)index, interval);
                            } else {
                                Main.Logger.LogWarning($"特效ID {Index_Interval[0]} 所设置的间隔 {Index_Interval[1]} 无效！");
                                continue;
                            }
                        } else {
                            Main.Logger.LogWarning($"特效ID {Index_Interval[0]} 无效！");
                            continue;
                        }
                    }
                }
            });
            关闭所有特效 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "特效-间隔", "开启时禁用所有特效", false, "用于后期防止卡顿。", onChanged: (newValue) => {
            });
            测试用1 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "测试", "测试用1", 0, 0, 1000, onChanged: OnSettingChanged1);
            测试用2 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "测试", "测试用2", 0, 0, 1000, onChanged: OnSettingChanged2);
            测试用3 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, Main.Instance.Config, "测试", "测试用3", 0, 0, 1000, onChanged: OnSettingChanged3);
        }

        private static void OnSettingChanged1(float newValue) {
        }

        private static void OnSettingChanged2(float newValue) {
        }

        private static void OnSettingChanged3(float newValue) {
        }
    }
}