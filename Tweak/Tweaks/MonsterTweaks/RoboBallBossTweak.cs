using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Skills;

namespace BTP.RoR2Plugin.Tweaks.MonsterTweaks {

    internal class RoboBallBossTweak : ModComponent, IRoR2LoadedMessageHandler {

        void IRoR2LoadedMessageHandler.Handle() {
            SkillDefPaths.FireEyeBeam.Load<SkillDef>().baseRechargeInterval = 3;
            var body = GameObjectPaths.RoboBallBossBody22.LoadComponent<CharacterBody>();
            body.baseAcceleration = 42f;  //14
            body.baseMoveSpeed = 14;  // 7
            body.baseArmor += 20;
        }
    }
}