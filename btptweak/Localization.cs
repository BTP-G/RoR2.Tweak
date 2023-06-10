using R2API;

namespace BtpTweak {

    internal class Localization {

        public static void 汉化() {
            On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuController_Start;
        }

        public static void RemoveHook() {
            On.RoR2.UI.MainMenu.MainMenuController.Start -= MainMenuController_Start;
        }

        private static void MainMenuController_Start(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self) {
            orig(self);
            LanguageAPI.AddOverlay("ITEM_AbyssalMedkit_PICKUP", "消耗品，可以替你抵挡10次减益，每一次抵挡都有概率给予你“祝·福”", "zh-CN");
            LanguageAPI.AddOverlay("ITEM_AbyssalMedkit_DESCRIPTION", "抵挡<style=cIsUtility> 10次 </style>减益后失效。每一次抵挡都有 10% 概率给予你<style=cIsHealing>“祝·福”</style>。<style=cIsUtility>每个祝福可提升 3% 的所有属性</style>。<style=cIsVoid>使所有医疗包无效化</style>", "zh-CN");
            LanguageAPI.AddOverlay("DIFFICULTY_CONFIGURABLEDIFFICULTYMOD_DESCRIPTION_DYNAMIC", "追求刺激，贯彻到底！开启进化神器，踏上弑神之路（24关）\n\n<style=cStack>难度调整：\n难度缩放+" + ConfigurableDifficulty.ConfigurableDifficultyPlugin.difficultyScaling + "%+其他\n（难度缩放随关卡增加而增加，最高+" + BtpTweak.造物难度最大修正难度缩放_.Value * 100 + "%）\n\n敌人调整：\n最大生命值+25%\n治疗量+25%\n护甲+10点\n暴击率+10%\n跳跃高度+10%\n技能冷却-10%\n金钱掉落-20%\n获得初始物品-璀璨珍珠\n\n友方调整:\n（血量随关卡增加而增加）\n治疗量-25%\n生命值再生速度-25%\n跌落伤害+100%\n获得初始物品-迪奥的朋友、Hopoo羽毛，野牛肉排</style>", "zh-CN");
            LanguageAPI.AddOverlay("ITEM_INFUSION_DESC", "每击败一名敌人，即可<style=cIsHealing>永久性</style>增加<style=cIsHealing>" + BtpTweak.浸剂击杀奖励倍率_.Value + "</style>点生命值<style=cStack>（每层增加" + BtpTweak.浸剂击杀奖励倍率_.Value + "点）</style>，<style=cIsHealing>无上限</style>。", "zh-CN");
            LanguageAPI.AddOverlay("SPIKESTRIPSKILL_DEEPROT_NAME", "腐朽", "zh-CN");
            LanguageAPI.AddOverlay("SPIKESTRIPSKILL_DEEPROT_DESCRIPTION", "<style=cIsVoid>“虚 空 的 馈 赠”</style>\n施加<style=cIsVoid>虚空之毒</style>，使<style=cIsVoid>速度减慢10%</style>。叠加<style=cIsVoid>5</style>次后，<style=cIsVoid>虚空之毒</style>将转化为<style=cIsVoid>灵魂之痛</style>，造成巨量伤害！", "zh-CN");
            LanguageAPI.AddOverlay("KEYWORD_SOULROT", "<style=cKeywordName>灵魂之痛</style><style=cSub> 总共造成等同于敌人最大生命值 <style=cIsVoid>62.5%</style> 的<style=cIsVoid>致命伤害</style>。</style>", "zh-CN");
            LanguageAPI.AddOverlay("HUNTRESS_PRIMARY_DESCRIPTION", "<style=cIsUtility>灵巧</style>。快速射出一名能够造成<style=cIsDamage>180%<style=cStack>（每级增加20%）</style>伤害</style>的跟踪箭。", "zh-CN");
            LanguageAPI.AddOverlay("HUNTRESS_PRIMARY_ALT_DESCRIPTION", "<style=cIsUtility>灵巧</style>。拉弓射出<style=cIsDamage>3<style=cStack>（每3级+1）</style>枚</style>跟踪箭，每枚造成<style=cIsDamage>120%的伤害</style>。如果暴击则发射<style=cIsDamage>双倍</style>跟踪箭。", "zh-CN");
            LanguageAPI.AddOverlay("STANDALONEANCIENTSCEPTER_BUFF_PERISHSONG_NAME", "灭绝之歌", "zh-CN");
            LanguageAPI.AddOverlay("STANDALONEANCIENTSCEPTER_BUFF_PERISHSONG_DESC", "30秒后，从攻击你的异端中受到5000%的伤害。（不明所以...）", "zh-CN");
            LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERNAME", "暗杀", "zh-CN");
            LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERDESC", "<style=cIsDamage>屠杀者</style>。使用左轮手枪进行射击，造成<style=cIsDamage>600%的伤害</style>。击杀敌人可以<style=cIsUtility>重置所有能力的冷却时间</style>。\n<color=#d299ff>权杖：额外发射一发子弹。</color>", "zh-CN");
            LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERNAME", "叛徒", "zh-CN");
            LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERDESC", "666666666666", "zh-CN");
            RemoveHook();
        }
    }
}