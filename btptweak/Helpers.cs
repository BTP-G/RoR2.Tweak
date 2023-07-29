using HIFUArtificerTweaks.Skills;
using HIFUCaptainTweaks.Skills;
using HIFUCommandoTweaks.Skills;
using HIFUEngineerTweaks.Skills;
using HIFUHuntressTweaks.Skills;
using HIFULoaderTweaks.Skills;
using HIFUMercenaryTweaks.Skills;
using HIFURailgunnerTweaks.Misc;
using HIFURailgunnerTweaks.Skills;
using HIFURexTweaks.Skills;
using RoR2;
using static R2API.LanguageAPI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;

namespace BtpTweak {

    public class Helpers {
        private static readonly List<LanguageOverlay> languageOverlays = new();

        public static void Log(string loginfo) {
            BtpTweak.logger_.LogInfo(loginfo);
        }

        public static string ScepterDescription(string desc) {
            return "<color=#d299ff>权杖：" + desc + "</color>";
        }

        public static string ScepterToken(string desc) {
            return desc;
        }

        public static float d(float damage) {
            return 100 * damage;
        }

        public static CharacterBody GetClosestCharacterBody(ReadOnlyCollection<TeamComponent> teamMembers, Vector3 location) {
            CharacterBody result = null;
            float num = float.MaxValue;
            foreach (TeamComponent teamComponent in teamMembers) {
                float num2 = (teamComponent.body.corePosition - location).sqrMagnitude;
                if (num2 != 0 && num2 < num) {
                    result = teamComponent.body;
                    num = num2;
                }
            }
            return result;
        }

        public static CharacterBody GetClosestPlayerCharacterBody(ReadOnlyCollection<TeamComponent> teamMembers, Vector3 location) {
            CharacterBody result = null;
            float num = float.MaxValue;
            foreach (TeamComponent teamComponent in teamMembers) {
                if (teamComponent.body.isPlayerControlled) {
                    float num2 = (teamComponent.body.corePosition - location).sqrMagnitude;
                    if (num2 != 0 && num2 < num) {
                        result = teamComponent.body;
                        num = num2;
                    }
                }
            }
            return result;
        }

        public static void AddTag(in ItemDef itemDef, ItemTag itemTag) {
            if (itemDef.DoesNotContainTag(itemTag)) {
                List<ItemTag> itemTags = new();
                for (int i = 0; i < itemDef.tags.Length; ++i) {
                    itemTags.Add(itemDef.tags[i]);
                }
                itemTags.Add(itemTag);
                itemDef.tags = itemTags.ToArray();
                itemTags.Clear();
            } else {
                BtpTweak.logger_.LogInfo($"{itemDef.name} contain tag {itemTag}, no need to add.");
            }
        }

        public static void 初始汉化() {
            //AddOverlay("VOIDCRID_ENTROPY", "<style=cArtifact>「虚混<style=cIsHealing>无</style>乱』</style>", "zh-CN");
            //AddOverlay("VOIDCRID_FLAMEBREATH", "火焰吐息", "zh-CN");
            //AddOverlay("VOIDCRID_FLAMEBREATH_DESC", "<style=cDeath>点燃</style>。<style=cIsDamage>灵巧</style>。向前方喷出<style=cIsDamage>火焰</style>，<style=cDeath>点燃</style>敌人，造成<style=cIsDamage>250%</style>的伤害。", "zh-CN");
            //AddOverlay("VOIDCRID_SCEPTER_ENTROPY", "<style=cArtifact>「幻影? 虚<style=cIsHealing>乱</style>无混』</style>", "zh-CN");
            AddOverlay("GOLDENKNURL_NAME", "<color=yellow>黄金隆起</color>", "zh-CN");
            AddOverlay("GOLDENKNURL_PICKUP", "增加最大生命值、基础生命值再生速度和护甲。", "zh-CN");
            AddOverlay("GOLDENKNURL_DESC", $"最大生命值增加<style=cIsHealing>{100 * GoldenCoastPlus.GoldenCoastPlus.KnurlHealth.Value}%</style><style=cStack>（每层+{100 * GoldenCoastPlus.GoldenCoastPlus.KnurlHealth.Value}）</style>，<style=cIsHealing>基础生命值再生</style>增加<style=cIsHealing>{GoldenCoastPlus.GoldenCoastPlus.KnurlRegen.Value}hp/s</style><style=cStack>（每层+{GoldenCoastPlus.GoldenCoastPlus.KnurlRegen.Value}hp/s）</style>，<style=cIsHealing>基础生命值<color=yellow>百分比</color>再生速度</style>增加<style=cIsHealing>1%<style=cStack>（每层+1%；每有100,000,000金钱+1%）</style>hp/s</style>，<style=cIsUtility>护甲</style>增加<style=cIsUtility>{GoldenCoastPlus.GoldenCoastPlus.KnurlArmor.Value}点</style><style=cStack>（每层+{GoldenCoastPlus.GoldenCoastPlus.KnurlArmor.Value}点）</style>。", "zh-CN");
            //AddOverlay("GoldenKnurl_LORE ", "", "zh-CN");
            AddOverlay("CAPTAIN_PRIMARY_DESCRIPTION", "喷射一大团弹丸，造成<style=cIsDamage>8x120%的伤害</style>。为攻击充能将缩小<style=cIsUtility>扩散范围</style>。<style=cStack>（每层完美巨兽+100%击退）</style>", "zh-CN");
            AddOverlay("CAPTAIN_SECONDARY_DESCRIPTION", $"<style=cIsDamage>震荡</style>。发射一枚造成<style=cIsDamage>100%伤害</style>的快速电镖，爆炸时<style=cIsUtility>电击</style>周围最多30名敌人。如果弹射将能飞行到更远地点。", "zh-CN");
            AddOverlay("CAPTAIN_SECONDARY_NAME", "能量电镖", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_EQUIPMENT_RESTOCK_DESCRIPTION", $"使用时<style=cIsUtility>为装备充能</style>。范围内的友方攻击速度<style=cIsUtility>提升{100 * ResupplyBeacon.AttackSpeed}%</style>，技能冷却<style=cIsUtility>减少{100 * ResupplyBeacon.CooldownReduction}%</style>。", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_EQUIPMENT_RESTOCK_NAME", "信标：补给", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_HACKING_DESCRIPTION", $"<style=cIsUtility>破解</style>附近所有可购买物品，这些物品的价格将逐渐下降至<style=cIsUtility>$0</style>。", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_HEAL_DESCRIPTION", $"每秒为附近所有友方<style=cIsHealing>恢复</style>等同于各个角色<style=cIsHealing>最大生命值</style><style=cIsHealing>10%<style=cStack>（每升一级+1%）</style></style>的生命值。", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_HEAL_NAME", "信标：治疗", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_SHOCKING_DESCRIPTION", $"间歇性<style=cIsDamage>震荡</style>所有附近的敌人，使其无法移动。", "zh-CN");
            AddOverlay("CAPTAIN_SUPPLY_SHOCKING_NAME", "信标：震荡", "zh-CN");
            AddOverlay("CAPTAIN_UTILITY_ALT1_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。向<style=cIsDamage>UES顺风号</style>请求发动一次<style=cIsDamage>动能打击</style>。在<style=cIsUtility>{OGM72DIABLOStrike.TimeToLand}秒后</style>，对所有角色造成<style=cIsDamage>{d(OGM72DIABLOStrike.Damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("CAPTAIN_UTILITY_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。向<style=cIsDamage>UES顺风号</style>请求至多<style=cIsDamage>3台</style>轨道探测器。每台探测器将造成<style=cIsDamage>{d(OrbitalProbe.Damage)}%伤害</style>。", "zh-CN");
            AddOverlay("COMMANDO_HEAVYTAP_DESCRIPTION", "<style=cIsDamage>绝对光滑</style>。射击两次，造成<style=cIsDamage>2x155%的伤害</style>。", "zh-CN");
            AddOverlay("COMMANDO_HEAVYTAP_NAME", "沉重双击", "zh-CN");
            AddOverlay("COMMANDO_PLASMATAP_DESCRIPTION", "<style=cIsDamage>击穿</style>。发射一道锥形闪电，造成<style=cIsDamage>100%的伤害</style>。", "zh-CN");
            AddOverlay("COMMANDO_PLASMATAP_NAME", "电弧子弹", "zh-CN");
            AddOverlay("COMMANDO_PRFRVWILDFIRESTORM_DESCRIPTION", "发射一股火焰，每秒造成<style=cIsDamage>550%的伤害</style>，并有机会<style=cIsDamage>点燃</style>敌人。", "zh-CN");
            AddOverlay("COMMANDO_PRFRVWILDFIRESTORM_NAME", "PRFR-V野火风暴", "zh-CN");
            AddOverlay("COMMANDO_SECONDARY_DESCRIPTION", $"发射一枚<style=cIsDamage>穿甲</style>弹，造成<style=cIsDamage>{d(PhaseRound.Damage)}%的伤害</style>。每次穿透敌人，造成的伤害提高<style=cIsDamage>40%</style>。", "zh-CN");
            AddOverlay("COMMANDO_SPECIAL_ALT1_DESCRIPTION", $"扔出一枚手雷，爆炸可造成<style=cIsDamage>{d(FragGrenade.Damage)}%的伤害</style>。最多可投掷2枚。", "zh-CN");
            AddOverlay("COMMANDO_SPECIAL_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。连续射击，每枚弹丸造成<style=cIsDamage>{d(SuppressiveFire.Damage)}%的伤害</style>。射击次数随攻击速度增加。", "zh-CN");
            AddOverlay("ENGI_PRIMARY_DESCRIPTION", $"发射<style=cIsDamage>{BouncingGrenades.maximumGrenadesCount}</style>颗手雷，每颗均可造成<style=cIsDamage>{d(BouncingGrenades.damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("ENGI_SECONDARY_DESCRIPTION", $"放置一枚二阶段地雷，能够造成<style=cIsDamage>300%的伤害</style>，或在完全引爆时造成<style=cIsDamage>{Mathf.Round(300f * PressureMines.damageScale)}%的伤害</style>。最多放置{PressureMines.charges}枚<style=cStack>（每级+1枚）</style>。", "zh-CN");
            AddOverlay("ENGI_SKILL_HARPOON_DESCRIPTION", $"进入<style=cIsUtility>目标标记模式</style>以发射热追踪鱼叉导弹，每发造成<style=cIsDamage>{d(ThermalHarpoons.damage)}%的伤害</style>。最多可储存{ThermalHarpoons.charges}发。", "zh-CN");
            AddOverlay("ENGI_SPIDERMINE_DESCRIPTION", $"放置一枚机器人地雷，在敌人走近时自动引爆，造成<style=cIsDamage>{d(SpiderMines.damage)}%的伤害</style>，最多放置{SpiderMines.charges}枚<style=cStack>（每级+1枚）</style>。", "zh-CN");
            AddOverlay("ENGI_UTILITY_DESCRIPTION", $"放置一个<style=cIsUtility>无法穿透且有击退力的护盾</style>来阻挡弹幕{((BubbleShield.damage > 0f) ? "，并且在击退时造成<style=cIsDamage>" + d(BubbleShield.damage / BubbleShield.ticks) + "</style>%伤害" : "")}。每个盾需要消耗{BubbleShield.chargesToConsume}层充能，存在<style=cIsUtility>{BubbleShield.duration}秒</style>。<style=cIsUtility>护盾</style>展开时为每个友方单位提供一个持续<style=cIsUtility>6秒</style>的<style=cIsUtility>个人护盾</style>。", "zh-CN");
            AddOverlay("HAT_MAGE_UTILITY_FIRE_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。<style=cIsDamage>点燃</style>。向前冲刺，在身后召唤造成每秒<style=cIsDamage>{d(HIFUArtificerTweaks.Main.flamewallDamage.Value)}%伤害的火柱</style>。", "zh-CN");
            AddOverlay("HAT_MAGE_UTILITY_FIRE_NAME", "火墙", "zh-CN");
            AddOverlay("HUNTRESS_PRIMARY_ALT_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。瞄准60米<style=cStack>（每升一级+{BtpTweak.女猎人射程每级增加距离_.Value}米）</style>内的敌人，拉弓射出<style=cIsDamage>{Flurry.minArrows}枚</style>跟踪箭，每枚造成<style=cIsDamage>{d(Flurry.damage)}%的伤害</style>。如果暴击则发射<style=cIsDamage>{Flurry.maxArrows}</style>枚跟踪箭。", "zh-CN");
            AddOverlay("HUNTRESS_PRIMARY_DESCRIPTION", $"<style=cIsUtility>灵巧</style>。瞄准60米<style=cStack>（每升一级+{BtpTweak.女猎人射程每级增加距离_.Value}米）</style>内的敌人，快速射出一枚能够造成<style=cIsDamage>{d(Strafe.damage)}%伤害</style>的跟踪箭。", "zh-CN");
            AddOverlay("HUNTRESS_SECONDARY_DESCRIPTION", $"{(LaserGlaive.agile ? "<style=cIsUtility>灵巧</style>。" : "")}投掷一把追踪月刃，可弹射最多<style=cIsDamage>{LaserGlaive.bounceCount}</style>次，初始造成<style=cIsDamage>{d(LaserGlaive.damage)}%的伤害</style>，每次弹射伤害增加<style=cIsDamage>{Math.Round((double)((LaserGlaive.bounceDamage - 1f) * 100f), 1)}%</style>。", "zh-CN");
            AddOverlay("HUNTRESS_SPECIAL_ALT1_DESCRIPTION", $"向后<style=cIsUtility>传送</style>至空中。最多发射<style=cIsDamage>{Ballista.boltCount}</style>道能量闪电，造成<style=cIsDamage>{Ballista.boltCount}x{d(Ballista.damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("HUNTRESS_SPECIAL_DESCRIPTION", $"<style=cIsUtility>传送</style>至空中，向目标区域射下箭雨，使区域内所有敌人<style=cIsUtility>减速</style>，并造成<style=cIsDamage>每秒{300f * ArrowRain.damage}%的伤害</style>。", "zh-CN");
            AddOverlay("ITEM_AbyssalMedkit_DESCRIPTION", "<style=cIsUtility>消耗品</style>，抵挡<style=cIsUtility>10次</style>减益后失效。每一次抵挡都有<style=cIsHealing>10%</style>概率给予你<style=cIsHealing>“祝·福”</style>。每个<style=cIsHealing>祝福</style>可使你<style=cIsUtility>所有属性提升3%</style>。<style=cIsVoid>使所有医疗包无效化</style>", "zh-CN");
            AddOverlay("ITEM_AbyssalMedkit_PICKUP", "消耗品，可以替你抵挡10次减益，每一次抵挡都有概率给予你“祝·福”", "zh-CN");
            AddOverlay("ITEM_HEADHUNTER_DESC", "获得所击败精英怪物身上的<style=cIsDamage>能力</style>，持续<style=cIsDamage>20秒</style><style=cStack>（每层增加10秒）</style>。", "zh-CN");
            AddOverlay("ITEM_INFUSION_DESC", $"每击败一名敌人，即可<style=cIsHealing>永久性</style>增加<style=cIsHealing>自身等级x层数</style>点生命值，最多增加<style=cIsHealing>自身等级x100x层数</style>点生命值。\n<style=cIsUtility>此物品不会被米斯历克斯拿走</style>。", "zh-CN");
            AddOverlay("KEYWORD_ARC", "<style=cKeywordName>击穿</style><style=cSub>在最多4个敌人之间形成电弧，每次造成30%的伤害。</style>", "zh-CN");
            AddOverlay("KEYWORD_FLEETING", "<style=cKeywordName>一闪</style><style=cSub><style=cIsDamage>攻速</style>转化为<style=cIsDamage>技能伤害</style>。", "zh-CN");
            AddOverlay("KEYWORD_FRICTIONLESS", "<style=cKeywordName>绝对光滑</style><style=cSub>无伤害衰减</style>。", "zh-CN");
            AddOverlay("KEYWORD_SOULROT", "<style=cKeywordName>灵魂之痛</style><style=cSub>每秒<style=cIsVoid>至少</style>造成敌人<style=cIsHealing>最大生命值</style>的<style=cIsVoid>2.5%</style>的伤害，持续<style=cIsDamage>20秒<style=cStack>（每层权杖+10秒）</style></style>后消失。</style>", "zh-CN");
            AddOverlay("KEYWORD_VERY_HEAVY", "<style=cKeywordName>超重</style><style=cSub>下落速度越快，技能造成的伤害越高。", "zh-CN");
            AddOverlay("LOADER_SPECIAL_ALT_DESCRIPTION", $"<style=cIsUtility>超重</style>。用重拳砸向地面，造成<style=cIsDamage>{d(Thunderslam.damage)}%</style>的伤害。", "zh-CN");
            AddOverlay("LOADER_SPECIAL_DESCRIPTION", $"扔出飘浮电塔，可<style=cIsDamage>电击</style>周围<style=cIsDamage>{M551Pylon.aoe}</style>米<style=cStack>（每层不稳定的特斯拉线圈+35米）</style>内最多<style=cIsDamage>3</style>名敌人，电流最多可弹射{M551Pylon.bounces}次<style=cStack>（每层不稳定的特斯拉线圈+1次）</style>，造成<style=cIsDamage>{d(M551Pylon.damage)}%的伤害</style>。可被<style=cIsUtility>格挡</style>。", "zh-CN");
            AddOverlay("MAGE_PRIMARY_FIRE_DESCRIPTION", "<style=cIsDamage>点燃</style>。发射一枚火炎弹，造成<style=cIsDamage>220%<style=cStack>（每层贾罗的手环+100%）</style>的伤害</style>。</style>", "zh-CN");
            AddOverlay("MAGE_PRIMARY_LIGHTNING_DESCRIPTION", $"发射一道闪电，造成<style=cIsDamage>300%的伤害</style>并<style=cIsDamage>引爆</style>小片区域。</style>", "zh-CN");
            AddOverlay("MAGE_SECONDARY_ICE_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。使用拥有<style=cIsDamage>穿透</style>效果的纳米枪发动攻击，充能后能造成<style=cIsDamage>{d(CastNanoSpear.minDamage)}%-{d(CastNanoSpear.maxDamage)}%</style>的伤害，爆炸时留下一个<style=cIsUtility>冰冻炸弹</style>，2秒后爆炸，造成纳米枪<style=cIsDamage>一半的伤害</style>并<style=cIsUtility>冰冻</style>附件6米<style=cStack>（每层鲁纳德的手环+1米）</style>的敌人，暴击时<style=cIsUtility>冰冻</style>范围翻倍。", "zh-CN");
            AddOverlay("MAGE_SECONDARY_LIGHTNING_DESCRIPTION", $"<style=cIsDamage>眩晕</style>。发射一枚会<style=cIsDamage>爆炸并分裂成0<style=cStack>（每4级+1）</style>颗闪电球</style>的纳米炸弹，如果充能将造成<style=cIsDamage>{d(ChargedNanoBomb.minDamage)}%-{d(ChargedNanoBomb.maxDamage)}%</style>的伤害（每颗闪电球造成<style=cIsDamage>一半伤害</style>）。", "zh-CN");
            AddOverlay("MAGE_SPECIAL_FIRE_DESCRIPTION", $"<style=cIsDamage>点燃</style>。灼烧面前的所有敌人，对其造成<style=cIsDamage>{d(Flamethrower.Damage + Flamethrower.Damage / (1f / (Flamethrower.BurnChance / 100f)))}%的伤害</style>。", "zh-CN");
            AddOverlay("MAGE_UTILITY_ICE_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。创造一道能够对敌人造成<style=cIsDamage>{Snapfreeze.damage}%伤害</style>的屏障。", "zh-CN");
            AddOverlay("MERC_PRIMARY_DESCRIPTION", "<style=cIsDamage>一闪</style>。<style=cIsUtility>灵巧</style>。向前挥砍并造成<style=cIsDamage>130%的伤害</style>。第三次攻击的范围将会变大并<style=cIsUtility>暴露</style>敌人。", "zh-CN");
            AddOverlay("MERC_SECONDARY_ALT1_DESCRIPTION", $"释放一个裂片上勾拳，造成<style=cIsDamage>{d(6)}%的伤害</style>，并将你送到半空。", "zh-CN");
            AddOverlay("MERC_SECONDARY_DESCRIPTION", "<style=cIsDamage>一闪</style>。快速横斩两次，造成<style=cIsDamage>2x200%的伤害</style>，若位于空中，则改为竖斩。", "zh-CN");
            AddOverlay("MERC_SPECIAL_ALT1_DESCRIPTION", $"发射一次刀刃之风，最多可对<style=cIsDamage>3</style>名敌人造成<style=cIsDamage>8x{d(SlicingWinds.damage)}%的伤害</style>。最后一次打击将<style=cIsUtility>暴露</style>敌人。", "zh-CN");
            AddOverlay("MERC_SPECIAL_DESCRIPTION", $"瞄准距离最近的敌人，攻击被瞄准的敌人可对其重复造成<style=cIsDamage>{d(Eviscerate.damageCoefficient)}%<style=cStack>（每击中一次+1.5%）</style>的伤害</style>。<style=cIsUtility>过程中无法被攻击</style>。", "zh-CN");
            AddOverlay("MERC_UTILITY_ALT1_DESCRIPTION", "<style=cIsDamage>一闪</style>。<style=cIsDamage>眩晕</style>。向前冲锋，造成<style=cIsDamage>700%的伤害</style>并在<style=cIsUtility>1秒</style>后<style=cIsUtility>暴露</style>敌人。", "zh-CN");
            AddOverlay("MERC_UTILITY_DESCRIPTION", "<style=cIsDamage>一闪</style>。<style=cIsDamage>眩晕</style>。向前冲锋并造成<style=cIsDamage>300%的伤害</style>。只要命中敌人，<style=cIsDamage>就可以再次发起冲锋</style>，最多<style=cIsDamage>3</style>次。", "zh-CN");
            AddOverlay("RAILGUNNER_ACTIVE_RELOAD_DESCRIPTION", $"在恰到好处的时机上弹可更快恢复，并使你下一发射弹伤害增加<style=cIsDamage>{d(ScopeAndReload.Damage)}%<style=cStack>（每层备用弹夹+100%）</style></style>。", "zh-CN");
            AddOverlay("RAILGUNNER_PRIMARY_DESCRIPTION", $"发射主动追踪弹药，造成<style=cIsDamage>100%的伤害</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SECONDARY_ALT_DESCRIPTION", $"启动你的<style=cIsUtility>近程瞄准镜</style>，高亮显示<style=cIsDamage>弱点</style>，并将你的武器转化为一门可造成<style=cIsDamage>{d(HH44Marksman.Damage)}%伤害</style>的快速磁轨炮。", "zh-CN");
            AddOverlay("RAILGUNNER_SECONDARY_DESCRIPTION", $"启动你的<style=cIsUtility>远程瞄准镜</style>，高亮显示<style=cIsDamage>弱点</style>，并将你的武器转化为一门可造成<style=cIsDamage>{d(M99Sniper.Damage)}%伤害</style>的穿刺磁轨炮。", "zh-CN");
            AddOverlay("RAILGUNNER_SNIPE_CRYO_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。发射一枚超低温射弹，造成<style=cIsDamage>{d(Cryocharge.Damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SNIPE_HEAVY_DESCRIPTION", $"发射一枚重型射弹，造成<style=cIsDamage>{d(M99Sniper.Damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SNIPE_LIGHT_DESCRIPTION", $"发射一枚轻型射弹，造成<style=cIsDamage>{d(HH44Marksman.Damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SNIPE_SUPER_DESCRIPTION", $"发射一枚造成<style=cIsDamage>{d(Supercharge.Damage)}%的伤害且具有{Supercharge.CritDamage}倍暴击伤害的超载射弹</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_ALT_DESCRIPTION", $"<style=cIsUtility>冰冻</style>。发射<style=cIsDamage>具有穿透效果</style>的子弹，造成<style=cIsDamage>{d(Cryocharge.Damage)}%的伤害</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_DESCRIPTION", $"发射一枚<style=cIsDamage>具有穿刺效果，</style>造成<style=cIsDamage>{d(Supercharge.Damage)}%的伤害且具有{Supercharge.CritDamage}倍暴击伤害</style>的超载射弹。之后，<style=cIsHealth>你的所有武器都将失灵</style>，持续<style=cIsHealth>{Supercharge.HopooBalance}</style>秒。", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_DESC", "同时持有<style=cIsUtility>两件装备</style>。激活'转换器'可以切换<style=cIsUtility>激活的装备</style>和<style=cIsDamage>磁轨炮手的次要技能攻击</style>。", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_DESC_ALT", "切换磁轨炮手的装备和瞄准镜", "zh-CN");
            AddOverlay("RAILGUNNER_SPECIAL_RAILRETOOL_NAME", "转换器", "zh-CN");
            AddOverlay("RAILGUNNER_UTILITY_ALT_DESCRIPTION", $"扔出一部装置，该装置可使附近所有<style=cIsUtility>敌人和射弹</style><style=cIsUtility>减速（射弹减99%）</style>，使所有友方提速{100 * PolarFieldDevice.SpeedBuffVal}%", "zh-CN");
            AddOverlay("RAILGUNNER_UTILITY_DESCRIPTION", $"扔出一部装置，该装置可将你和附近所有敌人<style=cIsUtility>推开</style>。最多可拥有{ConcussionDevice.Charges}部。", "zh-CN");
            AddOverlay("SPIKESTRIPSKILL_DEEPROT_DESCRIPTION", "<style=cIsVoid>虚 空 馈 赠</style>：<style=cIsHealing>毒化</style>攻击改为叠加<style=cIsVoid>虚空之毒</style>，使<style=cIsVoid>速度减慢10%</style>。当<style=cIsVoid>虚空之毒</style>叠加的层数超过<style=cIsVoid>灵魂之痛</style>层数的<style=cIsVoid>5</style>倍时，所有<style=cIsVoid>虚空之毒</style>将转化为<style=cIsVoid>灵魂之痛</style>。此外，所有<style=cArtifact>虚空</style>攻击都有几率叠加<style=cIsVoid>虚空之毒</style>。", "zh-CN");
            AddOverlay("SPIKESTRIPSKILL_DEEPROT_NAME", "腐朽", "zh-CN");
            AddOverlay("TOOLBOT_PRIMARY_ALT1_DESCRIPTION", "发射1条具有穿透效果的钢筋，造成<style=cIsDamage>600%</style>的伤害。", "zh-CN");
            AddOverlay("TOOLBOT_PRIMARY_ALT3_DESCRIPTION", "锯伤周围敌人，造成<style=cIsDamage>每秒1000%的伤害</style>。", "zh-CN");
            AddOverlay("TOOLBOT_PRIMARY_DESCRIPTION", "快速发射钉子，造成<style=cIsDamage>70%的伤害</style>。最后一次性发射<style=cIsDamage>12</style>枚伤害为<style=cIsDamage>70%的钉子。<style=cStack>（每发射1枚钉子+0.07米射程，松开后清零）</style>", "zh-CN");
            AddOverlay("TOOLBOT_SECONDARY_DESCRIPTION", "<style=cIsDamage>眩晕</style>。发射一枚造成<style=cIsDamage>220%<style=cStack>（每层眩晕手雷+100%）</style>伤害</style>的爆破筒。将分裂为造成<style=cIsDamage>5x44%<style=cStack>（每层眩晕手雷+20%）</style>伤害</style>以及<style=cIsDamage>眩晕</style>效果的小炸弹。", "zh-CN");
            AddOverlay("TREEBOT_SPECIAL_ALT1_DESCRIPTION", $"发射弩弹，造成<style=cIsDamage>{d(DIRECTIVEHarvest.damage)}%的伤害</style>且弩弹将<style=cIsDamage>注入</style>一个敌人。此敌人死亡时，掉落多个<style=cIsHealing>果实</style>，可治疗<style=cIsHealing>{100 * DIRECTIVEHarvest.percentHeal}%的生命值</style>，并且给予随机<style=cIsHealing>增益</style>。", "zh-CN");
            AddOverlay("TREEBOT_SPECIAL_DESCRIPTION", $"<style=cIsHealth>{d(TanglingGrowth.healthCost)}%生命值</style>。发射一朵会<style=cIsDamage>扎根</style>并造成<style=cIsDamage>{d(TanglingGrowth.rootDamage)}%伤害</style>的花朵。每命中一个目标便会对你<style=cIsHealing>治疗{d(TanglingGrowth.healPercent / (TanglingGrowth.pulseCount - 1) * 0.25f)}%生命值（最多{(TanglingGrowth.pulseCount - 1) * 4}次）</style>。", "zh-CN");
            AddOverlay("TREEBOT_UTILITY_DESCRIPTION", $"发射一次<style=cIsUtility>音爆</style>，<style=cIsDamage>弱化</style>命中的所有敌人。可储存{DIRECTIVEDisperse.maxStock}次", "zh-CN");
            AddOverlay("VOIDCRID_NULLBEAM", "<style=cArtifact>「虚空光束』</style>", "zh-CN");
            AddOverlay("VOIDCRID_NULLBEAM_DESC", "<style=cArtifact>虚空</style>。从<style=cIsVoid>虚空</style>中汲取力量，发射中距离<style=cIsVoid>虚空光束</style>攻击敌人，造成<style=cIsDamage>900%</style>的伤害，按住可增加发射的持续时间。每一击都有概率<style=cIsVoid>定身</style>敌人。", "zh-CN");
            AddOverlay("VOIDCRID_PASSIVE", "<style=cArtifact>虚空</style>克里德", "zh-CN");
            AddOverlay("VOIDCRID_PASSIVE_DESC", "所有<style=cArtifact>虚空</style>攻击都有几率<style=cArtifact>定身</style>敌人。（如果选择了“腐朽”被动，则额外叠加<style=cWorldEvent>虚空之毒</style>减益）", "zh-CN");
            AddOverlay("VOIDCRID_VOIDDRIFT", "<style=cArtifact>「虚无漂流』</style>", "zh-CN");
            AddOverlay("VOIDCRID_VOIDRIFT_DESC", "<style=cArtifact>虚空</style>。<style=cIsDamage>眩晕</style>。遁入<style=cIsVoid>虚空</style>，造成<style=cIsDamage>400%</style>的总伤害，有概率<style=cIsVoid>定身</style>敌人。", "zh-CN");
            AddOverlay("VOIDSURVIVOR_PRIMARY_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>发射一束造成2000%伤害的短程光束。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_PRIMARY_DESCRIPTION", "发射一束<style=cIsUtility>减速</style>远程光束，造成<style=cIsDamage>360%伤害</style>。", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SECONDARY_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>发射一枚造成2500%伤害的黑洞炸弹，半径变为25米。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SECONDARY_DESCRIPTION", "充能一枚虚空炸弹，造成<style=cIsDamage>666%伤害</style>。完全充能时可以变成爆炸性虚空炸弹，造成<style=cIsDamage>4444%伤害</style>。", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SECONDARY_UPRADE_TOOLTIP", "<style=cKeywordName>腐化升级</style><style=cSub>转化成能造成2500%伤害的黑洞炸弹，半径变为25米。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_SPECIAL_ALT_DESCRIPTION", "<style=cKeywordName>腐化升级</style><style=cSub>消耗25%的生命值来获得25%的腐化。</style>", "zh-CN");
            AddOverlay("VOIDSURVIVOR_UTILITY_ALT_DESCRIPTION", "<style=cIsUtility>消失</style>进入虚空，<style=cIsUtility>向前沿弧线</style>移动，同时<style=cIsUtility>清除所有减益效果</style>。", "zh-CN");
            AddOverlay("VV_ITEM_" + vanillaVoid.Items.CryoCanister.instance.ItemLangTokenName + "_DESCRIPTION", "杀死一个敌人会使<style=cIsDamage>10</style>米<style=cStack>（每层+2.5米）</style>内的所有敌人变慢，造成<style=cIsDamage>15%</style><style=cStack>（每层+15%）</style>的伤害，持续<style=cIsUtility>4</style>秒<style=cStack>（每层+2秒）</style>。<style=cIsVoid>腐化所有汽油</style>。", "zh-CN");
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

        public static void 象征汉化() {
            // "<color=#CCFFFF>冰霜的象征</color>：" "<color=#99CCFF>雷电的象征</color>："
            // "<color=#f25d25>火焰的象征</color>：" "<color=#20b2aa>无形的象征</color>："
            // "<color=#014421>腐蚀的象征</color>：" "<style=cIsLunar>完美的象征</style>："
            // "<style=cIsHealing>大地的象征</style>："
            // "<style=cIsVoid>虚空的象征</style>：" "<style=cDeath>忍耐力的象征</style> :"
            // "<style=cDeath>重力的象征</style> :" "<style=cDeath>模糊的象征</style> :"
            // "<style=cDeath>愤怒的象征</style> :" "<color=yellow>财富的象征</color>："
        }

        public static void 后续汉化() {
            if (Language.GetString("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERNAME", "zh-CN") == "暗杀") {
                return;
            }
            for (int i = languageOverlays.Count - 1; i >= 0; --i) {
                languageOverlays[i].Remove();
                languageOverlays.RemoveAt(i);
            }
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERDESC", Language.GetString("BANDIT2_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：额外发射1发子弹。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERNAME", "暗杀", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERDESC", Language.GetString("BANDIT2_SPECIAL_ALT_DESCRIPTION") + "\n<color=#d299ff>权杖：子弹有25%（每个标记+0.35%）概率弹射到30米内的其他敌人（最多8次）。\n每次弹射后距离和伤害-10%。不受运气影响。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERNAME", "叛徒", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEALTDESC", Language.GetString("CAPTAIN_UTILITY_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：以 自身等级x1<style=cStack>（每层权杖+1）</style> m/s的速度追踪附近的敌人，1.5倍等待时间，2倍范围，100,000%伤害。\n造成枯萎减益。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEALTNAME", "PHN-8300“<color=red>莉莉斯</color>”打击", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEDESC", Language.GetString("CAPTAIN_UTILITY_DESCRIPTION") + "\n<color=#d299ff>权杖：按住可连续呼叫UES顺风号，总共可造成21x500%伤害。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKENAME", "连续轨道炮", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_BARRAGEDESC", Language.GetString("COMMANDO_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：向射程内的每个敌人发射2<style=cStack>（每层权杖+2）</style>倍的子弹，两倍速度，两倍精度。\n按住你的主要技能可以更准确地射击。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_BARRAGENAME", "死亡绽放", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_GRENADEDESC", Language.GetString("COMMANDO_SPECIAL_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：扔出8枚具有一半伤害和击退的炸弹。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_GRENADENAME", "地毯式轰炸", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CROCO_DISEASEDESC", Language.GetString("CROCO_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：使受害者成为行走的瘟疫之源，持续将瘟疫传染给周围的敌人。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CROCO_DISEASENAME", "瘟疫", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_TURRETDESC", Language.GetString("ENGI_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：可放置更多炮塔。<style=cStack>（每层权杖，炮台+25%的伤害和攻速）</style></color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_TURRETNAME", "TR12-C 高斯自动炮台", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_WALKERDESC", Language.GetString("ENGI_SPECIAL_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：可放置更多炮塔。<style=cStack>（每层权杖，炮台+25%的伤害和攻速）</style></color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_WALKERNAME", "TR58-C 碳化器炮塔", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HERETIC_SQUAWKDESC", "<style=cIsHealth>歌声</style>将标记所有<style=cIsHealth>活体</style>，使其染上<link=\"BulwarksHauntWavy\"><color=red>GlobalEventManager_OnCharacterDeath</color></link>！<link=\"BulwarksHauntWavy\"><color=red>GlobalEventManager_OnCharacterDeath</color></link>：持续<style=cIsUtility>10</style>秒<style=cStack>（每层权杖+10秒）</style>，当带有<link=\"BulwarksHauntWavy\"><color=red>GlobalEventManager_OnCharacterDeath</color></link>的敌人死去时，会连带着它的<color=red>所有族人</color>一起<color=red>死去</color>。", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HERETIC_SQUAWKNAME", "<link=\"BulwarksHauntWavy\">灭绝之歌</link>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_BALLISTADESC", Language.GetString("HUNTRESS_SPECIAL_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：快速连发5根额外弩箭，造成2.5倍的总伤害。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_BALLISTANAME", "-腊包尔->", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_RAINDESC", Language.GetString("HUNTRESS_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：点燃。传送到附近的敌人。半径和持续时间增加50%<style=cStack>（每层权杖+50%持续时间）</style>。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_RAINNAME", "<color=#f25d25>火雨</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_LOADER_CHARGEFISTDESC", Language.GetString("LOADER_UTILITY_DESCRIPTION") + "\n<color=#d299ff>权杖：双倍伤害和冲刺速度。高到离谱的击退。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_LOADER_CHARGEFISTNAME", "百万吨重拳", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLAMETHROWERDESC", Language.GetString("MAGE_SPECIAL_FIRE_DESCRIPTION") + "\n<color=#d299ff>权杖：留下灼热的火云。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLAMETHROWERNAME", "龙息", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLYUPDESC", Language.GetString("MAGE_SPECIAL_LIGHTNING_DESCRIPTION") + "\n<color=#d299ff>权杖：双倍伤害，四倍半径。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLYUPNAME", "反物质浪涌", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISDESC", Language.GetString("MERC_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：持续时间+100%<style=cStack>（每层权杖+100%）</style>。击杀可重置持续时间！按住跳跃键可以提前退出技能。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISNAME", "屠戮", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISPROJDESC", Language.GetString("MERC_SPECIAL_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：四倍充能速度。按住可发射四次充能。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISPROJNAME", "死亡之风", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPECRYODESC", Language.GetString("RAILGUNNER_SPECIAL_ALT_DESCRIPTION") + "\n<color=#d299ff>权杖：<color=blue>冰冻</color>爆炸，对6米内的敌人造成射弹的 2倍 伤害，并减速80%，持续20秒</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPECRYONAME", "T°->绝对零度", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_FIRESNIPECRYONAME", "<color=blue>冰！</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPESUPERDESC", Language.GetString("RAILGUNNER_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：<color=yellow>发射时，将消耗当前金钱的10%<style=cStack>（每层权杖+10%）</style>转化为伤害。</color>永久降低敌人20点护甲。Proc系数增加0.5。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPESUPERNAME", "超电磁炮", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_FIRESNIPESUPERNAME", "“一枚硬币”", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_SCEPLOADER_CHARGEZAPFISTDESC", Language.GetString("LOADER_UTILITY_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：全向闪电。双倍冲刺速度，临时提高护甲。<color=#99CCFF>“<link=\"BulwarksHauntShaky\">以雷霆~ 击碎黑暗！</link>”</color></color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_SCEPLOADER_CHARGEZAPFISTNAME", "<color=#99CCFF>雷霆一击·超</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TOOLBOT_DASHDESC", Language.GetString("TOOLBOT_UTILITY_DESCRIPTION") + "\n<color=#d299ff>权杖：将传入的伤害减半（与护甲叠加），持续时间加倍。停止后：爆炸，以巨大的爆炸击晕敌人，造成所受伤害的200%的伤害。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TOOLBOT_DASHNAME", "毁灭模式", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FLOWER2DESC", Language.GetString("TREEBOT_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：双倍范围。造成随机减益。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FLOWER2NAME", "混沌生长", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FRUIT2DESC", Language.GetString("TREEBOT_SPECIAL_ALT1_DESCRIPTION") + "\n<color=#d299ff>权杖：生成额外的果实，果实增益增加。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FRUIT2NAME", "终极命令：收割", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_VOIDSURVIVOR_CRUSHCORRUPTIONDESC", Language.GetString("VOIDSURVIVOR_SPECIAL_DESCRIPTION") + "\n<color=#d299ff>权杖：影响周围25米内的敌人和盟友。<color=red>（未知原因，此权杖技能不生效）</color></color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_VOIDSURVIVOR_CRUSHCORRUPTIONNAME", "「促进」", "zh-CN"));
        }
    }
}