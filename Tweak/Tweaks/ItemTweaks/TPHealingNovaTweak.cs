using BTP.RoR2Plugin.Utils;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.ItemTweaks {

    internal class TPHealingNovaTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float HealFraction = 0.02f;
        public static GameObject TeleporterHealNovaWardPrefab { get; private set; } = GameObjectPaths.MushroomWard.Load<GameObject>().InstantiateClone("TeleporterHealNovaWard");

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.OnEnter += TeleporterHealNovaGeneratorMain_OnEnter;
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.FixedUpdate += TeleporterHealNovaGeneratorMain_FixedUpdate;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            Object.Destroy(TeleporterHealNovaWardPrefab.transform.Find("Indicator").transform.Find("MushroomMeshes").gameObject);
            var healingWard = TeleporterHealNovaWardPrefab.GetComponent<HealingWard>();
            healingWard.floorWard = false;
            healingWard.healPoints = 0;
        }

        private void TeleporterHealNovaGeneratorMain_OnEnter(On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.orig_OnEnter orig, EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain self) {
            orig(self);
            var teleporterHealNovaWard = Object.Instantiate(TeleporterHealNovaWardPrefab, self.outer.transform.position, self.outer.transform.rotation, self.outer.transform);
            teleporterHealNovaWard.GetComponent<TeamFilter>().teamIndex = self.teamIndex;
            NetworkServer.Spawn(teleporterHealNovaWard);
        }

        private void TeleporterHealNovaGeneratorMain_FixedUpdate(On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.orig_FixedUpdate orig, EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain self) {
            orig(self);
            var healingWard = self.outer.gameObject.GetComponentInChildren<HealingWard>();
            healingWard.healFraction = HealFraction * healingWard.interval * self.pulseCount;
            healingWard.Networkradius = self.holdoutZone.currentRadius;
        }
    }
}