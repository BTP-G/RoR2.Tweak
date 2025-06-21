using EntityStates.Toolbot;
using RoR2;
using RoR2.Projectile;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class ToolbotTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private static float _nailgunBaseMaxDistance;

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.Toolbot.BaseNailgunState.PullCurrentStats += BaseNailgunState_PullCurrentStats;
            On.EntityStates.Toolbot.AimGrenade.OnEnter += AimGrenade_OnEnter;
        }

        void IRoR2LoadedMessageHandler.Handle() {
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