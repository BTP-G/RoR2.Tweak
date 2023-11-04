using UnityEngine;

namespace BtpTweak.Utils {

    public static class GameObjectExtension {

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component => Object.Destroy(gameObject.GetComponent<T>());

        public static void RemoveComponents<T>(this GameObject gameObject) where T : Component {
            T[] coms = gameObject.GetComponents<T>();
            for (int i = 0; i < coms.Length; i++) {
                Object.Destroy(coms[i]);
            }
        }
    }
}