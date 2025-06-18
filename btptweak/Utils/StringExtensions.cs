using UnityEngine;

namespace BTP.RoR2Plugin.Utils {

    public static class StringExtensions {

        public static T Load<T>(this string path) => UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(path).WaitForCompletion();

        public static T LoadComponent<T>(this string path) where T : Component => path.Load<GameObject>().GetComponent<T>();

        public static T LoadComponentInChildren<T>(this string path) where T : Component => path.Load<GameObject>().GetComponentInChildren<T>();

        public static T[] LoadComponents<T>(this string path) where T : Component => path.Load<GameObject>().GetComponents<T>();

    }
}