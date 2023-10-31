using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class CrocoTweak : TweakBase {

        public override void AddHooks() {
            base.AddHooks();
            On.EntityStates.Croco.Bite.OnEnter += Bite_OnEnter;
        }

        public override void Load() {
            base.Load();
            DeepRot.scriptableObject.buffs[0].canStack = true;
        }

        private void Bite_OnEnter(On.EntityStates.Croco.Bite.orig_OnEnter orig, EntityStates.Croco.Bite self) {
            if (self.isAuthority) {
                self.damageCoefficient += self.characterBody.inventory.GetItemCount(RoR2Content.Items.Tooth.itemIndex);
            }
            orig(self);
        }
    }
}