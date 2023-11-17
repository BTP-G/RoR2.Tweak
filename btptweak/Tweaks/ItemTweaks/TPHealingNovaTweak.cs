using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class TPHealingNovaTweak : TweakBase<TPHealingNovaTweak> {
        public const float RegenBuffDuration = 1f;

        public override void SetEventHandlers() {
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.OnEnter += TeleporterHealNovaGeneratorMain_OnEnter;
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.Pulse += TeleporterHealNovaGeneratorMain_Pulse;
        }

        public override void ClearEventHandlers() {
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.OnEnter -= TeleporterHealNovaGeneratorMain_OnEnter;
            On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.Pulse -= TeleporterHealNovaGeneratorMain_Pulse;
        }

        private void TeleporterHealNovaGeneratorMain_Pulse(On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.orig_Pulse orig, EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain self) {
            orig(self);
            var holdoutZone = self.holdoutZone;
            if (holdoutZone.buffWard) {
                holdoutZone.buffWard.buffDuration = Util.GetItemCountForTeam(self.teamIndex, RoR2Content.Items.TPHealingNova.itemIndex, false, false);
            } else {
                var buffWard = holdoutZone.buffWard = self.gameObject.GetComponent<BuffWard>() ?? self.gameObject.AddComponent<BuffWard>();
                buffWard.buffDef = RoR2Content.Buffs.CrocoRegen;
                buffWard.buffDuration = RegenBuffDuration * Util.GetItemCountForTeam(holdoutZone.chargingTeam, RoR2Content.Items.TPHealingNova.itemIndex, false, false);
                buffWard.interval = 1f;
                buffWard.requireGrounded = false;
                buffWard.shape = BuffWard.BuffWardShape.Sphere;
                buffWard.animateRadius = false;
                buffWard.expires = false;
            }
        }

        private void TeleporterHealNovaGeneratorMain_OnEnter(On.EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain.orig_OnEnter orig, EntityStates.TeleporterHealNovaController.TeleporterHealNovaGeneratorMain self) {
            orig(self);
            var holdoutZone = self.holdoutZone;
            var buffWard = holdoutZone.buffWard = self.gameObject.GetComponent<BuffWard>() ?? self.gameObject.AddComponent<BuffWard>();
            buffWard.buffDef = RoR2Content.Buffs.CrocoRegen;
            buffWard.buffDuration = RegenBuffDuration * Util.GetItemCountForTeam(holdoutZone.chargingTeam, RoR2Content.Items.TPHealingNova.itemIndex, false, false);
            buffWard.interval = 1f;
            buffWard.requireGrounded = false;
            buffWard.shape = BuffWard.BuffWardShape.Sphere;
            buffWard.animateRadius = false;
            buffWard.expires = false;
        }
    }
}