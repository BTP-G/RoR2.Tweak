using BTP.RoR2Plugin.Utils;
using System.Text;

namespace BTP.RoR2Plugin.Language {

    internal static class StringBuilderExtensions {

        public static StringBuilder AppendStk<T>(this StringBuilder sb, T value) {
            return sb.Append("<style=cStack>").Append(value.ToString()).Append("</style>");
        }

        public static StringBuilder AppendBaseAndStk<T>(this StringBuilder sb, T value) {
            var str = value.ToString();
            return sb.Append(str).Append("<style=cStack>（每层").Append(str).Append("）</style>");
        }

        public static StringBuilder AppendBaseAndStkPct(this StringBuilder sb, float value) {
            var pctValue = value * 100;
            return sb.Append(pctValue).Append("%<style=cStack>（每层+").Append(pctValue).Append("%）</style>");
        }

        public static StringBuilder AppendDeath(this StringBuilder sb, string str) {
            return sb.Append("<style=cDeath>").Append(str).Append("</style>");
        }

        public static StringBuilder AppendShrine(this StringBuilder sb, string str) {
            return sb.Append("<style=cShrine>").Append(str).Append("</style>");
        }

        public static StringBuilder AppendFire(this StringBuilder sb, string str) {
            return sb.Append("<color=#f25d25>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendHealPct(this StringBuilder sb, float value) {
            return sb.Append("<style=cIsHealing>").AppendPct(value).Append("</style>");
        }

        public static StringBuilder AppendHealth<T>(this StringBuilder sb, T str) {
            return sb.Append("<style=cIsHealth>").Append(str).Append("</style>");
        }

        public static StringBuilder AppendIce(this StringBuilder sb, string str) {
            return sb.Append("<color=#CCFFFF>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendWavy(this StringBuilder sb, string str) {
            return sb.Append("<link=\"BulwarksHauntWavy\">").Append(str).Append("</link>");
        }

        public static StringBuilder AppendRainbowWavy(this StringBuilder sb, string str) {
            return sb.Append("<link=\"BulwarksHauntRainbowWavy\">").Append(str).Append("</link>");
        }

        public static StringBuilder AppendShaky(this StringBuilder sb, string str) {
            return sb.Append("<link=\"BulwarksHauntShaky\">").Append(str).Append("</link>");
        }

        public static StringBuilder AppendLightning(this StringBuilder sb, string str) {
            return sb.Append("<color=#99CCFF>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendLunar(this StringBuilder sb, string str) {
            return sb.Append("<style=cIsLunar>").Append(str).Append("</style>");
        }

        public static StringBuilder AppendPct(this StringBuilder sb, float value) {
            return sb.Append(100 * value).Append("%");
        }

        public static StringBuilder AppendPoison(this StringBuilder sb, string str) {
            return sb.Append("<color=#014421>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendRed(this StringBuilder sb, string str) {
            return sb.Append("<color=red>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendScepterDesc(this StringBuilder sb, string desc) {
            return sb.Append("\n<color=#d299ff>权杖：").Append(desc).Append("</color>");
        }

        public static StringBuilder AppendUtil<T>(this StringBuilder sb, T str) {
            return sb.Append("<style=cIsUtility>").Append(str.ToString()).Append("</style>");
        }

        public static StringBuilder AppendVoid(this StringBuilder sb, string str) {
            return sb.Append("<style=cIsVoid>").Append(str).Append("</style>");
        }

        public static StringBuilder AppendYellow(this StringBuilder sb, string str) {
            return sb.Append("<color=yellow>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendGold(this StringBuilder sb, string str) {
            return sb.Append("<color=#FFFF00>").Append(str).Append("</color>");
        }

        public static StringBuilder AppendBaseAndStkByCloseToPct(this StringBuilder sb, float 半数) {
            return sb.AppendPct(Util2.CloseTo1(1, 半数))
                .Append("<style=cStack>（结果 = 100%x(层数/(层数+")
                .Append(半数)
                .Append("))</style>");
        }
    }
}