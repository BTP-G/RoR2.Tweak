namespace BTP.RoR2Plugin {

    /// <summary>适用于 TweakBase 的非抽象派生类</summary>
    internal interface IModUnloadMessageHandler {

        /// <summary>Mod卸载时调用</summary>
        void Handle();
    }
}