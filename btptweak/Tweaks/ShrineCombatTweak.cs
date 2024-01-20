using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks {

    internal class ShrineCombatTweak : TweakBase<ShrineCombatTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.Util.DirectorCardIsReasonableChoice += Util_DirectorCardIsReasonableChoice;
            ShrineCombatBehavior.onDefeatedServerGlobal += ShrineCombatBehavior_onDefeatedServerGlobal;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            GameObjectPaths.ShrineCombat.LoadComponent<CombatDirector>().ignoreTeamSizeLimit = false;
            GameObjectPaths.ShrineCombatSandyVariant.LoadComponent<CombatDirector>().ignoreTeamSizeLimit = false;
            GameObjectPaths.ShrineCombatSnowyVariant.LoadComponent<CombatDirector>().ignoreTeamSizeLimit = false;
        }

        private bool Util_DirectorCardIsReasonableChoice(On.RoR2.Util.orig_DirectorCardIsReasonableChoice orig, float availableCredit, int maximumNumberToSpawnBeforeSkipping, int minimumToSpawn, DirectorCard card, float combatDirectorHighestEliteCostMultiplier) {
            return card.IsAvailable() && card.cost * minimumToSpawn <= availableCredit;
        }

        private void ShrineCombatBehavior_onDefeatedServerGlobal(ShrineCombatBehavior shrine) {
            var livingPlayerCount = Run.instance.livingPlayerCount;
            var rng = new Xoroshiro128Plus(Run.instance.treasureRng.nextUlong);
            var pickupIndex = PickupIndex.none;
            if (Random.value < 0.75f) {
                if (Random.value < 0.25f) {
                    if (Random.value < 0.1f) {
                        pickupIndex = rng.NextElementUniform(Run.instance.availableTier3DropList);
                    } else {
                        pickupIndex = rng.NextElementUniform(Run.instance.availableTier2DropList);
                    }
                } else {
                    pickupIndex = rng.NextElementUniform(Run.instance.availableTier1DropList);
                }
            }
            if (pickupIndex == PickupIndex.none) {
                ChatMessage.Send("挑战完成后什么也没有发生。".ToShrine());
                return;
            }
            var position = shrine.transform.position + (Vector3.up * 6);
            var velocity = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
            var rotation = Quaternion.AngleAxis(360f / livingPlayerCount, Vector3.up);
            for (int i = 0; i < livingPlayerCount; ++i) {
                PickupDropletController.CreatePickupDroplet(pickupIndex, position, velocity);
                velocity = rotation * velocity;
            }
            ChatMessage.Send("挑战完成，给予挑战者奖励。".ToShrine());
        }
    }
}