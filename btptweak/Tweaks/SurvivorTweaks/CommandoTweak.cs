namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class CommandoTweak : TweakBase<CommandoTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            EntityStates.Commando.CommandoWeapon.FireBarrage.baseBulletCount = 12;
        }
    }
}