using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace BtpTweak {

    internal class FinalBossHook {
        public static bool 处于天文馆_ = false;

        public static void AddHook() {
        }

        public static void RemoveHook() {
        }

        public static void LateInit() {
            void AdjustVoidRaidCrabBodyStats(GameObject gameObject) {
                CharacterBody body = gameObject.GetComponent<CharacterBody>();
                body.acceleration = 1000;
                body.baseDamage = 18;
                body.baseMaxHealth = 6666;
                body.levelArmor = 1;
                body.levelDamage = 3.6f;
                body.levelMaxHealth = 666;
            }
            AdjustVoidRaidCrabBodyStats(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyBase.prefab").WaitForCompletion());
            AdjustVoidRaidCrabBodyStats(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase1.prefab").WaitForCompletion());
            AdjustVoidRaidCrabBodyStats(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase2.prefab").WaitForCompletion());
            AdjustVoidRaidCrabBodyStats(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase3.prefab").WaitForCompletion());
            HealthHook.老米_ = BodyCatalog.FindBodyPrefab("BrotherBody").GetComponent<CharacterBody>().bodyIndex;
            HealthHook.负伤老米_ = BodyCatalog.FindBodyPrefab("BrotherHurtBody").GetComponent<CharacterBody>().bodyIndex;
        }
    }
}