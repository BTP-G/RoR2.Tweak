namespace BtpTweak.Utils {

    public static class AddressableUtils {

        public static T Load<T>(this string path) => UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(path).WaitForCompletion();

        public static T LoadComponent<T>(this string path) where T : UnityEngine.Component => path.Load<UnityEngine.GameObject>().GetComponent<T>();

        public static T LoadComponentInChildren<T>(this string path) where T : UnityEngine.Component => path.Load<UnityEngine.GameObject>().GetComponentInChildren<T>();

        public static T[] LoadComponents<T>(this string path) where T : UnityEngine.Component => path.Load<UnityEngine.GameObject>().GetComponents<T>();
    }
}