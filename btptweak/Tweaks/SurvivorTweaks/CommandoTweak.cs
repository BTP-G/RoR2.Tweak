using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class CommandoTweak : TweakBase<CommandoTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float FMJDamageCoefficient = 4f;
        public const float ThrowGrenadeDamageCoefficient = 12f;

        void IOnModLoadBehavior.OnModLoad() {
            EntityStateConfigurationPaths.EntityStatesCommandoCommandoWeaponFireFMJ.Load<EntityStateConfiguration>().Set("damageCoefficient", FMJDamageCoefficient.ToString());
            EntityStateConfigurationPaths.EntityStatesCommandoCommandoWeaponThrowGrenade.Load<EntityStateConfiguration>().Set("damageCoefficient", ThrowGrenadeDamageCoefficient.ToString());
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            EntityStates.Commando.CommandoWeapon.FireBarrage.baseBulletCount = 12;
        }
    }
}