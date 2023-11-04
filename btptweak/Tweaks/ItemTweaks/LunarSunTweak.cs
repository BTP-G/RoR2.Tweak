using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class LunarSunTweak : TweakBase<LunarSunTweak> {

        public override void SetEventHandlers() {
            On.RoR2.LunarSunBehavior.FixedUpdate += LunarSunBehavior_FixedUpdate;
        }

        public override void ClearEventHandlers() {
            On.RoR2.LunarSunBehavior.FixedUpdate -= LunarSunBehavior_FixedUpdate;
        }

        private void LunarSunBehavior_FixedUpdate(On.RoR2.LunarSunBehavior.orig_FixedUpdate orig, RoR2.LunarSunBehavior self) {
            self.projectileTimer += Time.fixedDeltaTime;
            if (!self.body.master.IsDeployableLimited(DeployableSlot.LunarSunBomb) && self.projectileTimer > 3f / self.stack) {
                self.projectileTimer = 0f;
                FireProjectileInfo fireProjectileInfo = new() {
                    projectilePrefab = self.projectilePrefab,
                    crit = self.body.RollCrit(),
                    damage = self.body.damage * 3.6f,
                    damageColorIndex = DamageColorIndex.Item,
                    force = 0f,
                    owner = self.gameObject,
                    position = self.body.transform.position,
                    rotation = Quaternion.identity
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            self.transformTimer += Time.fixedDeltaTime;
            if (self.transformTimer > 30f) {
                self.transformTimer = 0f;
                if (self.body.master && self.body.inventory) {
                    Inventory inventory = self.body.inventory;
                    ItemIndex itemIndex = ItemIndex.None;
                    Util.ShuffleList(inventory.itemAcquisitionOrder, self.transformRng);
                    foreach (ItemIndex itemIndex2 in inventory.itemAcquisitionOrder) {
                        if (itemIndex2 != DLC1Content.Items.LunarSun.itemIndex) {
                            ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex2);
                            if (itemDef && itemDef.tier != ItemTier.NoTier && itemDef.DoesNotContainTag(ItemTag.CannotSteal)) {
                                itemIndex = itemIndex2;
                                break;
                            }
                        }
                    }
                    if (itemIndex != ItemIndex.None) {
                        inventory.RemoveItem(itemIndex);
                        inventory.GiveItem(DLC1Content.Items.LunarSun.itemIndex);
                        CharacterMasterNotificationQueue.SendTransformNotification(self.body.master, itemIndex, DLC1Content.Items.LunarSun.itemIndex, CharacterMasterNotificationQueue.TransformationType.LunarSun);
                    }
                }
            }
        }
    }
}