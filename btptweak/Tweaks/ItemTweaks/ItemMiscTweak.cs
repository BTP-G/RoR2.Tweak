using BtpTweak.Utils;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ItemMiscTweak : TweakBase<ItemMiscTweak> {

        public override void SetEventHandlers() {
            RoR2Application.onLoad += Load;
        }

        public override void ClearEventHandlers() {
            RoR2Application.onLoad -= Load;
        }

        public void Load() {
            static void TryApplyTagToItem(ItemTag itemTag, ItemDef itemDef) {
                if (itemDef && itemDef.DoesNotContainTag(itemTag)) {
                    R2API.ItemAPI.ApplyTagToItem(itemTag, itemDef);
                }
            }
            LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/ElementalRingVoidBlackHole").AddComponent<ElementalRingVoidBlackHoleAction>();
            "RoR2/Base/BonusGoldPackOnKill/BonusMoneyPack.prefab".LoadComponentInChildren<GravitatePickup>().maxSpeed = 50;
            TryApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.CaptainDefenseMatrix);
            TryApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.NovaOnHeal);
            TryApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.ShockNearby);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.BearVoid);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.MinorConstructOnKill);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.BeetleGland);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ExecuteLowHealthElite);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.RoboBallBuddy);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ShockNearby);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Thorns);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectAragonite);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBackup);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBarrier);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlackHole);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlighted);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBlue);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectBuffered);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectEarth);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectGold);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectHaunted);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectLunar);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectMoney);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectNight);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectNullifier);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectOppressive);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPlated);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPoison);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectPurity);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectRealgar);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectRed);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectSanguine);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectSepia);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectVeiled);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectVoid);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWarped);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWater);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, TPDespair.ZetAspects.Catalog.Item.ZetAspectWhite);
            TryApplyTagToItem(ItemTag.CannotSteal, DLC1Content.Items.ExtraLifeVoid);
            TryApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            TryApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            TryApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.Infusion);
            RoR2Content.Items.Firework.tags = new ItemTag[] { ItemTag.Damage };
            RoR2Content.Items.FlatHealth.tags = new ItemTag[] { ItemTag.Healing };
        }

        [RequireComponent(typeof(ProjectileExplosion))]
        [RequireComponent(typeof(RadialForce))]
        [RequireComponent(typeof(ProjectileFuse))]
        private class ElementalRingVoidBlackHoleAction : MonoBehaviour {
            private ProjectileExplosion projectileExplosion;
            private ProjectileFuse projectileFuse;
            private RadialForce radialForce;

            private void Awake() {
                projectileExplosion = GetComponent<ProjectileExplosion>();
                projectileFuse = GetComponent<ProjectileFuse>();
                radialForce = GetComponent<RadialForce>();
            }

            private void FixedUpdate() {
                if (projectileFuse.fuse - Time.fixedDeltaTime <= 0) {
                    List<HurtBox> hurtBoxes = new();
                    radialForce.SearchForTargets(hurtBoxes);
                    projectileExplosion.blastDamageCoefficient += hurtBoxes.Count;
                    hurtBoxes.Clear();
                }
            }
        }
    }
}