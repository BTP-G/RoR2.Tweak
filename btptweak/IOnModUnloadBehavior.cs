namespace BtpTweak {

    /// <summary>适用于 TweakBase 的非抽象派生类</summary>
    internal interface IOnModUnloadBehavior {

        /// <summary>Mod卸载时调用</summary>
        void OnModUnload();
    }
}