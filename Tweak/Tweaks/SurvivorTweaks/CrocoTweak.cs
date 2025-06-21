using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class CrocoTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.Croco.Bite.OnMeleeHitAuthority += Bite_OnMeleeHitAuthority;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            DeepRot.scriptableObject.buffs[0].canStack = true;
        }

        private void Bite_OnMeleeHitAuthority(On.EntityStates.Croco.Bite.orig_OnMeleeHitAuthority orig, EntityStates.Croco.Bite self) {
            orig(self);
            var body = self.characterBody;
            var itemCount = body.inventory.GetItemCount(RoR2Content.Items.Tooth.itemIndex);
            if (itemCount > 0) {
                body.AddTimedBuffAuthority(RoR2Content.Buffs.CrocoRegen.buffIndex, 0.1f * itemCount);
            }
        }
    }
}