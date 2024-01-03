using BepInEx;
using BepInEx.Configuration;
using BtpTweak.Tweaks;
using BtpTweak.Tweaks.ItemTweaks;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RiskOfOptions;
using RiskOfOptions.Lib;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;

namespace BtpTweak {

    public static class ModConfig {
        public static KeyBindOption ReloadKey { get; private set; }
        public static CheckBoxOption 关闭所有特效 { get; private set; }
        public static CheckBoxOption 开启特效生成日志 { get; private set; }
        public static CheckBoxOption 开启战斗日志 { get; private set; }
        public static ChoiceOption 开启特拉法梅 { get; private set; }
        public static CheckBoxOption 启用阶梯触发链 { get; private set; }
        public static SliderOption Boss物品掉率 { get; private set; }
        public static SliderOption 测试用 { get; private set; }
        public static SliderOption 测试用2 { get; private set; }
        public static SliderOption 导弹发射间隔 { get; private set; }
        public static SliderOption 每关难度增加量 { get; private set; }
        public static SliderOption 喷泉喷射间隔 { get; private set; }
        public static SliderOption 牺牲基础掉率 { get; private set; }
        public static StringInputFieldOption 特效_间隔 { get; private set; }

        public static CheckBoxOption AddCheckBoxOption(ConfigFile config, string section, string key, bool defaultValue, string description, Action<bool> onChanged = null) {
            var configEntry = config.Bind(section, key, defaultValue, description);
            var checkBoxOption = new CheckBoxOption(configEntry);
            var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            ModSettingsManager.AddOption(checkBoxOption, modMetaData.Guid, modMetaData.Name);
            if (onChanged != null) {
                configEntry.SettingChanged += (sender, args) => {
                    onChanged(configEntry.Value);
                };
            }
            return checkBoxOption;
        }

        public static ChoiceOption AddChoiceOption<T>(ConfigFile config, string section, string key, T defaultValue, string description, Action<T> onChanged = null) where T : Enum {
            var configEntry = config.Bind(section, key, defaultValue, description);
            var choiceOption = new ChoiceOption(configEntry);
            var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            ModSettingsManager.AddOption(choiceOption, modMetaData.Guid, modMetaData.Name);
            if (onChanged != null) {
                configEntry.SettingChanged += (sender, args) => {
                    onChanged(configEntry.Value);
                };
            }
            return choiceOption;
        }

        public static StepSliderOption AddStepSliderOption(ConfigFile config, string section, string key, float defaultValue, float min, float max, float step_size, string description, string formatString = "{0}", Action<float> onChanged = null) {
            var configEntry = config.Bind(section, key, defaultValue, description);
            var stepSliderOption = new StepSliderOption(configEntry, new StepSliderConfig() { min = min, max = max, increment = step_size, formatString = formatString });
            var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            ModSettingsManager.AddOption(stepSliderOption, modMetaData.Guid, modMetaData.Name);
            if (onChanged != null) {
                configEntry.SettingChanged += (sender, args) => {
                    onChanged(configEntry.Value);
                };
            }
            return stepSliderOption;
        }

        public static SliderOption AddSliderOption(ConfigFile config, string section, string key, float defaultValue, float min, float max, string description, string formatString = "{0}", Action<float> onChanged = null) {
            var configEntry = config.Bind(section, key, defaultValue, description);
            var sliderOption = new SliderOption(configEntry, new SliderConfig() { min = min, max = max, formatString = formatString });
            var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            ModSettingsManager.AddOption(sliderOption, modMetaData.Guid, modMetaData.Name);
            if (onChanged != null) {
                configEntry.SettingChanged += (sender, args) => {
                    onChanged(configEntry.Value);
                };
            }
            return sliderOption;
        }

        public static KeyBindOption AddKeyBindOption(ConfigFile config, string section, string key, KeyboardShortcut defaultValue, string description, Action<KeyboardShortcut> onChanged = null) {
            var configEntry = config.Bind(section, key, defaultValue, description);
            var KeyBindOption = new KeyBindOption(configEntry);
            var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            ModSettingsManager.AddOption(KeyBindOption, modMetaData.Guid, modMetaData.Name);
            if (onChanged != null) {
                configEntry.SettingChanged += (sender, args) => {
                    onChanged(configEntry.Value);
                };
            }
            return KeyBindOption;
        }

        public static StringInputFieldOption AddStringInputFieldOption(ConfigFile config, string section, string key, string defaultValue, string description, InputFieldConfig.SubmitEnum submitOn = InputFieldConfig.SubmitEnum.OnExitOrSubmit, Action<string> onChanged = null) {
            var configEntry = config.Bind(section, key, defaultValue, description);
            var stringInputFieldOption = new StringInputFieldOption(configEntry, new InputFieldConfig() { submitOn = submitOn });
            var modMetaData = Assembly.GetCallingAssembly().GetModMetaData();
            ModSettingsManager.AddOption(stringInputFieldOption, modMetaData.Guid, modMetaData.Name);
            if (onChanged != null) {
                configEntry.SettingChanged += (sender, args) => {
                    onChanged(configEntry.Value);
                };
            }
            return stringInputFieldOption;
        }

        public static ModMetaData GetModMetaData(this Assembly assembly) {
            var result = default(ModMetaData);
            Type[] exportedTypes = assembly.GetExportedTypes();
            for (int i = 0; i < exportedTypes.Length; ++i) {
                var customAttribute = exportedTypes[i].GetCustomAttribute<BepInPlugin>();
                if (customAttribute != null) {
                    result.Guid = customAttribute.GUID;
                    result.Name = customAttribute.Name;
                }
            }
            return result;
        }

        internal static void InitConfig(ConfigFile config) {
            ModSettingsManager.SetModIcon(Texture2DPaths.texVoidCoinIcon.Load<Sprite>(), Main.PluginGUID, Main.PluginName);
            ModSettingsManager.SetModDescription("这里是关于BTP的整合包的内部的Mod设置", Main.PluginGUID, Main.PluginName);
            Boss物品掉率 = AddSliderOption(config,
                                       "测试",
                                       "Boss物品掉率",
                                       2.5f,
                                       0,
                                       100f,
                                       "任意模式下，具有特殊奖励的敌人死亡时，掉落该物品的概率，不受运气影响。",
                                       "{0}%");
            ReloadKey = AddKeyBindOption(config,
                                  "按键",
                                  "磁轨炮手上弹按键",
                                  new KeyboardShortcut(KeyCode.B),
                                  "按此按键进入上弹模式。");
            测试用 = AddSliderOption(config,
                            "测试",
                            "测试用",
                            0f,
                            -10000f,
                            10000f,
                            "");
            测试用2 = AddSliderOption(config,
                             "测试",
                             "测试用2",
                             0f,
                             -10000f,
                             10000f,
                             "");
            导弹发射间隔 = AddSliderOption(config,
                               "物品",
                               "导弹池发射间隔（单位：秒）（弃用）",
                               1f,
                               0f,
                               10f,
                               "每隔~发射一发导弹，间隔内添加的相同触发链的导弹将会整合为一枚。");
            关闭所有特效 = AddCheckBoxOption(config,
                               "特效-间隔",
                               "开启时禁用所有特效",
                               false,
                               "用于后期防止卡顿。");
            开启特拉法梅 = AddChoiceOption(config,
                               "物品",
                               "特拉法梅的祝福",
                               LunarWingsState.Default,
                               "游戏内选择后将直接更新物品效果，物品对应汉化也会更新。不了解此选项请勿修改！",
                               onChanged: LunarWingsTweak.UpdateLunarWingsState);
            开启特效生成日志 = AddCheckBoxOption(config,
                                 "日志",
                                 "是否开启特效生成日志",
                                 false,
                                 "用于获取特效的ID。");
            开启战斗日志 = AddCheckBoxOption(config,
                               "日志",
                               "是否开启战斗日志",
                               false,
                               "原版默认不开启，仅供测试用。",
                               onChanged: (newValue) => CombatDirector.cvDirectorCombatEnableInternalLogs.value = newValue);
            每关难度增加量 = AddSliderOption(config,
                                "难度",
                                "造物难度下，每完成一关增加的难度",
                                25f,
                                0f,
                                100000f,
                                "开始时难度为50%。难度计算公式：开始难度 + 当前关卡数 x ~",
                                "{0}%");
            喷泉喷射间隔 = AddSliderOption(config,
                               "物品",
                               "发射池喷射间隔（单位：秒）（弃用）",
                               1f,
                               0f,
                               10f,
                               "每隔~喷射一次投射物，间隔内添加的相同触发链的投射物将会整合为一枚。");
            启用阶梯触发链 = AddCheckBoxOption(config,
                                "测试",
                                "启用阶梯触发链",
                                false,
                                "开启后，某些物品只能触发比自己低品质的物品。（目前受影响的物品：黏弹 < Atg-MK1导弹 = 等离子虾 = 尤克里里 = 聚合鲁特琴 = 冰火双环 < 感应肉钩 < 雷火双钻）");
            特效_间隔 = AddStringInputFieldOption(config,
                              "特效-间隔",
                              "通过特效ID指定某些特效的生成间隔",
                              "",
                              "默认已含有某些会导致游戏卡顿的特效。\neg：假如共生蝎的特效ID为1，我想让其有0.1秒的生成间隔，那么应该填入：1:0.1，如果间隔为负数则表示取消间隔。\n每个设置之间用“;”隔开，所有标点均用英文标点！",
                              onChanged: EffectTweak.EffectSpawnLimit.AddAddLimitToEffects);
            牺牲基础掉率 = AddSliderOption(config,
                               "牺牲神器",
                               "牺牲基础掉率",
                               5f,
                               0f,
                               100f,
                               "基础掉率，非最终掉率。最终掉率与敌人种类和等级等属性相关。Boss基础掉率+100%，精英基础掉率+50%。\n在同一关卡中，每次击杀敌人后，如果未掉落物品，则临时增加掉率，掉落后重置；如果掉落物品，则临时降低掉率，过关后重置。",
                               "{0}%");
        }
    }
}