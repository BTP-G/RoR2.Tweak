using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using EntityStates;
using EntityStates.BrotherMonster;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace BtpTweak {

    public static class BtpContent {

        public static class Buffs {
            public static BuffDef VoidFire { get; private set; }

            [RuntimeInitializeOnLoadMethod]
            private static void InitBuffs() {
                VoidFire = ScriptableObject.CreateInstance<BuffDef>();
                VoidFire.name = "Void Fire";
                VoidFire.iconSprite = Texture2DPaths.texBuffVoidFog.Load<Sprite>();
                VoidFire.buffColor = Color.red;
                VoidFire.canStack = false;
                VoidFire.isHidden = true;
                VoidFire.isDebuff = false;
                VoidFire.isCooldown = false;
                if (!ContentAddition.AddBuffDef(VoidFire)) {
                    Main.Logger.LogError("Buff '" + VoidFire.name + "' failed to be added!");
                }
            }
        }

        public static class Items {
            public static ItemDef MoonscourgeAccursedItem { get; private set; }

            public static ItemDef StormscourgeAccursedItem { get; private set; }

            public static ItemDef HelscourgeAccursedItemDef { get; private set; }

            [RuntimeInitializeOnLoadMethod]
            private static void InitItems() {
                CreateMoonscourgeItem();
                CreateStormscourgeItem();
                CreateHelscourgeItem();
                On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            }

            private static void CreateMoonscourgeItem() {
                MoonscourgeAccursedItem = ScriptableObject.CreateInstance<ItemDef>();
                MoonscourgeAccursedItem.name = "MoonscourgeAccursedItem";
                MoonscourgeAccursedItem.nameToken = "ACCURSEDMITHRIX_ITEM1_NAME";
                MoonscourgeAccursedItem.pickupToken = "ACCURSEDMITHRIX_ITEM1_PICKUP";
                MoonscourgeAccursedItem.descriptionToken = "ACCURSEDMITHRIX_ITEM1_DESC";
                MoonscourgeAccursedItem.tier = ItemTier.NoTier;
                MoonscourgeAccursedItem.deprecatedTier = ItemTier.NoTier;
                if (!ContentAddition.AddItemDef(MoonscourgeAccursedItem)) {
                    Main.Logger.LogError("AddItemDef :: MoonscourgeAccursedItem Failed!");
                }
            }

            private static void CreateStormscourgeItem() {
                StormscourgeAccursedItem = ScriptableObject.CreateInstance<ItemDef>();
                StormscourgeAccursedItem.name = "StormscourgeAccursedItem";
                StormscourgeAccursedItem.nameToken = "ACCURSEDMITHRIX_ITEM2_NAME";
                StormscourgeAccursedItem.pickupToken = "ACCURSEDMITHRIX_ITEM2_PICKUP";
                StormscourgeAccursedItem.descriptionToken = "ACCURSEDMITHRIX_ITEM2_DESC";
                StormscourgeAccursedItem.tier = ItemTier.NoTier;
                StormscourgeAccursedItem.deprecatedTier = ItemTier.NoTier;
                if (!ContentAddition.AddItemDef(StormscourgeAccursedItem)) {
                    Main.Logger.LogError("AddItemDef :: StormscourgeAccursedItem Failed!");
                }
            }

            private static void CreateHelscourgeItem() {
                HelscourgeAccursedItemDef = ScriptableObject.CreateInstance<ItemDef>();
                HelscourgeAccursedItemDef.name = "HelscourgeAccursedItem";
                HelscourgeAccursedItemDef.nameToken = "ACCURSEDMITHRIX_ITEM3_NAME";
                HelscourgeAccursedItemDef.pickupToken = "ACCURSEDMITHRIX_ITEM3_PICKUP";
                HelscourgeAccursedItemDef.descriptionToken = "ACCURSEDMITHRIX_ITEM3_DESC";
                HelscourgeAccursedItemDef.tier = ItemTier.NoTier;
                HelscourgeAccursedItemDef.deprecatedTier = ItemTier.NoTier;
                if (!ContentAddition.AddItemDef(HelscourgeAccursedItemDef)) {
                    Main.Logger.LogError("AddItemDef :: HelscourgeAccursedItemDef Failed!");
                }
            }

            private static void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self) {
                orig(self);
                if (self.body?.bodyIndex == BodyIndexes.Brother && self.activeOverlayCount < CharacterModel.maxOverlays) {
                    var inventory = self.body.inventory;
                    if (inventory.GetItemCount(HelscourgeAccursedItemDef) > 0) {
                        self.currentOverlays[self.activeOverlayCount++] = AssetReferences.helscourgeMat;
                    } else if (inventory.GetItemCount(StormscourgeAccursedItem) > 0) {
                        self.currentOverlays[self.activeOverlayCount++] = AssetReferences.stormscourgeMat;
                    } else if (inventory.GetItemCount(MoonscourgeAccursedItem) > 0) {
                        self.currentOverlays[self.activeOverlayCount++] = AssetReferences.moonscourgeMat;
                    }
                }
            }
        }

        public static class Skills {
            public static SkillDef UngroundedDash { get; private set; }

            [RuntimeInitializeOnLoadMethod]
            private static void Init() {
                CreateUngroundedDash();
            }

            private static void CreateUngroundedDash() {
                UngroundedDash = ScriptableObject.CreateInstance<SkillDef>();
                UngroundedDash.skillName = "LunarDash";
                UngroundedDash.activationState = new SerializableEntityStateType(typeof(SlideIntroState));
                UngroundedDash.activationStateMachineName = "Body";
                UngroundedDash.baseMaxStock = 2;
                UngroundedDash.baseRechargeInterval = 3f;
                UngroundedDash.beginSkillCooldownOnSkillEnd = false;
                UngroundedDash.canceledFromSprinting = false;
                UngroundedDash.cancelSprintingOnActivation = true;
                UngroundedDash.dontAllowPastMaxStocks = false;
                UngroundedDash.forceSprintDuringState = false;
                UngroundedDash.fullRestockOnAssign = true;
                UngroundedDash.interruptPriority = InterruptPriority.Skill;
                UngroundedDash.isCombatSkill = false;
                UngroundedDash.rechargeStock = 2;
                UngroundedDash.requiredStock = 1;
                UngroundedDash.stockToConsume = 1;
                if (ContentAddition.AddSkillDef(UngroundedDash)) {
                    return;
                }
                Main.Logger.LogError("CreateUngroundedDash failed!");
            }
        }
    }
}