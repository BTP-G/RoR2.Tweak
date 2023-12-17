using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class NovaOnLowHealthTweak : TweakBase<NovaOnLowHealthTweak>, IOnModLoadBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.VagrantNovaItem.ReadyState.OnEnter += ReadyState_OnEnter;
        }

        private void ReadyState_OnEnter(On.EntityStates.VagrantNovaItem.ReadyState.orig_OnEnter orig, EntityStates.VagrantNovaItem.ReadyState self) {
            orig(self);
            GlobalEventManager.onServerDamageDealt -= self.OnDamaged;
        }
    }
}