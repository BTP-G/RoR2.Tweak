namespace BtpTweak {

    /// <summary>适用于 TweakBase 的非抽象派生类</summary>
    internal interface IOnModLoadBehavior {

        /// <summary>Mod加载开始时调用</summary>
        void OnModLoad();
    }
}