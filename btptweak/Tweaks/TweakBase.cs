using RoR2;
using System.Collections.Generic;

namespace BtpTweak.Tweaks
{

    internal abstract class TweakBase
    {

        public TweakBase()
        {
            Instances.Add(this);
        }

        public static List<TweakBase> Instances { get; } = new List<TweakBase>();

        public virtual void AddHooks()
        {
            Main.logger_.LogMessage(GetType().Name + "已添加钩子！");
        }

        public virtual void Load()
        {
            Main.logger_.LogMessage(GetType().Name + "已加载！");
        }

        public virtual void RemoveHooks()
        {
            Main.logger_.LogMessage(GetType().Name + "已移除钩子！");
        }

        public virtual void RunStartAction(Run run)
        {
            Main.logger_.LogMessage(GetType().Name + "已调用RunStartAction。");
        }

        public virtual void StageStartAction(Stage stage)
        {
            Main.logger_.LogMessage(GetType().Name + "已调用StageStartAction。");
        }
    }
}