using System.Linq;
using UnityEngine;

namespace BtpTweak.Utils {

    public static class StringExtension {

        public static T Load<T>(this string path) => UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(path).WaitForCompletion();

        public static T LoadComponent<T>(this string path) where T : Component => path.Load<GameObject>().GetComponent<T>();

        public static T LoadComponentInChildren<T>(this string path) where T : Component => path.Load<GameObject>().GetComponentInChildren<T>();

        public static T[] LoadComponents<T>(this string path) where T : Component => path.Load<GameObject>().GetComponents<T>();

        public static string ToDeath(this string str) => "<style=cDeath>" + str + "</style>";

        public static string ToDmg(this object str) => "<style=cIsDamage>" + str + "</style>";

        public static string ToDmgPct(this float damage) => "<style=cIsDamage>" + damage.ToPercent() + "</style>";

        public static string ToFire(this string str) => "<color=#f25d25>" + str + "</color>";

        public static string ToHealing(this object str) => "<style=cIsHealing>" + str + "</style>";

        public static string ToHealPct(this float value) => "<style=cIsHealing>" + value.ToPercent() + "</style>";

        public static string ToHealth(this object str) => "<style=cIsHealth>" + str + "</style>";

        public static string ToIce(this string str) => "<color=#CCFFFF>" + str + "</color>";

        public static string ToLightning(this string str) => "<color=#99CCFF>" + str + "</color>";

        public static string ToLunar(this string str) => "<style=cIsLunar>" + str + "</style>";

        public static string ToPctWithStack(this float value) {
            string pctStr = value.ToPercent();
            return pctStr + pctStr.ToStack("（每层+_）");
        }

        public static string ToValueWithStack(this float value) {
            return value + value.ToStack("（每层+_）");
        }

        public static string ToStdWithStack(this float baseValue, float stackValue) {
            return baseValue + stackValue.ToStack("（每层+_）");
        }

        public static string ToStdPctWithStack(this float baseValue, float stackValue) {
            return baseValue.ToPercent() + stackValue.ToPercent().ToStack("（每层+_）");
        }

        public static string ToPercent(this float value) => 100 * value + "%";

        public static string ToPoison(this string str) => "<color=#014421>" + str + "</color>";

        public static string ToRed(this string str) => "<color=red>" + str + "</color>";

        public static string ToScepterDescription(this string desc) => "\n<color=#d299ff>权杖：" + desc + "</color>";

        public static string ToStack(this object str) => "<style=cStack>" + str + "</style>";

        public static string ToStack(this object str, string prefix_suffix) {
            var content = str.ToString();
            if (content == "0") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cStack>" + p_s.ElementAtOrDefault(0) + content + p_s.ElementAtOrDefault(1) + "</style>";
        }

        public static string ToStackPct(this float value, string prefix_suffix = "（每层+_%）") {
            if (value == 0) {
                return string.Empty;
            }
            return value.ToPercent().ToStack(prefix_suffix);
        }

        public static string ToUtil(this object str) => "<style=cIsUtility>" + str + "</style>";

        public static string ToUtil(this object str, string prefix_suffix) {
            var content = str.ToString();
            if (content == "0") {
                return string.Empty;
            }
            var p_s = prefix_suffix.Split('_');
            return "<style=cIsUtility>" + p_s.ElementAtOrDefault(0) + content + p_s.ElementAtOrDefault(1) + "</style>";
        }

        public static string ToVoid(this string str) => "<style=cIsVoid>" + str + "</style>";

        public static string ToYellow(this string str) => "<color=yellow>" + str + "</color>";
    }
}