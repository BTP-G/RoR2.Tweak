using BTP.RoR2Plugin.Utils;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.MonsterTweaks {

    internal class MagmaWormTweak : TweakBase<MagmaWormTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var bodyObject = GameObjectPaths.MagmaWormBody32.Load<GameObject>();
            var wormBodyPositions2 = bodyObject.GetComponent<WormBodyPositions2>();
            wormBodyPositions2.followDelay = 0.2f;  // 0.6
            wormBodyPositions2.speedMultiplier = 60f;  // 20
            wormBodyPositions2.meatballCount = 8;  // 5
            var wormBodyPositionsDriver = bodyObject.GetComponent<WormBodyPositionsDriver>();
            wormBodyPositionsDriver.maxBreachSpeed = 80f;  // 40f
            wormBodyPositionsDriver.maxTurnSpeed = 720f;  // 360f
        }
    }
}