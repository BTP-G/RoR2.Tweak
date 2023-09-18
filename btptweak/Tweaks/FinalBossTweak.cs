using BtpTweak.Utils;
using RoR2;

namespace BtpTweak.Tweaks {

    internal class FinalBossTweak : TweakBase {

        public override void Load() {
            AdjustVoidRaidCrabBodyStats("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyBase.prefab".LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase1.prefab".LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase2.prefab".LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase3.prefab".LoadComponent<CharacterBody>());
        }

        private void AdjustVoidRaidCrabBodyStats(CharacterBody body) {
            body.baseAcceleration = 600f;
            body.baseMoveSpeed = 36f;
            body.baseDamage = 66f;
            body.baseMaxHealth = 6666f;
            body.levelArmor = 1f;
            body.levelDamage = 6.6f;
            body.levelMaxHealth = 666f;
        }
    }
}