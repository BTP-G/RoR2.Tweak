using Mono.Cecil.Cil;
using MonoMod.Cil;
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
            AssetReferences.elementalRingVoidBlackHole.AddComponent<ElementalRingVoidBlackHoleAction>();
            AssetReferences.bonusMoneyPack.GetComponentInChildren<GravitatePickup>().maxSpeed = 50;
            TryApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.CaptainDefenseMatrix);
            TryApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.NovaOnHeal);
            TryApplyTagToItem(ItemTag.AIBlacklist, RoR2Content.Items.ShockNearby);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.ImmuneToDebuff);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.MinorConstructOnKill);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.BeetleGland);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Behemoth);
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
            RoR2Content.Items.FlatHealth.tags = new ItemTag[] { ItemTag.Healing };
        }

        private void GlobalEventManager_OnHitEnemy(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(MoveType.After, c => c.MatchLdloc(4), c => c.MatchCall<CharacterMaster>("get_inventory"), c => c.MatchStloc(5))) {
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.Emit(OpCodes.Ldloc, 5);
                cursor.EmitDelegate((DamageInfo damageInfo, Inventory attackerInventory) => {
                });
            }
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