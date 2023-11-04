using UnityEngine;

namespace BtpTweak.Utils {

    public static class ComponentExtension {

        public static T AddComponent<T>(this Component self) where T : Component => self.gameObject.AddComponent<T>();
    } 
}