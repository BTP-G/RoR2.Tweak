using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Skills;

namespace BTP.RoR2Plugin.Tweaks.MonsterTweaks {

    internal class RoboBallBossTweak : TweakBase<RoboBallBossTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            SkillDefPaths.FireEyeBeam.Load<SkillDef>().baseRechargeInterval = 3;
            var body = GameObjectPaths.RoboBallBossBody22.LoadComponent<CharacterBody>();
            body.baseAcceleration = 42f;  //14
            body.baseMoveSpeed = 14;  // 7
            body.baseArmor += 20;
        }
    }
}