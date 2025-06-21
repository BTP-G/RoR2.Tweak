namespace BTP.RoR2Plugin {

    /// <summary>适用于 TweakBase 的非抽象派生类</summary>
    internal interface IModLoadMessageHandler {

        /// <summary>Mod加载开始时调用</summary>
        void Handle();
    }
}