using BTP.RoR2Plugin.Utils;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.MonsterTweaks {

    internal class VagrantTweak : ModComponent, IRoR2LoadedMessageHandler {

        void IRoR2LoadedMessageHandler.Handle() {
            var body = GameObjectPaths.VagrantBody15.LoadComponent<CharacterBody>();
            body.baseMaxHealth *= 1.25f;
            body.levelMaxHealth *= 1.25f;
            body.baseAcceleration = 27f;  // 15
            body.baseMoveSpeed = 9f;  // 6
            body.baseAttackSpeed = 1.5f;  // 1
        }
    }
}