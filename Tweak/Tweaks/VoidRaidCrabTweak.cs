using BTP.RoR2Plugin.Utils;
using EntityStates.VoidRaidCrab;
using RoR2;

namespace BTP.RoR2Plugin.Tweaks {

    internal class VoidRaidCrabTweak : TweakBase<VoidRaidCrabTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.VoidRaidCrab.SpinBeamAttack.OnEnter += SpinBeamAttack_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyBase.LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyPhase1.LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyPhase2.LoadComponent<CharacterBody>());
            AdjustVoidRaidCrabBodyStats(GameObjectPaths.MiniVoidRaidCrabBodyPhase3.LoadComponent<CharacterBody>());
        }

        private void SpinBeamAttack_OnEnter(On.EntityStates.VoidRaidCrab.SpinBeamAttack.orig_OnEnter orig, SpinBeamAttack self) {
            self.headForwardYCurve.RemoveKey(1);
            self.headForwardYCurve.AddKey(1, 0.3f);
            orig(self);
        }

        private void AdjustVoidRaidCrabBodyStats(CharacterBody body) {
            body.baseAcceleration = 666f;
            body.baseMoveSpeed = 36f;
            body.baseDamage = 66f;
            body.baseMaxHealth = 60000f;
            body.levelArmor = 1f;
            body.levelDamage = 6.6f;
            body.levelMaxHealth = 18000f;
        }
    }
}