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
                    Main.Logger.LogError("Buff '" + VoidFire.name + "' failed to be added!");
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
                    Main.Logger.LogError("Buff '" + DroneCommanderSpawnCooldown.name + "' failed to be added!");
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
                Main.Logger.LogError("CreateUngroundedDash failed!");
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
                    Main.Logger.LogError($"Difficulties {造物.nameToken} add failed!");
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