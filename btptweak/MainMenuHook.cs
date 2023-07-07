using HIFUEngineerTweaks.Skills;
using HIFUHuntressTweaks.Skills;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using static R2API.LanguageAPI;

namespace BtpTweak {

    internal class MainMenuHook {

        public static void AddHook() {
            On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuController_Start;
        }

        public static void RemoveHook() {
            On.RoR2.UI.MainMenu.MainMenuController.Start -= MainMenuController_Start;
        }

        private static void MainMenuController_Start(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self) {
            orig(self);
            初始汉化();
            圣骑士汉化();
            探路者汉化();
            物品调整();
            RemoveHook();
        }

        public static void 初始汉化() {
            AddOverlay("CAPTAIN_PRIMARY_DESCRIPTION", "喷射一大团弹丸，造成<style=cIsDamage>6<style=cStack>(每6级+1)</style>x100%的伤害</style>。为攻击充能将缩小<style=cIsUtility>扩散范围</style>。", "zh-CN");
            AddOverlay("CAPTAIN_SECONDARY_NAME", "能量电镖", "zh-CN");
            AddOverlay("COMMANDO_HEAVYTAP_DESCRIPTION", "<style=cIsDamage>绝对光滑</style>。射击两次，造成<style=cIsDamage>2x155%的伤害</style>。", "zh-CN");
            AddOverlay("COMMANDO_HEAVYTAP_NAME", "沉重双击", "zh-CN");
            AddOverlay("COMMANDO_PLASMATAP_DESCRIPTION", "<style=cIsDamage>击穿</style>。发射一道锥形闪电，造成<style=cIsDamage>100%的伤害</style>。", "zh-CN");
            AddOverlay("COMMANDO_PLASMATAP_NAME", "电弧子弹", "zh-CN");
            AddOverlay("COMMANDO_PRFRVWILDFIRESTORM_DESCRIPTION", "发射一股火焰，每秒造成<style=cIsDamage>550%的伤害</style>，并有机会<style=cIsDamage>点燃</style>敌人。", "zh-CN");
            AddOverlay("COMMANDO_PRFRVWILDFIRESTORM_NAME", "PRFR-V野火风暴", "zh-CN");
            AddOverlay("DIFFICULTY_CONFIGURABLEDIFFICULTYMOD_DESCRIPTION_DYNAMIC", "<color=#4190cc>世界不再是你熟悉的那样！</color>\n\n<style=cStack>世界调整：\n难度缩放+100%\n传送器半径-25%\n传送器未充能时，进度-0.5%每秒\n可交互物品数量增加\n\n敌人调整：\n最大生命值+25%\n治疗量+25%\n护甲+10点\n暴击率+10%\n跳跃高度+10%\n技能冷却-10%\n跌落伤害-100%\n金钱掉落-20%\n怪物低血量时获得强心剂增益，进入狂暴状态（10x当前关卡数）秒\n\n友方调整:\n治疗量-25%\n生命值再生速度-25%\n跌落伤害+100%\n获得初始物品-迪奥的朋友、Hopoo羽毛，谨慎的蛞蝓</style>", "zh-CN");
            AddOverlay("ENGI_SECONDARY_DESCRIPTION", $"放置一枚二阶段地雷，能够造成<style=cIsDamage>300%的伤害</style>，或在完全引爆时造成<style=cIsDamage>{Mathf.Round(300f * PressureMines.damageScale)}%的伤害</style>。最多放置{PressureMines.charges}枚<style=cStack>(每级+1枚)</style>。", "zh-CN");
            AddOverlay("ENGI_SPIDERMINE_DESCRIPTION", $"放置一枚机器人地雷，在敌人走近时自动引爆，造成<style=cIsDamage>{Helpers.D(SpiderMines.damage)}%的伤害</style>，最多放置{SpiderMines.charges}枚<style=cStack>(每级+1枚)</style>。", "zh-CN");
            AddOverlay("HAT_MAGE_UTILITY_FIRE_DESCRIPTION", "<style=cIsUtility>灵巧</style>。<style=cIsDamage>点燃</style>。向前冲刺，在身后召唤造成每秒<style=cIsDamage>" + Helpers.D(HIFUArtificerTweaks.Main.flamewallDamage.Value) + "%伤害的火柱</style>。", "zh-CN");
            AddOverlay("HAT_MAGE_UTILITY_FIRE_NAME", "火墙", "zh-CN");
            AddOverlay("HUNTRESS_PRIMARY_ALT_DESCRIPTION", "<style=cIsUtility>灵巧</style>。拉弓射出<style=cIsDamage>3枚<style=cStack>(每6级+1枚)</style></style>跟踪箭，每枚造成<style=cIsDamage>120%的伤害</style>。如果暴击则发射<style=cIsDamage>双倍</style>跟踪箭。", "zh-CN");
            AddOverlay("HUNTRESS_PRIMARY_DESCRIPTION", "<style=cIsUtility>灵巧</style>。快速射出一名能够造成<style=cIsDamage>180%<style=cStack>（每级+20%）</style>伤害</style>的跟踪箭。(暂时未想到更好的修改方式)", "zh-CN");
            AddOverlay("HUNTRESS_SECONDARY_DESCRIPTION", $"{(LaserGlaive.agile ? "<style=cIsUtility>灵巧</style>。" : "")}投掷一把追踪月刃，可弹射最多<style=cIsDamage>{LaserGlaive.bounceCount}</style>次，初始造成<style=cIsDamage>{Helpers.D(LaserGlaive.damage)}%的伤害</style>，每次弹射伤害增加<style=cIsDamage>{Math.Round((double)((LaserGlaive.bounceDamage - 1f) * 100f), 1)}%</style>。", "zh-CN");
            AddOverlay("HUNTRESS_SPECIAL_ALT1_DESCRIPTION", $"向后<style=cIsUtility>传送</style>至空中。最多发射<style=cIsDamage>{Ballista.boltCount}</style>道能量闪电，造成<style=cIsDamage>{Ballista.boltCount}x{Ballista.damage * 100}%的伤害</style>。", "zh-CN");
            AddOverlay("HUNTRESS_SPECIAL_DESCRIPTION", $"<style=cIsUtility>传送</style>至空中，向目标区域射下箭雨，使区域内所有敌人<style=cIsUtility>减速</style>，并造成<style=cIsDamage>每秒{300f * ArrowRain.damage}%的伤害</style>。", "zh-CN");
            AddOverlay("ITEM_AbyssalMedkit_DESCRIPTION", "<style=cIsUtility>消耗品</style>，抵挡<style=cIsUtility>10次</style>减益后失效。每一次抵挡都有<style=cIsHealing>10%</style>概率给予你<style=cIsHealing>“祝·福”</style>。每个<style=cIsHealing>祝福</style>可使你<style=cIsUtility>所有属性提升3%</style>。<style=cIsVoid>使所有医疗包无效化</style>", "zh-CN");
            AddOverlay("ITEM_AbyssalMedkit_PICKUP", "消耗品，可以替你抵挡10次减益，每一次抵挡都有概率给予你“祝·福”", "zh-CN");
            AddOverlay("ITEM_INFUSION_DESC", "每击败一名敌人，即可<style=cIsHealing>永久性</style>增加<style=cIsHealing>" + BtpTweak.浸剂击杀奖励倍率_.Value + "</style>点<style=cStack>（每层增加" + BtpTweak.浸剂击杀奖励倍率_.Value + "点）</style>生命值，<style=cIsHealing>无上限</style>。\n<style=cIsUtility>此物品不会被米斯历克斯拿走</style>。", "zh-CN");
            AddOverlay("KEYWORD_ARC", "<style=cKeywordName>击穿</style><style=cSub>在最多4个敌人之间形成电弧，每次造成30%的伤害。</style>", "zh-CN");
            AddOverlay("KEYWORD_EMPOWERING", "<style=cKeywordName>授权</style><style=cSub>使用此技能后短时间内增强你的其他技能。\n火神散弹枪：<style=cIsDamage>攻速+75%</style>。\n轨道探测器：<style=cIsDamage>伤害+400%</style>。\nOGM-72“大恶魔”打击：<style=cIsDamage>范围+100%</style>。</style>", "zh -CN");
            AddOverlay("KEYWORD_FLEETING", "<style=cKeywordName>一闪</style><style=cSub><style=cIsDamage>攻速</style>转化为<style=cIsDamage>技能伤害</style>。", "zh-CN");
            AddOverlay("KEYWORD_FRICTIONLESS", "<style=cKeywordName>绝对光滑</style><style=cSub>无伤害衰减</style>。", "zh-CN");
            AddOverlay("KEYWORD_SOULROT", "<style=cKeywordName>灵魂之痛</style><style=cSub>持续<style=cIsVoid>666</style>秒的<style=cIsVoid>致命debuff</style>。造成的<style=cIsDamage>伤害</style>基于敌人的<style=cIsHealing>最大生命值</style>。</style>", "zh-CN");
            AddOverlay("KEYWORD_VERY_HEAVY", "<style=cKeywordName>超重</style><style=cSub>下落速度越快，技能造成的伤害越高。<color=#FF7F7F>小心冲击力</color>。</style>", "zh-CN");
            AddOverlay("MAGE_PRIMARY_FIRE_DESCRIPTION", "<style=cIsDamage>点燃</style>。发射火焰弹，造成<style=cIsDamage>220%的伤害</style>。</style>", "zh-CN");
            AddOverlay("MAGE_PRIMARY_LIGHTNING_DESCRIPTION", $"发射一道闪电，造成<style=cIsDamage>300%的伤害</style>并<style=cIsDamage>引爆</style>小片区域。</style>", "zh-CN");
            AddOverlay("MAGE_SECONDARY_ICE_DESCRIPTION", "<style=cIsUtility>冰冻</style>。使用拥有<style=cIsDamage>穿透</style>效果的纳米枪发动攻击，充能后能造成<style=cIsDamage>400%-1200%</style>的伤害，爆炸时留下一个<style=cIsUtility>冰冻炸弹</style>，2秒后爆炸，造成纳米炸弹<style=cIsDamage>一半的伤害</style>并<style=cIsUtility>冰冻</style>附件6米<style=cStack>（每有一个鲁纳德的手环+1米）</style>的敌人，暴击时<style=cIsUtility>冰冻</style>范围翻倍。", "zh-CN");
            AddOverlay("MAGE_SECONDARY_LIGHTNING_DESCRIPTION", "<style=cIsDamage>眩晕</style>。发射一枚会<style=cIsDamage>爆炸并分裂0<style=cStack>(每4级+1)</style>颗闪电球</style>的纳米炸弹，如果充能将造成<style=cIsDamage>500%-2500%</style>的伤害（每颗闪电球造成<style=cIsDamage>一半</style>伤害）。", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_DESC", "同时持有<style=cIsUtility>两件装备</style>。激活'转换器'可以切换<style=cIsUtility>激活的装备</style>和<style=cIsDamage>磁轨炮手的次要技能攻击</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_DESC_ALT", "切换磁轨炮手的装备和瞄准镜", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_NAME", "转换器", "zh-CN");
            AddOverlay("SPIKESTRIPSKILL_DEEPROT_DESCRIPTION", "<style=cIsVoid>虚 空 馈 赠</style>：<style=cIsHealing>毒化</style>攻击改为叠加<style=cIsVoid>虚空之毒</style>，使<style=cIsVoid>速度减慢10%</style>。叠加<style=cIsVoid>5</style>次后，<style=cIsVoid>虚空之毒</style>将转化为<style=cIsVoid>灵魂之痛</style>，造成巨量伤害！所有<style=cArtifact>虚空</style>攻击都有几率<style=cArtifact>定身</style>敌人。 (如果选择了“腐朽”被动，则额外造成<style=cIsVoid>虚空之毒</style>减益)", "zh-CN");
            AddOverlay("SPIKESTRIPSKILL_DEEPROT_NAME", "腐朽", "zh-CN");
            AddOverlay("TOOLBOT_PRIMARY_ALT1_DESCRIPTION", "发射1条具有穿透效果的钢筋，造成<style=cIsDamage>600%</style>的伤害。", "zh-CN");
            AddOverlay("TOOLBOT_PRIMARY_ALT3_DESCRIPTION", "锯伤周围敌人，造成<style=cIsDamage>每秒1000%的伤害</style><style=cStack>（按住伤害会慢慢递增，增加的量随时间减少，松开后清零）</style>。", "zh-CN");
            AddOverlay("TOOLBOT_PRIMARY_DESCRIPTION", "快速发射钉子，造成<style=cIsDamage>77%<style=cStack>（每发射1发钉子+0.1%，最高到210%，松开后清零）</style>的伤害</style>。最后一次性发射<style=cIsDamage>12</style>枚伤害为<style=cIsDamage>70%<style=cStack>（每级+7%）</style><style=cStack>的钉子。", "zh-CN");
            AddOverlay("VOIDCRID_ENTROPY", "<style=cArtifact>「虚混<style=cIsHealing>无</style>乱』</style>", "zh-CN");
            AddOverlay("VOIDCRID_FLAMEBREATH", "火焰吐息", "zh-CN");
            AddOverlay("VOIDCRID_FLAMEBREATH_DESC", "<style=cDeath>点燃</style>。<style=cIsDamage>灵巧</style>。向前方喷出<style=cIsDamage>火焰</style>，<style=cDeath>点燃</style>敌人，造成<style=cIsDamage>250%</style>的伤害。", "zh-CN");
            AddOverlay("VOIDCRID_NULLBEAM", "<style=cArtifact>「虚空光束』</style>", "zh-CN");
            AddOverlay("VOIDCRID_NULLBEAM_DESC", "<style=cArtifact>虚空</style>。从<style=cArtifact>虚空</style>中汲取力量，发射中距离<style=cDeath>虚空光束</style>攻击敌人，造成<style=cIsDamage>900%</style>的伤害。", "zh-CN");
            AddOverlay("VOIDCRID_PASSIVE", "<style=cArtifact>虚空</style>克里德", "zh-CN");
            AddOverlay("VOIDCRID_PASSIVE_DESC", "所有<style=cArtifact>虚空</style>攻击都有几率<style=cArtifact>定身</style>敌人。 (如果选择了“腐朽”被动，则额外造成<style=cWorldEvent>虚空之毒</style>减益)", "zh-CN");
            AddOverlay("VOIDCRID_SCEPTER_ENTROPY", "<style=cArtifact>「幻影? 虚<style=cIsHealing>乱</style>无混』</style>", "zh-CN");
            AddOverlay("VOIDCRID_VOIDDRIFT", "<style=cArtifact>「虚无漂流』</style>", "zh-CN");
            AddOverlay("VOIDCRID_VOIDRIFT_DESC", "<style=cArtifact>虚空</style>。<style=cIsDamage>眩晕</style>。遁入<style=cArtifact>虚空</style>，造成<style=cIsDamage>400%</style>的总伤害，有概率直接<style=cIsVoid>监禁</style>敌人。", "zh-CN");
            AddOverlay("VOIDSURVIVOR_PRIMARY_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>发射一束造成2000%伤害的短程光束。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_PRIMARY_DESCRIPTION", "发射一束<style=cIsUtility>减速</style>远程光束，造成<style=cIsDamage>360%伤害</style>。", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SECONDARY_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>发射一枚造成2500%伤害的黑洞炸弹，半径变为25米。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SECONDARY_DESCRIPTION", "充能一枚虚空炸弹，造成<style=cIsDamage>666%伤害</style>。完全充能时可以变成爆炸性虚空炸弹，造成<style=cIsDamage>4444%伤害</style>。", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SECONDARY_UPRADE_TOOLTIP", "<style=cKeywordName>腐化升级</style><style=cSub>转化成能造成2500%伤害的黑洞炸弹，半径变为25米。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SPECIAL_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>消耗25%的生命值来获得25%的腐化。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_UTILITY_ALT_DESCRIPTION", "<style=cIsUtility>消失</style>进入虚空，<style=cIsUtility>向前沿弧线</style>移动，同时<style=cIsUtility>清除所有减益效果</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_DESCRIPTION", "杀死一个敌人会使<style=cIsDamage>10</style>米<style=cStack>(每层+2.5米)</style>内的所有敌人变慢，造成<style=cIsDamage>15%</style><style=cStack>(每层+15%)</style>的伤害，持续<style=cIsUtility>4</style>秒<style=cStack>(每层+2秒)</style>。<style=cIsVoid>腐化所有汽油</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_NAME", "超临界冷却剂", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_PICKUP", "杀死一个敌人会减缓附近的其他敌人。<style=cIsVoid>腐化所有汽油</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CrystalLotus.instance.ItemLangTokenName + "_DESCRIPTION", "在传送事件中释放一个<style=cIsUtility>减速</style>脉冲，使敌人和投射物<style=cIsUtility>减速</style>92.5%，持续30秒，发生<style=cIsHealing>1</style>次<style=cStack>（每层+1次）</style>。<style=cIsVoid>腐化所有轻粒子雏菊</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CrystalLotus.instance.ItemLangTokenName + "_NAME", "结晶的莲花", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CrystalLotus.instance.ItemLangTokenName + "_PICKUP", "在传送事件和‘滞留区’（如虚空领域）中定期释放减速脉冲。<style=cIsVoid>腐化所有轻粒子雏菊</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName + "_DESCRIPTION", "你的<style=cIsDamage>击杀效果</style>在杀死一个精英后会额外发生<style=cIsDamage>1</style>次<style=cStack>（每层+1次）</style>。另外会产生半径<style=cIsDamage>12</style>米的<style=cIsDamage>爆炸</style>，造成<style=cIsDamage>100%</style>的伤害。<style=cIsVoid>腐化所有陈旧断头台</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName + "_NAME", "刽子手的重负", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName + "_PICKUP", "你的击杀效果在杀死一个精英后会额外发生一次。在杀死精英时还会造成一个伤害性的AOE。<style=cIsVoid>腐化所有陈旧断头台</style>。", "zh-CN");
            string text = Language.GetString("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_DESCRIPTION");
            text = text.Replace(") will appear in a random location <style=cIsUtility>on each stage</style>. <style=cStack>(Increases rarity chances of the items per stack).</style> <style=cIsVoid>Corrupts all 运输申请单s</style>.", "）的<style=cIsUtility>深空信号</style>。<style=cStack>（层数越高，该物品拥有高稀有度的几率越高）</style>。<style=cIsVoid>腐化所有运输申请单</style>。");
            text = text.Replace("A <style=cIsVoid>special</style> delivery containing items (", "在<style=cIsUtility>每个关卡中</style>，都会在随机位置生成一个内含特殊物品（");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_DESCRIPTION", text, "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_NAME", "无尽的聚宝盆", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_PICKUP", "获得一个特殊的、危险的快递，并获得强大的奖励。<style=cIsVoid>腐化所有运输申请单</style>。", "zh-CN");
        }

        public static void 圣骑士汉化() {
            string text = "<color=yellow>(本人物由QQ用户“疯狂”(2437181705)翻译)</color>圣骑士是一位重击坦克，可以选择超凡的魔法或毁灭性的剑术来帮助盟友和消灭敌人。<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 你的被动状态占了你伤害的很大一部分，尽可能地保持它。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 旋转猛击既可以作为强大的群体控制技能，也可以作为限制移动的一种形式。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 疾走（Quickstep）的冷却时间随着每一次命中而降低，这是对你坚持到底的奖励。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 沉默誓言（Vow of Silence）是对付飞行敌人的好方法，因为它会把所有受影响的敌人拖到地上。</color>" + Environment.NewLine + Environment.NewLine;
            AddOverlay("PALADIN_NAME", "圣骑士", "zh-CN");
            AddOverlay("PALADIN_DESCRIPTION", text, "zh-CN");
            AddOverlay("PALADIN_SUBTITLE", "普罗维登斯的侍从", "zh-CN");
            AddOverlay("PALADIN_OUTRO_FLAVOR", "..于是他离开了，对他的教义的信心动摇了。", "zh-CN");
            AddOverlay("PALADIN_OUTRO_FAILURE", "..于是他消失了，他的祈祷没有人听到。", "zh-CN");
            AddOverlay("PALADINBODY_DEFAULT_SKIN_NAME", "默认", "zh-CN");
            AddOverlay("PALADINBODY_LUNAR_SKIN_NAME", "月球", "zh-CN");
            AddOverlay("PALADINBODY_LUNARKNIGHT_SKIN_NAME", "月光骑士", "zh-CN");
            AddOverlay("PALADINBODY_TYPHOON_SKIN_NAME", "君主", "zh-CN");
            AddOverlay("PALADINBODY_TYPHOONLEGACY_SKIN_NAME", "君主（经典）", "zh-CN");
            AddOverlay("PALADINBODY_POISON_SKIN_NAME", "腐败", "zh-CN");
            AddOverlay("PALADINBODY_POISONLEGACY_SKIN_NAME", "腐败（经典）", "zh-CN");
            AddOverlay("PALADINBODY_CLAY_SKIN_NAME", "阿菲利安", "zh-CN");
            AddOverlay("PALADINBODY_SPECTER_SKIN_NAME", "幽灵", "zh-CN");
            AddOverlay("PALADINBODY_DRIP_SKIN_NAME", "Drip", "zh-CN");
            AddOverlay("PALADINBODY_MINECRAFT_SKIN_NAME", "我的世界", "zh-CN");
            AddOverlay("LUNAR_KNIGHT_BODY_NAME", "月光骑士", "zh-CN");
            AddOverlay("LUNAR_KNIGHT_BODY_DESCRIPTION", text, "zh-CN");
            AddOverlay("LUNAR_KNIGHT_BODY_SUBTITLE", "米斯历克斯的侍从", "zh-CN");
            AddOverlay("LUNAR_KNIGHT_BODY_OUTRO_FLAVOR", "..于是他离开了，对他的教义的信心动摇了。", "zh-CN");
            AddOverlay("NEMPALADIN_NAME", "复仇圣骑士", "zh-CN");
            AddOverlay("NEMPALADIN_DESCRIPTION", text, "zh-CN");
            AddOverlay("NEMPALADIN_SUBTITLE", "普罗维登斯的侍从", "zh-CN");
            AddOverlay("NEMPALADIN_OUTRO_FLAVOR", "..于是他离开了，对他的教义的信心动摇了。", "zh-CN");
            AddOverlay("NEMPALADIN_OUTRO_FAILURE", "..于是他消失了，他的祈祷没有人听到。", "zh-CN");
            AddOverlay("PALADIN_PASSIVE_NAME", "堡垒的祝福", "zh-CN");
            AddOverlay("PALADIN_PASSIVE_DESCRIPTION", "每级获得<style=cIsHealing>1护甲</style>。当拥有<style=cIsHealth>90%生命值</style>或者拥有任意<style=cIsHealth>护盾</style>时，圣骑士会获得<style=cIsHealing>祝福</style>，强化所有剑技。", "zh-CN");
            text = "向前劈砍，造成<style=cIsDamage>350%伤害</style>。如果圣骑士拥有<style=cIsHealing>祝福</style>，会发射一道<style=cIsUtility>剑光</style>造成<style=cIsDamage>300%伤害</style>。";
            AddOverlay("PALADIN_PRIMARY_SLASH_NAME", "神圣之剑", "zh-CN");
            AddOverlay("PALADIN_PRIMARY_SLASH_DESCRIPTION", text, "zh-CN");
            text = "向前劈砍，造成<style=cIsDamage>380%伤害</style>。如果圣骑士拥有<style=cIsHealing>祝福</style>，会发射一道<style=cIsUtility>剑光</style>造成<style=cIsDamage>300%伤害</style>。";
            AddOverlay("PALADIN_PRIMARY_CURSESLASH_NAME", "诅咒之剑", "zh-CN");
            AddOverlay("PALADIN_PRIMARY_CURSESLASH_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>眩晕</style>。使用一个大范围的劈砍，造成<style=cIsDamage>1000% 伤害</style>，如果拥有<style=cIsHealing>祝福</style>会提升劈砍范围。在空中释放会变成裂地斩，如果拥有<style=cIsHealing>祝福</style>会造成<style=cIsUtility>冲击波</style>。";
            AddOverlay("PALADIN_SECONDARY_SPINSLASH_NAME", "回旋斩", "zh-CN");
            AddOverlay("PALADIN_SECONDARY_SPINSLASH_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>震荡</style>。<style=cIsUtility>灵巧</style>。充能并扔出一道<style=cIsUtility>闪电束</style>，造成<style=cIsDamage>800%伤害</style>。如果被闪电束击中，会在剑上附着<style=cIsUtility>闪电</style>效果，持续<style=cIsUtility>4秒</style>。";
            AddOverlay("PALADIN_SECONDARY_LIGHTNING_NAME", "圣光之矛", "zh-CN");
            AddOverlay("PALADIN_SECONDARY_LIGHTNING_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>灵巧</style>。发射一连串<style=cIsUtility>月球碎片</style>，每个造成<style=cIsDamage>75%伤害</style>。最多拥有<style=cIsDamage>12</style>碎片。";
            AddOverlay("PALADIN_SECONDARY_LUNARSHARD_NAME", "月球碎片", "zh-CN");
            AddOverlay("PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>冲刺</style>一小段距离并获得<style=cIsHealing>10%屏障</style>。成功使用<style=cIsDamage>神圣之剑</style>击中敌人会<style=cIsUtility>减少冷却</style><style=cIsDamage>1秒</style>。<style=cIsUtility>最多可储存2次充能<style=cIsHealing>";
            AddOverlay("PALADIN_UTILITY_DASH_NAME", "疾走", "zh-CN");
            AddOverlay("PALADIN_UTILITY_DASH_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "恢复<style=cIsHealing>",
                15.ToString(),
                "%最大生命值</style>并且为范围内的盟友提供<style=cIsHealing>",
                15.ToString(),
                "%屏障</style>。"
            });
            AddOverlay("PALADIN_UTILITY_HEAL_NAME", "补给", "zh-CN");
            AddOverlay("PALADIN_UTILITY_HEAL_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                1.5f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>祝福</style>区域，持续<style=cIsDamage>",
                12f.ToString(),
                "秒</style>。在范围内的所有盟友会缓慢<style=cIsHealing>恢复生命值</style>并且获得<style=cIsHealing>屏障</style>。"
            });
            AddOverlay("PALADIN_SPECIAL_HEALZONE_NAME", "神圣之光", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_HEALZONE_DESCRIPTION", text, "zh-CN");
            text += Helpers.ScepterDescription("双倍治疗。双倍屏障。清除减益。");
            AddOverlay("PALADIN_SPECIAL_SCEPTERHEALZONE_NAME", "神圣之光", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SCEPTERHEALZONE_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>沉默</style>区域，持续<style=cIsDamage>",
                10f.ToString(),
                "秒</style>，使区域内所有敌人<style=cIsHealth>麻木</style>。（禁用技能和特殊效果）"
            });
            AddOverlay("PALADIN_SPECIAL_TORPOR_NAME", "沉默誓言", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_TORPOR_DESCRIPTION", text, "zh-CN");
            text += Helpers.ScepterDescription("更强的减益。更大的范围。摧毁投射物。");
            AddOverlay("PALADIN_SPECIAL_SCEPTERTORPOR_NAME", "沉默誓言", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SCEPTERTORPOR_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>，然后释放一个<style=cIsUtility>强化</style>区域，持续<style=cIsDamage>",
                8f.ToString(),
                "秒</style>，提升范围内的所有盟友<style=cIsDamage>伤害</style>和<style=cIsDamage>攻速</style>。"
            });
            AddOverlay("PALADIN_SPECIAL_WARCRY_NAME", "神圣誓言", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_WARCRY_DESCRIPTION", text, "zh-CN");
            text += Helpers.ScepterDescription("更快的施法速度。双倍伤害。双倍攻速。");
            AddOverlay("PALADIN_SPECIAL_SCEPTERWARCRY_NAME", "神圣誓言(Scepter)", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SCEPTERWARCRY_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsHealth>过热</style>。<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>微型太阳</style>，持续<style=cIsDamage>",
                12.5f.ToString(),
                "</style>秒，使周围<style=cDeath>一切生物</style><style=cIsHealth>过热</style>（包括自己和队友）。在堆叠<style=cIsHealth>",
                2.ToString(),
                "</style>层或者更多时，目标会燃烧并造成<style=cIsDamage>",
                160f.ToString(),
                "%伤害</style>。"
            });
            AddOverlay("PALADIN_SPECIAL_SUN_NAME", "暴烈之阳", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SUN_DESCRIPTION", text, "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SUN_CANCEL_NAME", "取消暴烈之阳", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SUN_CANCEL_DESCRIPTION", "停止引导当前的暴烈之阳。", "zh-CN");
            text += Helpers.ScepterDescription("再次投掷并保持瞄准，然后释放太阳，爆炸对周围<style=cDeath>一切生物</style>造成<style=cIsDamage>4000%伤害</style>。");
            AddOverlay("PALADIN_SPECIAL_SCEPSUN_NAME", "太阳耀斑", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SCEPSUN_DESCRIPTION", text, "zh-CN");

            text = "<style=cIsUtility>引导</style><style=cIsDamage>5</style>秒，然后在指定位置创造一个<style=cIsUtility>微型太阳</style>，吸收周围<style=cIsHealth>所有</style>生物的<style=cIsDamage>生命</style> 。<color=red>让火焰净化一切！</color>";
            AddOverlay("PALADIN_SPECIAL_SUN_LEGACY_NAME", "暴烈之阳(经典)", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SUN_LEGACY_DESCRIPTION", text, "zh-CN");
            text += Helpers.ScepterDescription("产生大规模爆炸，造成9000%伤害。");
            AddOverlay("PALADIN_SPECIAL_SCEPSUN_LEGACY_NAME", "太阳耀斑", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_SCEPSUN_LEGACY_DESCRIPTION", text, "zh-CN");

            text = "While below <style=cIsHealth>25% health</style>, generate <style=cIsDamage>Rage</style>. When at max <style=cIsDamage>Rage</style>, use to enter <color=#dc0000>Berserker Mode</color>, gaining a <style=cIsHealing>massive buff</style> and a <style=cIsUtility>new set of skills</style>.";
            AddOverlay("PALADIN_SPECIAL_BERSERK_NAME", "狂暴", "zh-CN");
            AddOverlay("PALADIN_SPECIAL_BERSERK_DESCRIPTION", text, "zh-CN");
            AddOverlay("KEYWORD_SWORDBEAM", "<style=cKeywordName>剑光</style><style=cSub>一道短距离可穿透的光束，造成<style=cIsDamage>300%伤害</style>.", "zh-CN");
            AddOverlay("KEYWORD_TORPOR", "<style=cKeywordName>麻木</style><style=cSub>造成<style=cIsHealth>60%</style>移动和攻击速度<style=cIsDamage>减缓</style>。<style=cIsHealth>将敌人拖至地面。</style>", "zh-CN");
            AddOverlay("KEYWORD_OVERHEAT", "<style=cKeywordName>过热</style><style=cSub>乘以从<style=cIsDamage>太阳</style>受到的伤害。</style>", "zh-CN");
            AddOverlay("PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME", "圣骑士的誓言", "zh-CN");
            AddOverlay("PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC", "使用“忠诚之珠”，再次变得完整。", "zh-CN");
            AddOverlay("PALADIN_UNLOCKABLE_UNLOCKABLE_NAME", "圣骑士的誓言", "zh-CN");
            AddOverlay("PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "圣骑士：精通", "zh-CN");
            AddOverlay("PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，击败游戏或消灭季风。", "zh-CN");
            AddOverlay("PALADIN_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "圣骑士：精通", "zh-CN");
            string str = "\n<color=#8888>(台风困难需要星际风暴2模组)</color>";
            AddOverlay("PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME", "圣骑士：大师", "zh-CN");
            AddOverlay("PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，在台风或更高级别难度下通关或者抹除自己。" + str, "zh-CN");
            AddOverlay("PALADIN_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME", "圣骑士：大师", "zh-CN");
            AddOverlay("PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_NAME", "他的弟子", "zh-CN");
            AddOverlay("PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，与腐败女神立约。（看不懂）", "zh-CN");
            AddOverlay("PALADIN_POISONUNLOCKABLE_UNLOCKABLE_NAME", "她的弟子", "zh-CN");
            AddOverlay("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME", "Jolly Cooperation", "zh-CN");
            AddOverlay("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，用皇家电容器打击敌人。<color=#c11>仅主机</color>", "zh-CN");
            AddOverlay("PALADIN_LIGHTNINGSPEARUNLOCKABLE_UNLOCKABLE_NAME", "Jolly Cooperation", "zh-CN");
            AddOverlay("PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_NAME", "迷失国王的先驱", "zh-CN");
            AddOverlay("PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，一次持有8件月球物品。", "zh-CN");
            AddOverlay("PALADIN_LUNARSHARDUNLOCKABLE_UNLOCKABLE_NAME", "迷失国王的先驱", "zh-CN");
            AddOverlay("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME", "温暖的拥抱", "zh-CN");
            AddOverlay("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，用身形扭曲的木灵治疗一个盟友。<color=#c11>仅主机</color>", "zh-CN");
            AddOverlay("PALADIN_HEALUNLOCKABLE_UNLOCKABLE_NAME", "温暖的拥抱", "zh-CN");
            AddOverlay("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME", "抑制", "zh-CN");
            AddOverlay("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，对一个敌人堆叠4层debuff。", "zh-CN");
            AddOverlay("PALADIN_TORPORUNLOCKABLE_UNLOCKABLE_NAME", "抑制", "zh-CN");
            AddOverlay("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_NAME", "阳光", "zh-CN");
            AddOverlay("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，承受太阳的全部冲击并生存下来。", "zh-CN");
            AddOverlay("PALADIN_CRUELSUNUNLOCKABLE_UNLOCKABLE_NAME", "阳光", "zh-CN");
            AddOverlay("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_NAME", "古代遗物", "zh-CN");
            AddOverlay("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，获得一个泥沼之瓮。", "zh-CN");
            AddOverlay("PALADIN_CLAYUNLOCKABLE_UNLOCKABLE_NAME", "古代遗物", "zh-CN");
            AddOverlay("BROTHER_SEE_PALADIN_1", "兄弟？不，便宜的仿制品。", "zh-CN");
            AddOverlay("BROTHER_SEE_PALADIN_2", "我会对你的信仰负责。", "zh-CN");
            AddOverlay("BROTHER_SEE_PALADIN_3", "浪费潜力。", "zh-CN");
            AddOverlay("BROTHER_KILL_PALADIN_1", "你粗糙的盔甲让你失望了。", "zh-CN");
            AddOverlay("BROTHER_KILL_PALADIN_2", "看看你的信仰给你带来了什么。", "zh-CN");
            AddOverlay("BROTHER_KILL_PALADIN_3", "一无所获，愚蠢的奉献者。", "zh-CN");
        }

        private static void 探路者汉化() {
            string str = "BOG";
            string str2 = str + "_PATHFINDER_BODY_";
            string str3 = str + "_SQUALL_BODY_";
            AddOverlay(str3 + "NAME", "狂风", "zh-CN");
            AddOverlay(str2 + "NAME", "探路者", "zh-CN");
            AddOverlay(str2 + "SUBTITLE", "猛禽", "zh-CN");
            AddOverlay(str2 + "DEFAULT_SKIN_NAME", "默认", "zh-CN");
            AddOverlay(str2 + "MASTERY_SKIN_NAME", "猎头者", "zh-CN");
            AddOverlay(str2 + "PASSIVE_NAME", "驯鹰者", "zh-CN");
            AddOverlay(str2 + "PASSIVE_DESCRIPTION", "一只机器猎鹰（狂风）将跟随你，它会继承你的<style=cIsDamage>大部分</style>物品，并且<style=cIsUtility>免疫</style>所有伤害，但它靠<style=cIsUtility>电池</style>运行。", "zh-CN");
            AddOverlay("KEYWORD_BATTERY", "<style=cKeywordName>电池</style><style=cSub>狂风有两种模式：<color=#FF0000>攻击</color>和<color=#00FF00>跟随</color>。在<color=#FF0000>攻击模式</color>下，狂风每秒<style=cIsDamage>消耗 8%</style>的电量。在<color=#00FF00>跟随模式</color>下，狂风每秒<style=cIsHealing>充能 1%</style>电量，速度随<style=cIsUtility>攻速</style>变换。如果电量耗尽，狂风将强制进入<color=#00FF00>跟随模式</color>。</style>", "zh-CN");
            AddOverlay("KEYWORD_PIERCE", "<style=cKeywordName>穿孔</style><style=cSub>用尖端<style=cIsUtility>攻击</style>造成<style=cIsDamage>325%伤害</style>并忽略<style=cIsDamage>敌人护甲</style>。</style>", "zh-CN");
            AddOverlay("KEYWORD_ELECTROCUTE", "<style=cKeywordName>电击</style><style=cSub>使目标移动速度降低50%，每秒造成<style=cIsDamage>120% 的伤害</style>。</style>", "zh-CN");
            AddOverlay("KEYWORD_ATTACK", "<style=cKeywordName><color=#FF0000>攻击</color></style><style=cSub>指挥狂风攻击敌人，并激活<color=#FF0000>攻击模式</color>，使用机枪造成<style=cIsDamage>2x50% 伤害</style>，发射导弹造成<style=cIsDamage>300% 伤害</style>。</style>", "zh-CN");
            AddOverlay("KEYWORD_FOLLOW", "<style=cKeywordName><color=#00FF00>跟随</color></style><style=cSub>让狂风回到身边，激活<color=#00FF00>跟随模式</color>，使狂风跟随在身边。</style>", "zh-CN");
            AddOverlay("KEYWORD_SQUALL_UTILITY", "<style=cKeywordName><color=#87b9cf>辅助</color></style><style=cSub>指挥狂风使用<style=cIsUtility>辅助</style>技能。</style>", "zh-CN");
            AddOverlay("KEYWORD_SQUALL_SPECIAL", "<style=cKeywordName><color=#efeb1c>特殊--追击!</color></style><style=cSub>指挥狂风反复打击目标敌人，造成<style=cIsDamage>70% 伤害</style>。每次攻击都会降低目标<style=cIsDamage>5点</style><style=cIsDamage>护甲</style> ，并<style=cIsUtility>充能 2%</style>电量，暴击时有双倍效果。此技能可将暂时将电量充能到<style=cIsUtility>120%</style>。</style>", "zh-CN");
            AddOverlay(str2 + "PRIMARY_THRUST_NAME", "突刺", "zh-CN");
            AddOverlay(str2 + "PRIMARY_THRUST_DESCRIPTION", "<style=cIsUtility>穿孔</style>。将你的长矛向前刺去，造成<style=cIsDamage>250% 伤害</style>。", "zh-CN");
            AddOverlay(str2 + "SECONDARY_DASH_NAME", "旋风腿", "zh-CN");
            AddOverlay(str2 + "SECONDARY_DASH_DESCRIPTION", "<style=cIsUtility>冲刺</style>。短距离冲刺后，将矛举起准备投掷。投掷矛将产生<style=cIsDamage>爆炸</style>造成<style=cIsDamage>1000% 伤害</style>。", "zh-CN");
            AddOverlay(str2 + "SECONDARY_JAVELIN_NAME", "爆炸标枪", "zh-CN");
            AddOverlay(str2 + "SECONDARY_JAVELIN_DESCRIPTION", "投掷<style=cIsDamage>爆炸性</style>标枪，造成<style=cIsDamage>1000% 伤害</style>。", "zh-CN");
            AddOverlay(str2 + "UTILITY_SPIN_NAME", "撕裂之爪", "zh-CN");
            AddOverlay(str2 + "UTILITY_BOLAS_NAME", "闪电套索", "zh-CN");
            AddOverlay(str2 + "UTILITY_BOLAS_DESCRIPTION", "投掷带电的套索，<style=cIsUtility>电击</style>周围敌人，并产生<style=cIsUtility>电击</style>区域，存在<style=cIsUtility>10，秒</style>。", "zh-CN");
            AddOverlay(str2 + "SPECIAL_COMMAND_NAME", "指令", "zh-CN");
            AddOverlay(str2 + "SPECIAL_COMMAND2_NAME", "指令 - 辅助", "zh-CN");
            AddOverlay(str2 + "SPECIAL_COMMAND_DESCRIPTION", "向狂风发出指令。你可以指挥狂风<color=#FF0000>攻击</color>，<color=#00FF00>跟随</color>或<color=#efeb1c>特殊指令</color>。", "zh-CN");
            AddOverlay(str2 + "SPECIAL_COMMAND2_DESCRIPTION", "<color=#3ea252>特殊指令</color>。狂风的<color=#efeb1c>特殊指令</color>现在替换了你的<style=cIsUtility>特殊</style>技能。", "zh-CN");
            AddOverlay(str2 + "SPECIAL_ATTACK_NAME", "攻击 - 指令", "zh-CN");
            AddOverlay(str2 + "SPECIAL_ATTACK_DESCRIPTION", "</style><style=cSub>指挥狂风攻击敌人，并激活<color=#FF0000>攻击模式</color>，使用机枪造成<style=cIsDamage>2x50% 伤害</style>，发射导弹造成<style=cIsDamage>300% 伤害</style>。</style>", "zh-CN");
            AddOverlay(str2 + "SPECIAL_FOLLOW_NAME", "跟随 - 指令", "zh-CN");
            AddOverlay(str2 + "SPECIAL_FOLLOW_DESCRIPTION", "<style=cSub>让狂风回到身边，激活<color=#00FF00>跟随模式</color>，使狂风跟随在身边。</style>", "zh-CN");
            AddOverlay(str2 + "SPECIAL_CANCEL_NAME", "取消", "zh-CN");
            AddOverlay(str2 + "SPECIAL_CANCEL_DESCRIPTION", "取消", "zh-CN");
            AddOverlay(str + "_SQUALL_SPECIAL_GOFORTHROAT_NAME", "追击!", "zh-CN");
            AddOverlay(str + "_SQUALL_SPECIAL_GOFORTHROAT_DESCRIPTION", "</style><style=cSub>指挥狂风反复打击目标敌人，造成<style=cIsDamage>70% 伤害</style>。每次攻击都会降低目标<style=cIsDamage>5点</style><style=cIsDamage>护甲</style> ，并<style=cIsUtility>充能 2%</style>电量，暴击时有双倍效果。此技能可将暂时将电量充能到<style=cIsUtility>120%</style>。</style>", "zh-CN");
            AddOverlay(str2 + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "探路者：精通", "zh-CN");
            AddOverlay(str2 + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "作为探路者，在季风难度下通关或抹除自己。", "zh-CN");
            AddOverlay(str2 + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "探路者：精通", "zh-CN");
        }

        private static void 物品调整() {
            Helpers.AddTag(ref RoR2Content.Items.ExtraLife, ItemTag.CannotSteal);
            Helpers.AddTag(ref RoR2Content.Items.Infusion, ItemTag.CannotSteal);
            if (AncientScepter.AncientScepterItem.instance.ItemDef.DoesNotContainTag(ItemTag.CannotSteal)) {
                List<ItemTag> itemTags = new();
                for (int i = 0; i < AncientScepter.AncientScepterItem.instance.ItemDef.tags.Length; ++i) {
                    itemTags.Add(AncientScepter.AncientScepterItem.instance.ItemDef.tags[i]);
                }
                itemTags.Add(ItemTag.CannotSteal);
                AncientScepter.AncientScepterItem.instance.ItemDef.tags = itemTags.ToArray();
                itemTags.Clear();
            }
            //AddTag(ref DLC1Content.Items.VoidmanPassiveItem, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.VoidMegaCrabItem, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.BearVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.BleedOnHitVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.ChainLightningVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.CloverVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.CritGlassesVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.ElementalRingVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.EquipmentMagazineVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.ExplodeOnDeathVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.ExtraLifeVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.ExtraLifeVoidConsumed, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.MissileVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.MushroomVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.SlowOnHitVoid, ItemTag.CannotSteal);
            //AddTag(ref DLC1Content.Items.TreasureCacheVoid, ItemTag.CannotSteal);
        }
    }
}