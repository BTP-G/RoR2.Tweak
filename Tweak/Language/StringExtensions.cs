using BTP.RoR2Plugin.Utils;
using GuestUnion;
using System;
using System.Linq;
using System.Text;

namespace BTP.RoR2Plugin.Language {

    public static class StringExtensions {

        public static string ToDeath(this string str) => "<style=cDeath>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToShrine(this string str) => "<style=cShrine>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToDmg<T>(this T obj, string prefix_suffix = "") {
            if (obj == null) {
                return string.Empty;
            }
            var str = obj.ToString();
            if (str == "0" || str == "0%") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsDamage>".ToStringBuilder().Append(p_s.ElementAtOrDefault(0)).Append(str).Append(p_s.ElementAtOrDefault(1)).Append("</style>").ToStringAndReturn();
        }

        public static string ToDmgPct<T>(this T damage, string prefix_suffix = "") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return damage.ToPct().ToDmg(prefix_suffix);
        }

        public static string ToFire(this string str) => "<color=#f25d25>".ToStringBuilder().Append(str).Append("</color>").ToStringAndReturn();

        public static string ToHealing<T>(this T obj, string prefix_suffix = "") {
            if (obj == null) {
                return string.Empty;
            }
            var str = obj.ToString();
            if (str == "0" || str == "0%") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsHealing>".ToStringBuilder().Append(p_s.ElementAtOrDefault(0)).Append(str).Append(p_s.ElementAtOrDefault(1)).Append("</style>").ToStringAndReturn();
        }

        public static string ToHealPct(this float value) => "<style=cIsHealing>".ToStringBuilder().Append(value.ToPct()).Append("</style>").ToStringAndReturn();

        public static string ToHealth<T>(this T str) => "<style=cIsHealth>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToIce(this string str) => "<color=#CCFFFF>".ToStringBuilder().Append(str).Append("</color>").ToStringAndReturn();

        public static string ToWavy(this string str) => "<link=\"BulwarksHauntWavy\">".ToStringBuilder().Append(str).Append("</link>").ToStringAndReturn();

        public static string ToRainbowWavy(this string str) => "<link=\"BulwarksHauntRainbowWavy\">".ToStringBuilder().Append(str).Append("</link>").ToStringAndReturn();

        public static string ToShaky(this string str) => "<link=\"BulwarksHauntShaky\">".ToStringBuilder().Append(str).Append("</link>").ToStringAndReturn();

        public static string ToLightning(this string str) => "<color=#99CCFF>".ToStringBuilder().Append(str).Append("</color>").ToStringAndReturn();

        public static string ToLunar(this string str) => "<style=cIsLunar>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToStk<T>(this T str) => "<style=cStack>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToStk<T>(this T obj, string prefix_suffix) {
            if (obj == null) {
                return string.Empty;
            }
            var str = obj.ToString();
            if (str == "0" || str == "0%") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cStack>".ToStringBuilder().Append(p_s.ElementAtOrDefault(0)).Append(str).Append(p_s.ElementAtOrDefault(1)).Append("</style>").ToStringAndReturn();
        }

        public static string ToStkPct<T>(this T value, string prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return value.ToPct().ToStk(prefix_suffix);
        }

        public static string ToBaseAndStk<T>(this T baseValue, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return baseValue.ToStringBuilder().Append(baseValue.ToStk(stk_prefix_suffix)).ToStringAndReturn();
        }

        public static string ToBaseWithStk<T>(this T baseValue, T stackValue, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return baseValue.ToStringBuilder().Append(stackValue.ToStk(stk_prefix_suffix)).ToStringAndReturn();
        }

        public static string ToBaseAndStkPct<T>(this T value, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            var pctStr = value.ToPct();
            return pctStr.ToStringBuilder().Append(pctStr.ToStk(stk_prefix_suffix)).ToStringAndReturn();
        }

        public static string ToBaseWithStkPct<T>(this T baseValue, T stackValue, string stk_prefix_suffix = "（每层+_）") where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            return baseValue.ToPct().ToStringBuilder().Append(stackValue.ToPct().ToStk(stk_prefix_suffix)).ToStringAndReturn();
        }

        public static string ToPct<T>(this T value) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> {
            if (float.TryParse(value.ToString(), out var result)) {
                var pct = 100 * result;
                return pct.ToStringBuilder().Append('%').ToStringAndReturn();
            } else {
                return string.Empty;
            }
        }

        public static string ToPoison(this string str) => "<color=#014421>".ToStringBuilder().Append(str).Append("</color>").ToStringAndReturn();

        public static string ToRed(this string str) => "<color=red>".ToStringBuilder().Append(str).Append("</color>").ToStringAndReturn();

        public static string ToScepterDesc(this string desc) => "\n<color=#d299ff>权杖：".ToStringBuilder().Append(desc).Append("</color>").ToStringAndReturn();

        public static string ToUtil<T>(this T str) => "<style=cIsUtility>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToUtil<T>(this T str, string prefix_suffix) {
            var content = str.ToString();
            if (content == "0") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsUtility>".ToStringBuilder().Append(p_s.ElementAtOrDefault(0)).Append(str).Append(p_s.ElementAtOrDefault(1)).Append("</style>").ToStringAndReturn();
        }

        public static string ToVoid(this string str) => "<style=cIsVoid>".ToStringBuilder().Append(str).Append("</style>").ToStringAndReturn();

        public static string ToYellow(this string str) => "<color=yellow>".ToStringBuilder().Append(str).Append("</color>").ToStringAndReturn();

        public static string ToBaseAndStkByCloseToPct(this float 半数) => Util2.CloseTo1(1, 半数).ToPct().ToStringBuilder().Append("<style=cStack>（结果 = 100%x(层数/(层数+").Append(半数).Append(")）</style>").ToStringAndReturn();
    }
}