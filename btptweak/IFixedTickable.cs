namespace BTP.RoR2Plugin {

    internal interface IFixedTickable {

        void FixedTick();
    }

    internal static class FixedTickableExtensions {

        public static void RegisterFixedTick(this IFixedTickable fixedTickable) {
            Plugin.fixedTickableSet.Add(fixedTickable);
        }

        public static void UnregisterFixedTick(this IFixedTickable fixedTickable) {
            Plugin.fixedTickableSet.Remove(fixedTickable);
        }
    }
}