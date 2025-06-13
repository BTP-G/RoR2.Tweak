using BTP.RoR2Plugin.Utils;
using EntityStates.TitanMonster;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.MonsterTweaks {

    internal class TitanTweak : TweakBase<TitanTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var body = GameObjectPaths.TitanBody13.LoadComponent<CharacterBody>();
            body.baseArmor += 30f;
            body.baseAcceleration = 24f;
            body.baseMoveSpeed = 8f;
            body.baseAttackSpeed = 1.5f;
            FireMegaLaser.minimumDuration = FireMegaLaser.maximumDuration * 0.5f;
            FireFist.entryDuration = 1.5f;  // 2
            FireFist.exitDuration = 2;  // 3
            var titanRockController = RechargeRocks.rockControllerPrefab.GetComponent<TitanRockController>();
            titanRockController.damageCoefficient *= 1.5f;
            titanRockController.fireInterval *= 0.5f;
            titanRockController.startDelay *= 0.75f;
        }
    }
}