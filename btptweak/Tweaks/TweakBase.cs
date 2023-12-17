using System;

namespace BtpTweak.Tweaks {

    internal abstract class TweakBase<T> : TweakBase where T : TweakBase<T> {

        public TweakBase() {
            if (Instance != null) {
                throw new InvalidOperationException("Singleton class " + typeof(T).FullName + " was instantiated twice");
            }
            Instance = this as T;
        }

        public static T Instance { get; private set; }
    }

    /// <summary>此类的非抽象派生类将在Mod加载时自动实例化，请保留无参构造方法！</summary>
    internal abstract class TweakBase { }
}