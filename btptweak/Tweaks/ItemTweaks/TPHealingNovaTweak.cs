using RoR2;
using RoR2.Items;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class TPHealingNovaTweak : TweakBase<TPHealingNovaTweak>, IOnModLoadBehavior {
        public const float HealFraction = 0.02f;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.Pulse += TeleporterHealNovaGeneratorMain_Pulse;
        }

        private void TeleporterHealNovaGeneratorMain_Pulse(On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.orig_Pulse orig, EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain self) {
            orig(self);
            var transform = self.outer.transform;
            var TPHealingWard = Object.Instantiate(MushroomBodyBehavior.mushroomWardPrefab, transform.position, transform.rotation, transform);
            TPHealingWard.GetComponent<TeamFilter>().teamIndex = self.teamIndex;
            var healingWard = TPHealingWard.GetComponent<HealingWard>();
            healingWard.floorWard = false;
            healingWard.interval = 1f;
            healingWard.healFraction = HealFraction * healingWard.interval;
            healingWard.healPoints = 0f;
            healingWard.Networkradius = self.holdoutZone.currentRadius;
            NetworkServer.Spawn(TPHealingWard);
        }
    }
}