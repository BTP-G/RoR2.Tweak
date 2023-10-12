using EntityStates.Treebot.TreebotFlower;
using HIFUArtificerTweaks.Skills;
using HIFUCommandoTweaks.Skills;
using HIFUEngineerTweaks.Skills;
using HIFUHuntressTweaks.Skills;
using HIFULoaderTweaks.Skills;
using HIFUMercenaryTweaks.Skills;
using HIFURailgunnerTweaks.Misc;
using HIFURailgunnerTweaks.Skills;
using HIFURexTweaks.Skills;
using RoR2;
using System;
using TPDespair.ZetItemTweaks;
using UnityEngine;

namespace BtpTweak {

    public static class Localization {

        public static string ToDeath(this string str) => "<style=cDeath>" + str + "</style>";

        public static string ToDmg(this object str) => "<style=cIsDamage>" + str + "</style>";

        public static string ToHealth(this object str) => "<style=cIsHealth>" + str + "</style>";

        public static string ToDmgPct(this float damage) => "<style=cIsDamage>" + damage.ToPercent() + "</style>";

        public static string ToFire(this string str) => "<color=#f25d25>" + str + "</color>";

        public static string ToHealing(this object str) => "<style=cIsHealing>" + str + "</style>";

        public static string ToHealPct(this float value) => "<style=cIsHealing>" + value.ToPercent() + "</style>";

        public static string ToIce(this string str) => "<color=#CCFFFF>" + str + "</color>";

        public static string ToLightning(this string str) => "<color=#99CCFF>" + str + "</color>";

        public static string ToLunar(this string str) => "<style=cIsLunar>" + str + "</style>";

        public static string ToPercent(this float value) => 100 * value + "%";

        public static string ToPoison(this string str) => "<color=#014421>" + str + "</color>";

        public static string ToRed(this string str) => "<color=red>" + str + "</color>";

        public static string ToScepterDescription(this string desc) => "\n<color=#d299ff>权杖：" + desc + "</color>";

        public static string ToStack(this object str, string prefix = "", string suffix = "") => "<style=cStack>" + prefix + str + suffix + "</style>";

        public static string ToUtil(this object str) => "<style=cIsUtility>" + str + "</style>";

        public static string ToVoid(this string str) => "<style=cIsVoid>" + str + "</style>";

        public static string ToYellow(this string str) => "<color=yellow>" + str + "</color>";

        public static void 基础汉化() {
            //R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_EQUIPMENT_RESTOCK_NAME", "信标：补给", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_HACKING_DESCRIPTION", $"<style=cIsUtility>破解</style>附近所有可购买物品，这些物品的价格将逐渐下降至<style=cIsUtility>$0</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_HEAL_NAME", "信标：治疗", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_SHOCKING_NAME", "信标：震荡", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("CAPTAIN_UTILITY_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。向<style=cIsDamage>UES顺风号</style>请求至多<style=cIsDamage>3台</style>轨道探测器。每台探测器将造成{D(OrbitalProbe.Damage)}的伤害</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("COMMANDO_PLASMATAP_DESCRIPTION", "<style=cIsDamage>击穿</style>。发射一道锥形闪电，造成<style=cIsDamage>100%的伤害</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("COMMANDO_PLASMATAP_NAME", "电弧子弹", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("COMMANDO_PRFRVWILDFIRESTORM_DESCRIPTION", "发射一股火焰，每秒造成<style=cIsDamage>550%的伤害</style>，并有机会<style=cIsDamage>点燃</style>敌人。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("COMMANDO_PRFRVWILDFIRESTORM_NAME", "PRFR-V野火风暴", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_DESC", "同时持有<style=cIsUtility>两件装备</style>。激活'转换器'可以切换<style=cIsUtility>激活的装备</style>和<style=cIsDamage>磁轨炮手的次要技能攻击</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_DESC_ALT", "切换磁轨炮手的装备和瞄准镜", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_NAME", "转换器", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VOIDCRID_SCEPTER_ENTROPY", "<style=cArtifact>「幻影? 虚<style=cIsHealing>乱</style>无混』</style>", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VOIDCRID_ENTROPY", "<style=cArtifact>「虚混<style=cIsHealing>无</style>乱』</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_PRIMARY_DESCRIPTION", "喷射一大团弹丸，造成<style=cIsDamage>8x120%的伤害</style>。为攻击充能将缩小<style=cIsUtility>扩散范围</style>。<style=cStack>（每层完美巨兽+100%击退）</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_SECONDARY_DESCRIPTION", $"<style=cIsDamage>震荡</style>。发射一枚造成<style=cIsDamage>100%伤害</style>的快速电镖，弹射时<style=cIsUtility>电击</style>周围敌人。如果弹射将能飞行到更远地点。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_SECONDARY_NAME", "能量电镖", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_EQUIPMENT_RESTOCK_DESCRIPTION", $"使用时{ToUtil("为装备充能")}。消耗的能量随时间自动填充，速度随等级提升。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_HEAL_DESCRIPTION", $"每秒为附近10米{ToStack("（每级+1米）")}所有友方<style=cIsHealing>恢复</style>等同于各个角色<style=cIsHealing>最大生命值</style>{"10%".ToHealing() + "（每级+1%）".ToStack()}的生命值。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_SUPPLY_SHOCKING_DESCRIPTION", $"间歇性<style=cIsDamage>震荡</style>附近20米的所有敌人，使其无法移动。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("CAPTAIN_UTILITY_ALT1_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。{ToUtil("消耗4层充能")}向<style=cIsDamage>UES顺风号</style>请求发动一次<style=cIsDamage>动能打击</style>。在<style=cIsUtility>20秒</style>后，对所有角色造成{"50000%".ToDmg()}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("COMMANDO_HEAVYTAP_DESCRIPTION", "<style=cIsDamage>绝对光滑</style>。射击两次，造成<style=cIsDamage>2x155%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("COMMANDO_HEAVYTAP_NAME", "沉重双击", "zh-CN");
            R2API.LanguageAPI.AddOverlay("COMMANDO_SECONDARY_DESCRIPTION", $"发射一枚<style=cIsDamage>穿甲弹</style>，造成{ToDmgPct(PhaseRound.Damage)}的伤害。每次穿透敌人，造成的伤害提高<style=cIsDamage>40%</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("COMMANDO_SPECIAL_ALT1_DESCRIPTION", $"扔出一枚手雷，爆炸可造成{ToDmgPct(FragGrenade.Damage)}的伤害。最多可投掷2枚。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("COMMANDO_SPECIAL_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。连续射击，每枚弹丸造成{ToDmgPct(SuppressiveFire.Damage)}的伤害。射击次数随攻击速度增加。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_PRIMARY_DESCRIPTION", $"发射<style=cIsDamage>{BouncingGrenades.maximumGrenadesCount}</style>颗手雷，每颗均可造成{ToDmgPct(BouncingGrenades.damage)}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_SECONDARY_DESCRIPTION", $"放置一枚二阶段地雷，能够造成<style=cIsDamage>300%的伤害</style>，或在完全引爆时造成<style=cIsDamage>{Mathf.Round(300f * PressureMines.damageScale)}%的伤害</style>。最多放置{PressureMines.charges}枚<style=cStack>（每级+1枚）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_SKILL_HARPOON_DESCRIPTION", $"进入<style=cIsUtility>目标标记模式</style>以发射热追踪鱼叉导弹，每发造成{ToDmgPct(ThermalHarpoons.damage)}的伤害。最多可储存{ThermalHarpoons.charges}发。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_SPECIAL_ALT1_DESCRIPTION", $"放置一个<style=cIsUtility>移动</style>炮塔可<style=cIsUtility>继承你所有物品</style>。发射的激光可造成<style=cIsDamage>每秒200%的伤害</style>，并可<style=cIsUtility>减速敌人</style>，最多放置2座。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_SPECIAL_DESCRIPTION", $"放置一个<style=cIsUtility>继承你所有物品</style>的炮塔，发射的炮弹可造成<style=cIsDamage>100%的伤害</style>，最多放置2座。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_SPIDERMINE_DESCRIPTION", $"放置一枚机器人地雷，在敌人走近时自动引爆，造成{ToDmgPct(SpiderMines.damage)}的伤害，最多放置{SpiderMines.charges}枚<style=cStack>（每级+1枚）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ENGI_UTILITY_DESCRIPTION", $"放置一个<style=cIsUtility>无法穿透且有击退力的护盾</style>来阻挡弹幕{((BubbleShield.damage > 0f) ? "，并且在击退时造成" + ToDmgPct(BubbleShield.damage / BubbleShield.ticks) + "的伤害" : "")}。每个盾需要消耗{BubbleShield.chargesToConsume}层充能，存在<style=cIsUtility>{BubbleShield.duration}秒</style>。<style=cIsUtility>护盾</style>展开时为每个友方单位提供一个持续<style=cIsUtility>6秒</style>的<style=cIsUtility>个人护盾</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_BFG_DESC", $"发射前子卷须，对{"66.6".ToUtil()}米范围内的敌人造成最高<style=cIsDamage>每秒666%的伤害</style>。接触目标并引爆后，会对20米范围内的敌人造成<style=cIsDamage>6666%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_COMMANDMISSILE_DESC", $"发射一轮包含<style=cIsDamage>12</style>枚导弹的导弹雨，每枚导弹造成<style=cIsDamage>300%{"（每层AtG导弹MK.1+300%）".ToStack()}的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_CRITONUSE_DESC", $"<style=cIsDamage>暴击几率增加100%</style>，并使超过{"100%".ToDmg()}的{"暴击几率".ToDmg()}转换为{"暴击伤害".ToDmg()}，持续8秒。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_CRITONUSE_PICKUP", $"获得100%暴击几率，并使超过100%的暴击几率转换为暴击伤害，持续8秒。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_FIREBALLDASH_DESC", $"变成<style=cIsDamage>龙之火球</style>持续<style=cIsDamage>6</style>秒，受击可造成<style=cIsDamage>500%的伤害</style>，飞行时持续喷射造成{"400%".ToDmg() + "（每层熔融钻机+400%）".ToDmg()}伤害的岩浆球。结束时会引爆，造成<style=cIsDamage>800%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_GOLDGAT_DESC", $"发射一阵钱雨，<style=cIsDamage>每颗子弹均造成100%{"（每层成卷的零钱+100%）".ToStack()}，外加等同于消耗金钱的伤害</style>。每颗子弹消耗1枚{"（启动后，每秒+1枚）".ToStack()}金钱，价格随游戏时间不断上升。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_LIGHTNING_DESC", $"召唤雷电攻击目标怪物，造成<style=cIsDamage>3000%{"（每层电能钻机+3000%）".ToStack()}的伤害</style>并<style=cIsDamage>眩晕</style>附近怪物。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_RECYCLER_DESC", $"将一个物品或装备<style=cIsUtility>转化</style>成另一个<style=cIsUtility>同等级</style>的物品或装备。无法{"转化".ToUtil()}碎片和回收机。在月球外{"转化".ToUtil() + "月球".ToLunar()}物品或装备时有可能被发现，物品或装备将会被传送回{"月球".ToLunar()}。{"转化".ToUtil() + "虚空".ToVoid()}物品时可能导致物品爆炸。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("EQUIPMENT_RECYCLER_PICKUP", $"将一个物品或装备转化成另一个。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("FOGBOUND_SCENEDEF_NAME_TOKEN", $"雾气泻湖", "zh-CN");
            R2API.LanguageAPI.AddOverlay("FOGBOUND_SCENEDEF_SUBTITLE_TOKEN", $"冥河浅滩", "zh-CN");
            R2API.LanguageAPI.AddOverlay("GOLDENKNURL_DESC", $"{"最大生命值增加".ToHealing() + GoldenCoastPlus.GoldenCoastPlus.KnurlHealth.Value.ToHealPct() + GoldenCoastPlus.GoldenCoastPlus.KnurlHealth.Value.ToPercent().ToStack("（每层+", "）")}，{ToHealing("生命值再生")}增加{(GoldenCoastPlus.GoldenCoastPlus.KnurlRegen.Value + "hp/s").ToHealing() + ("（每层+" + GoldenCoastPlus.GoldenCoastPlus.KnurlRegen.Value + "hp/s）").ToStack()}，{ToUtil("护甲")}增加{(GoldenCoastPlus.GoldenCoastPlus.KnurlArmor.Value + "点").ToHealing() + GoldenCoastPlus.GoldenCoastPlus.KnurlArmor.Value.ToStack("（每层+", "点）")}。 \n<color=yellow>黄金</color>：{ToHealing("再生速度")}提升{"50%".ToHealing() + "（每层+50%；每有1,000,000金钱+1%）".ToStack()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("GOLDENKNURL_NAME", "<color=yellow>黄金隆起</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("GOLDENKNURL_PICKUP", "增加最大生命值、生命值再生速度和护甲。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HAT_MAGE_UTILITY_FIRE_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。<style=cIsDamage>点燃</style>。向前冲刺，在身后召唤造成每秒{HIFUArtificerTweaks.Main.flamewallDamage.Value.ToDmgPct()}的伤害的火柱</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HAT_MAGE_UTILITY_FIRE_NAME", "火墙", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HUNTRESS_PRIMARY_ALT_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。瞄准60米<style=cStack>（每升一级+{ModConfig.女猎人射程每级增加距离.Value}米）</style>内的敌人，拉弓射出<style=cIsDamage>{Flurry.minArrows}枚</style>跟踪箭，每枚造成{ToDmgPct(Flurry.damage)}的伤害。如果暴击则发射<style=cIsDamage>{Flurry.maxArrows}</style>枚跟踪箭。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HUNTRESS_PRIMARY_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。瞄准60米<style=cStack>（每升一级+{ModConfig.女猎人射程每级增加距离.Value}米）</style>内的敌人，快速射出一枚能够造成{ToDmgPct(Strafe.damage)}伤害的跟踪箭。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HUNTRESS_SECONDARY_DESCRIPTION", $"{(LaserGlaive.agile ? "<style=cIsUtility>灵巧</style>。" : "")}投掷一把追踪月刃，可弹射最多<style=cIsDamage>{LaserGlaive.bounceCount}</style>次，初始造成{ToDmgPct(LaserGlaive.damage)}的伤害，每次弹射伤害增加<style=cIsDamage>{Math.Round((double)((LaserGlaive.bounceDamage - 1f) * 100f), 1)}%</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HUNTRESS_SPECIAL_ALT1_DESCRIPTION", $"向后<style=cIsUtility>传送</style>至空中。最多发射<style=cIsDamage>{Ballista.boltCount}</style>道能量闪电，造成<style=cIsDamage>{Ballista.boltCount}x{ToDmgPct(Ballista.damage)}的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("HUNTRESS_SPECIAL_DESCRIPTION", $"<style=cIsUtility>传送</style>至空中，向目标区域射下箭雨，使区域内所有敌人<style=cIsUtility>减速</style>，并造成每秒{(3f * ArrowRain.damage).ToDmgPct()}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_AbyssalMedkit_DESCRIPTION", "<style=cIsUtility>消耗品</style>，抵挡<style=cIsUtility>10次</style>减益后失效。每一次抵挡都有<style=cIsHealing>10%</style>概率给予你<style=cIsHealing>“祝·福”</style>。每个<style=cIsHealing>祝福</style>可使你<style=cIsUtility>所有属性提升3%</style>。<style=cIsVoid>使所有医疗包无效化</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_AbyssalMedkit_PICKUP", "消耗品，可以替你抵挡10次减益，每一次抵挡都有概率给予你“祝·福”", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ALIENHEAD_DESC", $"使技能{"冷却时间".ToUtil()}减少{AlienHead.BaseCooldown.Value.ToPercent().ToUtil() + AlienHead.StackCooldown.Value.ToPercent().ToStack("（每层+", "）")}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ANCIENT_SCEPTER_DESCRIPTION", $"{"原始：来自原始传送器。".ToRed()}\n改变你的某个<style=cIsUtility>技能</style>。<style=cStack>（具体效果见技能介绍，对某些角色无效果）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ARMORREDUCTIONONHIT_DESC", $"击中一名敌人造成<style=cIsDamage>粉碎</style>减益，将对方<style=cIsDamage>护甲</style>降低{ShatteringJustice.ArmorReduction.Value.ToUtil()}点，持续{ShatteringJustice.DebuffDuration.Value.ToUtil()}秒。对被粉碎的敌人造成最多{ShatteringJustice.BaseDamage.Value.ToDmgPct() + ShatteringJustice.StackDamage.Value.ToPercent().ToStack("（每层+", "）")}的额外伤害，当目标剩余{ShatteringJustice.FullEffectThreshold.Value.ToDmgPct()}的生命值时效果最大。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ATTACKSPEEDANDMOVESPEED_DESC", $"使<style=cIsDamage>攻击速度</style>提高<style=cIsDamage>7.5%</style><style=cStack>（每层+7.5%）</style>，使<style=cIsUtility>移动速度</style>提高<style=cIsUtility>7%</style><style=cStack>（每层+7%）</style>。<color=#FFFF00>珍娜特殊效果：最大生命值和生命值再生速度增加5%<style=cStack>（每层+5%）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_AUTOCASTEQUIPMENT_DESC", $"<style=cIsUtility>使装备冷却时间减少</style><style=cIsUtility>50%</style><style=cStack>（每层增加15%）</style>，但会使装备增加{"0.15秒强制冷却时间".ToUtil() + "（每层+0.15秒）".ToStack()}。装备会在<style=cIsUtility>冷却时间</style>结束时被迫自动<style=cIsUtility>激活</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BARRIERONKILL_DESC", $"击败敌人时可获得一道<style=cIsHealing>临时屏障</style>，相当于增加<style=cIsHealing>15<style=cStack>（每层增加15）</style>点生命值</style>。</style><color=#FFFF00>船长特殊效果：给予15点<style=cStack>（每层+15点）</style>最大护盾值，最大护盾值增加25%<style=cStack>（每层+25%）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BARRIERONOVERHEAL_DESC", $"过量<style=cIsHealing>治疗</style>会使你获得一道<style=cIsHealing>临时屏障</style>，可阻挡相当于{Aegis.BaseOverheal.Value.ToHealPct() + ("（每层增加" + 100 * Aegis.StackOverheal.Value + "%）").ToStack()}已<style=cIsHealing>治疗</style>生命值的伤害。增加{(Aegis.BaseArmor.Value + "点").ToHealing() + ("（每层增加" + Aegis.StackArmor.Value + "点）").ToStack()}护甲，<style=cIsHealing>临时护盾</style>激活时再增加{(Aegis.BaseBarrierArmor.Value + "点").ToHealing() + ("（每层+" + Aegis.StackBarrierArmor.Value + "点）").ToStack()}护甲。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BEAR_DESC", $"增加<style=cIsHealing>10</style>点<style=cStack>（每层增加10点）</style>护甲。\n<style=cIsHealing>【护甲减伤公式：护甲值 /（护甲值+100）】</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BEARVOID_DESC", $"有<style=cIsHealing>50%</style>概率<style=cIsUtility>（成为虚空的象征+50%概率）</style><style=cIsHealing>格挡</style>一次来袭的伤害。充能时间<style=cIsUtility>15秒</style><style=cStack>（每层-10%）</style>。<style=cIsVoid>使所有更艰难的时光无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BEHEMOTH_DESC", $"你的所有<style=cIsDamage>攻击均会爆炸</style>，对{(BrilliantBehemoth.BaseRadius.Value + "米").ToDmg() + BrilliantBehemoth.StackRadius.Value.ToStack("（每层+", "米）")}范围内的敌人合计造成{BrilliantBehemoth.BaseDamage.Value.ToDmgPct() + ("（每层+" + 100 * BrilliantBehemoth.StackDamage.Value + "%）").ToStack()}的额外伤害。<color=#FFFF00>船长特殊效果：关于“火神霰弹枪”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BLEEDONHIT_DESC", $"有<style=cIsDamage>10%</style><style=cStack>（每层增加10%）</style>的几率使敌人<style=cIsDamage>流血</style>，造成<style=cIsDamage>240%</style>的基础伤害。<style=cIsDamage>1000</style>层<style=cIsDamage>流血</style>将转化为<style=cIsDamage>1</style>层具有同等伤害的<style=cIsHealth>出血</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BLEEDONHITANDEXPLODE_DESC", $"<style=cIsDamage>暴击流血</style>将对敌人造成<style=cIsDamage>240%</style>的基础伤害。<style=cIsDamage>流血或出血</style>的敌人将会在死亡时<style=cIsDamage>爆炸</style>并造成<style=cIsDamage>400%<style=cStack>（每层+400%）</style> + 层数x(流血层数+1000*出血层数)%</style>伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BLEEDONHITVOID_DESC", $"攻击有<style=cIsDamage>10%</style><style=cStack>（每层+10%）</style>几率<style=cIsDamage>瓦解</style>敌人，合计造成<style=cIsDamage>30%</style>的伤害。<style=cIsVoid>使所有三尖匕首无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BLESSING_NAME_DESCRIPTION", $"凝视深渊过久，深渊将回以凝视！\n<style=cIsVoid>所有属性提升3%</style><style=cStack>（每层+3%）</style>\n<style=cIsVoid>祝·福深入灵魂，将伴随你一生</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BONUSGOLDPACKONKILL_DESC", $"<style=cIsDamage>击杀</style>获得的<color=yellow>金钱</color>增加{(100 * GhorsTome.BaseMult.Value + "%").ToUtil() + ("（每层+" + 100 * GhorsTome.StackMult.Value + "%）").ToStack()}。\n<style=cIsDamage>击杀敌人</style>时有{(GhorsTome.BaseChance.Value + "%").ToUtil() + ("（每层+" + GhorsTome.StackChance.Value + "%）").ToStack()}的几率掉落价值<style=cIsUtility>25<color=yellow>金钱</color></style>的宝物<style=cIsUtility>（价值随时间变化）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BOSSDAMAGEBONUS_DESC", $"对<style=cIsHealing>护盾</style>和<style=cIsHealing>临时屏障</style>额外造成<style=cIsDamage>20%</style>的伤害<style=cStack>（每层增加20%）</style>。<color=#FFFF00>指挥官特殊效果：基础伤害增加2点<style=cStack>（每层+2点）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BOUNCENEARBY_DESC", $"命中时有<style=cIsDamage>20%</style><style=cStack>（每层增加20%）</style>的几率向最多<style=cIsDamage>10名</style><style=cStack>（每层增加5名）</style>敌人<style=cIsDamage>发射追踪勾爪</style>，造成<style=cIsDamage>100%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BULWARKSHAUNT_SWORD_DESC", $"神秘声音在你的脑海中回荡，低语着：<link=\"BulwarksHauntWavy\">\"方尖碑...方尖碑...\"</link>，看样子它好像想让你带它去方尖碑处", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_BULWARKSHAUNT_SWORD_UNLEASHED_DESC", $"神秘声音在你的脑海中回荡，低语着：<link=\"BulwarksHauntWavy\">\"方尖碑...方尖碑...\"</link>，看样子它好像想让你带它去方尖碑处", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_CARTRIDGECONSUMED_DESCRIPTION", $"他曾梦想成为一名艺术家...", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_CHAINLIGHTNING_DESC", $"有<style=cIsDamage>25%</style><style=cStack>（每层+5%）</style>的几率发射<style=cIsDamage>连锁闪电</style>{0.2f.ToStack("（发射间隔：", "s）")}，对<style=cIsDamage>20米</style><style=cStack>（每层增加4米）</style>内的最多<style=cIsDamage>4个</style>目标合计造成<style=cIsDamage>80%</style>的伤害。<color=#FFFF00>工匠特殊效果：关于“充能完毕的纳米炸弹”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_CHAINLIGHTNINGVOID_DESC", $"有<style=cIsDamage>25%</style><style=cStack>（每层+5%）</style>的几率发射<style=cIsDamage>虚空闪电</style>，对同一个敌人合计造成<style=cIsDamage>60%</style>的伤害，最多<style=cIsDamage>3</style>次。<style=cIsVoid>使所有尤克里里无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_CRITDAMAGE_DESC", $"<style=cIsDamage>暴击</style>额外造成<style=cIsDamage>100%的伤害</style><style=cStack>（每层+100%）</style>。<color=#FFFF00>磁轨炮手特殊效果：敌人弱点范围增加100%<style=cStack>（每层+100%）</style>（弱点框不再变大，防止遮挡视野）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_CRITGLASSESVOID_DESC", $"你的攻击有<style=cIsDamage>0.5%</style><style=cStack>（每层+0.5%）</style>几率对<style=cIsDamage>非Boss敌人</style>造成<style=cIsDamage>生命诅咒</style>，降低<style=cIsHealing>最大生命值</style>。<style=cIsVoid>使所有透镜制作者的眼镜无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_CROWBAR_DESC", $"对生命值超过<style=cIsDamage>90%</style>的敌人造成<style=cIsDamage>75%</style><style=cStack>（每层增加75%）</style>的伤害。<color=#FFFF00>多功能枪兵特殊效果：关于“钢筋发射器”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_DEATHMARK_DESC", $"拥有{DeathMark.CountTrigger.Value}个或更多减益效果的敌人<style=cIsDamage>将被标记为死亡</style>，从所有来源受到的伤害增加{DeathMark.BaseDamage.Value.ToDmgPct() + ("（全队每层+" + DeathMark.StackDamage.Value.ToPercent() + "）").ToStack()}，持续{(DeathMark.DebuffDuration.Value + "秒").ToUtil()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ELEMENTALRINGVOID_DESC", $"攻击造成<style=cIsDamage>至少400%的伤害</style>时，产生一个黑洞，<style=cIsUtility>将15米范围内的敌人吸引至其中心</style>。持续<style=cIsUtility>5</style>秒，之后坍缩，合计造成<style=cIsDamage>100%</style><style=cStack>（每层+100%）</style>的伤害。每多一名敌人在黑洞中就增加{"100%".ToDmg()}的伤害。充能时间<style=cIsUtility>20</style>秒<style=cStack>（每层减少1秒。至少2秒）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ENERGIZEDONEQUIPMENTUSE_DESC", $"激活装备可使你获得{WarHorn.BaseBuffAtkSpd.Value.ToDmgPct() + ("（每层+" + 100 * WarHorn.StackBuffAtkSpd.Value + "%）").ToStack()}攻击速度加成，持续{(WarHorn.BaseBuffDuration.Value + "秒").ToUtil()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_EQUIPMENTMAGAZINE_DESC", $"获得1次<style=cIsUtility>额外的装备充能</style><style=cStack>（每层增加1次）</style>。<style=cIsUtility>将装备冷却时间减少</style><style=cIsUtility>15%</style><style=cStack>（每层增加15%）</style>。<color=#FFFF00>雷克斯特殊效果：技能<style=cIsUtility>冷却时间</style>减少<style=cIsUtility>10%</style><style=cStack>（每层+10%）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ESSENCEOFTAR_DESC", $"成为焦油的象征，<style=cDeath>生命不再自然恢复</style>。攻击敌人<style=cIsHealing>吸收他们的生命</style>。<style=cDeath>移除将导致你直接死亡</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_EXECUTELOWHEALTHELITE_DESC", $"立即击败生命值低于<style=cIsHealth>10%的精英怪物</style><style=cStack>（叠加公式：斩杀线 = 0.5x(层数÷(层数+4))）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_EXPLODEONDEATH_DESC", $"击败一名敌人后，召唤一道<style=cIsDamage>岩浆柱</style>对<style=cIsDamage>12米</style><style=cStack>（每层增加6米）</style>范围内的敌人造成<style=cIsDamage>400%</style>的基础伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_EXPLODEONDEATHVOID_DESC", $"命中<style=cIsDamage>生命值大于或等于100%</style>的敌人时，<style=cIsDamage>引爆</style>它们，爆炸半径<style=cIsDamage>12米</style>{"（每层+3米）".ToStack()}，造成<style=cIsDamage>360%</style>的基础伤害，并且<style=cIsDamage>点燃</style>敌人{"（每次+100%燃烧伤害）".ToStack()}，总共造成等同于敌人<style=cIsDamage>最大生命值</style>的伤害，被<style=cIsDamage>点燃</style>的敌人不能再被<style=cIsDamage>引爆</style>。<style=cIsVoid>使所有鬼火无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_EXTRALIFE_DESC", $"<style=cIsUtility>倒下后</style>，该物品将被<style=cIsUtility>消耗</style>，你将<style=cIsHealing>起死回生</style>并获得<style=cIsHealing>3秒的无敌时间</style>。\n<style=cIsUtility>死去的迪奥将在过关时复活。\n此物品不会被米斯历克斯拿走</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_FALLBOOTSVOID_DESC", $"按住'E'可<style=cIsUtility>漂浮</style>并吸收<style=cIsUtility>引力波</style>。松开创造一个半径<style=cIsUtility>30米</style>的<style=cIsUtility>反重力区</style>，持续<style=cIsUtility>15</style>秒。之后进入<style=cIsUtility>20</style><style=cStack>（每层-50%）</style>秒的充能时间，反重力区中心会吸引周围敌人并造成<style=cIsDamage>200-2000%</style>的基础伤害。<style=cIsVoid>使所有H3AD-5T v2无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_FIREBALLSONHIT_DESC", $"命中时有<style=cIsDamage>30%</style>的几率向敌人的{"岩浆球喷泉".ToFire()}中添加{"1颗岩浆球".ToFire() + ModConfig.喷泉喷射间隔.Value.ToStack("（喷射间隔：", "s）")}，造成<style=cIsDamage>300%</style><style=cStack>（每层+300%）</style>的伤害，并<style=cIsDamage>点燃</style>所有敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_FIRERING_DESC", $"造成<style=cIsDamage>高于400%伤害</style>的攻击会产生一道<style=cIsDamage>符文火焰龙卷风</style>轰击敌人，造成<style=cIsDamage>300%</style><style=cStack>（每层+300%）</style>的总持续伤害，同时<style=cIsDamage>点燃</style>敌人。充能时间<style=cIsUtility>10</style>秒<style=cStack>（每双手环减少1秒。至少1秒）</style>。<color=#FFFF00>工匠特殊效果：关于“火炎弹”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_FIREWORK_DESC", $"有{"5%".ToDmg() + "（每层+5%）".ToStack()}机率向{"烟花发射池".ToUtil()}里添加一枚烟花{ModConfig.导弹发射间隔.Value.ToStack("（发射间隔：", "s）")}。每枚合计造成{1.5f.ToDmgPct()}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_FLATHEALTH_DESC", $"<style=cIsHealing>最大生命值</style>增加<style=cIsHealing>25</style>点<style=cStack>（每层+25点）</style>。升级获得的<style=cIsHealing>最大生命值</style>增加<style=cIsHealing>2.5</style>点<style=cStack>（每层+2.5点）</style>。<color=#FFFF00>厨师特殊效果：野牛肉排的效果翻倍。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_FRAGILEDAMAGEBONUS_DESC", $"使伤害提高<style=cIsDamage>20%</style><style=cStack>（每层+20%）</style>。受到伤害导致生命值降到<style=cIsHealth>25%</style>以下时，该物品会<style=cIsUtility>损坏</style>。\n<style=cIsUtility>损坏的手表将在过关时修复</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_HEADHUNTER_DESC", "获得所击败精英怪物身上的<style=cIsDamage>能力</style>，持续<style=cIsDamage>20秒</style><style=cStack>（每层增加10秒）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_HEALINGPOTION_DESC", $"受到伤害导致<style=cIsHealth>生命值降到25%</style>以下时会<style=cIsUtility>消耗</style>该物品，并为你<style=cIsHealing>恢复</style><style=cIsHealing>最大生命值</style>的<style=cIsHealing>75%</style>。\n<style=cIsUtility>使用后的空瓶将在过关时重新填满</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_HEALONCRIT_DESC", $"获得{(HarvestersScythe.BaseCrit.Value + "%").ToDmg() + ("（每层+" + HarvestersScythe.StackCrit.Value + "%）").ToStack()}</style>的暴击几率</style>，<style=cIsDamage>暴击</style>还会使你{("恢复" + HarvestersScythe.BaseCritHeal.Value + "点").ToHealing() + ("（每层+" + HarvestersScythe.StackCritHeal.Value + "点）").ToStack() + "生命值".ToHealing()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_HEALWHILESAFE_DESC", $"使脱离战斗状态下的<style=cIsHealing>生命值再生</style>增加{(CautiousSlug.BaseSafeRegen.Value + "hp/s").ToHealing() + ("（每层+" + CautiousSlug.StackSafeRegen.Value + "hp/s）").ToStack()}，外加<style=cIsHealing>生命值<color=yellow>百分比</color>再生</style>增加{(CautiousSlug.BaseSafeRegenFraction.Value.ToHealPct() + "hp/s").ToHealing() + ("（每层+" + 100 * CautiousSlug.StackSafeRegenFraction.Value + "%hp/s）").ToStack()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_ICERING_DESC", $"造成<style=cIsDamage>高于400%伤害</style>的攻击会产生一道<style=cIsDamage>符文冰霜爆炸</style>轰击敌人，使其<style=cIsUtility>减速</style><style=cIsUtility>80%</style>，持续<style=cIsUtility>3秒</style><style=cStack>（每层+3秒）</style>并造成<style=cIsDamage>300%</style><style=cStack>（每层+300%）</style>的总伤害。充能时间<style=cIsUtility>10</style>秒<style=cStack>（每双手环减少1秒。至少1秒）</style>。<color=#FFFF00>工匠特殊效果：关于“攻击纳米枪”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_IGNITEONKILL_DESC", $"击杀敌人后<style=cIsDamage>点燃</style><style=cIsDamage>16米</style>内的所有敌人，造成<style=cIsDamage>150%</style>的基础伤害。<style=cIsDamage>点燃</style>可对敌人造成<style=cIsDamage>150%</style><style=cStack>（每层增加150%）</style>的基础伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_IMMUNETODEBUFF_DESC", $"增加{BensRainCoat.BaseHealthPercent.Value.ToHealPct() + ("（每层+" + 100 * BensRainCoat.StackHealthPercent.Value + "%）").ToStack()}最大生命值，防止<style=cIsUtility>{BensRainCoat.BaseImmunity.Value}个<style=cStack>（每层+{BensRainCoat.StackImmunity.Value}个）</style></style><style=cIsDamage>减益效果</style>，并在触发后的<style=cIsUtility>{BensRainCoat.ImmunityDuration.Value}</style>秒内免疫所有<style=cIsDamage>减益</style>。每<style=cIsUtility>{BensRainCoat.BaseRecharge.Value}</style>秒<style=cStack>（每层减少{100 * BensRainCoat.StackReduction.Value}%）</style>充能一次</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_INFUSION_DESC", $"每击败一名敌人，即可为{"自身外加主人".ToHealing() + "（如果有）".ToStack()}<style=cIsHealing>永久性</style>增加<style=cIsHealing>1</style>点<style=cStack>（每层+1点）</style>生命值，最多增加<style=cIsHealing>自身等级x0.3x自身基础血量x全队层数</style>点生命值。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_KNURL_DESC", $"{ToHealing("最大生命值")}增加{(TitanicKnurl.BaseHealth.Value + "点").ToHealing() + TitanicKnurl.StackHealth.Value.ToStack("（每层+", "点）")}。{("生命值再生增加" + TitanicKnurl.BaseRegen.Value + "hp/s").ToHealing() + TitanicKnurl.StackRegen.Value.ToStack("（每层+", "hp/s）")}，外加{"生命值<color=yellow>百分比</color>再生".ToHealing()}增加{(TitanicKnurl.BaseRegenFraction.Value.ToHealPct() + "hp/s").ToHealing() + ("（每层+" + TitanicKnurl.StackRegenFraction.Value.ToPercent() + "hp/s）").ToStack()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LIGHTNINGSTRIKEONHIT_DESC", $"命中时有<style=cIsDamage>30%</style>的几率向敌人的{"雷电召唤池".ToLightning()}中添加{"1次雷击".ToLightning() + 0.5f.ToStack("（基础召唤间隔：", "s）")}，造成<style=cIsDamage>300%</style><style=cStack>（每层+300%）</style>的伤害。<color=#FFFF00>装卸工特殊效果：关于“雷霆拳套”（详情看 权杖 技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARPRIMARYREPLACEMENT_DESC", "<style=cIsUtility>替换主要技能</style>为<style=cIsUtility>渴望凝视</style>。\n\n发射一批会延迟引爆的<style=cIsUtility>追踪碎片</style>，造成<style=cIsDamage>120%</style>的基础伤害。最多充能12次<style=cStack>（每层增加12次）</style>，2秒后重新充能<style=cStack>（每层增加2秒）</style>。\n<style=cIsLunar>异教徒：追踪能力加强。每层使技能冷却降低2秒。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARSECONDARYREPLACEMENT_DESC", "<style=cIsUtility>将你的次要技能替换为</style><style=cIsUtility>万刃风暴</style>。\n\n充能并射出一发子弹，对附近的敌人造成<style=cIsDamage>每秒175%的伤害</style>，并在<style=cIsUtility>3</style>秒后爆炸，造成<style=cIsDamage>700%的伤害</style>，并使敌人<style=cIsDamage>定身</style><style=cIsUtility>3</style><style=cStack>（每层增加+3秒）</style>秒。5秒<style=cStack>（每层增加5秒）</style>后充能。\n<style=cIsLunar>异教徒：每层使攻击速度增加50%</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARSPECIALREPLACEMENT_DESC", "<style=cIsUtility>将你的特殊技能替换为</style><style=cIsUtility>毁坏</style>。\n\n造成伤害可以施加一层<style=cIsDamage>毁坏</style>，持续10<style=cStack>（每层增加+10秒）秒</style>。启动此技能可以<style=cIsDamage>引爆</style>所有的毁坏层数，不限距离，并造成<style=cIsDamage>300%的伤害</style>，外加<style=cIsDamage>每层毁坏120%<style=cIsLunar>（异教徒：每层+120%）</style>的伤害</style>。8秒<style=cStack>（每层增加8秒）</style>后充能。<style=cIsLunar>异教徒：每层使最大生命值增加10%。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARSUN_DESC", $"每<style=cIsUtility>3</style><style=cStack>（每层-50%）</style>秒获得一个<style=cIsDamage>环绕运动的炸弹</style>，碰撞到敌人时爆炸，造成<style=cIsDamage>360%</style>的伤害，最多可获得<style=cIsUtility>3<style=cStack>（每层+1）</style>个炸弹</style>。每<style=cIsUtility>30</style>秒将一件随机物品<style=cIsUtility>{ToStack("（排除米斯历克斯不会偷走的物品）")}转化</style>为该物品。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARSUN_PICKUP", "获得多个环绕运动的炸弹。<color=#FF7F7F>每30秒，将一件其他物品吸收并转化为自我中心。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARUTILITYREPLACEMENT_DESC", "<style=cIsUtility>将你的辅助技能替换</style>为<style=cIsUtility>影逝</style>。\n\n隐去身形，进入<style=cIsUtility>隐形状态</style>并获得<style=cIsUtility>30%移动速度加成</style>。<style=cIsHealing>治疗</style><style=cIsHealing>25%<style=cStack>（每层增加25%）</style>的最大生命值</style>。持续3<style=cStack>（每层加3）</style>秒。\n<style=cIsLunar>异教徒：可通过技能按键切换形态。每层使移动速度增加50%。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARWINGS_DESC", $"{"工匠·过去时".ToLunar()}：随着{"时间".ToUtil()}流逝已经丧失了全部力量，{"或许在某个地方可以恢复...".ToDeath()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARWINGS_NAME", "特拉法梅的祝福".ToLunar(), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_LUNARWINGS_PICKUP", "一双翅膀。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_MISSILE_DESC", $"有{"25%".ToDmg()}机率向{"导弹发射池".ToUtil()}里添加一枚导弹{ModConfig.导弹发射间隔.Value.ToStack("（发射间隔：", "s）")}。每枚合计造成{"250%".ToDmg() + "（每层增加250%）".ToStack()}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_MISSILEVOID_DESC", $"获得一个相当于你最大生命值<style=cIsHealing>{100 * PlasmaShrimp.BaseHealthAsShield.Value}%</style>的<style=cIsHealing>护盾</style>。你拥有<style=cIsHealing>护盾</style>时，命中敌人会发射一发虾米，合计造成<style=cIsDamage>{100 * PlasmaShrimp.BaseDamage.Value}%</style><style=cStack>（每层+{100 * PlasmaShrimp.StackDamage.Value}%）</style>的伤害。<style=cIsVoid>使所有AtG导弹MK.1无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_MOVESPEEDONKILL_DESC", $"击杀敌人会使<style=cIsUtility>移动速度</style>提高<style=cIsUtility>125%</style>，在<style=cIsUtility>1</style><style=cStack>（每层+0.5）</style>秒内逐渐失效。<color=#FFFF00>女猎人特殊效果：主要技能有<style=cIsDamage>10%</style><style=cStack>（每层+10%）</style>的概率额外发射一枚跟踪箭。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_NEARBYDAMAGEBONUS_DESC", $"对<style=cIsDamage>{FocusCrystal.NearbyRange.Value}米</style>内的敌人伤害增加{FocusCrystal.BaseDamage.Value.ToDmgPct()}<style=cStack>（每层+{FocusCrystal.StackDamage.Value.ToPercent()}）</style>.", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_NOVAONHEAL_DESC", $"将{NkuhanasOpinion.BaseStore.Value.ToHealPct()}<style=cStack>（每层增加{NkuhanasOpinion.StackStore.Value.ToPercent()}）</style>的治疗量储存为<style=cIsHealing>灵魂能量</style>，最大存储量等同于<style=cIsHealing>最大生命值</style>的{NkuhanasOpinion.BaseCapacity.Value.ToHealPct()}<style=cStack>（每层增加{NkuhanasOpinion.StackCapacity.Value.ToPercent()}）</style>。当<style=cIsHealing>灵魂能量</style>达到<style=cIsHealing>最大生命值</style>的<style=cIsHealing>{NkuhanasOpinion.ThresholdFraction.Value.ToPercent()}</style>时，自动向半径<style=cIsDamage>{NkuhanasOpinion.BaseRange.Value}</style>米内的敌人<style=cIsDamage>发射一颗头骨</style>，造成相当于<style=cIsDamage>{NkuhanasOpinion.BaseDamage.Value.ToDmgPct()}</style><style=cIsHealing>灵魂能量</style>的<style=cIsDamage>伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_PARENTEGG_DESC", $"在<style=cIsDamage>受到伤害时</style>回复<style=cIsHealing>20点</style>，外加<style=cIsHealing>受到伤害x1%<style=cStack>（每层+1%）</style></style>的生命值。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_PERMANENTDEBUFFONHIT_DESC", $"命中时有<style=cIsDamage>100%</style>几率使<style=cIsDamage>护甲</style>永久降低<style=cIsDamage>2点</style><style=cStack>（每层+2点）</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_PERSONALSHIELD_DESC", $"获得一个相当于你最大生命值<style=cIsHealing>8%</style><style=cStack>（每层+8%）</style>的<style=cIsHealing>护盾</style>。脱险后可重新充能。\n<style=cIsLunar>（<style=cIsHealing>护盾</style>受到的<style=cIsDamage>Dot伤害</style>降低50%，成为完美的象征再降低50%）</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_RANDOMLYLUNAR_DESC", $"月球切片的使用次数增加{"3次".ToUtil() + "（每层+3次）".ToStack()}，但使用价格提升速度{"增加".ToDeath() + "3月球币".ToLunar() + "（每层+3月球币）".ToStack()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_RANDOMLYLUNAR_PICKUP", "增加月球切片的使用次数，但会使其价格提升速度增加。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_RELICENERGY_DESC", $"似乎没什么用。<style=cIsUtility>可能会有一些用处...?</style><style=cIsUtility>（此物品可用于充能特殊传送器）</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_REPEATHEAL_DESC", $"<style=cIsHealing>治疗效果+100%</style> <style=cStack>（每层增加100%）</style>。<style=cIsHealing>所有治疗效果变为缓慢治疗</style>。<style=cIsHealing>每秒</style><style=cIsHealing>最多恢复</style><style=cIsHealing>最大生命值</style>的<style=cIsHealing>(50 / 层数)%</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_REPULSIONARMORPLATE_DESC", $"所有<style=cIsDamage>传入伤害</style>减少<style=cIsDamage>5</style>点<style=cStack>（每层+5点）</style>，但不能减少到<style=cIsDamage>1以下</style>。<color=#FFFF00>工程师及其炮塔特殊效果：护甲增加<style=cIsDamage>10</style>点<style=cStack>（每层+10点）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SAGESBOOK_DESC", $"你的过去将变得...<style=cIsUtility>熟悉</style>...<style=cIsUtility>（若在游戏结束时持有此物品，会使下一把游戏种子和这把相同）</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SECONDARYSKILLMAGAZINE_DESC", $"为<style=cIsUtility>次要技能</style>增加<style=cIsUtility>1次</style><style=cStack>（每层增加1次）</style>充能。<color=#FFFF00></color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SEED_DESC", $"<style=cIsHealing>恢复</style>等同于你造成<style=cIsDamage>伤害</style>{LeechingSeed.BaseLeech.Value.ToHealPct() + ("（每层+" + LeechingSeed.StackLeech.Value.ToPercent() + "）").ToStack()}的生命值。<style=cIsUtility>（所有伤害均生效）</style>\n<style=cStack>吸血计算公式 => Pow([伤害] x 0.01, 0.65) x 2</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SHOCKNEARBY_DESC", $"发射<style=cIsDamage>电流</style>，每<style=cIsDamage>0.5秒</style>对<style=cIsDamage>3名</style><style=cStack>（每层增加2名）</style>敌人造成<style=cIsDamage>200%</style>的基础伤害。特斯拉线圈每<style=cIsDamage>10秒</style>关闭一次。<color=#FFFF00>装卸工特殊效果：关于“M551电塔”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SLOWONHITVOID_DESC", $"命中时有<style=cIsUtility>5%</style><style=cStack>（每层+5%）</style>几率使敌人<style=cIsDamage>定身</style><style=cIsUtility>1秒</style><style=cStack>（每层+1秒）</style>，但是对<style=cIsVoid>虚空生物</style><style=cDeath>无效</style>。<style=cIsVoid>使所有时空装置无效化</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SPRINTARMOR_DESC", $"<style=cIsHealing>护甲</style>增加<style=cIsHealing>{RoseBuckler.BaseArmor.Value}</style>点<style=cStack>（每层+{RoseBuckler.StackArmor.Value}点）</style>，<style=cIsUtility>奔跑时</style>会逐渐获得<style=cIsUtility>移速</style>和<style=cIsHealing>护甲</style>加成，最高增加<style=cIsUtility>{RoseBuckler.BaseMomentumMove.Value.ToPercent()}</style><style=cStack>（每层+{RoseBuckler.StackMomentumMove.Value.ToPercent()}）</style><style=cIsUtility>移速</style>和<style=cIsHealing>{RoseBuckler.BaseMomentumArmor.Value}</style>点<style=cStack>（每层+{RoseBuckler.StackMomentumArmor.Value}点）</style><style=cIsHealing>护甲</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_STICKYBOMB_DESC", $"命中时有<style=cIsDamage>5%</style><style=cStack>（每层增加5%）</style>的机率向敌人的{"黏弹喷泉".ToDmg()}中添加{"1颗黏弹".ToDmg() + ModConfig.喷泉喷射间隔.Value.ToStack("（喷射间隔：", "s）")}，爆炸时合计造成<style=cIsDamage>180%</style>伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_STUNCHANCEONHIT_DESC", $"命中时有<style=cIsUtility>5%</style><style=cStack>（每层+5%）</style>的几率<style=cIsUtility>眩晕</style>敌人，持续<style=cIsUtility>2秒</style>。<color=#FFFF00>多功能枪兵特殊效果：关于“爆破筒”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SUNBLADE_DESCRIPTION", $"第一次攻击会<style=cIsDamage>点燃</style>敌人，造成<style=cIsDamage>1500%</style>的基础伤害。之后30秒内，对该敌人的每次攻击都会<style=cIsDamage>点燃</style>它，造成<style=cIsDamage>100%</style><style=cStack>（每层+100%）</style>基础伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_SYRINGE_DESC", $"使<style=cIsDamage>攻击速度</style>提高<style=cIsDamage>15%<style=cStack>（每层增加15%）</style></style>。<color=#FFFF00>指挥官特殊效果：额外使攻速，移速和最大生命值增加3%<style=cStack>（每层+3%）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_TITANGOLDDURINGTP_DESC", "在传送器场景中召唤<style=cIsDamage>奥利雷奥尼特</style>，它具有<style=cIsDamage>100%<style=cStack>（每层增加100%）</style>伤害</style>和<style=cIsHealing>100%<style=cStack>（每层增加100%）</style>生命值</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_TOOTH_DESC", $"击败敌人后召唤一个<style=cIsHealing>治疗球</style>，恢复<style=cIsHealing>8</style>点外加等同于<style=cIsHealing>最大生命值</style><style=cIsHealing>2%<style=cStack>（每层+2%）</style></style>的生命值。<color=#FFFF00>呛鼻毒师特殊效果：关于“贪婪撕咬”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_TPHEALINGNOVA_DESC", $"在传送事件中每<style=cIsHealing>{LeptonDaisy.BaseInterval.Value}</style>秒释放一次<style=cIsHealing>治疗新星</style>，<style=cIsHealing>治疗</style>附近所有友方，使他们恢复{LeptonDaisy.BasePulseHeal.Value.ToHealPct()}<style=cStack>（每层+{LeptonDaisy.StackPulseHeal.Value.ToPercent()}）</style>的最大生命值。传送器范围内的友方<style=cIsHealing>生命值再生</style>增加<style=cIsHealing>{LeptonDaisy.BaseHoldoutRegen.Value}hp/s</style><style=cStack>（每层+{LeptonDaisy.StackHoldoutRegen.Value}hp/s）</style>，外加<style=cIsHealing>生命值<color=yellow>百分比</color>再生</style>增加<style=cIsHealing>{LeptonDaisy.BaseHoldoutRegenFraction.Value.ToHealPct()}hp/s</style><style=cStack>（每层+{LeptonDaisy.StackHoldoutRegenFraction.Value.ToPercent()}hp/s）</style>。<color=#FFFF00>雷克斯特殊效果：关于“触须生长”（详情看技能介绍）。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_UTILITYSKILLMAGAZINE_DESC", $"为<style=cIsUtility>辅助技能</style>增加<style=cIsUtility>2次</style><style=cStack>（每层增加2次）</style>充能。<style=cIsUtility>使辅助技能的冷却时间减少</style><style=cIsUtility>33%</style>。<color=#FFFF00>船长特殊效果：使UES顺风号等待时间缩短33%<style=cStack>（每层+33%）</style>。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_WARCRYONMULTIKILL_DESC", $"<style=cIsDamage>击杀敌人</style>会使你获得<style=cIsDamage>狂热</style>增益，持续<style=cIsDamage>{BerzerkersPauldron.BaseBuffDuration.Value}秒</style>。<style=cIsDamage>狂热</style>使<style=cIsUtility>移动速度</style>提高<style=cIsUtility>{BerzerkersPauldron.BuffMove.Value.ToPercent()}</style>，最高<style=cIsUtility>{(BerzerkersPauldron.BuffMove.Value * BerzerkersPauldron.BaseBuffCount.Value).ToPercent()}<style=cStack>（每层+{(BerzerkersPauldron.BuffMove.Value * BerzerkersPauldron.StackBuffCount.Value).ToPercent()}）</style></style>，使<style=cIsDamage>攻击速度</style>提高<style=cIsDamage>{BerzerkersPauldron.BuffAtkSpd.Value.ToPercent()}</style>，最高<style=cIsUtility>{(BerzerkersPauldron.BuffAtkSpd.Value * BerzerkersPauldron.BaseBuffCount.Value).ToPercent()}<style=cStack>（每层+{(BerzerkersPauldron.BuffAtkSpd.Value * BerzerkersPauldron.StackBuffCount.Value).ToPercent()}）</style></style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ITEM_WARDONLEVEL_DESC", $"<style=cIsUtility>升级</style>或开始<style=cIsUtility>传送器事件</style>时放置一面旗帜，强化<style=cIsUtility>16米</style><style=cStack>（每层+8米）</style>内的全体友方，使其<style=cIsUtility>护甲</style>增加<style=cIsUtility>30</style>点，<style=cIsDamage>攻击</style>和<style=cIsUtility>移动速度</style>均提高<style=cIsDamage>30%</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_ACTIVERELOAD", $"<style=cKeywordName>主动上弹</style><style=cSub>开火后给你的磁轨炮上弹。<style=cIsDamage>完美上弹</style>后，下一次射击可额外造成{ScopeAndReload.Damage.ToDmgPct() + "（每次连续的完美上弹x110%）".ToStack()}的基础伤害。{"（每次连续的完美上弹会使上弹条长度缩短10%，中断后重置伤害和上弹条长度）".ToUtil()}", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_ARC", "<style=cKeywordName>击穿</style><style=cSub>在最多4个敌人之间形成电弧，每次造成30%的伤害。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_FLEETING", "<style=cKeywordName>一闪</style><style=cSub><style=cIsDamage>攻速</style>转化为<style=cIsDamage>技能伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_FRICTIONLESS", "<style=cKeywordName>绝对光滑</style><style=cSub>无伤害衰减</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_SOULROT", $"<style=cKeywordName>灵魂之痛</style><style=cSub>每秒<style=cIsVoid>至少</style>造成敌人<style=cIsHealing>最大生命值</style>的<style=cIsVoid>2.5%</style>的伤害，持续{"20秒".ToDmg()}后消失。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_VERY_HEAVY", "<style=cKeywordName>超重</style><style=cSub>下落速度越快，技能造成的伤害越高。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LOADER_SPECIAL_ALT_DESCRIPTION", $"<style=cIsUtility>超重</style>。用重拳砸向地面，造成{(Thunderslam.damage + Thunderslam.yVelocityCoeff * 25f).ToDmgPct()}的范围伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LOADER_SPECIAL_DESCRIPTION", $"扔出飘浮电塔，可<style=cIsDamage>电击</style>周围{M551Pylon.aoe.ToUtil()}米{ToStack("（每层不稳定的特斯拉线圈+" + M551Pylon.aoe + "米）")}内最多<style=cIsDamage>3</style>名敌人，电流最多可弹射{M551Pylon.bounces}次{ToStack("（每层不稳定的特斯拉线圈+1次）")}，造成{ToDmgPct(M551Pylon.damage)}的伤害。可被<style=cIsUtility>格挡</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LOADER_UTILITY_ALT1_DESCRIPTION", $"<style=cIsUtility>重型</style>。发动一次<style=cIsUtility>单体攻击</style>直拳，造成{"2100%的伤害".ToDmg()}，<style=cIsDamage>震荡</style>锥形区域内的所有敌人并造成<style=cIsDamage>1000%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LOADER_UTILITY_DESCRIPTION", $"<style=cIsUtility>重型</style>。发动一次<style=cIsUtility>穿透</style>直拳，造成{"600%-2700%的伤害".ToDmg()}。{"（Proc系数0.6-2.7）".ToStack()}", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MAGE_PRIMARY_FIRE_DESCRIPTION", "<style=cIsDamage>点燃</style>。发射一枚火炎弹，造成<style=cIsDamage>220%<style=cStack>（每层贾罗的手环+100%）</style>的伤害</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MAGE_PRIMARY_LIGHTNING_DESCRIPTION", $"发射一道闪电，造成<style=cIsDamage>300%的伤害</style>并<style=cIsDamage>引爆</style>小片区域。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MAGE_SECONDARY_ICE_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。使用拥有<style=cIsDamage>穿透</style>效果的纳米枪发动攻击，充能后能造成{ToDmgPct(CastNanoSpear.minDamage)}-{ToDmgPct(CastNanoSpear.maxDamage)}的伤害。爆炸后留下一个等同于纳米枪伤害的<style=cIsUtility>冰冻炸弹</style>，2秒后爆炸，<style=cIsUtility>冰冻</style>附件10米{ToStack("（每层鲁纳德的手环+1米，如果暴击则使范围翻倍）")}的敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MAGE_SECONDARY_LIGHTNING_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。发射一枚纳米炸弹，如果充能将造成{ToDmgPct(ChargedNanoBomb.minDamage)}-{ToDmgPct(ChargedNanoBomb.maxDamage)}的伤害{ToStack("（每层尤克里里使伤害增加10%）")}。爆炸后电击周围<style=cIsUtility>30</style>米{ToStack("（每层尤克里里+3米）")}内的<style=cIsUtility>3</style>名{ToStack("（每层尤克里里+1名）")}敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MAGE_SPECIAL_FIRE_DESCRIPTION", $"<style=cIsDamage>点燃</style>。灼烧面前的所有敌人，对其造成每秒{(Flamethrower.Damage + Flamethrower.Damage / (1f / (Flamethrower.BurnChance / 100f))).ToDmgPct()}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MAGE_UTILITY_ICE_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。创造一道能够对敌人造成{ToDmgPct(Snapfreeze.damage)}伤害的屏障。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_PRIMARY_DESCRIPTION", "<style=cIsDamage>一闪</style>。<style=cIsUtility>灵巧</style>。向前挥砍并造成<style=cIsDamage>130%的伤害</style>。第三次攻击的范围将会变大并<style=cIsUtility>暴露</style>敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_SECONDARY_ALT1_DESCRIPTION", $"<style=cIsDamage>一闪</style>。释放一个裂片上勾拳，造成{"600%".ToDmg()}的伤害，并将你送到半空。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_SECONDARY_DESCRIPTION", "<style=cIsDamage>一闪</style>。快速横斩两次，造成<style=cIsDamage>2x200%的伤害</style>，若位于空中，则改为竖斩。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_SPECIAL_ALT1_DESCRIPTION", $"<style=cIsDamage>一闪</style>。发射一次刀刃之风，最多可对<style=cIsDamage>3</style>名敌人造成<style=cIsDamage>8x{ToDmgPct(SlicingWinds.damage)}的伤害</style>。最后一次打击将<style=cIsUtility>暴露</style>敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_SPECIAL_DESCRIPTION", $"<style=cIsDamage>一闪</style>。瞄准距离最近的敌人，攻击被瞄准的敌人可对其重复造成{ToDmgPct(Eviscerate.damageCoefficient)}的伤害。<style=cIsUtility>过程中无法被攻击，跳跃键可以提前退出技能。</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_UTILITY_ALT1_DESCRIPTION", "<style=cIsDamage>一闪</style>。<style=cIsDamage>眩晕</style>。向前冲锋，造成<style=cIsDamage>700%的伤害</style>并在<style=cIsUtility>1秒</style>后<style=cIsUtility>暴露</style>敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("MERC_UTILITY_DESCRIPTION", "<style=cIsDamage>一闪</style>。<style=cIsDamage>眩晕</style>。向前冲锋并造成<style=cIsDamage>300%的伤害</style>。只要命中敌人，<style=cIsDamage>就可以再次发起冲锋</style>，最多<style=cIsDamage>3</style>次。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_ACTIVE_RELOAD_DESCRIPTION", $"在恰到好处的时机上弹可更快恢复，并使你下一发射弹额外造成{ScopeAndReload.Damage.ToDmgPct() + "（每次连续的完美上弹x110%）".ToStack()}的基础伤害。{"（中断后重置伤害）".ToUtil()}", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_PRIMARY_DESCRIPTION", $"发射主动追踪弹药，造成<style=cIsDamage>100%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SECONDARY_ALT_DESCRIPTION", $"启动你的<style=cIsUtility>近程瞄准镜</style>，高亮显示<style=cIsDamage>弱点</style>，并将你的武器转化为一门可造成{ToDmgPct(HH44Marksman.Damage)}伤害的快速磁轨炮。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SECONDARY_DESCRIPTION", $"{"主动上弹".ToUtil()}。启动你的<style=cIsUtility>远程瞄准镜</style>，高亮显示<style=cIsDamage>弱点</style>，并将你的武器转化为一门可造成{ToDmgPct(M99Sniper.Damage)}伤害的穿刺磁轨炮。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SNIPE_CRYO_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。发射一枚超低温射弹，造成{ToDmgPct(Cryocharge.Damage)}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SNIPE_HEAVY_DESCRIPTION", $"发射一枚重型射弹，造成{ToDmgPct(M99Sniper.Damage)}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SNIPE_LIGHT_DESCRIPTION", $"发射一枚轻型射弹，造成{ToDmgPct(HH44Marksman.Damage)}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SNIPE_SUPER_DESCRIPTION", $"发射一枚造成{ToDmgPct(Supercharge.Damage)}的伤害且具有{Supercharge.CritDamage}倍暴击伤害的超载射弹</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SPECIAL_ALT_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。发射<style=cIsDamage>具有穿透效果</style>的子弹，造成{ToDmgPct(Cryocharge.Damage)}的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_SPECIAL_DESCRIPTION", $"发射一枚<style=cIsDamage>具有穿刺效果，</style>造成{ToDmgPct(Supercharge.Damage)}%的伤害且具有{Supercharge.CritDamage}倍暴击伤害</style>的超载射弹。之后，<style=cIsHealth>你的所有武器都将失灵</style>，持续<style=cIsHealth>{Supercharge.HopooBalance}</style>秒。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_UTILITY_ALT_DESCRIPTION", $"扔出一部装置，该装置可使附近所有<style=cIsUtility>敌人和射弹</style><style=cIsUtility>减速（射弹减99%）</style>，使所有友方提速{ToUtil((100 * PolarFieldDevice.SpeedBuffVal).ToString() + "%")}", "zh-CN");
            R2API.LanguageAPI.AddOverlay("RAILGUNNER_UTILITY_DESCRIPTION", $"扔出一部装置，该装置可将你和附近所有敌人<style=cIsUtility>推开</style>。最多可拥有{ConcussionDevice.Charges}部。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("SPIKESTRIPSKILL_DEEPROT_DESCRIPTION", "<style=cIsVoid>虚 空 馈 赠</style>：<style=cIsHealing>毒化</style>攻击改为叠加<style=cIsVoid>虚空之毒</style>，使<style=cIsVoid>速度减慢10%</style>。当<style=cIsVoid>虚空之毒</style>叠加的层数超过<style=cIsVoid>灵魂之痛</style>层数的<style=cIsVoid>5</style>倍时，所有<style=cIsVoid>虚空之毒</style>将转化为<style=cIsVoid>灵魂之痛</style>。此外，所有<style=cArtifact>虚空</style>攻击都有几率叠加<style=cIsVoid>虚空之毒</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("SPIKESTRIPSKILL_DEEPROT_NAME", "腐朽", "zh-CN");
            R2API.LanguageAPI.AddOverlay("STAGE_DRYBASIN_NAME", $"干旱盆地", "zh-CN");
            R2API.LanguageAPI.AddOverlay("STAGE_DRYBASIN_SUBTITLE", $"废弃通道", "zh-CN");
            R2API.LanguageAPI.AddOverlay("STAGE_FORGOTTENHAVEN_NAME", $"遗忘天堂", "zh-CN");
            R2API.LanguageAPI.AddOverlay("STAGE_FORGOTTENHAVEN_SUBTITLE", $"废弃杰作", "zh-CN");
            R2API.LanguageAPI.AddOverlay("STAGE_WEATHEREDSATELLITE_NAME", $"沉睡卫星", "zh-CN");
            R2API.LanguageAPI.AddOverlay("STAGE_WEATHEREDSATELLITE_SUBTITLE", $"高架整体", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TOOLBOT_PRIMARY_ALT1_DESCRIPTION", "发射1条具有穿透效果的钢筋，造成<style=cIsDamage>600%</style><style=cStack>（每层撬棍+30%）</style>的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TOOLBOT_PRIMARY_ALT3_DESCRIPTION", "锯伤周围敌人，造成<style=cIsDamage>每秒1000%的伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TOOLBOT_PRIMARY_DESCRIPTION", "快速发射钉子，造成<style=cIsDamage>70%的伤害</style>。最后一次性发射<style=cIsDamage>12</style>枚伤害为<style=cIsDamage>70%的钉子。<style=cStack>（每发射1枚钉子+0.07米射程，松开后清零）</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TOOLBOT_SECONDARY_DESCRIPTION", "<style=cIsDamage>眩晕</style>。发射一枚造成<style=cIsDamage>220%<style=cStack>（每层眩晕手雷+100%）</style>伤害</style>的爆破筒。将分裂为造成<style=cIsDamage>5x44%<style=cStack>（每层眩晕手雷+20%）</style>伤害</style>以及<style=cIsDamage>眩晕</style>效果的小炸弹。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TREEBOT_SPECIAL_ALT1_DESCRIPTION", $"发射弩弹，造成{ToDmgPct(DIRECTIVEHarvest.damage)}的伤害且弩弹将<style=cIsDamage>注入</style>一个敌人。此敌人死亡时，掉落多个<style=cIsHealing>果实</style>，可治疗<style=cIsHealing>{DIRECTIVEHarvest.percentHeal.ToPercent()}的生命值</style>{(DIRECTIVEHarvest.giveBuffs ? "，并且给予随机<style=cIsHealing>增益</style>" : "")}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TREEBOT_SPECIAL_ALT1_NAME", "命令：收获", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TREEBOT_SPECIAL_DESCRIPTION", $"<style=cIsHealth>25%生命值</style>。发射一朵会<style=cIsDamage>扎根</style>并造成<style=cIsDamage>200%伤害</style>的花朵。每命中一个目标便会对你治疗{TreebotFlower2Projectile.healthFractionYieldPerHit.ToHealPct() + (TreebotFlower2Projectile.healthFractionYieldPerHit * 0.5f).ToPercent().ToStack("（每层轻粒子雏菊+", "）")}的生命值。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TREEBOT_UTILITY_ALT1_DESCRIPTION", $"{BrambleVolley.healthCost.ToPercent().ToHealth()}生命值。发射一次<style=cIsUtility>音爆</style>并对敌人造成{BrambleVolley.damage.ToDmgPct()}的伤害。每命中一个目标便会对你治疗{BrambleVolley.healPercent.ToHealPct()}的生命值。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("TREEBOT_UTILITY_DESCRIPTION", $"发射一次<style=cIsUtility>音爆</style>，<style=cIsDamage>弱化</style>命中的所有敌人。可储存{DIRECTIVEDisperse.maxStock}次", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_FLAMEBREATH", "火焰吐息", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_FLAMEBREATH_DESC", "<style=cDeath>点燃</style>。<style=cIsDamage>灵巧</style>。向前方喷出<style=cIsDamage>火焰</style>，<style=cDeath>点燃</style>敌人，造成<style=cIsDamage>250%</style>的伤害。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_NULLBEAM", "<style=cArtifact>「虚空光束』</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_NULLBEAM_DESC", "<style=cArtifact>虚空</style>。从<style=cIsVoid>虚空</style>中汲取力量，发射中距离<style=cIsVoid>虚空光束</style>攻击敌人，造成<style=cIsDamage>900%</style>的伤害，按住可增加发射的持续时间。每一击都有概率<style=cIsVoid>定身</style>敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_PASSIVE", "<style=cArtifact>虚空</style>克里德", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_PASSIVE_DESC", "所有<style=cArtifact>虚空</style>攻击都有几率<style=cArtifact>定身</style>敌人。（如果选择了“腐朽”被动，则额外叠加<style=cWorldEvent>虚空之毒</style>减益）", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_VOIDDRIFT", "<style=cArtifact>「虚无漂流』</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDCRID_VOIDRIFT_DESC", "<style=cArtifact>虚空</style>。<style=cIsDamage>眩晕</style>。遁入<style=cIsVoid>虚空</style>，造成<style=cIsDamage>400%</style>的总伤害，有概率<style=cIsVoid>定身</style>敌人。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_PRIMARY_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>发射一束造成2000%伤害的短程光束。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_PRIMARY_DESCRIPTION", "发射一束<style=cIsUtility>减速</style>远程光束，造成<style=cIsDamage>240%伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_SECONDARY_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>发射一枚造成2500%伤害的黑洞炸弹，半径变为25米。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_SECONDARY_DESCRIPTION", "充能一枚虚空炸弹，造成<style=cIsDamage>666%伤害</style>。完全充能时可以变成爆炸性虚空炸弹，造成<style=cIsDamage>4444%伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_SECONDARY_UPRADE_TOOLTIP", "<style=cKeywordName>腐化升级</style><style=cSub>转化成能造成2500%伤害的黑洞炸弹，半径变为25米。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_SPECIAL_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>消耗25%的生命值来获得25%的腐化。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VOIDSURVIVOR_UTILITY_ALT_DESCRIPTION", "<style=cIsUtility>消失</style>进入虚空，<style=cIsUtility>向前沿弧线</style>移动，同时<style=cIsUtility>清除所有减益效果</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_DESCRIPTION", "杀死一个敌人会使<style=cIsDamage>10</style>米<style=cStack>（每层+2.5米）</style>内的所有敌人变慢，造成<style=cIsDamage>15%</style><style=cStack>（每层+15%）</style>的伤害，持续<style=cIsUtility>4</style>秒<style=cStack>（每层+2秒）</style>。<style=cIsVoid>腐化所有汽油</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_NAME", "超临界冷却剂", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_PICKUP", "杀死一个敌人会减缓附近的其他敌人。<style=cIsVoid>腐化所有汽油</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.CrystalLotus.instance.ItemLangTokenName + "_DESCRIPTION", "在传送事件中释放<style=cIsUtility>减速</style>脉冲，使敌人和投射物<style=cIsUtility>减速</style>92.5%，持续30秒，释放<style=cIsHealing>3次<style=cStack>（每层+3次）</style></style>。<style=cIsVoid>腐化所有轻粒子雏菊</style>。", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.CrystalLotus.instance.ItemLangTokenName + "_NAME", "结晶的莲花", "zh-CN");
            //R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.CrystalLotus.instance.ItemLangTokenName + "_PICKUP", "在传送事件和‘滞留区’（如虚空领域）中定期释放减速脉冲。<style=cIsVoid>腐化所有轻粒子雏菊</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName + "_DESCRIPTION", $"你的<style=cIsDamage>击杀效果</style>在杀死一个精英后会额外发生<style=cIsDamage>1</style>次<style=cStack>（每层+1次）</style>。另外会产生半径<style=cIsDamage>12</style>米的<style=cIsDamage>爆炸</style>，造成<style=cIsDamage>100%</style>的伤害。<style=cIsVoid>腐化所有陈旧断头台</style>。</style><color=#FFFF00>圣骑士特殊效果：基础伤害增加3点{ToStack("（每层+3点）")}。</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName + "_NAME", "刽子手的重负", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName + "_PICKUP", "你的击杀效果在杀死一个精英后会额外发生一次。在杀死精英时还会造成一个伤害性的AOE。<style=cIsVoid>腐化所有陈旧断头台</style>。", "zh-CN");
            string text = Language.GetString("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_DESCRIPTION");
            text = text.Replace(") will appear in a random location <style=cIsUtility>on each stage</style>. <style=cStack>(Increases rarity chances of the items per stack).</style> <style=cIsVoid>Corrupts all 运输申请单s</style>.", "）的<style=cIsUtility>深空信号</style>。<style=cStack>（层数越高，该物品拥有高稀有度的几率越高）</style>。<style=cIsVoid>腐化所有运输申请单</style>。");
            text = text.Replace("A <style=cIsVoid>special</style> delivery containing items (", "在<style=cIsUtility>每个关卡中</style>，都会在随机位置生成一个内含特殊物品（");
            R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_DESCRIPTION", text, "zh-CN");
            R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_NAME", "无尽的聚宝盆", "zh-CN");
            R2API.LanguageAPI.AddOverlay("VV_ITEM_" + vanillaVoid.Items.VoidShell.instance.ItemLangTokenName + "_PICKUP", "获得一个特殊的、危险的快递，并获得强大的奖励。<style=cIsVoid>腐化所有运输申请单</style>。", "zh-CN");
        }

        public static void 权杖技能汉化() {
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERDESC", Language.GetString("BANDIT2_SPECIAL_DESCRIPTION") + ToScepterDescription($"额外发射{"1发".ToDmg() + ToStack("（每层+1发）")}子弹。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERNAME", "暗杀", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERDESC", Language.GetString("BANDIT2_SPECIAL_ALT_DESCRIPTION") + ToScepterDescription($"子弹有25%{ToStack("（每个标记+0.25%）")}概率{ToStack("（不受运气影响）")}弹射到36米{"（每层+12米）".ToStack()}内的最多9名敌人{"（每层+3名）".ToStack()}。\n每次弹射后距离和伤害减少{"10%".ToDeath()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERNAME", "叛徒", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEALTDESC", Language.GetString("CAPTAIN_UTILITY_ALT1_DESCRIPTION") + ToScepterDescription($"自动制导：以{" 自身等级 x 权杖层数 m/s".ToUtil()}的速度追踪敌人。{"莉莉丝".ToRed()}具有{"2倍".ToUtil()}的范围和伤害，并且在打击后产生{"冲击波".ToDmg()}。呼叫{"莉莉丝".ToRed()}将{"消耗6层充能".ToUtil()}，打击抵达前有{"30秒".ToUtil()}等待时间。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEALTNAME", "PHN-8300“莉莉丝”打击".ToRed(), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEDESC", Language.GetString("CAPTAIN_UTILITY_DESCRIPTION") + ToScepterDescription($"按住可连续呼叫UES顺风号，总共可造成{"21x500%".ToDmg()}伤害。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKENAME", "连续轨道炮", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_COMMANDO_BARRAGEDESC", Language.GetString("COMMANDO_SPECIAL_DESCRIPTION") + ToScepterDescription($"向射程内的每个敌人以{"2倍".ToUtil()}的速度和精度发射{"2倍".ToUtil()}的子弹。按住你的主要技能可以更精确地射击。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_COMMANDO_BARRAGENAME", "死亡绽放", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_COMMANDO_GRENADEDESC", Language.GetString("COMMANDO_SPECIAL_ALT1_DESCRIPTION") + ToScepterDescription($"扔出{"10".ToDmg()}枚具有{"一半".ToDmg()}的伤害和击退的炸弹。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_COMMANDO_GRENADENAME", "地毯式轰炸", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_CROCO_DISEASEDESC", Language.GetString("CROCO_SPECIAL_DESCRIPTION") + ToScepterDescription($"使受害者成为行走的{"瘟疫之源".ToPoison()}，持续将瘟疫{"传染".ToPoison()}给周围的敌人。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_CROCO_DISEASENAME", "瘟疫", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_ENGI_TURRETDESC", Language.GetString("ENGI_SPECIAL_DESCRIPTION") + ToScepterDescription($"可额外放置{"1".ToUtil()}座炮台。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_ENGI_TURRETNAME", "TR12-C 高斯自动炮台", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_ENGI_WALKERDESC", Language.GetString("ENGI_SPECIAL_ALT1_DESCRIPTION") + ToScepterDescription($"可额外放置{"2".ToUtil()}座炮塔。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_ENGI_WALKERNAME", "TR58-C 碳化器炮塔", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_HERETIC_SQUAWKDESC", $"<style=cIsHealth>歌声</style>将标记所有<style=cIsHealth>活体</style>，使其染上<link=\"BulwarksHauntWavy\">{"灭绝".ToRed()}</link>！<link=\"BulwarksHauntWavy\">{"灭绝".ToRed()}</link>：持续{"10秒".ToUtil() + "（每层权杖+10秒）".ToStack()}，当带有<link=\"BulwarksHauntWavy\">{"灭绝".ToRed()}</link>的敌人{"死去".ToRed()}时，会连带着它的{"所有族人".ToRed()}一起{"死去".ToRed()}。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_HERETIC_SQUAWKNAME", "<link=\"BulwarksHauntWavy\">灭绝之歌</link>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_HUNTRESS_BALLISTADESC", Language.GetString("HUNTRESS_SPECIAL_ALT1_DESCRIPTION") + ToScepterDescription($"每次射击额外发射{"3根".ToDmg() + "（每层+3根）".ToStack()}弩箭。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_HUNTRESS_BALLISTANAME", "-腊包尔->", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_HUNTRESS_RAINDESC", Language.GetString("HUNTRESS_SPECIAL_DESCRIPTION") + ToScepterDescription($"点燃。自动传送到敌人脚下。{"1.5倍".ToUtil()}半径和持续时间。{"2倍Proc系数".ToUtil()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_HUNTRESS_RAINNAME", "火雨", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_LOADER_CHARGEFISTDESC", Language.GetString("LOADER_UTILITY_DESCRIPTION") + ToScepterDescription($"冲刺速度增加{"100%".ToUtil() + "（每层权杖+100%）".ToStack()}。高到离谱的击退。将{"移速属性".ToUtil()}纳入伤害计算。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_LOADER_CHARGEFISTNAME", "百万吨重拳", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MAGE_FLAMETHROWERDESC", Language.GetString("MAGE_SPECIAL_FIRE_DESCRIPTION") + ToScepterDescription($"燃烧将留下{"灼热的火云".ToFire()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MAGE_FLAMETHROWERNAME", "龙息", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MAGE_FLYUPDESC", Language.GetString("MAGE_SPECIAL_LIGHTNING_DESCRIPTION") + ToScepterDescription($"{"2倍".ToDmg()}伤害、{"5倍".ToUtil()}半径，{"10倍".ToUtil()}击退。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MAGE_FLYUPNAME", "反物质浪涌", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MERC_EVISDESC", Language.GetString("MERC_SPECIAL_DESCRIPTION") + ToScepterDescription($"持续时间增加{"100%".ToUtil() + "（每层权杖+100%）".ToStack()}。此技能击杀敌人可{"重置持续时间".ToUtil()}，按住跳跃键可在击杀时{"退出".ToDeath()}技能！"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MERC_EVISNAME", "屠戮", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MERC_EVISPROJDESC", Language.GetString("MERC_SPECIAL_ALT1_DESCRIPTION") + ToScepterDescription($"刀刃之风的飞行速度和攻击频率增加{"200%".ToDmg()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_MERC_EVISPROJNAME", "死亡之风", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPECRYODESC", Language.GetString("RAILGUNNER_SPECIAL_ALT_DESCRIPTION") + ToScepterDescription($"击中产生{"冰冻".ToIce()}爆炸，{"冰冻".ToIce() + "20米".ToUtil()}内的敌人{"2秒".ToUtil()}，合计造成{"200%".ToDmg()}伤害并使其减少{"80%".ToUtil()}的移速{"20秒".ToUtil()}"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPECRYONAME", "T°->绝对零度", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_RAILGUNNER_FIRESNIPECRYONAME", "<color=blue>冰！</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPESUPERDESC", Language.GetString("RAILGUNNER_SPECIAL_DESCRIPTION") + ToScepterDescription($"发射时，将所有{"金钱".ToYellow()}按{"10%".ToYellow() + "（每层权杖+10%）".ToStack()}的转换率转化为伤害。击中将永久降低敌人{"20点".ToDmg()}护甲。Proc系数增加{"0.5".ToUtil()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPESUPERNAME", "超电磁炮", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_RAILGUNNER_FIRESNIPESUPERNAME", "“一枚硬币”", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_SCEPLOADER_CHARGEZAPFISTDESC", Language.GetString("LOADER_UTILITY_ALT1_DESCRIPTION") + ToScepterDescription($"全向{"闪电".ToLightning()}，击中时召唤{"雷电".ToLightning()}，合计造成{"400%".ToDmg() + "（每层电能钻机+400%）".ToStack()}的伤害。冲刺速度增加{"100%".ToUtil() + "（每层权杖+100%）".ToStack()}，冲刺时{"临时提高护甲".ToUtil()}。将{"移速属性".ToUtil()}纳入伤害计算。{"“<link=\"BulwarksHauntShaky\">以雷霆~ 击碎黑暗！</link>”".ToLightning()}"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_SCEPLOADER_CHARGEZAPFISTNAME", "雷霆拳套".ToLightning(), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_TOOLBOT_DASHDESC", Language.GetString("TOOLBOT_UTILITY_DESCRIPTION") + ToScepterDescription($"进入{"毁灭模式".ToRed()}，可通过{"跳跃按键".ToUtil()}退出。将传入的伤害{"减半".ToUtil()}（与护甲叠加）。退出时产生巨大的{"爆炸".ToDmg()}击晕敌人，合计造成所受伤害{"200%".ToDmg()}的伤害。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_TOOLBOT_DASHNAME", "毁灭模式".ToRed(), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_TREEBOT_FLOWER2DESC", Language.GetString("TREEBOT_SPECIAL_DESCRIPTION") + ToScepterDescription($"{"2倍".ToUtil()}范围。造成随机{"减益".ToDeath()}。从{"第二层".ToUtil()}权杖开始，每层权杖使花朵的持续时间增加{"100%".ToUtil()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_TREEBOT_FLOWER2NAME", "混沌生长", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_TREEBOT_FRUIT2DESC", Language.GetString("TREEBOT_SPECIAL_ALT1_DESCRIPTION") + ToScepterDescription($"生成额外的{"果实".ToHealing()}，{"果实".ToHealing()}给予强大的随机{"增益".ToHealing()}。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_TREEBOT_FRUIT2NAME", "终极命令：收割", "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_VOIDSURVIVOR_CRUSHCORRUPTIONDESC", Language.GetString("VOIDSURVIVOR_SPECIAL_DESCRIPTION") + ToScepterDescription($"技能效果影响周围{"25米".ToUtil() + "（每层+25米）".ToStack()}内的敌人和盟友。"), "zh-CN");
            R2API.LanguageAPI.AddOverlay("ANCIENTSCEPTER_VOIDSURVIVOR_CRUSHCORRUPTIONNAME", "「促进」", "zh-CN");
        }

        public static void 圣骑士汉化() {
            string text = "<color=yellow>(本人物由QQ用户“疯狂”(2437181705)翻译)</color>圣骑士是一位重击坦克，可以选择超凡的魔法或毁灭性的剑术来帮助盟友和消灭敌人。<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 你的被动状态占了你伤害的很大一部分，尽可能地保持它。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 旋转猛击既可以作为强大的群体控制技能，也可以作为限制移动的一种形式。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 疾走（Quickstep）的冷却时间随着每一次命中而降低，这是对你坚持到底的奖励。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 沉默誓言（Vow of Silence）是对付飞行敌人的好方法，因为它会把所有受影响的敌人拖到地上。</color>" + Environment.NewLine + Environment.NewLine;
            R2API.LanguageAPI.AddOverlay("PALADIN_NAME", "圣骑士", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_DESCRIPTION", text, "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SUBTITLE", "普罗维登斯的侍从", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_OUTRO_FLAVOR", "..于是他离开了，对他的教义的信心动摇了。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_OUTRO_FAILURE", "..于是他消失了，他的祈祷没有人听到。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_DEFAULT_SKIN_NAME", "默认", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_LUNAR_SKIN_NAME", "月球", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_LUNARKNIGHT_SKIN_NAME", "月光骑士", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_TYPHOON_SKIN_NAME", "君主", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_TYPHOONLEGACY_SKIN_NAME", "君主（经典）", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_POISON_SKIN_NAME", "腐败", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_POISONLEGACY_SKIN_NAME", "腐败（经典）", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_CLAY_SKIN_NAME", "阿菲利安", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_SPECTER_SKIN_NAME", "幽灵", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_DRIP_SKIN_NAME", "Drip", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADINBODY_MINECRAFT_SKIN_NAME", "我的世界", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LUNAR_KNIGHT_BODY_NAME", "月光骑士", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LUNAR_KNIGHT_BODY_DESCRIPTION", text, "zh-CN");
            R2API.LanguageAPI.AddOverlay("LUNAR_KNIGHT_BODY_SUBTITLE", "米斯历克斯的侍从", "zh-CN");
            R2API.LanguageAPI.AddOverlay("LUNAR_KNIGHT_BODY_OUTRO_FLAVOR", "..于是他离开了，对他的教义的信心动摇了。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("NEMPALADIN_NAME", "复仇圣骑士", "zh-CN");
            R2API.LanguageAPI.AddOverlay("NEMPALADIN_DESCRIPTION", text, "zh-CN");
            R2API.LanguageAPI.AddOverlay("NEMPALADIN_SUBTITLE", "普罗维登斯的侍从", "zh-CN");
            R2API.LanguageAPI.AddOverlay("NEMPALADIN_OUTRO_FLAVOR", "..于是他离开了，对他的教义的信心动摇了。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("NEMPALADIN_OUTRO_FAILURE", "..于是他消失了，他的祈祷没有人听到。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_PASSIVE_NAME", "堡垒的祝福", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_PASSIVE_DESCRIPTION", "每级获得<style=cIsHealing>1护甲</style>。当拥有<style=cIsHealth>90%生命值</style>或者拥有任意<style=cIsHealth>护盾</style>时，圣骑士会获得<style=cIsHealing>祝福</style>，强化所有剑技。", "zh-CN");
            text = "向前劈砍，造成<style=cIsDamage>350%伤害</style>。如果圣骑士拥有<style=cIsHealing>祝福</style>，会发射一道<style=cIsUtility>剑光</style>造成<style=cIsDamage>300%伤害</style>。";
            R2API.LanguageAPI.AddOverlay("PALADIN_PRIMARY_SLASH_NAME", "神圣之剑", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_PRIMARY_SLASH_DESCRIPTION", text, "zh-CN");
            text = "向前劈砍，造成<style=cIsDamage>380%伤害</style>。如果圣骑士拥有<style=cIsHealing>祝福</style>，会发射一道<style=cIsUtility>剑光</style>造成<style=cIsDamage>300%伤害</style>。";
            R2API.LanguageAPI.AddOverlay("PALADIN_PRIMARY_CURSESLASH_NAME", "诅咒之剑", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_PRIMARY_CURSESLASH_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>眩晕</style>。使用一个大范围的劈砍，造成<style=cIsDamage>1000% 伤害</style>，如果拥有<style=cIsHealing>祝福</style>会提升劈砍范围。在空中释放会变成裂地斩，如果拥有<style=cIsHealing>祝福</style>会造成<style=cIsUtility>冲击波</style>。";
            R2API.LanguageAPI.AddOverlay("PALADIN_SECONDARY_SPINSLASH_NAME", "回旋斩", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SECONDARY_SPINSLASH_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>震荡</style>。<style=cIsUtility>灵巧</style>。充能并扔出一道<style=cIsUtility>闪电束</style>，造成<style=cIsDamage>800%伤害</style>。如果被闪电束击中，会在剑上附着<style=cIsUtility>闪电</style>效果，持续<style=cIsUtility>4秒</style>。";
            R2API.LanguageAPI.AddOverlay("PALADIN_SECONDARY_LIGHTNING_NAME", "圣光之矛", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SECONDARY_LIGHTNING_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>灵巧</style>。发射一连串<style=cIsUtility>月球碎片</style>，每个造成<style=cIsDamage>75%伤害</style>。最多拥有<style=cIsDamage>12</style>碎片。";
            R2API.LanguageAPI.AddOverlay("PALADIN_SECONDARY_LUNARSHARD_NAME", "月球碎片", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>冲刺</style>一小段距离并获得<style=cIsHealing>10%屏障</style>。成功使用<style=cIsDamage>神圣之剑</style>击中敌人会<style=cIsUtility>减少冷却</style><style=cIsDamage>1秒</style>。<style=cIsUtility>最多可储存2次充能<style=cIsHealing>";
            R2API.LanguageAPI.AddOverlay("PALADIN_UTILITY_DASH_NAME", "疾走", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_UTILITY_DASH_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "恢复<style=cIsHealing>",
                15.ToString(),
                "%最大生命值</style>并且为范围内的盟友提供<style=cIsHealing>",
                15.ToString(),
                "%屏障</style>。"
            });
            R2API.LanguageAPI.AddOverlay("PALADIN_UTILITY_HEAL_NAME", "补给", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_UTILITY_HEAL_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                1.5f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>祝福</style>区域，持续<style=cIsDamage>",
                12f.ToString(),
                "秒</style>。在范围内的所有盟友会缓慢<style=cIsHealing>恢复生命值</style>并且获得<style=cIsHealing>屏障</style>。"
            });
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_HEALZONE_NAME", "神圣之光", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_HEALZONE_DESCRIPTION", text, "zh-CN");
            text += ToScepterDescription("双倍治疗。双倍屏障。清除减益。");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPTERHEALZONE_NAME", "神圣之光", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPTERHEALZONE_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>沉默</style>区域，持续<style=cIsDamage>",
                10f.ToString(),
                "秒</style>，使区域内所有敌人<style=cIsHealth>麻木</style>。（禁用技能和特殊效果）"
            });
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_TORPOR_NAME", "沉默誓言", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_TORPOR_DESCRIPTION", text, "zh-CN");
            text += ToScepterDescription("更强的减益。更大的范围。摧毁投射物。");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPTERTORPOR_NAME", "沉默誓言", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPTERTORPOR_DESCRIPTION", text, "zh-CN");
            text = string.Concat(new string[]
            {
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>，然后释放一个<style=cIsUtility>强化</style>区域，持续<style=cIsDamage>",
                8f.ToString(),
                "秒</style>，提升范围内的所有盟友<style=cIsDamage>伤害</style>和<style=cIsDamage>攻速</style>。"
            });
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_WARCRY_NAME", "神圣誓言", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_WARCRY_DESCRIPTION", text, "zh-CN");
            text += ToScepterDescription("更快的施法速度。双倍伤害。双倍攻速。");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPTERWARCRY_NAME", "神圣誓言(Scepter)", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPTERWARCRY_DESCRIPTION", text, "zh-CN");
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
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SUN_NAME", "暴烈之阳", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SUN_DESCRIPTION", text, "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SUN_CANCEL_NAME", "取消暴烈之阳", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SUN_CANCEL_DESCRIPTION", "停止引导当前的暴烈之阳。", "zh-CN");
            text += ToScepterDescription("再次投掷并保持瞄准，然后释放太阳，爆炸对周围<style=cDeath>一切生物</style>造成<style=cIsDamage>4000%伤害</style>。");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPSUN_NAME", "太阳耀斑", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPSUN_DESCRIPTION", text, "zh-CN");
            text = "<style=cIsUtility>引导</style><style=cIsDamage>5</style>秒，然后在指定位置创造一个<style=cIsUtility>微型太阳</style>，吸收周围<style=cIsHealth>所有</style>生物的<style=cIsDamage>生命</style> 。<color=red>让火焰净化一切！</color>";
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SUN_LEGACY_NAME", "暴烈之阳(经典)", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SUN_LEGACY_DESCRIPTION", text, "zh-CN");
            text += ToScepterDescription("产生大规模爆炸，造成9000%伤害。");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPSUN_LEGACY_NAME", "太阳耀斑", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_SCEPSUN_LEGACY_DESCRIPTION", text, "zh-CN");
            text = "While below <style=cIsHealth>25% health</style>, generate <style=cIsDamage>Rage</style>. When at max <style=cIsDamage>Rage</style>, use to enter <color=#dc0000>Berserker Mode</color>, gaining a <style=cIsHealing>massive buff</style> and a <style=cIsUtility>new set of skills</style>.";
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_BERSERK_NAME", "狂暴", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_SPECIAL_BERSERK_DESCRIPTION", text, "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_SWORDBEAM", "<style=cKeywordName>剑光</style><style=cSub>一道短距离可穿透的光束，造成<style=cIsDamage>300%伤害</style>.", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_TORPOR", "<style=cKeywordName>麻木</style><style=cSub>造成<style=cIsHealth>60%</style>移动和攻击速度<style=cIsDamage>减缓</style>。<style=cIsHealth>将敌人拖至地面。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_OVERHEAT", "<style=cKeywordName>过热</style><style=cSub>乘以从<style=cIsDamage>太阳</style>受到的伤害。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME", "圣骑士的誓言", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC", "使用“忠诚之珠”，再次变得完整。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_UNLOCKABLE_UNLOCKABLE_NAME", "圣骑士的誓言", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "RobPaladin：精通", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，击败游戏或消灭季风。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "RobPaladin：精通", "zh-CN");
            string str = "\n<color=#8888>(台风困难需要星际风暴2模组)</color>";
            R2API.LanguageAPI.AddOverlay("PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME", "RobPaladin：大师", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，在台风或更高级别难度下通关或者抹除自己。" + str, "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME", "RobPaladin：大师", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_NAME", "他的弟子", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，与腐败女神立约。（看不懂）", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_POISONUNLOCKABLE_UNLOCKABLE_NAME", "她的弟子", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME", "Jolly Cooperation", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，用皇家电容器打击敌人。<color=#c11>仅主机</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_LIGHTNINGSPEARUNLOCKABLE_UNLOCKABLE_NAME", "Jolly Cooperation", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_NAME", "迷失国王的先驱", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，一次持有8件月球物品。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_LUNARSHARDUNLOCKABLE_UNLOCKABLE_NAME", "迷失国王的先驱", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME", "温暖的拥抱", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，用身形扭曲的木灵治疗一个盟友。<color=#c11>仅主机</color>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_HEALUNLOCKABLE_UNLOCKABLE_NAME", "温暖的拥抱", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME", "抑制", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，对一个敌人堆叠4层debuff。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_TORPORUNLOCKABLE_UNLOCKABLE_NAME", "抑制", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_NAME", "阳光", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，承受太阳的全部冲击并生存下来。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_CRUELSUNUNLOCKABLE_UNLOCKABLE_NAME", "阳光", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_NAME", "古代遗物", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_DESC", "作为圣骑士，获得一个泥沼之瓮。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("PALADIN_CLAYUNLOCKABLE_UNLOCKABLE_NAME", "古代遗物", "zh-CN");
            R2API.LanguageAPI.AddOverlay("BROTHER_SEE_PALADIN_1", "兄弟？不，便宜的仿制品。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("BROTHER_SEE_PALADIN_2", "我会对你的信仰负责。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("BROTHER_SEE_PALADIN_3", "浪费潜力。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("BROTHER_KILL_PALADIN_1", "你粗糙的盔甲让你失望了。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("BROTHER_KILL_PALADIN_2", "看看你的信仰给你带来了什么。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("BROTHER_KILL_PALADIN_3", "一无所获，愚蠢的奉献者。", "zh-CN");
        }

        public static void 探路者汉化() {
            string str = "BOG";
            string str2 = str + "_PATHFINDER_BODY_";
            string str3 = str + "_SQUALL_BODY_";
            R2API.LanguageAPI.AddOverlay(str3 + "NAME", "狂风", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "NAME", "探路者", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SUBTITLE", "猛禽", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "DEFAULT_SKIN_NAME", "默认", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "MASTERY_SKIN_NAME", "猎头者", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "PASSIVE_NAME", "驯鹰者", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "PASSIVE_DESCRIPTION", "一只机器猎鹰（狂风）将跟随你，它会继承你的<style=cIsDamage>大部分</style>物品，并且<style=cIsUtility>免疫</style>所有伤害，但它靠<style=cIsUtility>电池</style>运行。", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_BATTERY", "<style=cKeywordName>电池</style><style=cSub>狂风有两种模式：<color=#FF0000>攻击</color>和<color=#00FF00>跟随目标</color>。在<color=#FF0000>攻击模式</color>下，狂风每秒<style=cIsDamage>消耗 8%</style>的电量。在<color=#00FF00>跟随模式</color>下，狂风每秒<style=cIsHealing>充能 1%</style>电量，速度随<style=cIsUtility>攻速</style>变换。如果电量耗尽，狂风将强制进入<color=#00FF00>跟随模式</color>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_PIERCE", "<style=cKeywordName>穿孔</style><style=cSub>用尖端<style=cIsUtility>攻击</style>造成<style=cIsDamage>325%伤害</style>并忽略<style=cIsDamage>敌人护甲</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_ELECTROCUTE", "<style=cKeywordName>电击</style><style=cSub>使目标移动速度降低50%，每秒造成<style=cIsDamage>120% 的伤害</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_ATTACK", "<style=cKeywordName><color=#FF0000>攻击</color></style><style=cSub>指挥狂风攻击敌人，并激活<color=#FF0000>攻击模式</color>，使用机枪造成<style=cIsDamage>2x50% 伤害</style>，发射导弹造成<style=cIsDamage>300% 伤害</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_FOLLOW", "<style=cKeywordName><color=#00FF00>跟随目标</color></style><style=cSub>让狂风回到身边，激活<color=#00FF00>跟随模式</color>，使狂风跟随在身边。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_SQUALL_UTILITY", "<style=cKeywordName><color=#87b9cf>辅助</color></style><style=cSub>指挥狂风使用<style=cIsUtility>辅助</style>技能。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay("KEYWORD_SQUALL_SPECIAL", "<style=cKeywordName><color=#efeb1c>特殊--追击!</color></style><style=cSub>指挥狂风反复打击目标敌人，造成<style=cIsDamage>70% 伤害</style>。每次攻击都会降低目标<style=cIsDamage>5点</style><style=cIsDamage>护甲</style> ，并<style=cIsUtility>充能 2%</style>电量，暴击时有双倍效果。此技能可将暂时将电量充能到<style=cIsUtility>120%</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "PRIMARY_THRUST_NAME", "突刺", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "PRIMARY_THRUST_DESCRIPTION", "<style=cIsUtility>穿孔</style>。将你的长矛向前刺去，造成<style=cIsDamage>250% 伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SECONDARY_DASH_NAME", "旋风腿", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SECONDARY_DASH_DESCRIPTION", "<style=cIsUtility>冲刺</style>。短距离冲刺后，将矛举起准备投掷。投掷矛将产生<style=cIsDamage>爆炸</style>造成<style=cIsDamage>1000% 伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SECONDARY_JAVELIN_NAME", "爆炸标枪", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SECONDARY_JAVELIN_DESCRIPTION", "投掷<style=cIsDamage>爆炸性</style>标枪，造成<style=cIsDamage>1000% 伤害</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "UTILITY_SPIN_NAME", "撕裂之爪", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "UTILITY_BOLAS_NAME", "闪电套索", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "UTILITY_BOLAS_DESCRIPTION", "投掷带电的套索，<style=cIsUtility>电击</style>周围敌人，并产生<style=cIsUtility>电击</style>区域，存在<style=cIsUtility>10秒</style>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_COMMAND_NAME", "指令", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_COMMAND2_NAME", "指令 - 辅助", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_COMMAND_DESCRIPTION", "向狂风发出指令。你可以指挥狂风<color=#FF0000>攻击</color>，<color=#00FF00>跟随目标</color>或<color=#efeb1c>特殊指令</color>。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_COMMAND2_DESCRIPTION", "<color=#3ea252>特殊指令</color>。狂风的<color=#efeb1c>特殊指令</color>现在替换了你的<style=cIsUtility>特殊</style>技能。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_ATTACK_NAME", "攻击 - 指令", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_ATTACK_DESCRIPTION", "</style><style=cSub>指挥狂风攻击敌人，并激活<color=#FF0000>攻击模式</color>，使用机枪造成<style=cIsDamage>2x50%伤害</style>，发射导弹造成<style=cIsDamage>300% 伤害</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_FOLLOW_NAME", "跟随目标 - 指令", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_FOLLOW_DESCRIPTION", "<style=cSub>让狂风回到身边，激活<color=#00FF00>跟随模式</color>，使狂风跟随在身边。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_CANCEL_NAME", "取消", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "SPECIAL_CANCEL_DESCRIPTION", "取消", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str + "_SQUALL_SPECIAL_GOFORTHROAT_NAME", "追击!", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str + "_SQUALL_SPECIAL_GOFORTHROAT_DESCRIPTION", "</style><style=cSub>指挥狂风反复打击目标敌人，造成<style=cIsDamage>70% 伤害</style>。每次攻击都会降低目标<style=cIsDamage>5点</style><style=cIsDamage>护甲</style> ，并<style=cIsUtility>充能 2%</style>电量，暴击时有双倍效果。此技能可将暂时将电量充能到<style=cIsUtility>120%</style>。</style>", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Pathfinder.：精通", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "作为探路者，在季风难度下通关或抹除自己。", "zh-CN");
            R2API.LanguageAPI.AddOverlay(str2 + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Pathfinder.：精通", "zh-CN");
        }

        public static void 象征汉化() {
            On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuController_Start;
        }

        private static void MainMenuController_Start(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self) {
            orig(self);
            TPDespair.ZetAspects.Language.tokens["zh-CN"]["ITEM_SHIELDONLY_DESC"] += $"\n<style=cIsLunar>完美的象征：护盾百分比再生速度增加{TPDespair.ZetAspects.Configuration.AspectLunarRegen.Value}%{TPDespair.ZetAspects.Configuration.AspectLunarRegen.Value.ToStack("（每层+", "%）")}。</style>";
            On.RoR2.UI.MainMenu.MainMenuController.Start -= MainMenuController_Start;
        }
    }
}