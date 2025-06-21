using BTP.RoR2Plugin.Language;
using BTP.RoR2Plugin.Utils;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks {

    internal class ShrineCombatTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {

        void IModLoadMessageHandler.Handle() {
            On.RoR2.Util.DirectorCardIsReasonableChoice += Util_DirectorCardIsReasonableChoice;
            ShrineCombatBehavior.onDefeatedServerGlobal += ShrineCombatBehavior_onDefeatedServerGlobal;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            GameObjectPaths.ShrineCombat.LoadComponent<CombatDirector>().ignoreTeamSizeLimit = false;
            GameObjectPaths.ShrineCombatSandyVariant.LoadComponent<CombatDirector>().ignoreTeamSizeLimit = false;
            GameObjectPaths.ShrineCombatSnowyVariant.LoadComponent<CombatDirector>().ignoreTeamSizeLimit = false;
        }

        private bool Util_DirectorCardIsReasonableChoice(On.RoR2.Util.orig_DirectorCardIsReasonableChoice orig, float availableCredit, int maximumNumberToSpawnBeforeSkipping, int minimumToSpawn, DirectorCard card, float combatDirectorHighestEliteCostMultiplier) {
            return card.IsAvailable() && card.cost * minimumToSpawn <= availableCredit;
        }

        private void ShrineCombatBehavior_onDefeatedServerGlobal(ShrineCombatBehavior shrine) {
            var rng = new Xoroshiro128Plus(Run.instance.treasureRng.nextUlong);
            PickupIndex pickupIndex;
            if (rng.nextNormalizedFloat < 0.6f) {
                if (rng.nextNormalizedFloat < 0.3f) {
                    if (rng.nextNormalizedFloat < 0.2f) {
                        pickupIndex = rng.NextElementUniform(Run.instance.availableTier3DropList);
                    } else {
                        pickupIndex = rng.NextElementUniform(Run.instance.availableTier2DropList);
                    }
                } else {
                    pickupIndex = rng.NextElementUniform(Run.instance.availableTier1DropList);
                }
            } else {
                pickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.MiscPickups.LunarCoin.miscPickupIndex);
            }
            if (pickupIndex == PickupIndex.none) {
                ChatMessage.Send("挑战完成后什么也没有发生。".ToShrine());
                return;
            }
            var playerCount = Run.instance.participatingPlayerCount;
            var position = shrine.transform.position + Vector3.up * 6;
            var velocity = Quaternion.AngleAxis(Random.Range(0, 360f), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
            var rotation = Quaternion.AngleAxis(360f / playerCount, Vector3.up);
            for (int i = 0; i < playerCount; ++i) {
                PickupDropletController.CreatePickupDroplet(pickupIndex, position, velocity);
                velocity = rotation * velocity;
            }
            ChatMessage.Send("挑战完成，给予挑战者奖励。".ToShrine());
        }
    }
}