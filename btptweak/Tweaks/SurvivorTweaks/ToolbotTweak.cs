using EntityStates.Toolbot;
using RoR2;
using RoR2.Projectile;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class ToolbotTweak : TweakBase<ToolbotTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        private static float _nailgunBaseMaxDistance;

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.Toolbot.BaseNailgunState.PullCurrentStats += BaseNailgunState_PullCurrentStats;
            On.EntityStates.Toolbot.AimGrenade.OnEnter += AimGrenade_OnEnter;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            _nailgunBaseMaxDistance = BaseNailgunState.maxDistance;
        }

        private void BaseNailgunState_PullCurrentStats(On.EntityStates.Toolbot.BaseNailgunState.orig_PullCurrentStats orig, BaseNailgunState self) {
            BaseNailgunState.maxDistance = _nailgunBaseMaxDistance + 0.07f * self.fireNumber;
            orig(self);
        }

        private void AimGrenade_OnEnter(On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, AimGrenade self) {
            if (self.isAuthority) {
                self.projectilePrefab.GetComponent<ProjectileExplosion>().childrenCount = 5 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.StunChanceOnHit.itemIndex);
            }
            orig(self);
        }
    }
}