using BTP.RoR2Plugin.Utils;
using EntityStates.GoldGat;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace BTP.RoR2Plugin.Tweaks.EquipmentTweaks {

    internal class GoldGatTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float 半数 = 3f;

        void IModLoadMessageHandler.Handle() {
            On.EntityStates.GoldGat.GoldGatFire.FireBullet += GoldGatFire_FireBullet;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            GoldGatFire.windUpDuration = 10f;
            GoldGatFire.minFireFrequency = 1f;
            GoldGatFire.maxFireFrequency = 10f;
        }

        private void GoldGatFire_FireBullet(On.EntityStates.GoldGat.GoldGatFire.orig_FireBullet orig, GoldGatFire self) {
            self.body.SetAimTimer(2f);
            float t = Mathf.Clamp01(self.totalStopwatch / GoldGatFire.windUpDuration);
            self.fireFrequency = Mathf.Lerp(GoldGatFire.minFireFrequency, GoldGatFire.maxFireFrequency, t);
            var itemCount = self.bodyMaster.inventory.GetItemCount(DLC1Content.Items.GoldOnHurt.itemIndex);
            var scaledCost = Run.instance.GetDifficultyScaledCost(GoldGatFire.baseMoneyCostPerBullet + (int)self.totalStopwatch);
            var costToDamage = Util2.CloseTo(1 + itemCount, 半数, scaledCost);
            if (self.isAuthority) {
                if (self.body.aimOriginTransform) {
                    new BulletAttack {
                        aimVector = self.bodyInputBank.aimDirection,
                        bulletCount = (uint)((int)costToDamage).ToString().Length,
                        damage = self.body.damage * GoldGatFire.damageCoefficient + costToDamage,
                        damageColorIndex = DamageColorIndex.Item,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        force = GoldGatFire.force,
                        isCrit = self.body.RollCrit(),
                        maxSpread = Mathf.Lerp(GoldGatFire.minSpread, GoldGatFire.maxSpread, t),
                        minSpread = 0f,
                        muzzleName = "Muzzle",
                        origin = self.bodyInputBank.aimOrigin,
                        owner = self.networkedBodyAttachment.attachedBodyObject,
                        procCoefficient = GoldGatFire.procCoefficient,
                        tracerEffectPrefab = GoldGatFire.tracerEffectPrefab,
                        weapon = self.gameObject
                    }.Fire();
                    self.gunAnimator?.Play("Fire");
                }
            }
            if (NetworkServer.active) {
                self.bodyMaster.money -= scaledCost > self.bodyMaster.money ? self.bodyMaster.money : (uint)scaledCost;
            }
            self.gunAnimator?.SetFloat("Crank.playbackRate", self.fireFrequency);
            Util.PlaySound(GoldGatFire.attackSoundString, self.gameObject);
            EffectManager.SimpleMuzzleFlash(GoldGatFire.muzzleFlashEffectPrefab, self.gameObject, "Muzzle", false);
        }
    }
}