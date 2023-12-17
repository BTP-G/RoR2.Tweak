using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;

namespace BtpTweak.Tweaks {

    internal class VoidRaidCrabTweak : TweakBase<VoidRaidCrabTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyBase.LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyPhase1.LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyPhase2.LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyPhase3.LoadComponent<CharacterBody>());
        }

        private void AdjustVoidRaidCrabBodyStats(CharacterBody body) {
            body.baseAcceleration = 666f;
            body.baseMoveSpeed = 36f;
            body.baseDamage = 66f;
            body.baseMaxHealth = 60000f;
            body.levelArmor = 1f;
            body.levelDamage = 6.6f;
            body.levelMaxHealth = 18000f;
        }
    }
}