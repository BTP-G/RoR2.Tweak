using System;

namespace BtpTweak.Tweaks
{

    public abstract class TweakBase<T> : TweakBase, IEventHandlers where T : TweakBase<T> {

        public TweakBase() {
            if (Instance != null) {
                throw new InvalidOperationException("Singleton class " + typeof(T).FullName + " was instantiated twice");
            }
            Instance = this as T;
        }

        public static T Instance { get; private set; }

        public abstract void ClearEventHandlers();

        public abstract void SetEventHandlers();
    }

    public abstract class TweakBase {
    }
}