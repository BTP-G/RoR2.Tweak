using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class ItemMiscTweak : TweakBase<ItemMiscTweak>, IOnRoR2LoadedBehavior {

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
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
            TryApplyTagToItem(ItemTag.BrotherBlacklist, DLC1Content.Items.MinorConstructOnKill);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.BeetleGland);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Behemoth);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Dagger);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ExecuteLowHealthElite);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ExplodeOnDeath);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.IgniteOnKill);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.NovaOnLowHealth);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.RoboBallBuddy);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.ShockNearby);
            TryApplyTagToItem(ItemTag.BrotherBlacklist, RoR2Content.Items.Thorns);
            TryApplyTagToItem(ItemTag.CannotSteal, DLC1Content.Items.ExtraLifeVoid);
            TryApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.CaptainDefenseMatrix);
            TryApplyTagToItem(ItemTag.CannotSteal, RoR2Content.Items.ExtraLife);
            foreach (var item in ItemCatalog.allItemDefs) {
                TryApplyTagToItem(ItemTag.BrotherBlacklist, item);
            }
            var tags = RoR2Content.Items.TonicAffliction.tags.ToList();
            tags.Remove(ItemTag.CannotSteal);
            RoR2Content.Items.TonicAffliction.tags = [.. tags];
            RoR2Content.Items.FlatHealth.tags = [ItemTag.Healing];
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
                    List<HurtBox> hurtBoxes = [];
                    radialForce.SearchForTargets(hurtBoxes);
                    projectileExplosion.blastDamageCoefficient += hurtBoxes.Count;
                    hurtBoxes.Clear();
                }
            }
        }
    }
}