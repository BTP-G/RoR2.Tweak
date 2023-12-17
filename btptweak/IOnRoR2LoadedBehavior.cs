namespace BtpTweak {

    /// <summary>适用于 TweakBase 的非抽象派生类</summary>
    internal interface IOnRoR2LoadedBehavior {

        /// <summary>游戏本体加载完成时调用</summary>
        void OnRoR2Loaded();
    }
}