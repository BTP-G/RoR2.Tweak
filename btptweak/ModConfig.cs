using BepInEx.Configuration;
using BtpTweak.Tweaks;
using MysticsRisky2Utils;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using UnityEngine;

namespace BtpTweak {

    public static class ModConfig {
        public static ConfigEntry<KeyboardShortcut> ReloadKey;
        public static ConfigOptions.ConfigurableValue<bool> 关闭所有特效;
        public static ConfigOptions.ConfigurableValue<bool> 开启特效生成日志;
        public static ConfigOptions.ConfigurableValue<bool> 开启战斗日志;
        public static ConfigOptions.ConfigurableValue<bool> 开启特拉法梅;
        public static ConfigOptions.ConfigurableValue<bool> 中断触发链;
        public static ConfigOptions.ConfigurableValue<float> Boss物品掉率;
        public static ConfigOptions.ConfigurableValue<float> 测试用;
        public static ConfigOptions.ConfigurableValue<float> 导弹发射间隔;
        public static ConfigOptions.ConfigurableValue<float> 每关难度增加量;
        public static ConfigOptions.ConfigurableValue<float> 喷泉喷射间隔;
        public static ConfigOptions.ConfigurableValue<float> 牺牲基础掉率;
        public static ConfigOptions.ConfigurableValue<string> 特效_间隔;

        internal static void InitConfig(ConfigFile config) {
            开启特拉法梅 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, config, "物品", "是否直接启用特拉法梅", false, "打开后将直接获得效果，物品对应汉化也会更新。", onChanged: MiscTweak.SetLunarWingsState);
            导弹发射间隔 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, config, "物品", "导弹池发射间隔（单位：秒）", 1f, 0, 10f, "每隔~发射一次导弹，间隔随池中导弹数量自动缩短，间隔添加的相同触发链的导弹将会整合为一枚。");
            喷泉喷射间隔 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, config, "物品", "发射池喷射间隔（单位：秒）", 1f, 0, 10f, "每隔~喷射一次投射物，间隔随池中投射物数量自动缩短，间隔添加的相同触发链的投射物将会整合为一枚。");
            ReloadKey = config.Bind("按键", "磁轨炮手上弹按键", new KeyboardShortcut(KeyCode.R), "按此按键进入上弹模式。");
            ModSettingsManager.AddOption(new KeyBindOption(ReloadKey));
            每关难度增加量 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, config, "难度", "每完成一关增加的难度（%）", 20f, 0, 1000f, "基础为100%，计算公式：基础难度 + 当前关卡数 x ~%");
            特效_间隔 = ConfigOptions.ConfigurableValue.CreateString(Main.PluginGUID, Main.PluginName, config, "特效-间隔", "通过特效ID指定某些特效的生成间隔", null, "默认已含有某些会导致游戏卡顿的特效。\neg：假如共生蝎的特效ID为1，我想让其有0.1秒的生成间隔，那么应该填入：1:0.1，如果间隔为负数则表示取消间隔。\n每个设置之间用“;”隔开，所有标点均用英文标点！", onChanged: EffectTweak.EffectSpawnLimit.AddAddLimitToEffects);
            关闭所有特效 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, config, "特效-间隔", "开启时禁用所有特效", false, "用于后期防止卡顿。");
            牺牲基础掉率 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, config, "牺牲神器", "牺牲基础掉率", 5f, 0f, 100f, "基础掉率，非最终掉率。最终掉率与敌人种类和等级等属性相关。Boss基础掉率+100%，精英基础掉率+50%。\n在同一关卡中，每次击杀敌人后，如果未掉落物品，则临时增加掉率，掉落后重置；如果掉落物品，则临时降低掉率，过关后重置。");
            开启战斗日志 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, config, "日志", "是否开启战斗日志", false, "原版默认不开启，仅供测试用。", onChanged: (newValue) => CombatDirector.cvDirectorCombatEnableInternalLogs.value = newValue);
            开启特效生成日志 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, config, "日志", "是否开启特效生成日志", false, "用于获取特效的ID。");
            Boss物品掉率 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, config, "测试", "Boss物品掉率", 2.5f, 0, 1000f, "具有特殊奖励的敌人死亡时, 掉落该物品的概率, 不受其他因素影响。");
            中断触发链 = ConfigOptions.ConfigurableValue.CreateBool(Main.PluginGUID, Main.PluginName, config, "测试", "开启中断物品触发链", false, "开启后，触发物品直接不再互相触发。");
            测试用 = ConfigOptions.ConfigurableValue.CreateFloat(Main.PluginGUID, Main.PluginName, config, "测试", "测试用", 0, 0, 1000);
        }
    }
}