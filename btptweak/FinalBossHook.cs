using RoR2;

namespace BtpTweak {

    internal class FinalBossHook {
        public static bool 处于天文馆_ = false;

        public static void AddHook() {
        }

        public static void RemoveHook() {
        }

        public static void LateInit() {
            HealthHook.虚灵_ = BodyCatalog.FindBodyPrefab("VoidRaidCrabBody").GetComponent<CharacterBody>().bodyIndex;
            HealthHook.老米_ = BodyCatalog.FindBodyPrefab("BrotherBody").GetComponent<CharacterBody>().bodyIndex;
            HealthHook.负伤老米_ = BodyCatalog.FindBodyPrefab("BrotherHurtBody").GetComponent<CharacterBody>().bodyIndex;
        }
    }
}