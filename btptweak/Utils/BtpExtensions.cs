using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.Utils {

    public static class BtpExtensions {

        public static T AddComponent<T>(this Component self) where T : Component => self.gameObject.AddComponent<T>();

        public static T GetRandom<T>(this List<T> list, Xoroshiro128Plus rng = null) {
            if (rng == null) {
                return list[Random.RandomRangeInt(0, list.Count)];
            } else {
                return list[rng.RangeInt(0, list.Count)];
            }
        }

        public static T Load<T>(this string path) => UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(path).WaitForCompletion();

        public static T LoadComponent<T>(this string path) where T : Component => path.Load<GameObject>().GetComponent<T>();

        public static T LoadComponentInChildren<T>(this string path) where T : Component => path.Load<GameObject>().GetComponentInChildren<T>();

        public static T[] LoadComponents<T>(this string path) where T : Component => path.Load<GameObject>().GetComponents<T>();

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component => Object.Destroy(gameObject.GetComponent<T>());

        public static void RemoveComponents<T>(this GameObject gameObject) where T : Component {
            T[] coms = gameObject.GetComponents<T>();
            for (int i = 0; i < coms.Length; i++) {
                Object.Destroy(coms[i]);
            }
        }

        public static void Qlog(this object loginfo, string prefix = "QQQlog: ") => Main.Logger.LogInfo(prefix + loginfo);
    }
}