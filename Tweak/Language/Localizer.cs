using BTP.RoR2Plugin.Tweaks.EquipmentTweaks;
using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using BTP.RoR2Plugin.Tweaks.SurvivorTweaks;
using EntityStates.Commando.CommandoWeapon;
using EntityStates.GoldGat;
using EntityStates.Huntress;
using EntityStates.Mage.Weapon;
using EntityStates.Merc;
using EntityStates.Treebot.TreebotFlower;
using EntityStates.Treebot.Weapon;
using RedGuyMod.SkillStates.Ravager;
using RoR2;
using RoR2.Items;
using RoR2.UI.MainMenu;
using System;
using System.Threading.Tasks;
using TPDespair.ZetAspects;
using UnityEngine;

namespace BTP.RoR2Plugin.Language {

    internal static class Localizer {
        public const string lang = "zh-CN";
        public const string 暴击 = "<style=cIsDamage>暴击</style>";
        public const string 爆炸 = "<style=cIsDamage>爆炸</style>";
        public const string 不与其他流血和出血重复 = "<style=cIsUtility>（不与其他流血和出血重复）</style>";
        public const string 不与其他瓦解重复 = "<style=cIsUtility>（不与其他瓦解重复）</style>";
        public const string 出血 = "<style=cDeath>出血</style>";
        public const string 此处忽略触发系数 = "<style=cIsUtility>（此处忽略触发系数）</style>";
        public const string 大小写字母串 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public const string 额外 = "<style=cIsDamage>额外</style>";
        public const string 焚烧 = "<color=#f25d25>焚烧</color>";
        public const string 攻击速度 = "<style=cIsDamage>攻击速度</style>";
        public const string 护盾 = "<style=cIsHealing>护盾</style>";
        public const string 护甲 = "<style=cIsHealing>护甲</style>";
        public const string 基础护盾百分比再生速度 = "<style=cIsHealing>基础护盾<color=yellow>百分比</color>再生速度</style>";
        public const string 基础伤害 = "<style=cIsDamage>基础伤害</style>";
        public const string 基础生命值百分比再生速度 = "<style=cIsHealing>基础生命值<color=yellow>百分比</color>再生速度</style>";
        public const string 基础生命值再生速度 = "<style=cIsHealing>基础生命值再生速度</style>";
        public const string 金钱 = "<color=yellow>金钱</color>";
        public const string 冷却时间 = "<style=cIsUtility>冷却时间</style>";
        public const string 流血 = "<style=cIsDamage>流血</style>";
        public const string 燃烧 = "<style=cIsDamage>燃烧</style>";
        public const string 伤害 = "<style=cIsDamage>伤害</style>";
        public const string 眩晕 = "<style=cIsUtility>眩晕</style>";
        public const string 移动速度 = "<style=cIsUtility>移动速度</style>";
        public const string 总伤害 = "<style=cIsDamage>总伤害</style>";

        [ModLoadMessageHandler]
        private static void Init() {
            MainMenuController.OnMainMenuInitialised += OnMainMenuFirstInitialised;
        }

        private static void 基础汉化() {
            "ITEM_JUMPBOOST_DESC".AddOverlay($"在地面上<style=cIsUtility>奔跑时跳跃</style>可以增加<style=cIsUtility>10米</style><style=cStack>（每层增加10米）</style>的距离。<color=#FFFF00>掠夺者特殊效果：跳跃能力提高{RavagerTweak.JumpPowerMultCoefficient.ToBaseAndStkPct()}</color>", lang);
            "ITEM_RANDOMEQUIPMENTTRIGGER_DESC".AddOverlay($"每隔{RandomEquipmentTriggerTweak.RandomEquipmentTriggerBehavior.transformInterval.ToUtil("_秒")}，{"随机选择".ToUtil()}你的一件物品转化为{"相同品质".ToUtil()}的一件随机物品，每次转化的数量最多{RandomEquipmentTriggerTweak.RandomEquipmentTriggerBehavior.transformCountPerStack.ToBaseAndStk().ToUtil("_个")}。", lang);
            "EQUIPMENT_SAWMERANG_DESC".AddOverlay($"投掷<style=cIsDamage>三个穿透性的回旋锯</style>，每个造成{SawTweak.DamageCoefficient.ToDmgPct("_的基础伤害")}，同时锯伤敌人，造成额外的<style=cIsDamage>每秒100％的基础伤害</style>，并使其{流血}，在{BleedTweak.BleedDuration.ToUtil("_秒")}内合计造成{BleedTweak.BleedDamageCoefficient.ToDmgPct("_的总伤害")}；若造成{暴击}，则{额外}造成{出血}，在{BleedTweak.SurperBleedDuration.ToUtil("_秒")}内合计造成{BleedTweak.SurperBleedDamageCoefficient.ToDmgPct("_的总伤害")}。", lang);
            "KEYWORD_SUPERBLEED".AddOverlay($"<style=cKeywordName>出血</style><style=cSub>造成每秒{BleedTweak.SurperBleedDamageCoefficient.ToDmgPct("_的基础伤害")}。<i>出血可以叠加。</i></style>", lang);
            "BUFF_SUPERBLEED_DESC".AddOverlay($"每秒受到{BleedTweak.SurperBleedDamageCoefficient.ToDmgPct("_的基础伤害")}。", lang);
            "ITEM_NOVAONLOWHEALTH_PICKUP".AddOverlay("受到一定量的伤害后或者处于低生命值时，爆发一次大范围新星。", lang);
            "ITEM_NOVAONLOWHEALTH_DESC".AddOverlay($"受到的伤害{"累计".ToDmg()}超过{"75%的生命值".ToHealing()}后或者低于<style=cIsHealth>25%的生命值</style>时，以你为中心爆发一次{"闪电新星".ToLightning()}，造成{EntityStates.VagrantNovaItem.DetonateState.blastDamageCoefficient.ToBaseAndStkPct().ToDmg("_的基础伤害")}并{"击晕".ToLightning()}敌人，然后进入<style=cIsUtility>30秒<style=cStack>（每层减少50%）</style></style>冷却阶段。", lang);
            "ITEM_LASERTURBINE_PICKUP".AddOverlay($"获得一个完全充能后自动发射的共鸣圆盘。", lang);
            "ITEM_LASERTURBINE_DESC".AddOverlay($"获得一个{"自动".ToUtil()}充能的共鸣圆盘，基础充能速率为{LaserTurbineTweak.ChargeCoefficient.ToBaseAndStkPct().ToUtil("_每秒")}，击杀敌人将给予一层持续{LaserTurbineTweak.ChargeDuration.ToUtil("_秒的充能增益")}，每层增益使充能速率增加{LaserTurbineTweak.ChargeCoefficientPerKill.ToPct().ToUtil("_每秒")}。圆盘充能完毕后{"自动".ToUtil()}发射，在{"可视范围内".ToUtil()}所有个体之间弹射，每一次弹射可造成{LaserTurbineTweak.MainBeamDamageCoefficient.ToBaseAndStkPct().ToDmg("_的基础伤害")}。弹射时圆盘将{"穿透".ToUtil()}沿途所有个体并产生{爆炸}，{爆炸}造成{LaserTurbineTweak.SecondBombDamageCoefficient.ToBaseAndStkPct().ToDmg("_的基础伤害")}。", lang);
            "ITEM_ICICLE_DESC".AddOverlay($"召唤{"寒冰风暴".ToIce()}攻击附近的{IcicleTweak.BaseRadius.ToUtil("_米") + IcicleTweak.RadiusPerIcicle.ToStk("（每层风暴+_米）")}敌人，造成<style=cIsDamage>每秒{IcicleTweak.DamageCoefficient.ToDmgPct() + IcicleTweak.DamageCoefficientPerIcicle.ToStkPct("（每层风暴+_）")}的伤害</style>，并使敌人<style=cIsUtility>减速80%</style>。<style=cIsDamage>每达成一次击杀</style>，增加一层持续{IcicleTweak.IcicleDuration.ToUtil("_秒")}的{"风暴".ToIce()}，最低为{IcicleTweak.BaseIcicleMin.ToBaseWithStk(IcicleTweak.StacIcicleMin).ToUtil("_层")}，最高为{IcicleTweak.BaseIcicleMax.ToBaseWithStk(IcicleTweak.StackicicleMax).ToUtil("_层")}。", lang);
            "ITEM_DRONEWEAPONS_DESC".AddOverlay($"获得{"1架".ToUtil()}<style=cIsDamage>德鲁曼上校</style>。\n无人机获得<style=cIsDamage>+50%</style><style=cStack>（每层+50%）</style>攻击速度和冷却时间缩减。\n无人机有<style=cIsDamage>10%<style=cStack>（每层+10%）</style></style>的几率在命中时发射一枚<style=cIsDamage>导弹</style>，合计造成<style=cIsDamage>100%<style=cStack>（每层+100%）</style></style>的伤害。\n无人机获得<style=cIsDamage>自动链式机枪</style>，造成<style=cIsDamage>6x100%</style>的伤害，可弹射命中<style=cIsDamage>2</style>名敌人。", lang);
            "BANDIT2_SPECIAL_ALT_DESCRIPTION".AddOverlay("<style=cIsDamage>屠杀者</style>。使用左轮手枪进行射击，造成<style=cIsDamage>600%的伤害</style>，可以<style=cIsDamage>斩杀</style>血量低于<style=cIsDamage>15%</style>的敌人。击杀敌人可以<style=cIsDamage>叠加亡命徒</style><style=cIsUtility>（死亡和过关不消失）</style>，使亡命徒的伤害提高<style=cIsDamage>10%</style>。当<style=cIsDamage>亡命徒层数</style>超过<style=cIsDamage>（5×自身等级）</style> 的1倍后，射击需要消耗1层<style=cStack>（每超过一倍+1层）</style>亡命徒。", lang);
            "BANDIT2_SPECIAL_DESCRIPTION".AddOverlay($"<style=cIsDamage>屠杀者</style>。使用左轮手枪进行射击，造成{Bandit2Tweak.ResetRevolverDamageCoefficient.ToDmgPct("_的伤害")}，可以<style=cIsDamage>斩杀</style>血量低于<style=cIsDamage>15%</style>的敌人。击杀敌人可以<style=cIsUtility>重置所有能力的冷却时间</style>。", lang);
            "TOOLBOT_PRIMARY_ALT3_DESCRIPTION".AddOverlay($"锯伤周围敌人并给予自身{"临时屏障".ToHealing()}，造成<style=cIsDamage>每秒1000%的伤害</style>。", lang);
            "EQUIPMENT_DEATHPROJECTILE_PICKUP".AddOverlay("投掷一个能够反复触发你的“击杀时起效”效果的诅咒娃娃。", lang);
            "EQUIPMENT_DEATHPROJECTILE_DESC".AddOverlay($"投掷一个诅咒娃娃，每<style=cIsUtility>1</style>秒<style=cIsDamage>触发</style>你所有的<style=cIsDamage>击杀时起效</style>效果，持续<style=cIsUtility>8</style>秒。{"同时最多存在3个娃娃".ToUtil()}。", lang);
            "ITEM_SQUIDTURRET_DESC".AddOverlay("激活一个可交互目标时，召唤一个<style=cIsDamage>乌贼机枪塔</style>以<style=cIsDamage>100%<style=cStack>（每层增加100%）</style>的攻击速度</style>攻击附近的敌人。持续<style=cIsUtility>30</style>秒。", lang);
            "VV_SHELL_NAME".AddOverlay("失落的信标", lang);
            "STAGE_WEATHEREDSATELLITE_DREAM".AddOverlay("<style=cWorldEvent>你梦见天空和石头。</style>", lang);
            "STAGE_DRYBASIN_DREAM".AddOverlay("<style=cWorldEvent>你梦见了无尽的荒芜。</style>", lang);
            "BAZAAR_SEER_FBLSCENE".AddOverlay("<style=cWorldEvent>你梦见了雾与水。</style>", lang);
            "ELITE_MODIFIER_GOLD".AddOverlay("黄金的 {0}", lang);
            "ACHIEVEMENT_COMMANDOHEAVYTAP_DESCRIPTION".AddOverlay("作为指挥官，在不使用主要技能的情况下完成一关。", lang);
            "ACHIEVEMENT_COMMANDOHEAVYTAP_NAME".AddOverlay("指挥官：狂欢", lang);
            "ACHIEVEMENT_COMMANDOPLASMATAP_DESCRIPTION".AddOverlay("作为指挥官，在一局游戏中造成70次闪电链攻击。", lang);
            "ACHIEVEMENT_COMMANDOPLASMATAP_NAME".AddOverlay("指挥官：闪电精华", lang);
            "ACHIEVEMENT_COMMANDOPRFRVWILDFIRESTORM_DESCRIPTION".AddOverlay("作为指挥官，在同一关监禁敌人30次。", lang);
            "ACHIEVEMENT_COMMANDOPRFRVWILDFIRESTORM_NAME".AddOverlay("指挥官：燃烧", lang);
            "ACHIEVEMENT_GRANDFATHERPARADOX_DESCRIPTION".AddOverlay("出乎意料的两栖动物，不幸的结局。", lang);
            "ACHIEVEMENT_GRANDFATHERPARADOX_NAME".AddOverlay("呛鼻毒师：祖父悖论", lang);
            "ACHIEVEMENT_RIGHTTOJAIL_DESCRIPTION".AddOverlay("作为呛鼻毒师，监禁一名虚空狱卒。", lang);
            "ACHIEVEMENT_RIGHTTOJAIL_NAME".AddOverlay("呛鼻毒师：监禁权", lang);
            "ACHIEVEMENT_SPIKESTRIPACRIDDEFEATVOID_DESCRIPTION".AddOverlay("作为呛鼻毒师，逃离天文馆或在模拟器中完成50波次。", lang);
            "ACHIEVEMENT_SPIKESTRIPACRIDDEFEATVOID_NAME".AddOverlay("呛鼻毒师：来自灵魂深渊的饥饿", lang);
            "ACHIEVEMENT_SPIKESTRIPCOMMANDODEFEATVOID_DESCRIPTION".AddOverlay("作为指挥官，逃离天文馆或在模拟器中完成50波次。", lang);
            "ACHIEVEMENT_SPIKESTRIPCOMMANDODEFEATVOID_NAME".AddOverlay("指挥官：缠绕~束缚—", lang);
            "ACHIEVEMENT_VOIDCRIDUNLOCK_DESCRIPTION".AddOverlay("作为呛鼻毒师，腐蚀自己7次以打破限制。", lang);
            "BOG_PATHFINDER_BODY_UTILITY_SPIN_DESCRIPTION".AddOverlay("<style=cWorldEvent>未完成</style>。跃向空中，快速旋转造成<style=cIsDamage>300% 的伤害</style>，落地时会进行一次水平横扫并造成<style=cIsDamage>800% 的伤害</style>。", lang);
            "BRASSMONOLITH_BODY_NAME".AddOverlay("黄铜钟楼", lang);
            "BRASSMONOLITH_BODY_SUBTITLE".AddOverlay("坚如磐石的守望者", lang);
            "BUFF_DEAFENED_NAME".AddOverlay("失聪", lang);
            "CAPTAIN_PRIMARY_DESCRIPTION".AddOverlay("喷射一大团弹丸，造成<style=cIsDamage>8x120%的伤害</style>。为攻击充能将缩小<style=cIsUtility>扩散范围</style>。<style=cStack>（每层完美巨兽+100%击退）</style>", lang);
            "CAPTAIN_SECONDARY_DESCRIPTION".AddOverlay($"<style=cIsDamage>震荡</style>。发射一枚造成<style=cIsDamage>100%伤害</style>的快速电镖，弹射时<style=cIsUtility>电击</style>周围敌人。如果弹射将能飞行到更远地点。", lang);
            "CAPTAIN_SECONDARY_NAME".AddOverlay("能量电镖", lang);
            "CAPTAIN_SUPPLY_EQUIPMENT_RESTOCK_DESCRIPTION".AddOverlay($"使用时{"为装备充能".ToUtil()}。消耗的能量随时间自动填充，速度随等级提升。", lang);
            "CAPTAIN_SUPPLY_HEAL_DESCRIPTION".AddOverlay($"每秒为附近10米{"（每级+1米）".ToStk()}所有友方<style=cIsHealing>恢复</style>等同于各个角色<style=cIsHealing>最大生命值</style>{"10%".ToHealing() + "（每级+1%）".ToStk()}的生命值。", lang);
            "CAPTAIN_SUPPLY_SHOCKING_DESCRIPTION".AddOverlay($"间歇性<style=cIsDamage>震荡</style>附近20米的所有敌人，使其无法移动。", lang);
            "CAPTAIN_UTILITY_ALT1_DESCRIPTION".AddOverlay($"<style=cIsDamage>眩晕</style>。{"消耗4层充能".ToUtil()}向<style=cIsDamage>UES顺风号</style>请求发动一次<style=cIsDamage>动能打击</style>。在<style=cIsUtility>20秒</style>后，对所有角色造成{CaptainTweak.CallAirstrikeAltDamageCoefficient.ToDmgPct("_的伤害")}。", lang);
            "CHARACTER_SIGMACONSTRUCT_NAME".AddOverlay("西格玛结构体", lang);
            "COMMANDO_HEAVYTAP_DESCRIPTION".AddOverlay("<style=cIsDamage>绝对光滑</style>。射击两次，造成<style=cIsDamage>2x155%的伤害</style>。", lang);
            "COMMANDO_HEAVYTAP_NAME".AddOverlay("沉重双击", lang);
            "COMMANDO_SECONDARY_DESCRIPTION".AddOverlay($"发射一枚<style=cIsDamage>穿甲弹</style>，造成{CommandoTweak.FMJDamageCoefficient.ToDmgPct("_的伤害")}。每次穿透敌人，造成的伤害提高<style=cIsDamage>40%</style>。", lang);
            "COMMANDO_SPECIAL_ALT1_DESCRIPTION".AddOverlay($"扔出一枚手雷，爆炸可造成{CommandoTweak.ThrowGrenadeDamageCoefficient.ToDmgPct("_的伤害")}。最多可投掷2枚。", lang);
            "COMMANDO_SPECIAL_DESCRIPTION".AddOverlay($"<style=cIsDamage>眩晕</style>。连续射击，每枚弹丸造成{FireBarrage.damageCoefficient.ToDmgPct("_的伤害")}。射击次数随攻击速度增加。", lang);
            "CROCO_PASSIVE_ALT_DESCRIPTION".AddOverlay($"使用<style=cIsHealing>毒化</style>的攻击改为叠加<style=cIsDamage>枯萎</style>效果，{"忽略护甲".ToDeath()}，造成<style=cIsDamage>每秒60%的伤害</style>。", lang);
            "CROCO_PASSIVE_DESCRIPTION".AddOverlay($"<style=cIsHealing>毒化</style>攻击将会造成大量持续性伤害并{"忽略护甲".ToDeath()}。", lang);
            "CROCO_SECONDARY_ALT_DESCRIPTION".AddOverlay($"<style=cIsHealing>毒化</style>。<style=cIsDamage>屠杀者</style>。<style=cIsHealing>自我恢复</style><style=cStack>（每层怪物牙齿可使每咬中一名敌人时额外恢复最大生命值的1%）</style>。啃咬一名敌人并造成<style=cIsDamage>320%的伤害</style>。", lang);
            "DOTFLARE_SKIN_ARMERC_NAME".AddOverlay("沙特", lang);
            "DOTFLARE_SKIN_DACRID_NAME".AddOverlay("洪荒", lang);
            "DOTFLARE_SKIN_DVIEND_NAME".AddOverlay("来自深渊", lang);
            "DOTFLARE_SKIN_FMAGE_NAME".AddOverlay("空姐", lang);
            "DOTFLARE_SKIN_GMULT_NAME".AddOverlay("格雷德", lang);
            "DOTFLARE_SKIN_HBANDIT_NAME".AddOverlay("拦路强盗", lang);
            "DOTFLARE_SKIN_MHUNTRESS_NAME".AddOverlay("舍伍德流氓", lang);
            "DOTFLARE_SKIN_MOBSTERMANDO_NAME".AddOverlay("剃刀边缘", lang);
            "DOTFLARE_SKIN_OMERC_NAME".AddOverlay("翡翠", lang);
            "DOTFLARE_SKIN_PBANDIT_NAME".AddOverlay("平克顿", lang);
            "DOTFLARE_SKIN_PCAP_NAME".AddOverlay("钢须", lang);
            "DOTFLARE_SKIN_SPLODR_NAME".AddOverlay("女士钟表", lang);
            "DOTFLARE_SKIN_SRAILER_NAME".AddOverlay("白色死亡", lang);
            "DOTFLARE_SKIN_SREX_NAME".AddOverlay("真菌", lang);
            "DOTFLARE_SKIN_VAHUNTRESS_NAME".AddOverlay("丘比特之箭", lang);
            "DOTFLARE_SKIN_VHUNTRESS_NAME".AddOverlay("幽灵猎手", lang);
            "DOTFLARE_SKIN_VTIFICER_NAME".AddOverlay("高贵", lang);
            "DOTFLARE_SKIN_WENGI_NAME".AddOverlay("莫洛克", lang);
            "ELITE_MODIFIER_PLATED".AddOverlay("装甲的 {0}", lang);
            "ELITE_MODIFIER_WARPED".AddOverlay("扭曲的 {0}", lang);
            "ENGI_UTILITY_DESCRIPTION".AddOverlay($"放置一个<style=cIsUtility>无法穿透且有击退力的护盾</style>来阻挡弹幕。每个盾需要消耗{2}层充能，存在<style=cIsUtility>{Settings.BubbleShieldLifetime.Value}秒</style>。", lang);
            "EQUIPMENT_BFG_DESC".AddOverlay($"发射前子卷须，对{"66.6".ToUtil()}米范围内的敌人造成最高<style=cIsDamage>每秒666%的伤害</style>。接触目标并引爆后，会对{20.ToUtil("_米")}范围内的敌人造成<style=cIsDamage>6666%的伤害</style>。", lang);
            "EQUIPMENT_COMMANDMISSILE_DESC".AddOverlay($"发射一轮包含<style=cIsDamage>12</style>枚导弹的导弹雨，每枚导弹造成<style=cIsDamage>300%的伤害</style>。", lang);
            "EQUIPMENT_CRITONUSE_DESC".AddOverlay($"<style=cIsDamage>暴击几率增加100%</style>，并使超过{"100%".ToDmg()}的{"暴击几率".ToDmg()}转换为{"暴击伤害".ToDmg()}，持续8秒。", lang);
            "EQUIPMENT_CRITONUSE_PICKUP".AddOverlay($"获得100%暴击几率，并使超过100%的暴击几率转换为暴击伤害，持续8秒。", lang);
            "EQUIPMENT_FIREBALLDASH_DESC".AddOverlay($"变成<style=cIsDamage>龙之火球</style>持续<style=cIsDamage>6</style>秒，受击可造成<style=cIsDamage>500%的伤害</style>，飞行时持续喷射造成{"300%".ToDmg() + "（每层熔融钻机+300%）".ToDmg()}基础伤害的岩浆球。结束时会引爆，造成<style=cIsDamage>800%的伤害</style>。", lang);
            "EQUIPMENT_GOLDGAT_DESC".AddOverlay($"发射一阵钱雨，<style=cIsDamage>每颗子弹均造成{GoldGatFire.damageCoefficient.ToDmgPct()}，外加等同于消耗金钱的{GoldGatTweak.半数.ToBaseAndStkByCloseToPct()}{"（此处层数=成卷的零钱层数+1）".ToStk()}的伤害</style>。每颗子弹消耗{GoldGatFire.baseMoneyCostPerBullet.ToDmg("_枚")}金钱，价格随{"开火时间".ToDmg()}和{"游戏时间".ToUtil()}不断上升。", lang);
            "EQUIPMENT_LIFESTEALONHIT_DESC".AddOverlay($"击中敌人时<style=cIsHealing>恢复</style>等同于你造成<style=cIsDamage>伤害</style><style=cIsHealing>20%</style>的生命值，并且使其{流血}，在{BleedTweak.BleedDuration.ToUtil("_秒")}内合计造成{BleedTweak.BleedDamageCoefficient.ToDmgPct("_的总伤害")}；若此次攻击造成{暴击}，则改为使敌人{出血}，在{BleedTweak.SurperBleedDuration.ToUtil("_秒")}内合计造成{BleedTweak.SurperBleedDamageCoefficient.ToDmgPct("_的总伤害")}{不与其他流血和出血重复}。效果持续<style=cIsHealing>8</style>秒。", lang);
            "EQUIPMENT_LIFESTEALONHIT_PICKUP".AddOverlay($"击中敌人时造成流血或出血，并恢复等同于你造成伤害的一定比例的生命值，持续8秒。", lang);
            "EQUIPMENT_LIGHTNING_DESC".AddOverlay($"召唤雷电攻击目标敌人，造成<style=cIsDamage>3000%的基础伤害</style>并<style=cIsDamage>眩晕</style>附近的敌人。", lang);
            "EQUIPMENT_MOLOTOV_DESC".AddOverlay($"投掷<style=cIsDamage>6</style>个燃烧瓶，碎裂时<style=cIsDamage>点燃</style>敌人，造成<style=cIsDamage>500%的基础伤害</style>。每个燃烧瓶都能制造一片燃烧区域，<style=cIsDamage>每秒造成200%的伤害</style>。{MolotovTweak.DamageBonusCoefficient.ToStkPct("（每层汽油使伤害增加_）")}", lang);
            "EQUIPMENT_RECYCLER_DESC".AddOverlay($"将一个物品或装备<style=cIsUtility>转化</style>成另一个<style=cIsUtility>同等级</style>的物品或装备。无法{"转化".ToUtil()}碎片和回收机。在月球外{"转化".ToUtil() + "月球".ToLunar()}物品或装备时有可能被发现，物品或装备将会被传送回{"月球".ToLunar()}。{"转化".ToUtil() + "虚空".ToVoid()}物品时可能导致物品爆炸。", lang);
            "EQUIPMENT_RECYCLER_PICKUP".AddOverlay($"将一个物品或装备转化成另一个。", lang);
            "EQUIPMENT_TEAMWARCRY_DESC".AddOverlay($"所有友方进入<style=cIsDamage>狂热</style>状态，持续<style=cIsUtility>7</style>秒。<style=cIsUtility>移动速度</style>提升<style=cIsUtility>50%</style>{TeamWarCryTweak.每层战争号角攻速提升系数.ToStkPct("（每层战争号角+_）")}，<style=cIsDamage>攻击速度</style>提升<style=cIsDamage>100%</style>{TeamWarCryTweak.每层战争号角移速提升系数.ToStkPct("（每层战争号角+_）")}。", lang);
            "EXPANSION_FORGOTTENRELICS_DESC".AddOverlay("将'被遗忘的遗物'的内容添加到游戏。", lang);
            "EXPANSION_FORGOTTENRELICS_NAME".AddOverlay("被遗忘的遗物", lang);
            "FOGBOUND_SCENEDEF_NAME_TOKEN".AddOverlay($"雾气泻湖", lang);
            "FOGBOUND_SCENEDEF_SUBTITLE_TOKEN".AddOverlay($"冥河浅滩", lang);
            "FROSTWISP_BODY_NAME".AddOverlay("冰霜幽魂", lang);
            "FRUJO_SKIN_PARDOFELISDEFINITION_NAME".AddOverlay("帕朵菲莉丝 喵~", lang);
            "GOLDENKNURL_DESC".AddOverlay($"{"最大生命值".ToHealing()}增加{0.1f.ToBaseAndStkPct().ToHealing()}，{"基础生命值再生速度".ToHealing()}增加{2.4f.ToBaseAndStk().ToHealing("_hp/s")}，外加{"生命值再生速度".ToHealing()}提升{"50%".ToHealing()}，{"护甲".ToUtil()}增加{20.ToBaseAndStk().ToHealing("_点")}。", lang);
            "GOLDENKNURL_NAME".AddOverlay("<color=yellow>黄金隆起</color>", lang);
            "GOLDENKNURL_PICKUP".AddOverlay("增加最大生命值、生命值再生和护甲。", lang);
            "HIDDENGOLDBUFFITEM_NAME".AddOverlay("奥利雷奥尼特的祝福", lang);
            "HUNTRESS_PRIMARY_ALT_DESCRIPTION".AddOverlay($"<style=cIsUtility>灵巧</style>。瞄准{HuntressTweak.基础射程.ToUtil("_米")}{HuntressTweak.猎人的鱼叉叠加射程.ToStk("（每层猎人的鱼叉+_米）")}内的敌人，拉弓射出<style=cIsDamage>{3}枚</style>跟踪箭，每枚造成{HuntressTweak.FlurryDamageCoefficient.ToDmgPct("_的伤害")}。如果暴击则发射<style=cIsDamage>{6}</style>枚跟踪箭。", lang);
            "HUNTRESS_PRIMARY_DESCRIPTION".AddOverlay($"<style=cIsUtility>灵巧</style>。瞄准{HuntressTweak.基础射程.ToUtil("_米")}{HuntressTweak.猎人的鱼叉叠加射程.ToStk("（每层猎人的鱼叉+_米）")}内的敌人，快速射出一枚能够造成{HuntressTweak.StrafeDamageCoefficient.ToDmgPct()}伤害的跟踪箭。", lang);
            "HUNTRESS_SECONDARY_DESCRIPTION".AddOverlay($"<style=cIsUtility>灵巧</style>。投掷一把追踪月刃，可弹射最多<style=cIsDamage>{HuntressTweak.LaserGlaiveBounceCount}</style>次，初始造成{HuntressTweak.LaserGlaiveDamageCoefficient.ToDmgPct("_的伤害")}，每次弹射伤害增加{(HuntressTweak.LaserGlaiveBounceDamageCoefficient - 1f).ToDmgPct()}。", lang);
            "HUNTRESS_SPECIAL_ALT1_DESCRIPTION".AddOverlay($"向后<style=cIsUtility>传送</style>至空中。最多发射<style=cIsDamage>{HuntressTweak.BallistaBoltCount}</style>道能量闪电，每道造成<style=cIsDamage>{HuntressTweak.BallistaDamageCoefficient.ToDmgPct("_的伤害")}</style>。", lang);
            "HUNTRESS_SPECIAL_DESCRIPTION".AddOverlay($"<style=cIsUtility>传送</style>至空中，向目标区域射下箭雨，使区域内所有敌人<style=cIsUtility>减速</style>，并造成每秒{ArrowRain.damageCoefficient.ToDmgPct("_的伤害")}。", lang);
            "IN_LOBBY_CONFIG_POPOUT_PANEL_NAME".AddOverlay("Mods配置", lang);
            "INTERACTABLE_AMBER_MOUNTAIN_SHRINE_CONTEXT".AddOverlay("这个神龛看起来不寻常...你确定要激活吗?", lang);
            "INTERACTABLE_AMBER_MOUNTAIN_SHRINE_MESSAGE".AddOverlay("<style=cShrine>{0} 请求发起琥珀挑战..</color>", lang);
            "INTERACTABLE_AMBER_MOUNTAIN_SHRINE_MESSAGE_2P".AddOverlay("<style=cShrine>你请求发起琥珀挑战..</color>", lang);
            "INTERACTABLE_AMBER_MOUNTAIN_SHRINE_NAME".AddOverlay("琥珀山之神龛", lang);
            "INTERACTABLE_CRIMSON_MOUNTAIN_SHRINE_CONTEXT".AddOverlay("这个神龛看起来不寻常...你确定要激活吗?", lang);
            "INTERACTABLE_CRIMSON_MOUNTAIN_SHRINE_MESSAGE".AddOverlay("<style=cShrine>{0} 请求发起深红挑战..</color>", lang);
            "INTERACTABLE_CRIMSON_MOUNTAIN_SHRINE_MESSAGE_2P".AddOverlay("<style=cShrine>你请求发起深红挑战..</color>", lang);
            "INTERACTABLE_CRIMSON_MOUNTAIN_SHRINE_NAME".AddOverlay("深红山之神龛", lang);
            "INTERACTIBLE_BATTERYBANK_CONTEXT".AddOverlay("更换燃料电池 ({0}/{1})", lang);
            "INTERACTIBLE_BATTERYBANK_NAME".AddOverlay("燃料电池组", lang);
            "INTERACTIBLE_BATTERYBANK_NAMEFULL".AddOverlay("充能完毕的燃料电池组", lang);
            "INTERACTIBLE_BATTERYSOURCE_CONTEXT".AddOverlay("激活", lang);
            "INTERACTIBLE_BATTERYSOURCE_NAME".AddOverlay("老旧传送器", lang);
            "INTERACTIBLE_BROKENTELEPORTER_ACTIVATED".AddOverlay("<style=cWorldEvent>能源遗物产生了共鸣。</style>", lang);
            "INTERACTIBLE_BROKENTELEPORTER_CONTEXT".AddOverlay("放置遗物", lang);
            "INTERACTIBLE_BROKENTELEPORTER_NAME".AddOverlay("破碎传送器", lang);
            "INTERACTIBLE_CLOAKEDSHRINE_NAME".AddOverlay("被遮掩的神龛", lang);
            "INTERACTIBLE_RELICENERGY_CONTEXT".AddOverlay("调查", lang);
            "INTERACTIBLE_SHRINE_TAR_CONTEXT".AddOverlay("供奉", lang);
            "INTERACTIBLE_SHRINE_TAR_NAME".AddOverlay("焦油神龛", lang);
            "INTERACTIBLE_SLUMBERINGPEDESTAL_CONTEXT".AddOverlay("放置遗物", lang);
            "INTERACTIBLE_SLUMBERINGPEDESTAL_FINISHED".AddOverlay("<style=cWorldEvent>基座已苏醒。</style>", lang);
            "INTERACTIBLE_SLUMBERINGPEDESTAL_NAME".AddOverlay("沉睡基座", lang);
            "INTERACTIBLE_STAGEORDERBUTTON_CONTEXT".AddOverlay("切换传送网络", lang);
            "INTERACTIBLE_STATICPORTAL_CONTEXT".AddOverlay("返回星球", lang);
            "ITEM_AbyssalMedkit_DESCRIPTION".AddOverlay("<style=cIsUtility>消耗品</style>，抵挡<style=cIsUtility>10次</style>减益后失效。每一次抵挡都有<style=cIsHealing>10%</style>概率给予你<style=cIsHealing>“祝·福”</style>。每个<style=cIsHealing>祝福</style>可使你<style=cIsUtility>所有属性提升3%</style>。<style=cIsVoid>使所有医疗包无效化</style>", lang);
            "ITEM_AbyssalMedkit_PICKUP".AddOverlay("消耗品，可以替你抵挡10次减益，每一次抵挡都有概率给予你“祝·福”", lang);
            "ITEM_ARMORREDUCTIONONHIT_DESC".AddOverlay($"{"2秒".ToUtil()}内命中敌人{"5次".ToDmg()}造成{"粉碎".ToDmg()}减益，将对方<style=cIsDamage>护甲</style>降低<style=cIsDamage>60</style>点，持续<style=cIsDamage>8秒</style><style=cStack>（每层+8秒）</style>。你的攻击将{"穿透".ToDmg()}被{"粉碎".ToDmg()}的敌人{ArmorReductionOnHitTweak.半数.ToBaseAndStkByCloseToPct().ToDmg()}的护甲。", lang);
            "ITEM_ATTACKSPEEDANDMOVESPEED_DESC".AddOverlay($"使<style=cIsDamage>攻击速度</style>提高<style=cIsDamage>7.5%</style><style=cStack>（每层+7.5%）</style>，使<style=cIsUtility>移动速度</style>提高<style=cIsUtility>7%</style><style=cStack>（每层+7%）</style>。<color=#FFFF00>加里翁特殊效果：最大生命值和基础生命值再生速度速度增加5%<style=cStack>（每层+5%）</style>。</color>", lang);
            "ITEM_AUTOCASTEQUIPMENT_DESC".AddOverlay($"<style=cIsUtility>使装备冷却时间减少50%</style><style=cStack>（每层增加15%）</style>，但会使装备增加{"0.15秒强制冷却时间".ToUtil() + "（每层+0.15秒）".ToStk()}。装备会在<style=cIsUtility>冷却时间</style>结束时被迫自动<style=cIsUtility>激活</style>。", lang);
            "ITEM_BARRIERONKILL_DESC".AddOverlay($"击败敌人时可获得一道<style=cIsHealing>临时屏障</style>，相当于增加<style=cIsHealing>15<style=cStack>（每层增加15）</style>点外加{BarrierOnKillTweak.AddBarrierFraction.ToBaseAndStkPct().ToHealing()}的最大生命值</style>。</style><color=#FFFF00>船长特殊效果：给予10点<style=cStack>（每层+10点）</style>最大护盾值，外加最大护盾值增加10%<style=cStack>（每层+10%）</style>。</color>", lang);
            "ITEM_BARRIERONOVERHEAL_DESC".AddOverlay($"基础护甲增加{50.ToBaseAndStk().ToHealing("_点")}，外加升级获得护甲增加{1.ToBaseAndStk().ToHealing("_点")}。过量<style=cIsHealing>治疗</style>会使你获得一道相当于{0.5f.ToBaseAndStkPct().ToHealing()}已<style=cIsHealing>治疗</style>生命值的<style=cIsHealing>临时屏障</style>。", lang);
            "ITEM_BEAR_DESC".AddOverlay($"增加<style=cIsHealing>10</style>点<style=cStack>（每层增加10点）</style>护甲。\n<style=cIsHealing>【护甲减伤公式：100%x护甲值÷(护甲值+100)】</style>", lang);
            "ITEM_BEARVOID_DESC".AddOverlay($"有<style=cIsHealing>50%</style>概率<style=cIsUtility>（成为虚空的象征+50%概率）</style><style=cIsHealing>格挡</style>一次来袭的伤害。充能时间<style=cIsUtility>15秒</style><style=cStack>（每层-10%）</style>。<style=cIsVoid>使所有更艰难的时光无效化</style>。", lang);
            "ITEM_BEHEMOTH_DESC".AddOverlay($"你的所有<style=cIsDamage>攻击均会产生爆炸</style>{BehemothTweak.Interval.ToStk("（爆炸间隔：_秒）")}，爆炸将{"继承".ToUtil()}这次攻击的{"所有属性".ToUtil()}，对{BehemothTweak.Radius.ToBaseAndStk().ToDmg("_米")}范围内的敌人合计造成{BehemothTweak.BaseDamageCoefficient.ToDmgPct()}的额外伤害。<color=#FFFF00>船长特殊效果：关于“火神霰弹枪”（详情看技能介绍）。</color>", lang);
            "ITEM_BLEEDONHIT_DESC".AddOverlay($"命中敌人时有<style=cIsDamage>10%</style><style=cStack>（每层+10%）</style>的几率使其{流血}，在{BleedTweak.BleedDuration.ToUtil("_秒")}内合计造成{BleedTweak.BleedDamageCoefficient.ToDmgPct("_的总伤害")}；若造成{暴击}，则改为{出血}，在{BleedTweak.SurperBleedDuration.ToUtil("_秒")}内合计造成{BleedTweak.SurperBleedDamageCoefficient.ToDmgPct("_的总伤害")}{不与其他流血和出血重复}。", lang);
            "ITEM_BLEEDONHITANDEXPLODE_DESC".AddOverlay($"使{流血}或{出血}的敌人将会在死亡时产生半径{BleedOnHitAndExplodeTweak.BaseRadius.ToBaseWithStk(BleedOnHitAndExplodeTweak.StackRadius).ToDmg("_米") + "（具体范围与敌人体型相关）".ToUtil()}的<style=cIsDamage>鲜血爆炸</style>，造成等同于敌人身上剩余{流血}和{出血}伤害的{BleedOnHitAndExplodeTweak.半数.ToBaseAndStkByCloseToPct().ToDmg("_的伤害")}。", lang);
            "ITEM_BLEEDONHITVOID_DESC".AddOverlay($"攻击有{BleedOnHitVoidTweak.PercnetChance.ToBaseAndStk().ToDmg("_%的几率")}<style=cIsDamage>瓦解</style>敌人{不与其他瓦解重复}，造成{Configuration.AspectVoidBaseCollapseDamage.Value.ToBaseWithStkPct(Configuration.AspectVoidStackCollapseDamage.Value, "（每层熵的破裂+_）").ToDmg(Configuration.AspectVoidUseBase.Value ? "_的基础伤害" : "_的总伤害")}。<style=cIsVoid>使所有三尖匕首无效化</style>。", lang);
            "ITEM_BLESSING_NAME_DESCRIPTION".AddOverlay($"凝视深渊过久，深渊将回以凝视！\n<style=cIsVoid>所有属性提升3%</style><style=cStack>（每层+3%）</style>\n<style=cIsVoid>祝·福深入灵魂，将伴随你一生</style>。", lang);
            "ITEM_BONUSGOLDPACKONKILL_DESC".AddOverlay($"<style=cIsDamage>击杀敌人</style>时有{BonusGoldPackOnKillTweak.DropPercentChance.ToBaseAndStk().ToUtil("_%")}的几率掉落价值<style=cIsUtility>25{BonusGoldPackOnKillTweak.StackMoney.ToStk("（每层+_）")}<color=yellow>金钱</color></style>的宝物<style=cIsUtility>（价值随时间变化）</style>。", lang);
            "ITEM_BOSSDAMAGEBONUS_DESC".AddOverlay($"对<style=cIsHealing>护盾</style>和<style=cIsHealing>临时屏障</style>额外造成<style=cIsDamage>20%</style>的伤害<style=cStack>（每层增加20%）</style>。<color=#FFFF00>指挥官特殊效果：基础伤害增加2点<style=cStack>（每层+2点）</style>。</color>", lang);
            "ITEM_BOUNCENEARBY_DESC".AddOverlay($"命中敌人时有{BounceNearbyTweak.BasePercentChance.ToBaseWithStk(BounceNearbyTweak.StackPercentChance).ToDmg("_%")}的几率向周围{BounceNearbyTweak.BaseRadius.ToUtil("_米")}内最多<style=cIsDamage>{BounceNearbyTweak.BaseMaxTargets}名</style>敌人发射<style=cIsDamage>追踪钩爪</style>{BounceNearbyTweak.Interval.ToStk("（发射间隔：_秒）")}，合计造成{BounceNearbyTweak.BaseDamageCoefficient.ToDmgPct("_的总伤害")}。", lang);
            "ITEM_BULWARKSHAUNT_SWORD_DESC".AddOverlay($"神秘声音在你的脑海中回荡，低语着：<link=\"BulwarksHauntWavy\">\"方尖碑...方尖碑...\"</link>，看样子它好像想让你带它去方尖碑处。", lang);
            "ITEM_BULWARKSHAUNT_SWORD_UNLEASHED_DESC".AddOverlay($"神秘声音在你的脑海中回荡，低语着：<link=\"BulwarksHauntWavy\">\"方尖碑...方尖碑...\"</link>，看样子它好像想让你带它去方尖碑处。", lang);
            "ITEM_CARTRIDGECONSUMED_DESCRIPTION".AddOverlay($"他曾梦想成为一名艺术家...", lang);
            "ITEM_CHAINLIGHTNING_DESC".AddOverlay($"有{ChainLightningTweak.半数.ToBaseAndStkByCloseToPct().ToDmg()}的几率发射<style=cIsDamage>连锁闪电</style>{ChainLightningTweak.Interval.ToStk("（发射间隔：_秒）")}，对{ChainLightningTweak.BaseRadius.ToBaseWithStk(ChainLightningTweak.StackRadius).ToUtil("_米")}内的最多<style=cIsDamage>{ChainLightningTweak.Bounces + 1}个</style>目标合计造成{ChainLightningTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。", lang);
            "ITEM_CHAINLIGHTNINGVOID_DESC".AddOverlay($"有{ChainLightningVoidTweak.半数.ToBaseAndStkByCloseToPct().ToDmg()}的几率发射<style=cIsDamage>虚空闪电</style>{ChainLightningVoidTweak.Interval.ToStk("（发射间隔：_秒）")}，对同一个敌人合计造成{ChainLightningVoidTweak.TotalStrikes.ToDmg()}x{ChainLightningVoidTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。<style=cIsVoid>使所有尤克里里无效化</style>。", lang);
            "ITEM_CLOVER_DESC".AddOverlay($"所有随机效果的概率提升<style=cIsUtility>25%</style>{"（叠加公式：幸运提升概率 = 原概率x(运气/(|运气|+3))）".ToStk()}。", lang);
            "ITEM_CRITDAMAGE_DESC".AddOverlay($"<style=cIsDamage>暴击伤害</style>增加<style=cIsDamage>100%</style><style=cStack>（每层+100%）</style>。<color=#FFFF00>磁轨炮手特殊效果：敌人弱点范围增加100%<style=cStack>（每层+100%）</style>（弱点框不再变大，防止遮挡视野）。</color>", lang);
            "ITEM_CRITGLASSESVOID_DESC".AddOverlay($"<style=cIsDamage>暴击伤害</style>增加{CritGlassesVoidTweak.CritDamageMultAdd.ToBaseAndStkPct().ToDmg()}。<style=cIsVoid>使所有透镜制作者的眼镜无效化</style>。", lang);
            "ITEM_CROWBAR_DESC".AddOverlay($"对生命值超过<style=cIsDamage>90%</style>的敌人造成<style=cIsDamage>75%</style><style=cStack>（每层增加75%）</style>的额外伤害。", lang);
            "ITEM_DEATHMARK_DESC".AddOverlay($"拥有不少于{"4个".ToDmg()}减益效果的敌人<style=cIsDamage>将被标记为死亡</style>，从所有来源受到的伤害增加{DeathMarkTweak.BaseDamageCoefficient.ToDmgPct() + DeathMarkTweak.StackDamageCoefficient.ToStkPct("（全队每层+_)")}，持续{7.ToBaseAndStk().ToDmg()}秒。", lang);
            "ITEM_ELEMENTALRINGVOID_DESC".AddOverlay($"充能完毕后，<style=cIsDamage>不低于{RingsTweak.RingDamageRequired.ToBaseAndStkPct()}伤害的攻击</style>击中敌人时会产生一个黑洞，<style=cIsUtility>将{RingsTweak.VoidRingBaseRadius.ToBaseWithStk(RingsTweak.VoidRingStackRadius)}米范围内的敌人吸引至其中心</style>。持续<style=cIsUtility>5</style>秒后坍缩，合计造成{RingsTweak.VoidRingDamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。充能时间<style=cIsUtility>20秒</style><style=cStack>（每层减少10%）</style>。<color=#FFFF00>虚空恶鬼特殊效果：基础充能时间降低至2秒。</color>", lang);
            "ITEM_ENERGIZEDONEQUIPMENTUSE_DESC".AddOverlay($"{"激活装备".ToUtil()}时吹响号角，使你{"攻击速度".ToDmg()}增加{0.7f.ToDmgPct()}，持续{8.ToBaseWithStk(4).ToUtil("_秒")}。", lang);
            "ITEM_EQUIPMENTMAGAZINE_DESC".AddOverlay($"获得1次<style=cStack>（每层增加1次）</style><style=cIsUtility>额外的装备充能</style>。<style=cIsUtility>将装备冷却时间减少15%</style><style=cStack>（每层增加15%）</style>。<color=#FFFF00>雷克斯特殊效果：技能<style=cIsUtility>冷却时间</style>减少<style=cIsUtility>10%</style><style=cStack>（每层+10%）</style>。</color>", lang);
            "ITEM_ESSENCEOFTAR_DESC".AddOverlay($"成为焦油的象征，<style=cDeath>生命不再自然恢复</style>。攻击敌人可<style=cIsHealing>吸收他们的生命</style>。<style=cDeath>移除将导致你直接死亡</style>", lang);
            "ITEM_EXECUTELOWHEALTHELITE_DESC".AddOverlay($"立即击败生命值低于<style=cIsHealth>10%的精英怪物</style><style=cStack>（叠加公式：斩杀线 = 50%x(层数÷(层数+4))）</style>。", lang);
            "ITEM_EXPLODEONDEATH_DESC".AddOverlay($"使{燃烧}或{焚烧}的敌人将会在死亡时召唤<style=cIsDamage>鬼火</style>，产生半径<style=cIsDamage>{ExplodeOnDeathTweak.BaseRadius}米</style>{ExplodeOnDeathTweak.StackRadius.ToStk("（每层+_米）") + "（具体范围与敌人体型相关）".ToUtil()}的{"火焰冲击波".ToFire()}点燃敌人，造成将敌人身上剩余{燃烧}和{焚烧}伤害的{ExplodeOnDeathTweak.半数.ToBaseAndStkByCloseToPct().ToDmg("_的持续伤害")}。", lang);
            "ITEM_EXPLODEONDEATHVOID_DESC".AddOverlay($"当你对敌人造成{"第一次".ToUtil()}伤害时将<style=cIsDamage>引爆</style>它们，产生半径{ExplodeOnDeathVoidTweak.BaseRadius.ToBaseWithStk(ExplodeOnDeathVoidTweak.StackRadius).ToUtil("_米") + "（具体范围与敌人体型和剩余血量相关）".ToUtil()}的爆炸，合计造成{"敌方剩余百分比生命值".ToHealth()}x{ExplodeOnDeathVoidTweak.半数.ToBaseAndStkByCloseToPct().ToDmg("_的总伤害")}。<style=cIsVoid>使所有鬼火无效化</style>。", lang);
            "ITEM_EXTRALIFE_DESC".AddOverlay($"<style=cIsUtility>倒下后</style>，该物品将被<style=cIsUtility>消耗</style>，你将<style=cIsHealing>起死回生</style>并获得<style=cIsHealing>3秒的无敌时间</style>。\n<style=cIsUtility>死去的迪奥将在过关时复活。\n此物品不会被米斯历克斯偷走</style>。", lang);
            "ITEM_FALLBOOTSVOID_DESC".AddOverlay($"按住'E'可<style=cIsUtility>漂浮</style>并吸收<style=cIsUtility>引力波</style>。松开创造一个半径<style=cIsUtility>30米</style>的<style=cIsUtility>反重力区</style>，持续<style=cIsUtility>15</style>秒。之后进入<style=cIsUtility>20</style><style=cStack>（每层-50%）</style>秒的充能时间，反重力区中心会吸引周围敌人并造成<style=cIsDamage>200-2000%</style>的基础伤害。<style=cIsVoid>使所有H3AD-5T v2无效化</style>。", lang);
            "ITEM_FIREBALLSONHIT_DESC".AddOverlay($"命中敌人时有{FireballsOnHitTweak.半数.ToBaseAndStkByCloseToPct().ToDmg()}的几率向敌人的{"岩浆球喷泉".ToFire()}中添加{"3颗岩浆球".ToFire() + FireballsOnHitTweak.Interval.ToStk("（喷射间隔：_秒）")}，每颗合计造成{FireballsOnHitTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}，并<style=cIsDamage>点燃</style>敌人。", lang);
            "ITEM_FIRERING_DESC".AddOverlay($"充能完毕后，<style=cIsDamage>不低于{RingsTweak.RingDamageRequired.ToBaseAndStkPct("（取双环中层数高的每层+_）")}伤害的攻击</style>击中敌人时会产生一道{"符文火焰龙卷风".ToFire()}轰击敌人，造成{RingsTweak.FireRingDamageCoefficient.ToBaseAndStkPct().ToDmg()}的总持续伤害，同时<style=cIsDamage>点燃</style>敌人。充能时间<style=cIsUtility>10秒</style><style=cStack>（每双手环减少10%）</style>。<color=#FFFF00>工匠特殊效果：基础充能时间降低至1秒，并无视伤害要求。</color>", lang);
            "ITEM_FLATHEALTH_DESC".AddOverlay($"<style=cIsHealing>最大生命值</style>增加<style=cIsHealing>25</style>点<style=cStack>（每层+25点）</style>。升级获得的<style=cIsHealing>最大生命值</style>增加{FlatHealthTweak.LevelHealthAddCoefficient.ToBaseAndStkPct().ToHealing()}。<color=#FFFF00>厨师特殊效果：野牛肉排的效果翻倍。</color>", lang);
            "ITEM_FRAGILEDAMAGEBONUS_DESC".AddOverlay($"使造成的伤害提高<style=cIsDamage>20%</style><style=cStack>（每层+20%）</style>。受到伤害导致生命值降到<style=cIsHealth>25%</style>以下时，该物品会<style=cIsUtility>损坏</style>。\n<style=cIsUtility>损坏的手表将在过关时修复</style>。", lang);
            "ITEM_HEADHUNTER_DESC".AddOverlay("获得所击败精英怪物身上的<style=cIsDamage>能力</style>，持续<style=cIsDamage>20秒</style><style=cStack>（每层增加10秒）</style>。", lang);
            "ITEM_HEALINGPOTION_DESC".AddOverlay($"受到伤害导致<style=cIsHealth>生命值降到25%</style>以下时会<style=cIsUtility>消耗</style>该物品，并为你<style=cIsHealing>恢复最大生命值</style>的<style=cIsHealing>75%</style>。\n<style=cIsUtility>使用后的空瓶将在过关时重新填满</style>。", lang);
            "ITEM_HEALONCRIT_DESC".AddOverlay($"获得{HealOnCritTweak.BaseCrit.ToBaseWithStk(HealOnCritTweak.StackCrit).ToDmg("_%")}的暴击几率。{"击杀".ToDmg()}敌人后使你恢复{"最大生命值".ToHealing()}的{HealOnCritTweak.HealFraction.ToBaseAndStkPct().ToHealing()}，若{"暴击击杀".ToDmg()}则{"恢复量".ToHealing()}将乘以{"暴击倍率".ToDmg()}。", lang);
            "ITEM_HEALWHILESAFE_DESC".AddOverlay($"使脱离战斗状态下的<style=cIsHealing>基础生命值再生速度</style>增加{3.ToBaseAndStk().ToHealing("_hp/s")}，外加<style=cIsHealing>基础生命值<color=yellow>百分比</color>再生速度</style>增加{HealWhileSafeTweak.RegenFraction.ToBaseAndStkPct().ToHealing("_hp/s")}。", lang);
            "ITEM_ICERING_DESC".AddOverlay($"充能完毕后，<style=cIsDamage>不低于{RingsTweak.RingDamageRequired.ToBaseAndStkPct("（取双环中层数高的每层+_）")}伤害的攻击</style>击中敌人时会产生一道{"符文冰霜爆炸".ToIce()}轰击敌人，{"减速".ToIce()}周围敌人使其降低<style=cIsUtility>80%</style>的移动速度{RingsTweak.IceRingSlow80BuffDuration.ToUtil("_秒")}，合计造成{RingsTweak.IceRingDamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。充能时间<style=cIsUtility>10秒</style><style=cStack>（每双手环减少10%）</style>。<color=#FFFF00>工匠特殊效果：基础充能时间降低至1秒，并无视伤害要求。</color>", lang);
            "ITEM_IGNITEONKILL_DESC".AddOverlay($"击败敌人时产生一次半径<style=cIsDamage>{IgniteOnKillTweak.BaseRadius.ToBaseWithStk(IgniteOnKillTweak.StackRadius)}米{"（具体范围与敌人体型相关）".ToUtil()}</style>的爆炸，造成{IgniteOnKillTweak.ExplosionBaseDamageCoefficient.ToDmgPct("_的基础伤害")}，并<style=cIsDamage>点燃</style>所有被爆炸波及的敌人。<style=cIsDamage>点燃</style>合计可对敌人造成{IgniteOnKillTweak.IgniteDamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。", lang);
            "ITEM_IMMUNETODEBUFF_DESC".AddOverlay($"防止<style=cIsUtility>1<style=cStack>（每层+1）</style>个</style><style=cIsDamage>减益效果</style>并施加一道<style=cIsHealing>临时屏障</style>，数值为<style=cIsHealing>最大生命值</style>的<style=cIsHealing>10%</style>。每<style=cIsUtility>5</style>秒充能一次</style>。", lang);
            "ITEM_INFUSION_DESC".AddOverlay($"每击败一名敌人后使{"自身外加主人".ToHealing() + "（如果有）".ToStk()}<style=cIsHealing>永久性</style>增加<style=cIsHealing>1点</style><style=cStack>（每层+1点）</style>最大生命值，最多增加<style=cIsHealing>自身等级x自身基础血量x{InfusionTweak.基础生命值占比.ToPct()}x全队层数</style>点。", lang);
            "ITEM_KNURL_DESC".AddOverlay($"{"最大生命值".ToHealing()}增加{40.ToBaseAndStk().ToHealing()}点，外加升级获得的{"最大生命值".ToHealing()}增加{KnurlTweak.LevelHealthAddCoefficient.ToBaseAndStkPct().ToHealing()}。{"基础生命值再生速度".ToHealing()}增加{1.6f.ToBaseAndStk().ToHealing("_hp/s")}，外加{"基础生命值<color=yellow>百分比</color>再生速度".ToHealing()}增加{KnurlTweak.RegenFraction.ToBaseAndStkPct().ToHealing("_hp/s")}。", lang);
            "ITEM_LIGHTNINGSTRIKEONHIT_DESC".AddOverlay($"命中敌人时有{LightningStrikeOnHitTweak.半数.ToBaseAndStkByCloseToPct().ToDmg()}的几率向敌人的{"雷电召唤池".ToLightning()}中添加{"1次雷击".ToLightning() + LightningStrikeOnHitTweak.Interval.ToStk("（召唤间隔：_秒）")}，合计造成{LightningStrikeOnHitTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。<color=#FFFF00>装卸工特殊效果：关于“雷霆拳套”（详情看 权杖 技能介绍）。</color>", lang);
            "ITEM_LUNARBADLUCK_DESC".AddOverlay($"所有技能冷却时间缩短<style=cIsUtility>2</style><style=cStack>（每层+1）</style>秒。所有随机效果的概率降低<style=cIsUtility>25%</style>{"（叠加公式：幸运降低概率 = 原概率x(运气/(|运气|+3))）".ToStk()}。", lang);
            "ITEM_LUNARPRIMARYREPLACEMENT_DESC".AddOverlay("<style=cIsUtility>替换主要技能</style>为<style=cIsUtility>渴望凝视</style>。\n\n发射一批会延迟引爆的<style=cIsUtility>追踪碎片</style>，造成<style=cIsDamage>120%</style>的基础伤害。最多充能12次<style=cStack>（每层增加12次）</style>，2秒后重新充能<style=cStack>（每层增加2秒）</style>。\n<style=cIsLunar>异教徒：追踪能力加强。每层使技能冷却降低2秒。</style>", lang);
            "ITEM_LUNARSECONDARYREPLACEMENT_DESC".AddOverlay($"<style=cIsUtility>将你的次要技能替换为万刃风暴</style>。\n\n充能并射出一发子弹，对附近的敌人造成<style=cIsDamage>每秒175%的伤害</style>，并在<style=cIsUtility>3</style>秒后爆炸，造成<style=cIsDamage>700%的伤害</style>{"（异教徒：每层+50%爆炸范围）".ToLunar()}，并使敌人<style=cIsDamage>定身</style><style=cIsUtility>3</style><style=cStack>（每层增加+3秒）</style>秒。5秒<style=cStack>（每层增加5秒）</style>后充能。\n<style=cIsLunar>异教徒：每层使攻击速度增加10%</style>。", lang);
            "ITEM_LUNARSPECIALREPLACEMENT_DESC".AddOverlay($"<style=cIsUtility>将你的特殊技能替换为毁坏</style>。\n\n造成伤害可以施加一层<style=cIsDamage>毁坏</style>，持续10<style=cStack>（每层增加+10秒）秒</style>。启动此技能可以<style=cIsDamage>引爆</style>所有的毁坏层数，不限距离，并造成<style=cIsDamage>300%的伤害</style>，外加<style=cIsDamage>每层毁坏120%<style=cIsLunar>（异教徒：每层+60%）</style>的伤害</style>。8秒<style=cStack>（每层增加8秒）</style>后充能。<style=cIsLunar>异教徒：每层使最大生命值增加10%。</style>", lang);
            "ITEM_LUNARSUN_DESC".AddOverlay($"每<style=cIsUtility>{LunarSunTweak.SecondsPerProjectile}</style><style=cStack>（每层-50%）</style>秒获得一个<style=cIsDamage>环绕运动的追踪炸弹</style>，最多可获得{(LunarSunBehavior.baseMaxProjectiles + LunarSunBehavior.maxProjectilesPerStack).ToBaseWithStk(LunarSunBehavior.maxProjectilesPerStack).ToUtil()}个炸弹，每个可造成{LunarSunTweak.BaseDamageCoefficient.ToBaseWithStkPct(LunarSunTweak.StackDamageCoefficient).ToDmg()}的基础伤害。每{LunarSunTweak.SecondsPerTransform.ToUtil()}秒将其他一件随机物品<style=cIsUtility>转化</style>为该物品。", lang);
            "ITEM_LUNARSUN_PICKUP".AddOverlay($"获得多个环绕运动的追踪炸弹。<color=#FF7F7F>每{LunarSunTweak.SecondsPerTransform.ToUtil()}秒，将一件其他物品吸收并转化为自我中心。</color>", lang);
            "ITEM_LUNARUTILITYREPLACEMENT_DESC".AddOverlay("<style=cIsUtility>将你的辅助技能替换</style>为<style=cIsUtility>影逝</style>。\n\n隐去身形，进入<style=cIsUtility>隐形状态</style>并获得<style=cIsUtility>30%移动速度加成</style>。<style=cIsHealing>治疗</style><style=cIsHealing>25%<style=cStack>（每层增加25%）</style>的最大生命值</style>。持续3<style=cStack>（每层加3）</style>秒。\n<style=cIsLunar>异教徒：可通过技能按键切换形态。每层使移动速度增加10%。</style>", lang);
            "ITEM_LUNARWINGS_NAME".AddOverlay("特拉法梅的祝福", lang);
            "ITEM_LUNARWINGS_PICKUP".AddOverlay("一双翅膀。", lang);
            "ITEM_MISSILE_DESC".AddOverlay($"有{MissileTweak.半数.ToBaseAndStkByCloseToPct().ToDmg()}几率向{"导弹发射池".ToUtil()}里添加一枚导弹{MissileTweak.Interval.ToStk("（发射间隔：_秒）")}。每枚合计造成{MissileTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。", lang);
            "ITEM_MISSILEVOID_DESC".AddOverlay($"获得一个相当于你最大生命值<style=cIsHealing>{10}%</style>的<style=cIsHealing>护盾</style>。命中敌人时有{"护盾/总护盾".ToHealing()}x{"100%".ToDmg()}几率{此处忽略触发系数}向敌人发射{"1".ToDmg()}发虾米{MissileVoidTweak.Interval.ToStk("（发射间隔：_秒）")}，合计造成{"护盾/总护盾".ToHealing()}x{MissileVoidTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的总伤害")}。<style=cIsVoid>使所有AtG导弹MK.1无效化</style>。", lang);
            "ITEM_MOVESPEEDONKILL_DESC".AddOverlay($"击杀敌人会使<style=cIsUtility>移动速度</style>提高<style=cIsUtility>125%</style>，在<style=cIsUtility>1</style><style=cStack>（每层+0.5）</style>秒内逐渐失效。<color=#FFFF00>女猎人特殊效果：锁敌距离增加10米{"（每层+10米）".ToStk()}。</color>", lang);
            "ITEM_NEARBYDAMAGEBONUS_DESC".AddOverlay($"对<style=cIsDamage>{13}米</style>内的敌人伤害增加{20}%<style=cStack>（每层+{20}%）</style>.", lang);
            "ITEM_NOVAONHEAL_DESC".AddOverlay($"将{1.ToBaseAndStkPct().ToHealing()}的治疗量储存为<style=cIsHealing>灵魂能量</style>，最大储量等同于<style=cIsHealing>最大生命值</style>的{1f.ToHealPct()}。当<style=cIsHealing>灵魂能量</style>达到<style=cIsHealing>最大生命值</style>的{NovaOnHealTweak.半数.ToBaseAndStkByCloseToPct().ToHealing()}时，自动向周围<style=cIsDamage>{NovaOnHealTweak.BaseRadius}</style>米内的敌人<style=cIsDamage>发射一颗头骨</style>，造成相当于{NovaOnHealTweak.BaseDamageCoefficient.ToDmgPct()}<style=cIsHealing>灵魂能量</style>的伤害。", lang);
            "ITEM_PARENTEGG_DESC".AddOverlay($"在<style=cIsDamage>受到伤害时</style>回复{ParentEggTweak.HealAmount.ToBaseAndStk().ToHealing("_点")}，外加<style=cIsHealing>受到伤害x{ParentEggTweak.HealFractionFromDamage.ToBaseAndStkPct()}</style>的生命值。</color><color=#FFFF00>（圣骑士）太阳特殊效果：火焰伤害增加{1.ToBaseAndStkPct()}。</color>", lang);
            "ITEM_PERMANENTDEBUFFONHIT_DESC".AddOverlay($"命中敌人时有<style=cIsDamage>100%</style>几率使<style=cIsDamage>护甲</style>永久降低<style=cIsDamage>2点</style><style=cStack>（每层+2点）</style>。", lang);
            "ITEM_PERSONALSHIELD_DESC".AddOverlay($"获得一个相当于你最大生命值<style=cIsHealing>8%</style><style=cStack>（每层+8%）</style>的<style=cIsHealing>护盾</style>。脱险后可重新充能。", lang);
            "ITEM_RANDOMLYLUNAR_DESC".AddOverlay($"月球切片的可用次数增加{RandomlyLunarTweak.UsageCount.ToBaseAndStk().ToUtil("_次")}，但使用价格提升速度{"增加".ToDeath() + RandomlyLunarTweak.UsageCount.ToBaseAndStk().ToLunar() + "月球币".ToLunar()}。", lang);
            "ITEM_RANDOMLYLUNAR_PICKUP".AddOverlay("增加月球切片的使用次数，但会使其价格提升速度增加。", lang);
            "ITEM_RELICENERGY_DESC".AddOverlay($"似乎没什么用。<style=cIsUtility>可能会有一些用处...?（此物品可用于充能特殊传送器）</style>", lang);
            "ITEM_REPULSIONARMORPLATE_DESC".AddOverlay($"所有<style=cIsDamage>传入伤害</style>减少<style=cIsDamage>5</style>点<style=cStack>（每层+5点）</style>，但不能减少到<style=cIsDamage>1以下</style>。<color=#FFFF00>工程师及其炮塔特殊效果：护甲增加<style=cIsDamage>10</style>点<style=cStack>（每层+10点）</style>。</color>", lang);
            "ITEM_SAGESBOOK_DESC".AddOverlay($"你的过去将变得...<style=cIsUtility>熟悉</style>...<style=cIsUtility>（若在游戏结束时持有此物品，会使下一把游戏种子和这把相同）</style>", lang);
            "ITEM_SECONDARYSKILLMAGAZINE_DESC".AddOverlay($"为<style=cIsUtility>次要技能</style>增加<style=cIsUtility>1次</style><style=cStack>（每层增加1次）</style>充能。<color=#FFFF00>磁轨炮手特殊效果：关于上弹（详情看技能词条）。</color><color=#FFFF00>狙击手特殊效果：关于瞄准射击（详情看技能词条）。</color>", lang);
            "ITEM_SEED_DESC".AddOverlay($"击中敌人时<style=cIsHealing>恢复</style>等同于你造成<style=cIsDamage>伤害</style>{SeedTweak.Leech.ToBaseAndStkPct().ToHealing()}的生命值。\n<style=cStack>吸血(计算公式) = Sqrt([伤害] x {SeedTweak.Leech.ToPct()})（[伤害] x {SeedTweak.Leech.ToPct()}开平方根）</style>", lang);
            "ITEM_SHOCKNEARBY_DESC".AddOverlay($"发射<style=cIsDamage>电流</style>，每<style=cIsDamage>0.5秒</style>对<style=cIsDamage>3名</style><style=cStack>（每层增加2名）</style>敌人造成<style=cIsDamage>200%</style>的基础伤害。特斯拉线圈每<style=cIsDamage>10秒</style>关闭一次。<color=#FFFF00>装卸工特殊效果：关于“M551电塔”（详情看技能介绍）。</color>", lang);
            "ITEM_SIPHONONLOWHEALTH_DESC".AddOverlay($"在战斗状态下，{SiphonOnLowHealthTweak.BaseRadius.ToBaseWithStk(SiphonOnLowHealthTweak.StackRadius).ToDmg("_米")}范围内距离你最近的{SiphonOnLowHealthTweak.MaxTargets.ToBaseAndStk().ToUtil()}个敌人会与你“拴”在一起，对其造成每秒{SiphonOnLowHealthTweak.DamageCoefficient.ToBaseAndStkPct().ToDmg("_的基础伤害")}，施加<style=cIsDamage>焦油</style>效果，造成伤害的<style=cIsHealing>100%</style>转化为对你的<style=cIsHealing>治疗量</style>。", lang);
            "ITEM_SLOWONHITVOID_DESC".AddOverlay($"命中敌人时有<style=cIsUtility>5%</style><style=cStack>（每层+5%）</style>几率使敌人<style=cIsDamage>定身</style><style=cIsUtility>1秒</style><style=cStack>（每层+1秒）</style>，但是对{"Boss".ToDmg()}和<style=cIsVoid>虚空生物</style><style=cDeath>无效</style>。<style=cIsVoid>使所有时空装置无效化</style>。", lang);
            "ITEM_SPRINTWISP_DESC".AddOverlay($"奔跑时，每{SprintWispTweak.FireInterval.ToUtil()}秒向{SprintWispBodyBehavior.searchRadius.ToDmg("_米")}内的敌人发射一道<style=cIsDamage>追踪幽魂</style>，造成{SprintWispBodyBehavior.damageCoefficient.ToBaseAndStkPct().ToDmg("_的伤害")}。发射速率随<style=cIsUtility>移动速度</style>增加。", lang);
            "ITEM_STICKYBOMB_DESC".AddOverlay($"命中敌人时有<style=cIsDamage>5%</style><style=cStack>（每层增加5%）</style>的几率向敌人的{"黏弹喷泉".ToDmg()}中添加{"1颗黏弹".ToDmg() + StickyBombTweak.Interval.ToStk("（喷射间隔：_秒）")}，爆炸时合计造成{StickyBombTweak.BaseDamageCoefficient.ToDmgPct()}伤害。", lang);
            "ITEM_STUNCHANCEONHIT_DESC".AddOverlay($"命中时有<style=cIsUtility>5%</style><style=cStack>（每层+5%）</style>的几率<style=cIsUtility>眩晕</style>敌人，持续<style=cIsUtility>2秒</style>。<color=#FFFF00>多功能枪兵特殊效果：关于“爆破筒”（详情看技能介绍）。</color>", lang);
            "ITEM_SUNBLADE_DESCRIPTION".AddOverlay($"第一次攻击会<style=cIsDamage>点燃</style>敌人，造成<style=cIsDamage>1500%</style>的基础伤害。之后30秒内，对该敌人的每次攻击都会<style=cIsDamage>点燃</style>它，造成<style=cIsDamage>100%</style><style=cStack>（每层+100%）</style>基础伤害。", lang);
            "ITEM_SYRINGE_DESC".AddOverlay($"使<style=cIsDamage>攻击速度</style>提高<style=cIsDamage>15%<style=cStack>（每层增加15%）</style></style>。<color=#FFFF00>指挥官特殊效果：额外使攻速，移速和最大生命值增加3%<style=cStack>（每层+3%）</style>。</color>", lang);
            "ITEM_THORNS_DESC".AddOverlay($"受伤时弹射出1发{"剃刀".ToDmg()}还击攻击者，若没有，则弹射到{ThornsTweak.BaseRadius.ToBaseWithStk(ThornsTweak.StackRadius).ToDmg("_米")}内最近的敌人。合计造成<style=cIsDamage>受到的伤害的{ThornsTweak.BaseDamageCoefficient.ToBaseWithStkPct(ThornsTweak.StackDamageCoefficient).ToDmg("_的总伤害")}</style>。{ThornsTweak.Interval.ToStk("（发射间隔：_秒）")}", lang);
            "ITEM_TITANGOLDDURINGTP_DESC".AddOverlay("在传送器场景中召唤<style=cIsDamage>奥利雷奥尼特</style>，它具有<style=cIsDamage>100%<style=cStack>（每层增加100%）</style>伤害</style>和<style=cIsHealing>100%<style=cStack>（每层增加100%）</style>生命值</style>。", lang);
            "ITEM_TOOTH_DESC".AddOverlay($"击败敌人后掉落一个<style=cIsHealing>治疗球</style>，拾取后恢复<style=cIsHealing>8</style>点外加等同于<style=cIsHealing>最大生命值</style><style=cIsHealing>2%<style=cStack>（每层+2%）</style></style>的生命值。<color=#FFFF00>呛鼻毒师特殊效果：关于“贪婪撕咬”（详情看技能介绍）。</color>", lang);
            "ITEM_TPHEALINGNOVA_DESC".AddOverlay($"在传送事件中释放<style=cIsHealing>1次</style><style=cStack>（每层增加1次）</style><style=cIsHealing>治疗新星</style>，<style=cIsHealing>治疗</style>传送器附近所有友方，使他们恢复<style=cIsHealing>50%</style>的最大生命值。在充能区域内产生{"治疗区域".ToHealing()}，每秒恢复友方单位{TPHealingNovaTweak.HealFraction.ToBaseAndStkPct().ToHealing()}的最大生命值。<color=#FFFF00>雷克斯特殊效果：治疗效果增加20%{"（每层+20%）".ToStk()}。</color>", lang);
            "ITEM_UTILITYSKILLMAGAZINE_DESC".AddOverlay($"为<style=cIsUtility>辅助技能</style>增加<style=cIsUtility>2次</style><style=cStack>（每层增加2次）</style>充能。<style=cIsUtility>使辅助技能的冷却时间减少33%</style>。<color=#FFFF00>船长特殊效果：使UES顺风号等待时间缩短33%<style=cStack>（每层+33%）</style>。</color>", lang);
            "ITEM_WARCRYONMULTIKILL_DESC".AddOverlay($"<style=cIsDamage>击杀敌人</style>会使你获得<style=cIsDamage>狂热</style>增益，最高{WarCryOnMultiKillTweak.BaseMaxBuffCount.ToBaseWithStk(WarCryOnMultiKillTweak.StackMaxBuffCount).ToUtil("_层")}，每层持续{WarCryOnMultiKillTweak.BuffDuration.ToDmg("_秒")}。每层<style=cIsDamage>狂热</style>可使{攻击速度}提高{WarCryOnMultiKillTweak.AttackSpeedMultAddPerBuff.ToDmgPct()}，使{移动速度}提高{WarCryOnMultiKillTweak.MoveSpeedMultAddPerBuff.ToPct().ToUtil()}。", lang);
            "KEYWORD_ACTIVERELOAD".AddOverlay($"<style=cKeywordName>主动上弹</style><style=cSub>开火后给你的磁轨炮上弹（按{Settings.ReloadKey.Value.MainKey.ToUtil()}键也可进入上弹）。<style=cIsDamage>完美上弹</style>后，下一发射弹额外造成{"50%".ToDmg() + "（每层备用弹夹+10%）".ToStk()}伤害。", lang);
            "KEYWORD_ARC".AddOverlay("<style=cKeywordName>击穿</style><style=cSub>在最多4个敌人之间形成电弧，每次造成30%的伤害。</style>", lang);
            StringExtensions.AddOverlay("KEYWORD_ENTANGLE", $"<style=cKeywordName>纠缠</style><style=cSub>攻击造成<style=cIsVoid>纠缠</style>减益，当此攻击击中被<style=cIsVoid>纠缠</style>的敌人时，其他所有被<style=cIsVoid>纠缠</style>敌人将同时受到<style=cIsVoid>{PlasmaCoreSpikestripContent.Content.Skills.States.FireWeave.damageCoefficient.ToDmgPct()}基础伤害</style>。</style>");
            "KEYWORD_FLEETING".AddOverlay("<style=cKeywordName>一闪</style><style=cSub><style=cIsDamage>攻速</style>转化为<style=cIsDamage>技能伤害</style>。", lang);
            "KEYWORD_FRICTIONLESS".AddOverlay("<style=cKeywordName>绝对光滑</style><style=cSub>无伤害衰减</style>。", lang);
            "KEYWORD_SOULROT".AddOverlay($"<style=cKeywordName>灵魂之痛</style><style=cSub>{"忽略护甲".ToDeath()}，每秒<style=cIsVoid>至少</style>造成敌人<style=cIsHealing>最大生命值</style>的<style=cIsVoid>2.5%</style>的伤害，持续{"20秒".ToDmg()}后消失。</style>", lang);
            "LOADER_SPECIAL_ALT_DESCRIPTION".AddOverlay($"<style=cIsUtility>重型</style>。{"眩晕".ToDmg()}。用重拳砸向地面，造成{LoaderTweak.GroundSlamBaseDamageCoefficient.ToDmgPct("_的范围伤害")}。", lang);
            "LOADER_SPECIAL_DESCRIPTION".AddOverlay($"扔出飘浮电塔，可<style=cIsDamage>电击</style>周围{LoaderTweak.PylonRange.ToBaseAndStk("（每层不稳定的特斯拉线圈+_）").ToUtil("_米")}内最多{LoaderTweak.PylonFireCount.ToBaseAndStk("（每层不稳定的特斯拉线圈+_）").ToDmg("_名")}敌人，电流最多可弹射{LoaderTweak.PylonBounces.ToBaseAndStk("（每层不稳定的特斯拉线圈+_）").ToDmg("_次")}，造成{LoaderTweak.PylonDamageCoefficient.ToDmgPct("_的伤害")}。可被<style=cIsUtility>格挡</style>。", lang);
            "LOADER_UTILITY_ALT1_DESCRIPTION".AddOverlay($"<style=cIsUtility>重型</style>。发动一次<style=cIsUtility>单体攻击</style>直拳，造成{Mathf.Pow(LoaderTweak.ChargeZapFistLungeSpeed * LoaderTweak.SwingChargedFistVelocityDamageCoefficient, LoaderTweak.pow).ToDmgPct("_的基础伤害")}，<style=cIsDamage>震荡</style>锥形区域内的所有敌人并合计造成{LoaderTweak.ChargeZapFistLightningDamageCoefficient.ToDmgPct("_的总伤害")}。", lang);
            "LOADER_UTILITY_DESCRIPTION".AddOverlay($"<style=cIsUtility>重型</style>。发动一次<style=cIsUtility>穿透</style>直拳，造成{Mathf.Pow(LoaderTweak.ChargeFistMinLungeSpeed * LoaderTweak.SwingChargedFistVelocityDamageCoefficient, LoaderTweak.pow).ToDmgPct()}-{Mathf.Pow(LoaderTweak.ChargeFistMaxLungeSpeed * LoaderTweak.SwingChargedFistVelocityDamageCoefficient, LoaderTweak.pow).ToDmgPct()}的{基础伤害}。", lang);
            "LUNARVAULT_NAME".AddOverlay("月球库", lang);
            "MAGE_PRIMARY_FIRE_DESCRIPTION".AddOverlay("<style=cIsDamage>点燃</style>。发射一枚火炎弹，造成<style=cIsDamage>220%的伤害</style>。</style>", lang);
            "MAGE_PRIMARY_LIGHTNING_DESCRIPTION".AddOverlay($"发射一枚闪电弹，造成<style=cIsDamage>300%的伤害</style>并<style=cIsDamage>引爆</style>小片区域。</style>", lang);
            "MAGE_SECONDARY_ICE_DESCRIPTION".AddOverlay($"<style=cIsUtility>冰冻</style>。使用拥有<style=cIsDamage>穿透</style>效果的纳米枪发动攻击，充能后能造成{MageTweak.IcebombMinDamageCoefficient.ToDmgPct()}-{MageTweak.IcebombMaxDamageCoefficient.ToDmgPct("_的伤害")}。爆炸后留下一个<style=cIsUtility>冰冻炸弹</style>，2秒后爆炸并<style=cIsUtility>冰冻</style>附近{"12米".ToUtil()}的敌人，合计造成{"100%".ToDmg("_的总伤害")}。", lang);
            "MAGE_SECONDARY_LIGHTNING_DESCRIPTION".AddOverlay($"<style=cIsDamage>眩晕</style>。发射一枚纳米炸弹，如果充能将造成{MageTweak.NavoBombMinDamageCoefficient.ToDmgPct()}-{MageTweak.NavoBombMaxDamageCoefficient.ToDmgPct("_的伤害")}。飞行期间电击周围<style=cIsUtility>{MageTweak.电击半径}</style>米内最多{MageTweak.每次最大电击数.ToDmg("_个")}敌人，每秒合计造成{MageTweak.电击伤害系数.ToDmgPct("_的伤害")}。", lang);
            "MAGE_SPECIAL_FIRE_DESCRIPTION".AddOverlay($"<style=cIsDamage>点燃</style>。灼烧面前的所有敌人，对其造成{(Flamethrower.totalDamageCoefficient / Flamethrower.baseFlamethrowerDuration).ToDmgPct("每秒_的伤害")}。", lang);
            "MAGE_UTILITY_ICE_DESCRIPTION".AddOverlay($"<style=cIsUtility>冰冻</style>。创造一道能够对敌人造成{PrepWall.damageCoefficient.ToDmgPct()}伤害的屏障。", lang);
            "MERC_PRIMARY_DESCRIPTION".AddOverlay("<style=cIsDamage>一闪</style>。<style=cIsUtility>灵巧</style>。向前挥砍并造成<style=cIsDamage>130%的伤害</style>。第三次攻击的范围将会变大并<style=cIsUtility>暴露</style>敌人。", lang);
            "MERC_SECONDARY_ALT1_DESCRIPTION".AddOverlay($"<style=cIsDamage>一闪</style>。释放一个裂片上勾拳，造成{Uppercut.baseDamageCoefficient.ToDmgPct("_的伤害")}，并将你送到半空。", lang);
            "MERC_SECONDARY_DESCRIPTION".AddOverlay("<style=cIsDamage>一闪</style>。快速横斩两次，造成<style=cIsDamage>2x200%的伤害</style>，若位于空中，则改为竖斩。", lang);
            "MERC_SPECIAL_ALT1_DESCRIPTION".AddOverlay($"<style=cIsDamage>一闪</style>。发射一次刀刃之风，最多可对<style=cIsDamage>3</style>名敌人造成<style=cIsDamage>8x{1.ToDmgPct("_的伤害")}</style>。最后一次打击将<style=cIsUtility>暴露</style>敌人。", lang);
            "MERC_SPECIAL_DESCRIPTION".AddOverlay($"<style=cIsDamage>一闪</style>。<style=cIsUtility>重型</style>。瞄准距离最近的敌人，攻击被瞄准的敌人可对其重复造成{Evis.damageCoefficient.ToDmgPct("_的伤害")}。<style=cIsUtility>过程中无法被攻击，跳跃键可提前退出技能。</style>", lang);
            "MERC_UTILITY_ALT1_DESCRIPTION".AddOverlay("<style=cIsUtility>重型</style>。<style=cIsDamage>眩晕</style>。向前冲锋，造成<style=cIsDamage>700%的伤害</style>并在<style=cIsUtility>1秒</style>后<style=cIsUtility>暴露</style>敌人。", lang);
            "MERC_UTILITY_DESCRIPTION".AddOverlay("<style=cIsUtility>重型</style>。<style=cIsDamage>眩晕</style>。向前冲锋并造成<style=cIsDamage>300%的伤害</style>。只要命中敌人，<style=cIsDamage>就可以再次发起冲锋</style>，最多<style=cIsDamage>3</style>次。", lang);
            "NIX_SKIN_LAZYHUNTRESSSKIN_NAME".AddOverlay("兔女郎", lang);
            "NIX_SKIN_MOBFIENDSKIN_NAME".AddOverlay("怪人", lang);
            "NIX_SKIN_NECROBUNNYSKIN_NAME".AddOverlay("修女", lang);
            "NIX_SKIN_TECHNOMERCSKIN_NAME".AddOverlay("女剑圣", lang);
            "OBJECTIVE_BATTERIES_INSERTED_TOKEN".AddOverlay("给燃料组充电 ({0}/{1})", lang);
            "POTMOBILEMONSTER_BODY_NAME".AddOverlay("生锅", lang);
            "RAILGUNNER_ACTIVE_RELOAD_DESCRIPTION".AddOverlay($"在恰到好处的时机上弹可更快恢复，并使你下一发射弹额外造成{"50%".ToDmg() + "（每层备用弹夹+10%）".ToStk()}伤害。", lang);
            "RAILGUNNER_PRIMARY_DESCRIPTION".AddOverlay($"发射主动追踪弹药，造成<style=cIsDamage>100%的伤害</style>。", lang);
            "RAILGUNNER_SECONDARY_ALT_DESCRIPTION".AddOverlay($"{"手动上弹".ToUtil()}。启动你的<style=cIsUtility>近程瞄准镜</style>，高亮显示<style=cIsDamage>弱点</style>，并将你的武器转化为一门可造成{RailgunnerTweak.HH44DamageCoefficient.ToDmgPct()}伤害的快速磁轨炮。", lang);
            "RAILGUNNER_SECONDARY_DESCRIPTION".AddOverlay($"{"主动上弹".ToUtil()}。启动你的<style=cIsUtility>远程瞄准镜</style>，高亮显示<style=cIsDamage>弱点</style>，并将你的武器转化为一门可造成{RailgunnerTweak.M99DamageCoefficient.ToDmgPct()}伤害的穿刺磁轨炮。", lang);
            "RAILGUNNER_SNIPE_CRYO_DESCRIPTION".AddOverlay($"<style=cIsUtility>冰冻</style>。发射一枚超低温射弹，造成{RailgunnerTweak.CryochargeDamageCoefficient.ToDmgPct("_的伤害")}。", lang);
            "RAILGUNNER_SNIPE_HEAVY_DESCRIPTION".AddOverlay($"发射一枚重型射弹，造成{RailgunnerTweak.M99DamageCoefficient.ToDmgPct("_的伤害")}。", lang);
            "RAILGUNNER_SNIPE_LIGHT_DESCRIPTION".AddOverlay($"发射一枚轻型射弹，造成{RailgunnerTweak.HH44DamageCoefficient.ToDmgPct("_的伤害")}。", lang);
            "RAILGUNNER_SNIPE_SUPER_DESCRIPTION".AddOverlay($"发射一枚造成{RailgunnerTweak.SuperchargeDamageCoefficient.ToDmgPct("_的伤害")}且具有{RailgunnerTweak.SuperchargeCritDamageCoefficient.ToDmg()}倍{"暴击伤害".ToDmg()}的超载射弹</style>。", lang);
            "RAILGUNNER_SPECIAL_ALT_DESCRIPTION".AddOverlay($"<style=cIsUtility>冰冻</style>。发射<style=cIsDamage>具有穿透效果</style>的子弹，造成{RailgunnerTweak.CryochargeDamageCoefficient.ToDmgPct("_的伤害")}。", lang);
            "RAILGUNNER_SPECIAL_DESCRIPTION".AddOverlay($"发射一枚<style=cIsDamage>具有穿刺效果，</style>造成{RailgunnerTweak.SuperchargeDamageCoefficient.ToDmgPct("_的伤害")}且具有{RailgunnerTweak.SuperchargeCritDamageCoefficient.ToDmg()}倍暴击伤害</style>的超载射弹。之后，<style=cIsHealth>你的所有武器都将失灵</style>，持续<style=cIsHealth>{5}</style>秒。", lang);
            "RAILGUNNER_UTILITY_ALT_DESCRIPTION".AddOverlay($"扔出一部装置，该装置可使附近的{"敌人减速80%，射弹减速99%".ToUtil()}。", lang);
            "RAILGUNNER_UTILITY_DESCRIPTION".AddOverlay($"扔出一部装置，该装置可将你和附近所有敌人<style=cIsUtility>推开</style>。最多可拥有{2}部。", lang);
            "SANCTUMWISP_BODY_NAME".AddOverlay("神圣幽魂", lang);
            "SHRINE_AEGIS_NAME".AddOverlay("灾难神龛", lang);
            "SIXFEARS7_SKIN_BLACKRID_NAME".AddOverlay("黑暗", lang);
            "SIXFEARS7_SKIN_RADIOACRID_NAME".AddOverlay("光明", lang);
            "SIXFEARS7_SKIN_VOIDCRID_NAME".AddOverlay("虚空", lang);
            "SPIKESTRIPCONTENT_EXPANSION_DESCRIPTION".AddOverlay("将'Spikestrip 2.0'的内容添加到游戏。", lang);
            $"SPIKESTRIPSKILL_{PlasmaCoreSpikestripContent.Content.Skills.Weave.instance.SkillToken}_DESCRIPTION".AddOverlay($"<style=cIsVoid>纠缠</style>。快速攻击敌人，每次造成<style=cIsDamage>{PlasmaCoreSpikestripContent.Content.Skills.States.FireWeave.damageCoefficient.ToDmgPct("_的伤害")}</style>。", lang);
            $"SPIKESTRIPSKILL_{PlasmaCoreSpikestripContent.Content.Skills.Weave.instance.SkillToken}_NAME".AddOverlay("「编织」", lang);
            "SPIKESTRIPSKILL_DEEPROT_DESCRIPTION".AddOverlay($"<style=cIsVoid>虚 空 馈 赠</style>：<style=cIsHealing>毒化</style>攻击改为叠加<style=cIsVoid>虚空之毒</style>，使<style=cIsVoid>速度减慢10%</style>。当<style=cIsVoid>虚空之毒</style>叠加的层数达到<style=cIsVoid>灵魂之痛</style>层数的<style=cIsVoid>3</style>倍时，所有<style=cIsVoid>虚空之毒</style>将转化为<style=cIsVoid>灵魂之痛</style>。此外，所有<style=cArtifact>虚空</style>攻击都有几率叠加<style=cIsVoid>虚空之毒</style>。", lang);
            "SPIKESTRIPSKILL_DEEPROT_NAME".AddOverlay("腐朽", lang);
            "STAGE_DRYBASIN_NAME".AddOverlay($"干旱盆地", lang);
            "STAGE_DRYBASIN_SUBTITLE".AddOverlay($"废弃通道", lang);
            "STAGE_FORGOTTENHAVEN_NAME".AddOverlay($"遗忘天堂", lang);
            "STAGE_FORGOTTENHAVEN_SUBTITLE".AddOverlay($"废弃杰作", lang);
            "STAGE_WEATHEREDSATELLITE_NAME".AddOverlay($"沉睡卫星", lang);
            "STAGE_WEATHEREDSATELLITE_SUBTITLE".AddOverlay($"高架整体", lang);
            "TOOLBOT_PRIMARY_ALT1_DESCRIPTION".AddOverlay("发射1条具有穿透效果的钢筋，造成<style=cIsDamage>600%</style>的伤害。", lang);
            "TOOLBOT_PRIMARY_ALT3_DESCRIPTION".AddOverlay("锯伤周围敌人，造成<style=cIsDamage>每秒1000%的伤害</style>。", lang);
            "TOOLBOT_PRIMARY_DESCRIPTION".AddOverlay("快速发射钉子，造成<style=cIsDamage>70%的伤害</style>。最后一次性发射<style=cIsDamage>12</style>枚伤害为<style=cIsDamage>70%的钉子。<style=cStack>（每发射1枚钉子+0.07米射程，松开后清零）</style>", lang);
            "TOOLBOT_SECONDARY_DESCRIPTION".AddOverlay($"<style=cIsDamage>眩晕</style>。发射一枚造成<style=cIsDamage>220%伤害</style>的爆破筒。将分裂为造成<style=cIsDamage>5{"（每层眩晕手雷+5）".ToStk()}x44%伤害</style>以及<style=cIsDamage>眩晕</style>效果的小炸弹。", lang);
            "TREEBOT_SPECIAL_DESCRIPTION".AddOverlay($"<style=cIsHealth>25%生命值</style>。发射一朵会<style=cIsDamage>扎根</style>并造成<style=cIsDamage>200%伤害</style>的花朵。每命中一个目标便会对你治疗{TreebotFlower2Projectile.healthFractionYieldPerHit.ToHealPct()}的生命值。", lang);
            "TREEBOT_UTILITY_ALT1_DESCRIPTION".AddOverlay($"{FirePlantSonicBoom.healthCostFraction.ToPct().ToHealth()}生命值。发射一次<style=cIsUtility>音爆</style>并对敌人造成{TreebotTweak.FirePlantSonicBoomDamageCoefficient.ToDmgPct("_的伤害")}。每命中一个目标便会对你治疗{FirePlantSonicBoom.healthFractionPerHit.ToHealPct()}的生命值。", lang);
            "VOIDCRID_NULLBEAM".AddOverlay("<style=cArtifact>「虚空光束』</style>", lang);
            "VOIDCRID_NULLBEAM_DESC".AddOverlay("<style=cArtifact>虚空</style>。从<style=cIsVoid>虚空</style>中汲取力量，发射中距离<style=cIsVoid>虚空光束</style>攻击敌人，造成<style=cIsDamage>900%</style>的伤害，按住可增加发射的持续时间。每一击都有概率<style=cIsVoid>定身</style>敌人。", lang);
            "VOIDCRID_PASSIVE".AddOverlay("<style=cArtifact>虚空</style>克里德", lang);
            "VOIDCRID_PASSIVE_DESC".AddOverlay("所有<style=cArtifact>虚空</style>攻击都有几率<style=cArtifact>定身</style>敌人。（如果选择了“腐朽”被动，则额外叠加<style=cWorldEvent>虚空之毒</style>减益）", lang);
            "VOIDCRID_VOIDDRIFT".AddOverlay("<style=cArtifact>「虚无漂流』</style>", lang);
            "VOIDCRID_VOIDRIFT_DESC".AddOverlay("<style=cArtifact>虚空</style>。<style=cIsDamage>眩晕</style>。遁入<style=cIsVoid>虚空</style>，眩晕周围敌人并造成<style=cIsDamage>400%</style>的伤害，退出时再次眩晕周围敌人并造成<style=cIsDamage>400%</style>的伤害，有概率<style=cIsVoid>定身</style>敌人。", lang);
            "VOIDSURVIVOR_PRIMARY_ALT_DESCRIPTION".AddOverlay("<style=cKeywordName>腐化升级</style><style=cSub>发射一束造成2000%伤害的短程光束。</style>", lang);
            "VOIDSURVIVOR_SECONDARY_ALT_DESCRIPTION".AddOverlay("<style=cKeywordName>腐化升级</style><style=cSub>发射一枚造成2500%伤害的黑洞炸弹，半径变为25米。</style>", lang);
            "VOIDSURVIVOR_SECONDARY_DESCRIPTION".AddOverlay($"充能一枚虚空炸弹，造成<style=cIsDamage>{VoidSurvivorTweak.FireMegaBlasterSmallDamageCoefficient.ToDmgPct()}伤害</style>。完全充能时可以变成爆炸性虚空炸弹，造成<style=cIsDamage>{VoidSurvivorTweak.FireMegaBlasterBigDamageCoefficient.ToDmgPct()}伤害</style>。", lang);
            "VOIDSURVIVOR_SECONDARY_UPRADE_TOOLTIP".AddOverlay("<style=cKeywordName>腐化升级</style><style=cSub>转化成能造成2500%伤害的黑洞炸弹，半径变为25米。</style>", lang);
            "VOIDSURVIVOR_SPECIAL_ALT_DESCRIPTION".AddOverlay("<style=cKeywordName>腐化升级</style><style=cSub>消耗25%的生命值来获得25%的腐化。</style>", lang);
            "VOIDSURVIVOR_UTILITY_ALT_DESCRIPTION".AddOverlay("<style=cIsUtility>消失</style>进入虚空，<style=cIsUtility>向前沿弧线</style>移动，同时<style=cIsUtility>清除所有减益效果</style>。", lang);
            "VV_ITEM_BROKEN_MESS_NAME".AddOverlay("破碎的混合物", lang);
            "VV_ITEM_EMPTY_VIALS_NAME".AddOverlay("空瓶", lang);
            "VV_OBJECTIVE_SHELL".AddOverlay("充能<style=cIsVoid>失落的信标</style> ({0}%)", lang);
            "VV_OBJECTIVE_SHELL_OOB".AddOverlay("进入<style=cIsVoid>失落的信标范围!</style> ({0}%)", lang);
            "VV_SHELL_CONTEXT".AddOverlay("激活失落的信标..?", lang);
            $"VV_ITEM_{vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName}_DESCRIPTION".AddOverlay($"你的<style=cIsDamage>击杀效果</style>在杀死一个精英后会额外发生<style=cIsDamage>1</style>次<style=cStack>（每层+1次）</style>。另外会产生半径<style=cIsDamage>12</style>米的<style=cIsDamage>爆炸</style>，造成<style=cIsDamage>100%</style>的伤害。<style=cIsVoid>腐化所有陈旧断头台</style>。</style><color=#FFFF00>圣骑士特殊效果：基础伤害增加3点{"（每层+3点）".ToStk()}。</color>", lang);
            $"VV_ITEM_{vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName}_NAME".AddOverlay("刽子手的重负", lang);
            $"VV_ITEM_{vanillaVoid.Items.ExeBlade.instance.ItemLangTokenName}_DESCRIPTION".AddOverlay("你的击杀效果在杀死一个精英后会额外发生一次。在杀死精英时还会造成一个伤害性的AOE。<style=cIsVoid>腐化所有陈旧断头台</style>。", lang);
            string text = RoR2.Language.GetString($"VV_ITEM_{vanillaVoid.Items.VoidShell.instance.ItemLangTokenName}_DESCRIPTION");
            text = text.Replace(") will appear in a random location <style=cIsUtility>on each stage</style>. <style=cStack>(Increases rarity chances of the items per stack).</style> <style=cIsVoid>Corrupts all 运输申请单s</style>.", "）的<style=cIsUtility>深空信号</style>。<style=cStack>（层数越高，该物品拥有高稀有度的几率越高）</style>。<style=cIsVoid>腐化所有运输申请单</style>。");
            text = text.Replace("A <style=cIsVoid>special</style> delivery containing items (", "在<style=cIsUtility>每个关卡中</style>，都会在随机位置生成一个内含特殊物品（");
            $"VV_ITEM_{vanillaVoid.Items.VoidShell.instance.ItemLangTokenName}_DESCRIPTION".AddOverlay(text, lang);
            $"VV_ITEM_{vanillaVoid.Items.VoidShell.instance.ItemLangTokenName}_DESCRIPTION".AddOverlay("无尽的聚宝盆", lang);
            $"VV_ITEM_{vanillaVoid.Items.VoidShell.instance.ItemLangTokenName}_DESCRIPTION".AddOverlay("获得一个特殊的、危险的快递，并获得强大的奖励。<style=cIsVoid>腐化所有运输申请单</style>。", lang);
        }

        private static void 权杖技能汉化() {
            "ANCIENTSCEPTER_VOIDSURVIVOR_CORRUPTEDCRUSHCORRUPTIONDESC".AddOverlay(RoR2.Language.GetString("VOIDSURVIVOR_SPECIAL_UPRADE_TOOLTIP") + $"技能效果影响周围{25.ToBaseAndStk().ToUtil("_米")}内的敌人，且对敌人的效果增加{1.ToBaseAndStkPct().ToUtil()}。".ToScepterDesc(), lang);
            "ANCIENTSCEPTER_HERETIC_SQUAWK_DESC".AddOverlay($"<style=cIsHealth>歌声</style>将标记所有<style=cIsHealth>活体</style>，使其染上<link=\"BulwarksHauntWavy\">{"灭绝".ToRed()}</link>！<link=\"BulwarksHauntWavy\">{"灭绝".ToRed()}</link>：持续{"10秒".ToUtil() + "（每层权杖+10秒）".ToStk()}，当带有<link=\"BulwarksHauntWavy\">{"灭绝".ToRed()}</link>的敌人{"死去".ToRed()}时，会连带着它的{"所有族人".ToRed()}一起{"死去".ToRed()}。", lang);
            "ANCIENTSCEPTER_HERETIC_SQUAWK_NAME".AddOverlay("<link=\"BulwarksHauntWavy\">灭绝之歌</link>", lang);
        }

        private static void 圣骑士汉化() {
            string text = "<color=yellow>(本人物由QQ用户“疯狂”(2437181705)翻译)</color>圣骑士是一位重击坦克，可以选择超凡的魔法或毁灭性的剑术来帮助盟友和消灭敌人。<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 你的被动状态占了你伤害的很大一部分，尽可能地保持它。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 旋转猛击既可以作为强大的群体控制技能，也可以作为限制移动的一种形式。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 疾走（Quickstep）的冷却时间随着每一次命中而降低，这是对你坚持到底的奖励。" + Environment.NewLine + Environment.NewLine;
            text += "< ! > 沉默誓言（Vow of Silence）是对付飞行敌人的好方法，因为它会把所有受影响的敌人拖到地上。</color>" + Environment.NewLine + Environment.NewLine;
            "PALADIN_NAME".AddOverlay("圣骑士", lang);
            "PALADIN_DESCRIPTION".AddOverlay(text, lang);
            "PALADIN_SUBTITLE".AddOverlay("普罗维登斯的侍从", lang);
            "PALADIN_OUTRO_FLAVOR".AddOverlay("..于是他离开了，对他的教义的信心动摇了。", lang);
            "PALADIN_OUTRO_FAILURE".AddOverlay("..于是他消失了，他的祈祷没有人听到。", lang);
            "PALADINBODY_DEFAULT_SKIN_NAME".AddOverlay("默认", lang);
            "PALADINBODY_LUNAR_SKIN_NAME".AddOverlay("月球", lang);
            "PALADINBODY_LUNARKNIGHT_SKIN_NAME".AddOverlay("月光骑士", lang);
            "PALADINBODY_TYPHOON_SKIN_NAME".AddOverlay("君主", lang);
            "PALADINBODY_TYPHOONLEGACY_SKIN_NAME".AddOverlay("君主（经典）", lang);
            "PALADINBODY_POISON_SKIN_NAME".AddOverlay("腐败", lang);
            "PALADINBODY_POISONLEGACY_SKIN_NAME".AddOverlay("腐败（经典）", lang);
            "PALADINBODY_CLAY_SKIN_NAME".AddOverlay("阿菲利安", lang);
            "PALADINBODY_SPECTER_SKIN_NAME".AddOverlay("幽灵", lang);
            "PALADINBODY_DRIP_SKIN_NAME".AddOverlay("水滴", lang);
            "PALADINBODY_MINECRAFT_SKIN_NAME".AddOverlay("我的世界", lang);
            "LUNAR_KNIGHT_BODY_NAME".AddOverlay("月光骑士", lang);
            "LUNAR_KNIGHT_BODY_DESCRIPTION".AddOverlay(text, lang);
            "LUNAR_KNIGHT_BODY_SUBTITLE".AddOverlay("米斯历克斯的侍从", lang);
            "LUNAR_KNIGHT_BODY_OUTRO_FLAVOR".AddOverlay("..于是他离开了，对他的教义的信心动摇了。", lang);
            "NEMPALADIN_NAME".AddOverlay("复仇圣骑士", lang);
            "NEMPALADIN_DESCRIPTION".AddOverlay(text, lang);
            "NEMPALADIN_SUBTITLE".AddOverlay("普罗维登斯的侍从", lang);
            "NEMPALADIN_OUTRO_FLAVOR".AddOverlay("..于是他离开了，对他的教义的信心动摇了。", lang);
            "NEMPALADIN_OUTRO_FAILURE".AddOverlay("..于是他消失了，他的祈祷没有人听到。", lang);
            "PALADIN_PASSIVE_NAME".AddOverlay("堡垒的祝福", lang);
            "PALADIN_PASSIVE_DESCRIPTION".AddOverlay("每级获得<style=cIsHealing>1护甲</style>。当拥有<style=cIsHealth>90%生命值</style>或者拥有任意<style=cIsHealth>护盾</style>时，圣骑士会获得<style=cIsHealing>祝福</style>，强化所有剑技。", lang);
            text = "向前劈砍，造成<style=cIsDamage>350%伤害</style>。如果圣骑士拥有<style=cIsHealing>祝福</style>，会发射一道<style=cIsUtility>剑光</style>造成<style=cIsDamage>300%伤害</style>。";
            "PALADIN_PRIMARY_SLASH_NAME".AddOverlay("神圣之剑", lang);
            "PALADIN_PRIMARY_SLASH_DESCRIPTION".AddOverlay(text, lang);
            text = "向前劈砍，造成<style=cIsDamage>380%伤害</style>。如果圣骑士拥有<style=cIsHealing>祝福</style>，会发射一道<style=cIsUtility>剑光</style>造成<style=cIsDamage>300%伤害</style>。";
            "PALADIN_PRIMARY_CURSESLASH_NAME".AddOverlay("诅咒之剑", lang);
            "PALADIN_PRIMARY_CURSESLASH_DESCRIPTION".AddOverlay(text, lang);
            text = "<style=cIsUtility>眩晕</style>。使用一个大范围的劈砍，造成<style=cIsDamage>1000% 伤害</style>，如果拥有<style=cIsHealing>祝福</style>会提升劈砍范围。在空中释放会变成裂地斩，如果拥有<style=cIsHealing>祝福</style>会造成<style=cIsUtility>冲击波</style>。";
            "PALADIN_SECONDARY_SPINSLASH_NAME".AddOverlay("回旋斩", lang);
            "PALADIN_SECONDARY_SPINSLASH_DESCRIPTION".AddOverlay(text, lang);
            text = "<style=cIsUtility>震荡</style>。<style=cIsUtility>灵巧</style>。充能并扔出一道<style=cIsUtility>闪电束</style>，造成<style=cIsDamage>800%伤害</style>。如果被闪电束击中，会在剑上附着<style=cIsUtility>闪电</style>效果，持续<style=cIsUtility>4秒</style>。";
            "PALADIN_SECONDARY_LIGHTNING_NAME".AddOverlay("圣光之矛", lang);
            "PALADIN_SECONDARY_LIGHTNING_DESCRIPTION".AddOverlay(text, lang);
            text = "<style=cIsUtility>灵巧</style>。发射一连串<style=cIsUtility>月球碎片</style>，每个造成<style=cIsDamage>75%伤害</style>。最多拥有<style=cIsDamage>12</style>碎片。";
            "PALADIN_SECONDARY_LUNARSHARD_NAME".AddOverlay("月球碎片", lang);
            "PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION".AddOverlay(text, lang);
            text = "<style=cIsUtility>冲刺</style>一小段距离并获得<style=cIsHealing>10%屏障</style>。成功使用<style=cIsDamage>神圣之剑</style>击中敌人会<style=cIsUtility>减少冷却</style><style=cIsDamage>1秒</style>。<style=cIsUtility>最多可储存2次充能<style=cIsHealing>";
            "PALADIN_UTILITY_DASH_NAME".AddOverlay("疾走", lang);
            "PALADIN_UTILITY_DASH_DESCRIPTION".AddOverlay(text, lang);
            text = string.Concat(
            [
                "恢复<style=cIsHealing>",
                15.ToString(),
                "%最大生命值</style>并且为范围内的盟友提供<style=cIsHealing>",
                15.ToString(),
                "%屏障</style>。"
            ]);
            "PALADIN_UTILITY_HEAL_NAME".AddOverlay("补给", lang);
            "PALADIN_UTILITY_HEAL_DESCRIPTION".AddOverlay(text, lang);
            text = string.Concat(
            [
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                1.5f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>祝福</style>区域，持续<style=cIsDamage>",
                12f.ToString(),
                "秒</style>。在范围内的所有盟友会缓慢<style=cIsHealing>恢复生命值</style>并且获得<style=cIsHealing>屏障</style>。"
            ]);
            "PALADIN_SPECIAL_HEALZONE_NAME".AddOverlay("神圣之光", lang);
            "PALADIN_SPECIAL_HEALZONE_DESCRIPTION".AddOverlay(text, lang);
            text += "双倍治疗。双倍屏障。清除减益。".ToScepterDesc();
            "PALADIN_SPECIAL_SCEPTERHEALZONE_NAME".AddOverlay("神圣之光", lang);
            "PALADIN_SPECIAL_SCEPTERHEALZONE_DESCRIPTION".AddOverlay(text, lang);
            text = string.Concat(
            [
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>沉默</style>区域，持续<style=cIsDamage>",
                10f.ToString(),
                "秒</style>，使区域内所有敌人<style=cIsHealth>麻木</style>。（禁用技能和特殊效果）"
            ]);
            "PALADIN_SPECIAL_TORPOR_NAME".AddOverlay("沉默誓言", lang);
            "PALADIN_SPECIAL_TORPOR_DESCRIPTION".AddOverlay(text, lang);
            text += "更强的减益。更大的范围。摧毁投射物。".ToScepterDesc();
            "PALADIN_SPECIAL_SCEPTERTORPOR_NAME".AddOverlay("沉默誓言", lang);
            "PALADIN_SPECIAL_SCEPTERTORPOR_DESCRIPTION".AddOverlay(text, lang);
            text = string.Concat(
            [
                "<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>，然后释放一个<style=cIsUtility>强化</style>区域，持续<style=cIsDamage>",
                8f.ToString(),
                "秒</style>，提升范围内的所有盟友<style=cIsDamage>伤害</style>和<style=cIsDamage>攻速</style>。"
            ]);
            "PALADIN_SPECIAL_WARCRY_NAME".AddOverlay("神圣誓言", lang);
            "PALADIN_SPECIAL_WARCRY_DESCRIPTION".AddOverlay(text, lang);
            text += "更快的施法速度。双倍伤害。双倍攻速。".ToScepterDesc();
            "PALADIN_SPECIAL_SCEPTERWARCRY_NAME".AddOverlay("神圣誓言(Scepter)", lang);
            "PALADIN_SPECIAL_SCEPTERWARCRY_DESCRIPTION".AddOverlay(text, lang);
            text = string.Concat(
            [
                "<style=cIsHealth>过热</style>。<style=cIsUtility>引导</style><style=cIsDamage>",
                2f.ToString(),
                "</style>秒，然后释放一个<style=cIsUtility>微型太阳</style>，持续<style=cIsDamage>",
                12.5f.ToString(),
                "</style>秒，使周围<style=cDeath>一切生物</style><style=cIsHealth>过热</style>（包括自己和队友）。在堆叠<style=cIsHealth>",
                2.ToString(),
                "</style>层或者更多时，目标会燃烧并造成<style=cIsDamage>",
                160f.ToString(),
                "%伤害</style>。"
            ]);
            "PALADIN_SPECIAL_SUN_NAME".AddOverlay("暴烈之阳", lang);
            "PALADIN_SPECIAL_SUN_DESCRIPTION".AddOverlay(text, lang);
            "PALADIN_SPECIAL_SUN_CANCEL_NAME".AddOverlay("取消暴烈之阳", lang);
            "PALADIN_SPECIAL_SUN_CANCEL_DESCRIPTION".AddOverlay("停止引导当前的暴烈之阳。", lang);
            text += "再次投掷并保持瞄准，然后释放太阳，爆炸对周围<style=cDeath>一切生物</style>造成<style=cIsDamage>4000%伤害</style>。".ToScepterDesc();
            "PALADIN_SPECIAL_SCEPSUN_NAME".AddOverlay("太阳耀斑", lang);
            "PALADIN_SPECIAL_SCEPSUN_DESCRIPTION".AddOverlay(text, lang);
            text = "<style=cIsUtility>引导</style><style=cIsDamage>5</style>秒，然后在指定位置创造一个<style=cIsUtility>微型太阳</style>，吸收周围<style=cIsHealth>所有</style>生物的<style=cIsDamage>生命</style> 。<color=red>让火焰净化一切！</color>";
            "PALADIN_SPECIAL_SUN_LEGACY_NAME".AddOverlay("暴烈之阳(经典)", lang);
            "PALADIN_SPECIAL_SUN_LEGACY_DESCRIPTION".AddOverlay(text, lang);
            text += "产生大规模爆炸，造成9000%伤害。".ToScepterDesc();
            "PALADIN_SPECIAL_SCEPSUN_LEGACY_NAME".AddOverlay("太阳耀斑", lang);
            "PALADIN_SPECIAL_SCEPSUN_LEGACY_DESCRIPTION".AddOverlay(text, lang);
            text = "While below <style=cIsHealth>25% health</style>, generate <style=cIsDamage>Rage</style>. When at max <style=cIsDamage>Rage</style>, use to enter <color=#dc0000>Berserker Mode</color>, gaining a <style=cIsHealing>massive buff</style> and a <style=cIsUtility>new set of skills</style>.";
            "PALADIN_SPECIAL_BERSERK_NAME".AddOverlay("狂暴", lang);
            "PALADIN_SPECIAL_BERSERK_DESCRIPTION".AddOverlay(text, lang);
            "KEYWORD_SWORDBEAM".AddOverlay("<style=cKeywordName>剑光</style><style=cSub>一道短距离可穿透的光束，造成<style=cIsDamage>300%伤害</style>.", lang);
            "KEYWORD_TORPOR".AddOverlay("<style=cKeywordName>麻木</style><style=cSub>造成<style=cIsHealth>60%</style>移动和攻击速度<style=cIsDamage>减缓</style>。<style=cIsHealth>将敌人拖至地面。</style>", lang);
            "KEYWORD_OVERHEAT".AddOverlay("<style=cKeywordName>过热</style><style=cSub>乘以从<style=cIsDamage>太阳</style>受到的伤害。</style>", lang);
            "PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("圣骑士的誓言", lang);
            "PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("使用“忠诚之珠”，再次变得完整。", lang);
            "PALADIN_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("圣骑士的誓言", lang);
            "PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("圣骑士：精通", lang);
            "PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，击败游戏或消灭季风。", lang);
            "PALADIN_MASTERYUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("圣骑士：精通", lang);
            string str = "\n<color=#8888>(台风困难需要星际风暴2模组)</color>";
            "PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("圣骑士：大师", lang);
            "PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，在台风或更高级别难度下通关或者抹除自己。" + str, lang);
            "PALADIN_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("圣骑士：大师", lang);
            "PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("他的弟子", lang);
            "PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，与腐败女神立约。（看不懂）", lang);
            "PALADIN_POISONUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("她的弟子", lang);
            "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("Jolly Cooperation", lang);
            "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，用皇家电容器打击敌人。<color=#c11>仅主机</color>", lang);
            "PALADIN_LIGHTNINGSPEARUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("Jolly Cooperation", lang);
            "PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("迷失国王的先驱", lang);
            "PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，一次持有8件月球物品。", lang);
            "PALADIN_LUNARSHARDUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("迷失国王的先驱", lang);
            "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("温暖的拥抱", lang);
            "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，用身形扭曲的木灵治疗一个盟友。<color=#c11>仅主机</color>", lang);
            "PALADIN_HEALUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("温暖的拥抱", lang);
            "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("抑制", lang);
            "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，对一个敌人堆叠4层debuff。", lang);
            "PALADIN_TORPORUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("抑制", lang);
            "PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("阳光", lang);
            "PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，承受太阳的全部冲击并生存下来。", lang);
            "PALADIN_CRUELSUNUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("阳光", lang);
            "PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("古代遗物", lang);
            "PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为圣骑士，获得一个泥沼之瓮。", lang);
            "PALADIN_CLAYUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("古代遗物", lang);
            "BROTHER_SEE_PALADIN_1".AddOverlay("兄弟？不，便宜的仿制品。", lang);
            "BROTHER_SEE_PALADIN_2".AddOverlay("我会对你的信仰负责。", lang);
            "BROTHER_SEE_PALADIN_3".AddOverlay("浪费潜力。", lang);
            "BROTHER_KILL_PALADIN_1".AddOverlay("你粗糙的盔甲让你失望了。", lang);
            "BROTHER_KILL_PALADIN_2".AddOverlay("看看你的信仰给你带来了什么。", lang);
            "BROTHER_KILL_PALADIN_3".AddOverlay("一无所获，愚蠢的奉献者。", lang);
        }

        private static void 探路者汉化() {
            "BOG_SQUALL_BODY_NAME".AddOverlay("狂风", lang);
            "BOG_PATHFINDER_BODY_NAME".AddOverlay("探路者", lang);
            "BOG_PATHFINDER_BODY_SUBTITLE".AddOverlay("猛禽", lang);
            "BOG_PATHFINDER_BODY_DEFAULT_SKIN_NAME".AddOverlay("默认", lang);
            "BOG_PATHFINDER_BODY_MASTERY_SKIN_NAME".AddOverlay("猎头者", lang);
            "BOG_PATHFINDER_BODY_PASSIVE_NAME".AddOverlay("驯鹰者", lang);
            "BOG_PATHFINDER_BODY_PASSIVE_DESCRIPTION".AddOverlay("一只机器猎鹰（狂风）将跟随你，它会继承你的<style=cIsDamage>大部分</style>物品，并且<style=cIsUtility>免疫</style>所有伤害，但它靠<style=cIsUtility>电池</style>运行。", lang);
            "KEYWORD_BATTERY".AddOverlay("<style=cKeywordName>电池</style><style=cSub>狂风有两种模式：<color=#FF0000>攻击</color>和<color=#00FF00>跟随目标</color>。在<color=#FF0000>攻击模式</color>下，狂风每秒<style=cIsDamage>消耗 8%</style>的电量。在<color=#00FF00>跟随模式</color>下，狂风每秒<style=cIsHealing>充能 1%</style>电量，速度随<style=cIsUtility>攻速</style>变换。如果电量耗尽，狂风将强制进入<color=#00FF00>跟随模式</color>。</style>", lang);
            "KEYWORD_PIERCE".AddOverlay("<style=cKeywordName>穿孔</style><style=cSub>用尖端<style=cIsUtility>攻击</style>造成<style=cIsDamage>325%伤害</style>并忽略<style=cIsDamage>敌人护甲</style>。</style>", lang);
            "KEYWORD_ELECTROCUTE".AddOverlay("<style=cKeywordName>电击</style><style=cSub>使目标移动速度降低50%，每秒造成<style=cIsDamage>120% 的伤害</style>。</style>", lang);
            "KEYWORD_ATTACK".AddOverlay("<style=cKeywordName><color=#FF0000>攻击</color></style><style=cSub>指挥狂风攻击敌人，并激活<color=#FF0000>攻击模式</color>，使用机枪造成<style=cIsDamage>2x50% 伤害</style>，发射导弹造成<style=cIsDamage>300% 伤害</style>。</style>", lang);
            "KEYWORD_FOLLOW".AddOverlay("<style=cKeywordName><color=#00FF00>跟随目标</color></style><style=cSub>让狂风回到身边，激活<color=#00FF00>跟随模式</color>，使狂风跟随在身边。</style>", lang);
            "KEYWORD_SQUALL_UTILITY".AddOverlay("<style=cKeywordName><color=#87b9cf>辅助</color></style><style=cSub>指挥狂风使用<style=cIsUtility>辅助</style>技能。</style>", lang);
            "KEYWORD_SQUALL_SPECIAL".AddOverlay("<style=cKeywordName><color=#efeb1c>特殊--追击!</color></style><style=cSub>指挥狂风反复打击目标敌人，造成<style=cIsDamage>70% 伤害</style>。每次攻击都会降低目标<style=cIsDamage>5点护甲</style> ，并<style=cIsUtility>充能 2%</style>电量，暴击时有双倍效果。此技能可将暂时将电量充能到<style=cIsUtility>120%</style>。</style>", lang);
            "BOG_PATHFINDER_BODY_PRIMARY_THRUST_NAME".AddOverlay("突刺", lang);
            "BOG_PATHFINDER_BODY_PRIMARY_THRUST_DESCRIPTION".AddOverlay("<style=cIsUtility>穿孔</style>。将你的长矛向前刺去，造成<style=cIsDamage>250% 伤害</style>。", lang);
            "BOG_PATHFINDER_BODY_SECONDARY_DASH_NAME".AddOverlay("旋风腿", lang);
            "BOG_PATHFINDER_BODY_SECONDARY_DASH_DESCRIPTION".AddOverlay("<style=cIsUtility>冲刺</style>。短距离冲刺后，将矛举起准备投掷。投掷矛将产生<style=cIsDamage>爆炸</style>造成<style=cIsDamage>1000% 伤害</style>。", lang);
            "BOG_PATHFINDER_BODY_SECONDARY_JAVELIN_NAME".AddOverlay("爆炸标枪", lang);
            "BOG_PATHFINDER_BODY_SECONDARY_JAVELIN_DESCRIPTION".AddOverlay("投掷<style=cIsDamage>爆炸性</style>标枪，造成<style=cIsDamage>1000% 伤害</style>。", lang);
            "BOG_PATHFINDER_BODY_UTILITY_SPIN_NAME".AddOverlay("撕裂之爪", lang);
            "BOG_PATHFINDER_BODY_UTILITY_BOLAS_NAME".AddOverlay("闪电套索", lang);
            "BOG_PATHFINDER_BODY_UTILITY_BOLAS_DESCRIPTION".AddOverlay("投掷带电的套索，<style=cIsUtility>电击</style>周围敌人，并产生<style=cIsUtility>电击</style>区域，存在<style=cIsUtility>10秒</style>。", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_COMMAND_NAME".AddOverlay("指令", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_COMMAND2_NAME".AddOverlay("指令 - 辅助", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_COMMAND_DESCRIPTION".AddOverlay("向狂风发出指令。你可以指挥狂风<color=#FF0000>攻击</color>，<color=#00FF00>跟随目标</color>或<color=#efeb1c>特殊指令</color>。", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_COMMAND2_DESCRIPTION".AddOverlay("<color=#3ea252>特殊指令</color>。狂风的<color=#efeb1c>特殊指令</color>现在替换了你的<style=cIsUtility>特殊</style>技能。", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_ATTACK_NAME".AddOverlay("攻击 - 指令", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_ATTACK_DESCRIPTION".AddOverlay("</style><style=cSub>指挥狂风攻击敌人，并激活<color=#FF0000>攻击模式</color>，使用机枪造成<style=cIsDamage>2x50%伤害</style>，发射导弹造成<style=cIsDamage>300% 伤害</style>。</style>", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_FOLLOW_NAME".AddOverlay("跟随目标 - 指令", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_FOLLOW_DESCRIPTION".AddOverlay("<style=cSub>让狂风回到身边，激活<color=#00FF00>跟随模式</color>，使狂风跟随在身边。</style>", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_CANCEL_NAME".AddOverlay("取消", lang);
            "BOG_PATHFINDER_BODY_SPECIAL_CANCEL_DESCRIPTION".AddOverlay("取消", lang);
            "BOG_SQUALL_SPECIAL_GOFORTHROAT_NAME".AddOverlay("追击!", lang);
            "BOG_SQUALL_SPECIAL_GOFORTHROAT_DESCRIPTION".AddOverlay("</style><style=cSub>指挥狂风反复打击目标敌人，造成<style=cIsDamage>70% 伤害</style>。每次攻击都会降低目标<style=cIsDamage>5点护甲</style> ，并<style=cIsUtility>充能 2%</style>电量，暴击时有双倍效果。此技能可将暂时将电量充能到<style=cIsUtility>120%</style>。</style>", lang);
            "BOG_PATHFINDER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("Pathfinder.：精通", lang);
            "BOG_PATHFINDER_BODY_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为探路者，在季风难度下通关或抹除自己。", lang);
            "BOG_PATHFINDER_BODY_MASTERYUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("Pathfinder.：精通", lang);
            "BOG_PATHFINDER_BODY_DESCRIPTION".AddOverlay($"探路者是一位灵活的、善于使用游击战术的游侠，通常与他可靠的猎鹰伙伴——狂风一同作战。<color=#CCD3E0>\n\n< ! > 与其他近战角色相比，你更加脆弱。好好利用你的移动手段、远距离攻击以及你的猎鹰狂风来分散敌人的注意力以生存下去。\n\n< ! > 轮流使用旋风腿和标枪投掷来投出尽可能多的标枪，但请确保至少留有一次充能以在必要时进行闪避。\n\n< ! > 闪电套索是一次性封锁大群敌人的好方法，同时如果有增加滞空时间的道具，撕裂之爪可以提供巨额伤害。\n\n< ! > 狂风是非常强大的伙伴，你可以在任何时候对他下令。优先考虑增加攻击速度和暴击几率以帮助更快充能。</color>", lang);
        }

        private static void 掠夺者汉化() {
            //"ROB_RAVAGER_BODY_NAME".AddOverlay("掠夺者", zh_CN);
            "ROB_RAVAGER_BODY_DEFAULT_SKIN_NAME".AddOverlay("默认", lang);
            "ROB_RAVAGER_BODY_MONSOON_SKIN_NAME".AddOverlay("虚空触碰", lang);
            "ROB_RAVAGER_BODY_TYPHOON_SKIN_NAME".AddOverlay("???", lang);
            "ROB_RAVAGER_BODY_VOID_SKIN_NAME".AddOverlay("虚空诞生", lang);
            "ROB_RAVAGER_BODY_MAHORAGA_SKIN_NAME".AddOverlay("神圣", lang);
            "ROB_RAVAGER_BODY_MINECRAFT_SKIN_NAME".AddOverlay("我的世界", lang);
            //"ROB_RAVAGER_BODY_BLOODWELL_NAME".AddOverlay("血井", zh_CN);
            "ROB_RAVAGER_BODY_BLOODWELL_DESCRIPTION".AddOverlay($"掠夺者每获得1点{"浸剂".ToHealing()}的{"生命值奖励".ToHealing()}将增加{RavagerTweak.InfusionToDamageCoefficient}点{"基础伤害".ToDmg()}。掠夺者在击中敌人时会储存<style=cIsHealth>血液</style>，填满{"血井".ToHealth()}后进入{"鲜血迸发".ToHealth()}，{"恢复".ToHealing()}自身<style=cIsHealing>75%的失去的生命值</style>并<style=cIsDamage>暂时强化你的技能</style>。", lang);
            //"ROB_RAVAGER_BODY_BLOODWELL2_NAME".AddOverlay("灵液罐", zh_CN);
            "ROB_RAVAGER_BODY_BLOODWELL2_DESCRIPTION".AddOverlay($"掠夺者每获得1点{"浸剂".ToHealing()}的{"生命值奖励".ToHealing()}将增加{RavagerTweak.InfusionToDamageCoefficient}点{"基础伤害".ToDmg()}。掠夺者在击中敌人时会储存<style=cIsHealth>血液</style>，填满{"灵液罐".ToHealth()}后进入{"灵液迸发".ToHealth()}，{"恢复".ToHealing()}自身<style=cIsHealing>100%的最大生命值</style>并<style=cIsDamage>暂时强化你的技能</style>。<style=cIsHealth>攻击时消耗速度更快。</style>", lang);
            //"ROB_RAVAGER_BODY_PASSIVE_NAME".AddOverlay("体能", zh_CN);
            "ROB_RAVAGER_BODY_PASSIVE_DESCRIPTION".AddOverlay("掠夺者可以<style=cIsUtility>蹬墙</style>和<style=cIsHealth>踩头</style>。<style=cIsUtility><style=cIsDamage>蓄力</style>蹬墙</style>将跳得更远。", lang);
            //"ROB_RAVAGER_BODY_PASSIVE2_NAME".AddOverlay("扭曲突变", zh_CN);
            //"ROB_RAVAGER_BODY_PASSIVE2_DESCRIPTION".AddOverlay("<style=cIsHealth>消耗10%的生命值。</style>在空中跳跃将<style=cIsUtility>向前突进</style>一段距离。落地或<style=cIsDamage>近战击中敌人</style>可刷新此能力。", zh_CN);
            //"ROB_RAVAGER_BODY_CONFIRM_NAME".AddOverlay("确认", zh_CN);
            //"ROB_RAVAGER_BODY_CONFIRM_DESCRIPTION".AddOverlay("继续当前技能。", zh_CN);
            //"ROB_RAVAGER_BODY_CANCEL_NAME".AddOverlay("取消", zh_CN);
            //"ROB_RAVAGER_BODY_CANCEL_DESCRIPTION".AddOverlay("取消当前技能。", zh_CN);
            //"ROB_RAVAGER_BODY_PRIMARY_SLASH_NAME".AddOverlay("挥舞", zh_CN);
            "ROB_RAVAGER_BODY_PRIMARY_SLASH_DESCRIPTION".AddOverlay($"向前挥舞武器造成{Slash._damageCoefficient.ToDmgPct("_的伤害")}。在<style=cIsUtility>蹬墙</style>时可进入<style=cIsDamage>蓄力姿态</style>。", lang);
            //"ROB_RAVAGER_BODY_PRIMARY_SLASHCOMBO_NAME".AddOverlay("解体", zh_CN);
            "ROB_RAVAGER_BODY_PRIMARY_SLASHCOMBO_DESCRIPTION".AddOverlay($"向前挥舞武器造成{SlashCombo._damageCoefficient.ToDmgPct("_的伤害")}。第三次攻击将<style=cIsUtility>眩晕</style>并造成{SlashCombo.finisherDamageCoefficient.ToDmgPct("_的伤害")}.", lang);
            //"ROB_RAVAGER_BODY_SECONDARY_SPINSLASH_NAME".AddOverlay("劈裂", zh_CN);
            "ROB_RAVAGER_BODY_SECONDARY_SPINSLASH_DESCRIPTION".AddOverlay($"向前跃起，进行<style=cIsUtility>大范围</style>斩击造成{SpinSlash._damageCoefficient.ToDmgPct("_的伤害")}。", lang);
            //"ROB_RAVAGER_BODY_UTILITY_BEAM_NAME".AddOverlay("吞噬", zh_CN);
            "ROB_RAVAGER_BODY_UTILITY_BEAM_DESCRIPTION".AddOverlay($"<style=cIsHealth>吞噬</style>来袭的<style=cIsUtility>弹幕</style>为一发{"毁灭轰击".ToDeath()}充能，可造成{ChargeBeam.minDamageCoefficient.ToDmgPct("_-")}{(ChargeBeam.maxDamageCoefficient * 0.5f).ToDmgPct("_的伤害")}，一半充能以上时轰击将转化为造成{FireBeam.damageCoefficientPerSecond.ToDmgPct("每秒_伤害的持续性激光")}。", lang);
            //"ROB_RAVAGER_BODY_UTILITY_HEAL_NAME".AddOverlay("凝结", zh_CN);
            //"ROB_RAVAGER_BODY_UTILITY_HEAL_DESCRIPTION".AddOverlay("立即排空<style=cIsHealth>血液</style>用于<style=cIsHealing>治疗自身</style>。", zh_CN);
            //"ROB_RAVAGER_BODY_UTILITY_SWAP_NAME".AddOverlay("Boogie Woogie", zh_CN);
            //"ROB_RAVAGER_BODY_UTILITY_SWAP_DESCRIPTION".AddOverlay("与<style=cIsUtility>任意实体</style>交换位置。", zh_CN);
            //"ROB_RAVAGER_BODY_UTILITY_SNATCH_NAME".AddOverlay("抓取", zh_CN);
            "ROB_RAVAGER_BODY_UTILITY_SNATCH_DESCRIPTION".AddOverlay("向前伸出你的<style=cIsDamage>麒麟臂</style>抓住敌人，并<style=cIsUtility>将你拉向目标物体</style>。", lang);
            //"ROB_RAVAGER_BODY_SPECIAL_TRANSFIGURE_NAME".AddOverlay("狂暴", zh_CN);
            //"ROB_RAVAGER_BODY_SPECIAL_GRAB_NAME".AddOverlay("狂暴", zh_CN);
            "ROB_RAVAGER_BODY_SPECIAL_GRAB_DESCRIPTION".AddOverlay($"向前跃进<style=cIsUtility>抓住</style>敌人，然后猛扑造成{DashGrab.groundSlamDamageCoefficient.ToDmgPct("_的伤害")}。如果击杀敌人，则<style=cIsHealth>消耗</style>它们以{"恢复".ToHealing()}自身<style=cIsHealing>15%的最大生命值</style>。", lang);
            //"ROB_RAVAGER_BODY_SPECIAL_GRAB_SCEPTER_NAME".AddOverlay("野蛮冲锋", zh_CN);
            "ROB_RAVAGER_BODY_SPECIAL_GRAB_SCEPTER_DESCRIPTION".AddOverlay($"向前跃进<style=cIsUtility>抓住</style>敌人，然后猛扑造成{DashGrab.groundSlamDamageCoefficient.ToDmgPct("_的伤害")}。如果击杀敌人，则<style=cIsHealth>消耗</style>它们以{"恢复".ToHealing()}自身<style=cIsHealing>10%的最大生命值</style>。{"强制抓取".ToScepterDesc()}", lang);
            "KEYWORD_REDGUY_M12".AddOverlay($"<style=cKeywordName>支撑点</style><style=cSub>在蹬墙跳蓄能时抓住 <style=cIsUtility>支撑点</style>。释放后向前{"横扫".ToDmg()}，至少造成{Slash._damageCoefficient.ToDmgPct()}的伤害，根据斩击时的移动速度提升伤害。", lang);
            //"KEYWORD_REDGUY_M1".AddOverlay("<style=cKeywordName>强化效果</style><style=cSub>挥舞得更快。", zh_CN);
            //"KEYWORD_REDGUY_M2".AddOverlay($"<style=cKeywordName>强化效果</style><style=cSub>跃进速度提升，并对{"低生命".ToHealth()}的敌人造成{"更多伤害".ToDmg()}。", zh_CN);
            "KEYWORD_REDGUY_HEAL".AddOverlay($"<style=cKeywordName>强化效果</style><style=cSub>产生{"鲜血爆炸".ToHealth()}造成{Heal.maxDamageCoefficient.ToDmgPct("_的伤害")}。", lang);
            //"KEYWORD_REDGUY_BEAM".AddOverlay("<style=cKeywordName>强化效果</style><style=cSub>充能更快。", zh_CN);
            //"KEYWORD_REDGUY_GRAB".AddOverlay($"<style=cKeywordName>强化效果</style><style=cSub>可以抓取更大的目标，并且将目标{"按在地上摩擦".ToDeath()}。", zh_CN);
            //"KEYWORD_REDGUY_GRAB2".AddOverlay("<style=cKeywordName>Bosses</style><style=cSub>可打击Boss，<style=cIsDamage>击打</style>它们造成相同的伤害。", zh_CN);
            //"ROB_RAVAGER_BODY_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("把这一切抛在脑后的人", zh_CN);
            //"ROB_RAVAGER_BODY_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("把这一切抛在脑后的人", zh_CN);
            //"ROB_RAVAGER_BODY_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("对一名敌人施加50层流血。", zh_CN);
            //"ROB_RAVAGER_BODY_MONSOONUNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：精通", zh_CN);
            //"ROB_RAVAGER_BODY_MONSOONUNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：精通", zh_CN);
            //"ROB_RAVAGER_BODY_MONSOONUNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，在季风难度下通关游戏或抹除自己。", zh_CN);
            //"ROB_RAVAGER_BODY_TYPHOON_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：大师", zh_CN);
            //"ROB_RAVAGER_BODY_TYPHOON_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：大师", zh_CN);
            //"ROB_RAVAGER_BODY_TYPHOON_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，在台风或日食难度下通关游戏或抹除自己。\n<color=#8888>(台风或者更高难度)</color>", zh_CN);
            //"ROB_RAVAGER_BODY_THROW_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：在火焰轨迹中", zh_CN);
            //"ROB_RAVAGER_BODY_THROW_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：在火焰轨迹中", zh_CN);
            //"ROB_RAVAGER_BODY_THROW_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，在一次抓取中将敌人猛砸到地上5次以上。", zh_CN);
            //"ROB_RAVAGER_BODY_BEAM_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：平静如海", zh_CN);
            //"ROB_RAVAGER_BODY_BEAM_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：平静如海", zh_CN);
            //"ROB_RAVAGER_BODY_BEAM_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，完成一个关卡而不填充血井。", zh_CN);
            "ROB_RAVAGER_BODY_WALLJUMP_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：吉吉国王", lang);
            "ROB_RAVAGER_BODY_WALLJUMP_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：吉吉国王", lang);
            "ROB_RAVAGER_BODY_WALLJUMP_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，在不落地的情况下跳了20次。", lang);
            //"ROB_RAVAGER_BODY_SUIT_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：我所有的伙伴", zh_CN);
            //"ROB_RAVAGER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：我所有的伙伴", zh_CN);
            //"ROB_RAVAGER_BODY_SUIT_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，同时拥有15个盟友。", zh_CN);
            //"ROB_RAVAGER_BODY_VOID_UNLOCKABLE_UNLOCKABLE_NAME".AddOverlay("掠夺者：从无到有", zh_CN);
            //"ROB_RAVAGER_BODY_VOID_UNLOCKABLE_ACHIEVEMENT_NAME".AddOverlay("掠夺者：从无到有", zh_CN);
            //"ROB_RAVAGER_BODY_VOID_UNLOCKABLE_ACHIEVEMENT_DESC".AddOverlay("作为掠夺者，逃离天文馆。", zh_CN);
        }

        private static void 象征汉化() {
            // TPDespair.ZetAspects.Language.targetLanguage = zh_CN;
            // TPDespair.ZetAspects.Language.tokens[zh_CN]["ITEM_SHIELDONLY_DESC"] +=
            // $"\n{基础护盾百分比再生速度}增加{0.01f.ToBaseAndStkPct().ToHealing("_hp/s")}"; var oldStr =
            // TPDespair.ZetAspects.Language.tokens[zh_CN][Catalog.Item.ZetAspectRed.descriptionToken];
            // TPDespair.ZetAspects.Language.tokens[zh_CN][Catalog.Item.ZetAspectRed.descriptionToken]
            // =
            // oldStr.Replace(TPDespair.ZetAspects.Language.SecondText(Configuration.AspectRedBurnDuration.Value,
            // "over"), ""); oldStr =
            // TPDespair.ZetAspects.Language.tokens[zh_CN][Catalog.Equip.AffixRed.descriptionToken];
            // TPDespair.ZetAspects.Language.tokens[zh_CN][Catalog.Equip.AffixRed.descriptionToken]
            // =
            // oldStr.Replace(TPDespair.ZetAspects.Language.SecondText(Configuration.AspectRedBurnDuration.Value,
            // "over"), "");
        }

        private static async void OnMainMenuFirstInitialised() {
            MainMenuController.OnMainMenuInitialised -= OnMainMenuFirstInitialised;
            await Task.Run(基础汉化);
            await Task.Run(象征汉化);
            await Task.Run(权杖技能汉化);
            //await Task.Run(圣骑士汉化);
            await Task.Run(探路者汉化);
            await Task.Run(掠夺者汉化);
        }
    }
}