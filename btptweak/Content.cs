using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using EntityStates;
using EntityStates.BrotherMonster;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace BTP.RoR2Plugin {

    /// <summary>新增内容</summary>
    public static class Content {

        public static class Buffs {
            public static BuffDef VoidFire { get; private set; }
            public static BuffDef DroneCommanderSpawnCooldown { get; private set; }

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
                    ("Buff '" + VoidFire.name + "' failed to be added!").LogError();
                }
                DroneCommanderSpawnCooldown = ScriptableObject.CreateInstance<BuffDef>();
                DroneCommanderSpawnCooldown.name = "DroneCommander SpawnCooldown";
                DroneCommanderSpawnCooldown.iconSprite = Texture2DPaths.texDroneWeaponsIcon.Load<Sprite>();
                //DroneCommanderSpawnCooldown.buffColor = Color.gray;
                DroneCommanderSpawnCooldown.canStack = false;
                DroneCommanderSpawnCooldown.isHidden = false;
                DroneCommanderSpawnCooldown.isDebuff = false;
                DroneCommanderSpawnCooldown.isCooldown = true;
                if (!ContentAddition.AddBuffDef(DroneCommanderSpawnCooldown)) {
                    ("Buff '" + DroneCommanderSpawnCooldown.name + "' failed to be added!").LogError();
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
                MoonscourgeAccursedItem.loreToken = "ACCURSEDMITHRIX_ITEM1_LORE";
                MoonscourgeAccursedItem.tier = ItemTier.NoTier;
                MoonscourgeAccursedItem.deprecatedTier = ItemTier.NoTier;
                MoonscourgeAccursedItem.TryApplyTag(ItemTag.WorldUnique);
                MoonscourgeAccursedItem.hidden = true;
                if (!ContentAddition.AddItemDef(MoonscourgeAccursedItem)) {
                    "AddItemDef :: MoonscourgeAccursedItem Failed!".LogError();
                }
            }

            private static void CreateStormscourgeItem() {
                StormscourgeAccursedItem = ScriptableObject.CreateInstance<ItemDef>();
                StormscourgeAccursedItem.name = "StormscourgeAccursedItem";
                StormscourgeAccursedItem.nameToken = "ACCURSEDMITHRIX_ITEM2_NAME";
                StormscourgeAccursedItem.pickupToken = "ACCURSEDMITHRIX_ITEM2_PICKUP";
                StormscourgeAccursedItem.descriptionToken = "ACCURSEDMITHRIX_ITEM2_DESC";
                StormscourgeAccursedItem.loreToken = "ACCURSEDMITHRIX_ITEM2_LORE";
                StormscourgeAccursedItem.tier = ItemTier.NoTier;
                StormscourgeAccursedItem.deprecatedTier = ItemTier.NoTier;
                StormscourgeAccursedItem.TryApplyTag(ItemTag.WorldUnique);
                StormscourgeAccursedItem.hidden = true;
                if (!ContentAddition.AddItemDef(StormscourgeAccursedItem)) {
                    "AddItemDef :: StormscourgeAccursedItem Failed!".LogError();
                }
            }

            private static void CreateHelscourgeItem() {
                HelscourgeAccursedItemDef = ScriptableObject.CreateInstance<ItemDef>();
                HelscourgeAccursedItemDef.name = "HelscourgeAccursedItem";
                HelscourgeAccursedItemDef.nameToken = "ACCURSEDMITHRIX_ITEM3_NAME";
                HelscourgeAccursedItemDef.pickupToken = "ACCURSEDMITHRIX_ITEM3_PICKUP";
                HelscourgeAccursedItemDef.descriptionToken = "ACCURSEDMITHRIX_ITEM3_DESC";
                HelscourgeAccursedItemDef.loreToken = "ACCURSEDMITHRIX_ITEM3_LORE";
                HelscourgeAccursedItemDef.tier = ItemTier.NoTier;
                HelscourgeAccursedItemDef.deprecatedTier = ItemTier.NoTier;
                HelscourgeAccursedItemDef.TryApplyTag(ItemTag.WorldUnique);
                HelscourgeAccursedItemDef.hidden = true;
                if (!ContentAddition.AddItemDef(HelscourgeAccursedItemDef)) {
                    "AddItemDef :: HelscourgeAccursedItemDef Failed!".LogError();
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
                UngroundedDash.skillNameToken = "LUNARDASH_NAME";
                UngroundedDash.skillDescriptionToken = "LUNARDASH_DESCRIPTION";
                UngroundedDash.activationState = new SerializableEntityStateType(typeof(SlideIntroState));
                UngroundedDash.activationStateMachineName = "Body";
                UngroundedDash.baseMaxStock = 1;
                UngroundedDash.baseRechargeInterval = 5f;
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
                "CreateUngroundedDash failed!".LogError();
            }
        }

        public static class Difficulties {
            public static DifficultyDef 造物 { get; private set; }
            public static DifficultyIndex 造物索引 { get; private set; }

            [RuntimeInitializeOnLoadMethod]
            private static void Init() {
                造物 = new DifficultyDef(3f,
                                       "DIFFICULTY_CREATION_NAME",
                                       null,
                                       "DIFFICULTY_CREATION_DESC",
                                       ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidCoin),
                                       "btp",
                                       true);
                造物索引 = DifficultyAPI.AddDifficulty(造物, Texture2DPaths.texVoidCoinIcon.Load<Sprite>());
                if (造物索引 == DifficultyIndex.Invalid) {
                    $"Difficulties {造物.nameToken} add failed!".LogError();
                }
                Localizer.AddOverlay("DIFFICULTY_CREATION_NAME", "造物");
                Localizer.AddOverlay("DIFFICULTY_CREATION_DESC", "准备好面对造物主的试炼了吗？\n\n".ToRainbowWavy()
                    + "以标准的季风难度开局，但难度随情况变化。\n\n".ToShaky()
                    + ">玩家根据所选角色获得初始物品\n".ToHealing()
                    + ">敌人将会根据情况获得各种增益\n".ToDeath()
                    + ">充能半径将与充能进度成反比\n".ToDeath()
                    + ">未充能时进度缓慢衰减\n".ToDeath());
            }
        }
    }
}