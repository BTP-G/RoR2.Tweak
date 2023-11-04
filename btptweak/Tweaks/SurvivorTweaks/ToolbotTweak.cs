using EntityStates.Toolbot;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class ToolbotTweak : TweakBase<ToolbotTweak> {

        public override void SetEventHandlers() {
            On.EntityStates.Toolbot.BaseNailgunState.FireBullet += BaseNailgunState_FireBullet;
            On.EntityStates.Toolbot.AimGrenade.OnEnter += AimGrenade_OnEnter;
        }

        public override void ClearEventHandlers() {
            On.EntityStates.Toolbot.BaseNailgunState.FireBullet -= BaseNailgunState_FireBullet;
            On.EntityStates.Toolbot.AimGrenade.OnEnter -= AimGrenade_OnEnter;
        }

        private void AimGrenade_OnEnter(On.EntityStates.Toolbot.AimGrenade.orig_OnEnter orig, AimGrenade self) {
            if (self.isAuthority) {
                self.projectilePrefab.GetComponent<ProjectileExplosion>().childrenCount = 5 + self.characterBody.inventory.GetItemCount(RoR2Content.Items.StunChanceOnHit.itemIndex);
            }
            orig(self);
        }

        private void BaseNailgunState_FireBullet(On.EntityStates.Toolbot.BaseNailgunState.orig_FireBullet orig, BaseNailgunState self, Ray aimRay, int bulletCount, float spreadPitchScale, float spreadYawScale) {
            float tmp = BaseNailgunState.maxDistance;
            BaseNailgunState.maxDistance += 0.07f * self.fireNumber;
            orig(self, aimRay, bulletCount, spreadPitchScale, spreadYawScale);
            BaseNailgunState.maxDistance = tmp;
        }
    }
}