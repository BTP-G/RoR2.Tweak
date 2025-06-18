using BTP.RoR2Plugin.Utils;
using System;
using System.Linq;
using System.Text;

namespace BTP.RoR2Plugin.Language {

    public static class StringExtensions {
        private static readonly StringBuilder _shared = new();

        public static string ToDeath(this string str) => _shared.Clear()
            .Append("<style=cDeath>")
            .Append(str)
            .Append("</style>").ToString();

        public static string ToShrine(this string str) => "<style=cShrine>" + str + "</style>";

        public static string ToDmg<T>(this T obj, string prefix_suffix = "") {
            if (obj == null) {
                return string.Empty;
            }
            var str = obj.ToString();
            if (str == "0" || str == "0%") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsDamage>" + p_s.ElementAtOrDefault(0) + str + p_s.ElementAtOrDefault(1) + "</style>";
        }

        public static string ToDmgPct<T>(this T damage, string prefix_suffix = "") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return damage.ToPct().ToDmg(prefix_suffix);
        }

        public static string ToFire(this string str) => "<color=#f25d25>" + str + "</color>";

        public static string ToHealing<T>(this T obj, string prefix_suffix = "") {
            if (obj == null) {
                return string.Empty;
            }
            var str = obj.ToString();
            if (str == "0" || str == "0%") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsHealing>" + p_s.ElementAtOrDefault(0) + str + p_s.ElementAtOrDefault(1) + "</style>";
        }

        public static string ToHealPct(this float value) => "<style=cIsHealing>" + value.ToPct() + "</style>";

        public static string ToHealth<T>(this T str) => "<style=cIsHealth>" + str + "</style>";

        public static string ToIce(this string str) => "<color=#CCFFFF>" + str + "</color>";

        public static string ToWavy(this string str) => "<link=\"BulwarksHauntWavy\">" + str + "</link>";

        public static string ToRainbowWavy(this string str) => "<link=\"BulwarksHauntRainbowWavy\">" + str + "</link>";

        public static string ToShaky(this string str) => "<link=\"BulwarksHauntShaky\">" + str + "</link>";

        public static string ToLightning(this string str) => "<color=#99CCFF>" + str + "</color>";

        public static string ToLunar(this string str) => "<style=cIsLunar>" + str + "</style>";

        public static string ToStk<T>(this T str) => "<style=cStack>" + str + "</style>";

        public static string ToStk<T>(this T obj, string prefix_suffix) {
            if (obj == null) {
                return string.Empty;
            }
            var str = obj.ToString();
            if (str == "0" || str == "0%") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cStack>" + p_s.ElementAtOrDefault(0) + str + p_s.ElementAtOrDefault(1) + "</style>";
        }

        public static string ToStkPct<T>(this T value, string prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return value.ToPct().ToStk(prefix_suffix);
        }

        public static string ToBaseAndStk<T>(this T baseValue, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return baseValue + baseValue.ToStk(stk_prefix_suffix);
        }

        public static string ToBaseWithStk<T>(this T baseValue, T stackValue, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return baseValue + stackValue.ToStk(stk_prefix_suffix);
        }

        public static string ToBaseAndStkPct<T>(this T value, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            var pctStr = value.ToPct();
            return pctStr + pctStr.ToStk(stk_prefix_suffix);
        }

        public static string ToBaseWithStkPct<T>(this T baseValue, T stackValue, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return baseValue.ToPct() + stackValue.ToPct().ToStk(stk_prefix_suffix);
        }

        public static string ToPct<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            if (float.TryParse(value.ToString(), out var result)) {
                return 100 * result + "%";
            } else {
                return string.Empty;
            }
        }

        public static string ToPoison(this string str) => "<color=#014421>" + str + "</color>";

        public static string ToRed(this string str) => "<color=red>" + str + "</color>";

        public static string ToScepterDesc(this string desc) => "\n<color=#d299ff>权杖：" + desc + "</color>";

        public static string ToUtil<T>(this T str) => "<style=cIsUtility>" + str + "</style>";

        public static string ToUtil<T>(this T str, string prefix_suffix) {
            var content = str.ToString();
            if (content == "0") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsUtility>" + p_s.ElementAtOrDefault(0) + content + p_s.ElementAtOrDefault(1) + "</style>";
        }

        public static string ToVoid(this string str) => "<style=cIsVoid>" + str + "</style>";

        public static string ToYellow(this string str) => "<color=yellow>" + str + "</color>";

        public static string ToBaseAndStkBy逼近Pct(this float 半数) => $"{BtpUtils.简单逼近1(1, 半数).ToPct()}<style=cStack>（结果 = 100%x(层数/(层数+{半数})）</style>";
    }
}