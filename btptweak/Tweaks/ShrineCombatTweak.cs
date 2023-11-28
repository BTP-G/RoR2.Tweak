using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class ShrineCombatTweak : TweakBase<ShrineCombatTweak> {

        public override void SetEventHandlers() {
            ShrineCombatBehavior.onDefeatedServerGlobal += ShrineCombatBehavior_onDefeatedServerGlobal;
        }

        public override void ClearEventHandlers() {
            ShrineCombatBehavior.onDefeatedServerGlobal -= ShrineCombatBehavior_onDefeatedServerGlobal;
        }

        private void ShrineCombatBehavior_onDefeatedServerGlobal(ShrineCombatBehavior shrine) {
            var livingPlayerCount = Run.instance.livingPlayerCount;
            var pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableTier1DropList);
            var position = shrine.transform.position + (Vector3.up * 6);
            var velocity = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
            var rotation = Quaternion.AngleAxis(360f / livingPlayerCount, Vector3.up);
            for (int i = 0; i < livingPlayerCount; ++i) {
                PickupDropletController.CreatePickupDroplet(pickupIndex, position, velocity);
                velocity = rotation * velocity;
            }
        }
    }
}