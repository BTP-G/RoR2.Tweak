using HIFUHuntressTweaks.Skills;
using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using static R2API.LanguageAPI;

namespace BtpTweak {

    public class Helpers {
        private static List<LanguageOverlay> languageOverlays = new();

        public static string ScepterDescription(string desc) {
            return "<color=#d299ff>权杖：" + desc + "</color>";
        }

        public static string ScepterToken(string desc) {
            return "";
        }

        public static float D(float damage) {
            return 100 * damage;
        }

        public static Transform GetClosestPlayerTransform(ReadOnlyCollection<TeamComponent> players, Vector3 location) {
            Transform result = null;
            float num = float.MaxValue;
            foreach (TeamComponent teamComponent in players) {
                if (Util.LookUpBodyNetworkUser(teamComponent.gameObject)) {
                    float num2 = Vector3.Distance(teamComponent.body.corePosition, location);
                    if (num2 < num) {
                        result = teamComponent.body.coreTransform;
                        num = num2;
                    }
                }
            }
            return result;
        }

        public static void AddTag(ref ItemDef itemDef, ItemTag itemTag) {
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

        public static void 后续汉化() {
            if (Language.GetString("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERNAME", "zh-CN") == "暗杀") {
                return;
            }
            for (int i = languageOverlays.Count - 1; i >= 0; --i) {
                languageOverlays[i].Remove();
                languageOverlays.RemoveAt(i);
            }
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERDESC", "<style=cIsDamage>屠杀者</style>。使用左轮手枪进行射击，造成<style=cIsDamage>600%的伤害</style>。击杀敌人可以<style=cIsUtility>重置所有能力的冷却时间</style>。\n<color=#d299ff>权杖：额外发射一发子弹。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_RESETREVOLVERNAME", "暗杀", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERDESC", "<style=cIsDamage>屠杀者</style>。使用左轮手枪进行射击，造成<style=cIsDamage>600%的伤害</style>（可以直接斩杀血量低于12.5%的敌人）。击杀敌人可以<style=cIsDamage>叠加效果</style>（死亡和过关不消失），使亡命徒的伤害提高<style=cIsDamage>10%</style>。射击需要消耗当前<style=cIsDamage>叠加层数</style>的<style=cIsDamage>1 / (3x人物等级)</style>。\n<color=#d299ff>权杖：子弹有25%（每个标记+0.35%）概率弹射到30米内的其他敌人（最多8次）。\n每次弹射后距离和伤害-10%。不受运气影响。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_BANDIT2_SKULLREVOLVERNAME", "叛徒", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEALTDESC", "<style=cIsDamage>眩晕</style>。向<style=cIsDamage>UES顺风号</style>请求发动一次<style=cIsDamage>动能打击</style>。在<style=cIsUtility>20秒后</style>，对所有角色造成<style=cIsDamage>50000%的伤害</style>。\n<color=#d299ff>权杖：1.5倍等待时间，2倍范围，100,000%伤害。\n造成疫病减益。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEALTNAME", "PHN-8300“<color=red>莉莉斯</color>”打击", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKEDESC", "<style=cIsDamage>眩晕</style>。向<style=cIsDamage>UES顺风号</style>请求至多<style=cIsDamage>3台</style>轨道探测器。每台探测器将造成<style=cIsDamage>1111%伤害</style>。\n<color=#d299ff>权杖：按住可连续呼叫UES顺风号，总共可造成21x500%伤害。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CAPTAIN_AIRSTRIKENAME", "21-探测炮", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_BARRAGEDESC", "<style=cIsDamage>眩晕</style>。连续射击，每枚弹丸造成<style=cIsDamage>200%的伤害</style>。射击次数随攻击速度增加。\n<color=#d299ff>权杖：向射程内的每个敌人发射两倍子弹，两倍速度，两倍精度。\n按住你的主要技能可以更准确地射击。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_BARRAGENAME", "死亡绽放", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_GRENADEDESC", "扔出一枚手雷，爆炸可造成<style=cIsDamage>1000%的伤害</style>。最多可投掷2枚。\n<color=#d299ff>权杖：扔出8枚具有一半伤害和击退的炸弹。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_COMMANDO_GRENADENAME", "地毯式轰炸", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CROCO_DISEASEDESC", "<style=cIsHealing>毒化</style>。释放一种能够造成<style=cIsDamage>100%伤害</style>的致命病毒。病毒将会传播给最多<style=cIsDamage>20</style>名目标。\n<color=#d299ff>权杖：受害者成为行走的瘟疫之源。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_CROCO_DISEASENAME", "瘟疫", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_TURRETDESC", "放置一个<style=cIsUtility>继承你所有物品</style>的炮塔，发射的炮弹可造成<style=cIsDamage>100%的伤害</style>，最多放置2座。\n<color=#d299ff>权杖：可放置更多炮塔。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_TURRETNAME", "TR12-C 高斯自动炮台", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_WALKERDESC", "放置一个<style=cIsUtility>移动</style>炮塔可<style=cIsUtility>继承你所有物品</style>。发射的激光可造成<style=cIsDamage>每秒200%的伤害</style>，并可<style=cIsUtility>减速敌人</style>，最多放置2座。\n<color=#d299ff>权杖：可放置更多炮塔。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_ENGI_WALKERNAME", "TR58-C 碳化器炮塔", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HERETIC_SQUAWKDESC", "（未知原因，此技能仅主机有效）灭绝buff：攻击敌人可传播，染上buff30秒后，敌人受到5000%的伤害。\n<color=#d299ff>权杖：</color>“<link=\"BulwarksHauntWavy\"><color=red>灭绝</color></link>”buff：当带有<link=\"BulwarksHauntWavy\"><color=red>灭绝</color></link>buff的敌人死去时，会连带着它的<color=red>所有族人</color>一起<color=red>死去</color>。", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HERETIC_SQUAWKNAME", "<link=\"BulwarksHauntWavy\">灭绝之歌</link>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_BALLISTADESC", $"向后<style=cIsUtility>传送</style>至空中。最多发射<style=cIsDamage>{Ballista.boltCount}</style>道能量闪电，造成<style=cIsDamage>{Ballista.boltCount}x{Helpers.D(Ballista.damage)}%的伤害</style>。\n<color=#d299ff>权杖：快速连发5根额外弩箭，造成2.5倍的总伤害。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_BALLISTANAME", "-腊包尔->", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_RAINDESC", $"<style=cIsUtility>传送</style>至空中，向目标区域射下箭雨，使区域内所有敌人<style=cIsUtility>减速</style>，并造成<style=cIsDamage>每秒{300 * ArrowRain.damage}%的伤害</style>。\n<color=#d299ff>权杖：半径和持续时间+50%。点燃敌人。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_HUNTRESS_RAINNAME", "火雨", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_LOADER_CHARGEFISTDESC", "<style=cIsUtility>重型</style>。发动一次<style=cIsUtility>穿透</style>直拳，造成 <style=cIsDamage>600%-2700%的伤害</style>。\n<color=#d299ff>权杖：双倍伤害和冲刺速度。高到离谱的击退。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_LOADER_CHARGEFISTNAME", "百万吨重拳", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLAMETHROWERDESC", "<style=cIsDamage>点燃</style>。灼烧面前的所有敌人，对其造成<style=cIsDamage>2000%的伤害</style>。\n<color=#d299ff>权杖：留下了灼热的火云。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLAMETHROWERNAME", "龙息", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLYUPDESC", "<style=cIsDamage>眩晕</style>。一飞冲天，造成<style=cIsDamage>800%的伤害</style>。\n<color=#d299ff>权杖：双倍伤害，四倍半径。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MAGE_FLYUPNAME", "反物质浪涌", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISDESC", "瞄准距离最近的敌人，攻击被瞄准的敌人可对其重复造成<style=cIsDamage>130%<style=cStack>（每击中一次+1.3%）</style>的伤害</style>。<style=cIsUtility>过程中无法被攻击</style>。\n<color=#d299ff>权杖：双倍持续时间。击杀可重置持续时间。\n按住技能按键可以提前结束。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISNAME", "屠戮", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISPROJDESC", "发射一次刀刃之风，最多可对<style=cIsDamage>3</style>名敌人造成<style=cIsDamage>8x100%的伤害</style>。最后一次打击将<style=cIsUtility>暴露</style>敌人。\n<color=#d299ff>权杖：四倍充能速度。按住可发射四次充能。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_MERC_EVISPROJNAME", "死亡之风", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPECRYODESC", "<style=cIsUtility>冰冻</style>。发射<style=cIsDamage>具有穿透效果</style>的子弹，造成<style=cIsDamage>2000%的伤害</style>。\n<color=#d299ff>权杖：<color=blue>冰冻</color>爆炸，对6米内的敌人造成射弹的 2倍 伤害，并减速80%，持续20秒</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPECRYONAME", "T°->绝对零度", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_FIRESNIPECRYONAME", "<color=blue>冰</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPESUPERDESC", "发射一枚<style=cIsDamage>具有穿刺效果，</style>造成<style=cIsDamage>3000%的伤害且具有双倍暴击伤害</style>的超载射弹。之后，<style=cIsHealth>你的所有武器都将失灵</style>，持续<style=cIsHealth>5</style>秒。\n<color=#d299ff>权杖：<color=yellow>当前金钱的1%<style=cStack>（每有个备用弹夹+1%）</style>转化为伤害。</color>永久降低20点护甲。Proc系数+0.5。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_SNIPESUPERNAME", "超电磁炮", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_RAILGUNNER_FIRESNIPESUPERNAME", "“一枚硬币”", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_SCEPLOADER_CHARGEZAPFISTDESC", "<style=cIsUtility>重型</style>。发动一次<style=cIsUtility>单体攻击</style>直拳，造成<style=cIsDamage>2100%的伤害</style>，<style=cIsDamage>震荡</style>锥形区域内的所有敌人并造成<style=cIsDamage>1000%的伤害</style>。\n<color=#d299ff>权杖：全向闪电。“<link=\"BulwarksHauntShaky\">以雷霆~ 击碎黑暗！</link>”</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_SCEPLOADER_CHARGEZAPFISTNAME", "雷霆一击·超", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TOOLBOT_DASHDESC", "<style=cIsUtility>重型</style>。向前方冲刺，获得<style=cIsUtility>200护甲</style>与<style=cIsUtility>220%移动速度</style>。对敌人造成<style=cIsDamage>250%伤害</style>。\n<color=#d299ff>权杖：将传入的伤害减半（与护甲叠加），持续时间加倍。停止后：爆炸，以巨大的爆炸击晕敌人，造成所受伤害的200%的伤害。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TOOLBOT_DASHNAME", "毁灭模式", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FLOWER2DESC", "<style=cIsHealth>25%生命值</style>。发射一朵会<style=cIsDamage>扎根</style>并造成<style=cIsDamage>200%伤害</style>的花朵。<style=cIsHealing>每命中一个目标便会对你治疗 0.5% 生命值（一共16次）</style>。\n<color=#d299ff>权杖：双倍范围。造成随机减益。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FLOWER2NAME", "混沌生长", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FRUIT2DESC", "发射弩弹，造成<style=cIsDamage>600%的伤害</style>且弩弹将<style=cIsDamage>注入</style>一个敌人。此敌人死亡时，掉落多个<style=cIsHealing>治疗果实</style>，可治疗<style=cIsHealing>20%的生命值</style>。\n<color=#d299ff>权杖：生成额外的果实，可提供BUFF。</color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_TREEBOT_FRUIT2NAME", "命令：收割", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_VOIDSURVIVOR_CRUSHCORRUPTIONDESC", "消耗<style=cIsVoid>25%的腐化</style>治疗自己，恢复<style=cIsHealing>25%的最大生命值</style>。\n<color=#d299ff>权杖：影响周围25米内的敌人和盟友。<color=red>（未知原因，此权杖技能不生效）</color></color>", "zh-CN"));
            languageOverlays.Add(AddOverlay("ANCIENTSCEPTER_VOIDSURVIVOR_CRUSHCORRUPTIONNAME", "「促进」", "zh-CN"));
        }
    }
}