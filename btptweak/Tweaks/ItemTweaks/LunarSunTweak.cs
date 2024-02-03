using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class LunarSunTweak : TweakBase<LunarSunTweak>, IOnModLoadBehavior {
        public const float BaseDamageCoefficient = 3.6f;
        public const float StackDamageCoefficient = 1.8f;
        public const float SecondsPerProjectile = 3.6f;
        public const float SecondsPerTransform = 36f;

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.LunarSunBehavior.FixedUpdate += LunarSunBehavior_FixedUpdate;
        }

        private void LunarSunBehavior_FixedUpdate(On.RoR2.LunarSunBehavior.orig_FixedUpdate orig, LunarSunBehavior self) {
            if ((self.projectileTimer += Time.fixedDeltaTime) > SecondsPerProjectile / self.stack
                && self.body.master.IsDeployableSlotAvailable(DeployableSlot.LunarSunBomb)) {
                var fireProjectileInfo = new FireProjectileInfo {
                    projectilePrefab = self.projectilePrefab,
                    crit = self.body.RollCrit(),
                    damage = (BaseDamageCoefficient + StackDamageCoefficient * (self.stack - 1)) * self.body.damage,
                    damageColorIndex = DamageColorIndex.Item,
                    force = 0f,
                    owner = self.gameObject,
                    position = self.body.corePosition,
                    rotation = Quaternion.identity
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                self.projectileTimer = 0f;
            }
            if ((self.transformTimer += Time.fixedDeltaTime) > SecondsPerTransform) {
                var inventory = self.body.inventory;
                var itemIndex = self.transformRng.NextElementUniform(inventory.itemAcquisitionOrder);
                if (itemIndex != DLC1Content.Items.LunarSun.itemIndex) {
                    var itemDef = ItemCatalog.GetItemDef(itemIndex);
                    if (itemDef && itemDef.tier != ItemTier.NoTier) {
                        inventory.RemoveItem(itemIndex);
                        inventory.GiveItem(DLC1Content.Items.LunarSun.itemIndex);
                        self.transformTimer = 0f;
                        CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, itemIndex, DLC1Content.Items.LunarSun.itemIndex, CharacterMasterNotificationQueue.TransformationType.LunarSun);
                    }
                }
            }
        }
    }
}