using BtpTweak.Utils;
using EntityStates.Missions.LunarScavengerEncounter;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class MiscHook {
        public static int 造物难度敌人珍珠_ = 0;
        public static int 战斗祭坛物品掉落数_ = 0;
        public static int 零号颂词To切片次数 = 0;

        public static bool 处于虚空之境 = false;

        public static void AddHook() {
            On.RoR2.CharacterMaster.Awake += CharacterMaster_Awake;
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            ShrineCombatBehavior.onDefeatedServerGlobal += ShrineCombatBehavior_onDefeatedServerGlobal;
            TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
            On.RoR2.BazaarController.Awake += BazaarController_Awake;
            On.RoR2.PurchaseInteraction.SetAvailable += PurchaseInteraction_SetAvailable;
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacement += RandomlyLunarUtils_CheckForLunarReplacement;
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacementUniqueArray += RandomlyLunarUtils_CheckForLunarReplacementUniqueArray;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter += FadeOut_OnEnter;
            On.RoR2.LunarSunBehavior.FixedUpdate += LunarSunBehavior_FixedUpdate;
            IL.RoR2.Orbs.MissileVoidOrb.Begin += IL_MissileVoidOrb_Begin;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterMaster.Awake -= CharacterMaster_Awake;
            CharacterBody.onBodyStartGlobal -= CharacterBody_onBodyStartGlobal;
            ShrineCombatBehavior.onDefeatedServerGlobal -= ShrineCombatBehavior_onDefeatedServerGlobal;
            TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
            On.RoR2.BazaarController.Awake -= BazaarController_Awake;
            On.RoR2.PurchaseInteraction.SetAvailable -= PurchaseInteraction_SetAvailable;
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacement -= RandomlyLunarUtils_CheckForLunarReplacement;
            On.RoR2.Items.RandomlyLunarUtils.CheckForLunarReplacementUniqueArray -= RandomlyLunarUtils_CheckForLunarReplacementUniqueArray;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter -= TrueDeathState_OnEnter;
            On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.OnEnter -= FadeOut_OnEnter;
            On.RoR2.LunarSunBehavior.FixedUpdate -= LunarSunBehavior_FixedUpdate;
            IL.RoR2.Orbs.MissileVoidOrb.Begin -= IL_MissileVoidOrb_Begin;
        }

        private static void CharacterMaster_Awake(On.RoR2.CharacterMaster.orig_Awake orig, CharacterMaster self) {
            orig(self);
            if (Run.instance.selectedDifficulty == ConfigurableDifficulty.ConfigurableDifficultyPlugin.configurableDifficultyIndex && self.playerCharacterMasterController) {
                self.inventory.GiveItem(DLC1Content.Items.ScrapWhiteSuppressed.itemIndex);
            }
        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody body) {
            if (NetworkServer.active && body.inventory) {
                Inventory inventory = body.inventory;
                if (Main.是否选择造物难度_ && body.isPlayerControlled) {
                    if (inventory.GetItemCount(DLC1Content.Items.ScrapWhiteSuppressed.itemIndex) == 1) {
                        inventory.RemoveItem(DLC1Content.Items.ScrapWhiteSuppressed.itemIndex);
                        if (StatHook.BodyIndexToName_.TryGetValue(body.bodyIndex, out StatHook.BodyName loc1)) {
                            switch (loc1) {
                                case StatHook.BodyName.Arbiter: {
                                    inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Bandit2: {
                                    inventory.GiveItem(RoR2Content.Items.BleedOnHit.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.DeathMark.itemIndex);
                                    inventory.GiveItem(DLC1Content.Items.GoldOnHurt.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Captain: {
                                    inventory.GiveItem(RoR2Content.Items.BarrierOnKill.itemIndex, 3);
                                    break;
                                }
                                case StatHook.BodyName.CHEF: {
                                    inventory.GiveItem(RoR2Content.Items.FlatHealth.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.Mushroom.itemIndex);
                                    inventory.SetEquipmentIndex(RoR2Content.Equipment.Fruit.equipmentIndex);
                                    break;
                                }
                                case StatHook.BodyName.Commando: {
                                    inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Croco: {
                                    inventory.GiveItem(RoR2Content.Items.Hoof.itemIndex, 2);
                                    inventory.GiveItem(RoR2Content.Items.Tooth.itemIndex, 2);
                                    break;
                                }
                                case StatHook.BodyName.Engi: {
                                    inventory.GiveItem(RoR2Content.Items.ArmorPlate.itemIndex, 4);
                                    break;
                                }
                                case StatHook.BodyName.Heretic: {
                                    inventory.GiveItem(RoR2Content.Items.LunarPrimaryReplacement.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.LunarSecondaryReplacement.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.LunarUtilityReplacement.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Huntress: {
                                    inventory.GiveItem(RoR2Content.Items.Feather.itemIndex);
                                    inventory.GiveItem(DLC1Content.Items.MoveSpeedOnKill.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Loader: {
                                    inventory.GiveItem(RoR2Content.Items.PersonalShield.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.Thorns.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Mage: {
                                    inventory.GiveItem(RoR2Content.Items.FireRing.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.IceRing.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Merc: {
                                    inventory.GiveItem(RoR2Content.Items.CritGlasses.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.NearbyDamageBonus.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Pathfinder: {
                                    inventory.GiveItem(DLC1Content.Items.DroneWeapons.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Railgunner: {
                                    inventory.GiveItem(DLC1Content.Items.CritDamage.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.RedMist: {
                                    break;
                                }
                                case StatHook.BodyName.RobPaladin: {
                                    inventory.GiveItem(RoR2Content.Items.ExecuteLowHealthElite.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.SniperClassic: {
                                    inventory.GiveItem(DLC1Content.Items.CritDamage.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Toolbot: {
                                    inventory.GiveItem(RoR2Content.Items.Crowbar.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.StunChanceOnHit.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.Treebot: {
                                    inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.TPHealingNova.itemIndex);
                                    break;
                                }
                                case StatHook.BodyName.VoidSurvivor: {
                                    inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                                    break;
                                }
                                default: {
                                    Main.logger_.LogMessage(body.name + "not set special start items");
                                    break;
                                }
                            }
                        }
                    }
                }
                if (body.teamComponent.teamIndex == TeamIndex.Player) {
                    if (StatHook.BodyIndexToName_.TryGetValue(body.bodyIndex, out StatHook.BodyName loc2)) {
                        switch (loc2) {
                            case StatHook.BodyName.Bandit2: {
                                body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                                break;
                            }
                            case StatHook.BodyName.EngiTurret: {
                                inventory.infusionBonus = body.masterObject.GetComponent<Deployable>().ownerMaster.inventory.infusionBonus;
                                break;
                            }
                            case StatHook.BodyName.EngiWalkerTurret: {
                                inventory.infusionBonus = body.masterObject.GetComponent<Deployable>().ownerMaster.inventory.infusionBonus;
                                break;
                            }
                        }
                    }
                    if (FinalBossHook.处于天文馆_) {
                        body.AddBuff(TPDespair.ZetAspects.Catalog.Buff.ZetWarped.buffIndex);
                    }
                } else if (Main.是否选择造物难度_) {
                    inventory.GiveItem(RoR2Content.Items.Pearl.itemIndex, 造物难度敌人珍珠_);
                    if (body.name.StartsWith("MiniVoidRaidCrab")) {
                        inventory.GiveItem(DLC1Content.Items.BearVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.BleedOnHitVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.ChainLightningVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.ExplodeOnDeathVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.MissileVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.MoreMissile.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.SlowOnHitVoid.itemIndex);
                    }
                }
            }
        }

        private static void BazaarController_Awake(On.RoR2.BazaarController.orig_Awake orig, BazaarController self) {
            orig(self);
            零号颂词To切片次数 = 6 * Util.GetItemCountGlobal(DLC1Content.Items.RandomlyLunar.itemIndex, false, false);
        }

        private static void PurchaseInteraction_SetAvailable(On.RoR2.PurchaseInteraction.orig_SetAvailable orig, PurchaseInteraction self, bool newAvailable) {
            if (self.name.StartsWith("LunarRecycler")) {
                if (零号颂词To切片次数 > 0 && newAvailable == false) {
                    orig(self, true);
                    --零号颂词To切片次数;
                    return;
                }
            }
            orig(self, newAvailable);
        }

        private static PickupIndex RandomlyLunarUtils_CheckForLunarReplacement(On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacement orig, PickupIndex pickupIndex, Xoroshiro128Plus rng) {
            if (处于虚空之境) {
                PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                if (pickupDef != null && Util.CheckRoll(5)) {
                    switch (pickupDef.itemTier) {
                        case ItemTier.Tier1: {
                            return rng.NextElementUniform(Run.instance.availableVoidTier1DropList);
                        }
                        case ItemTier.Tier2: {
                            return rng.NextElementUniform(Run.instance.availableVoidTier2DropList);
                        }
                        case ItemTier.Tier3: {
                            return rng.NextElementUniform(Run.instance.availableVoidTier3DropList);
                        }
                    }
                }
            }
            return pickupIndex;
        }

        private static void RandomlyLunarUtils_CheckForLunarReplacementUniqueArray(On.RoR2.Items.RandomlyLunarUtils.orig_CheckForLunarReplacementUniqueArray orig, PickupIndex[] pickupIndices, Xoroshiro128Plus rng) {
            if (处于虚空之境) {
                List<PickupIndex> list = null;
                for (int i = 0; i < pickupIndices.Length; ++i) {
                    PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndices[i]);
                    if (pickupDef != null && Util.CheckRoll(5)) {
                        List<PickupIndex> list3 = null;
                        if (pickupDef.itemIndex != ItemIndex.None) {
                            if (list == null) {
                                switch (pickupDef.itemTier) {
                                    case ItemTier.Tier1: {
                                        list = Run.instance.availableVoidTier1DropList;
                                        break;
                                    }
                                    case ItemTier.Tier2: {
                                        list = Run.instance.availableVoidTier2DropList;
                                        break;
                                    }
                                    case ItemTier.Tier3: {
                                        list = Run.instance.availableVoidTier3DropList;
                                        break;
                                    }
                                }
                                Util.ShuffleList<PickupIndex>(list, rng);
                            }
                            list3 = list;
                        }
                        if (list3 != null && list3.Count > 0) {
                            pickupIndices[i] = list3[i % list3.Count];
                        }
                    }
                }
            }
        }

        private static void ShrineCombatBehavior_onDefeatedServerGlobal(ShrineCombatBehavior shrine) {
            if (战斗祭坛物品掉落数_ > 0) {
                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableTier1DropList);
                Vector3 pos = shrine.transform.position + (6 * Vector3.up);
                Vector3 velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
                Quaternion rotation = Quaternion.AngleAxis(360f / 战斗祭坛物品掉落数_, Vector3.up);
                for (int i = 0; i < 战斗祭坛物品掉落数_; ++i) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, pos, velocity);
                    velocity = rotation * velocity;
                }
                战斗祭坛物品掉落数_ /= 2;
            }
        }

        private static void TeleporterInteraction_onTeleporterChargedGlobal(TeleporterInteraction teleporter) {
            if (NetworkServer.active && teleporter.name.StartsWith("LunarTeleporter")) {
                int dropCount = Run.instance.participatingPlayerCount;
                PickupIndex pickupIndex = PickupCatalog.FindPickupIndex(SkillHook.古代权杖_);
                Vector3 pos = teleporter.bossGroup.dropPosition.position;
                Vector3 velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
                Quaternion rotation = Quaternion.AngleAxis(360f / dropCount, Vector3.up);
                while (dropCount-- > 0) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, pos, velocity);
                    velocity = rotation * velocity;
                }
            }
        }

        private static void TrueDeathState_OnEnter(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self) {
            orig(self);
            if (Main.是否选择造物难度_ && RunHook.往日不再_ == false) {
                if (NetworkServer.active) {
                    ChatMessage.SendColored("--世界不再是你熟悉的那样！！！", Color.red);
                }
                RunHook.往日不再_ = true;
            }
        }

        private static void FadeOut_OnEnter(On.EntityStates.Missions.LunarScavengerEncounter.FadeOut.orig_OnEnter orig, FadeOut self) {
            orig(self);
            if (NetworkServer.active) {
                foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances) {
                    player.master.inventory.GiveItem(StatHook.特拉法梅的祝福_);
                }
            }
        }

        private static void LunarSunBehavior_FixedUpdate(On.RoR2.LunarSunBehavior.orig_FixedUpdate orig, LunarSunBehavior self) {
            self.projectileTimer += Time.fixedDeltaTime;
            if (!self.body.master.IsDeployableLimited(DeployableSlot.LunarSunBomb) && self.projectileTimer > 3f / self.stack) {
                self.projectileTimer = 0f;
                FireProjectileInfo fireProjectileInfo = new() {
                    projectilePrefab = self.projectilePrefab,
                    crit = self.body.RollCrit(),
                    damage = self.body.damage * 5f,
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

        private static void IL_MissileVoidOrb_Begin(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[1];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 75f);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 150f);
            } else {
                Main.logger_.LogError("MissileVoidOrb Hook Error");
            }
        }
    }
}