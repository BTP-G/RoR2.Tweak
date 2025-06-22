﻿using BTP.RoR2Plugin.Items;
using BTP.RoR2Plugin.Tweaks.MithrixTweaks.MithrixEntityStates;
using EntityStates;
using EntityStates.BrotherMonster;
using KinematicCharacterController;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static BTP.RoR2Plugin.RoR2Indexes.BodyIndexes;

namespace BTP.RoR2Plugin.Tweaks {

    internal class CharacterStartTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        private int _造物难度敌人血量提升物品数量;

        void IModLoadMessageHandler.Handle() {
            CharacterMaster.onStartGlobal += OnMasterStartGlobal;
            CharacterBody.onBodyStartGlobal += OnBodyStartGlobal;
            Run.onPlayerFirstCreatedServer += (_, player) => player.master.onBodyStart += 造物难度_OnPlayerBodyFirstStartServer;
            Run.onRunAmbientLevelUp += (run) => _造物难度敌人血量提升物品数量 = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(run.ambientLevel * 0.1f, RunInfo.第二次大旋风 ? 1f + 0.1f * run.stageClearCount : 1f), 100000000)); ;
            Run.onRunStartGlobal += (_) => _造物难度敌人血量提升物品数量 = 0;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            var body = BodyCatalog.GetBodyPrefabBodyComponent(BodyCatalog.FindBodyIndex(BodyNameIndex.ShopkeeperBody.ToString()));
            body.baseMaxHealth = 1E+30f;
            body.levelMaxHealth = 0f;
            body.baseRegen = 0f;
            body.levelRegen = 0f;
            body.baseArmor = 0f;
            body.levelArmor = 0f;
            body.bodyFlags |= CharacterBody.BodyFlags.ImmuneToExecutes;
        }

        private void OnMasterStartGlobal(CharacterMaster master) {
            if (NetworkServer.active && RunInfo.已选择大旋风难度 && master.teamIndex != TeamIndex.Player) {
                master.inventory.GiveItem(RoR2Content.Items.BoostHp.itemIndex, _造物难度敌人血量提升物品数量);
            }
        }

        private void OnBodyStartGlobal(CharacterBody body) {
            if (NetworkServer.active) {
                body.healthComponent.ospTimer += 0.3f + Random.value;
                if (RunInfo.位于天文馆 && body.teamComponent.teamIndex != TeamIndex.Void) {
                    body.baseJumpPower *= 0.5f;
                }
                var inventory = body.inventory;
                if (inventory) {
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                }
                if (BodyIndexToNameIndex.TryGetValue((int)body.bodyIndex, out var nameIndex)) {
                    switch (nameIndex) {
                        case BodyNameIndex.TreebotBody: {
                            var kinematicCharacterMotor = body.GetComponent<KinematicCharacterMotor>();
                            kinematicCharacterMotor.MaxStableSlopeAngle = 89f;
                            kinematicCharacterMotor.MaxStableDenivelationAngle = 180f;
                            kinematicCharacterMotor.MinRequiredStepDepth = 0;
                            break;
                        }
                        case BodyNameIndex.BrotherBody:
                        case BodyNameIndex.BrotherHurtBody: {
                            switch (PhaseCounter.instance?.phase) {  // Give Mithrix the Scourge items
                                case 1: {
                                    inventory.GiveItem(MoonscourgeAccursedItem.ItemDef);
                                    body.skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(SlideIntroState));
                                    body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                                    break;
                                }
                                case 2: {
                                    inventory.GiveItem(StormscourgeAccursedItem.ItemDef);
                                    body.skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(LunarBlink));
                                    body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                                    break;
                                }
                                case 3: {
                                    inventory.GiveItem(HelscourgeAccursedItem.ItemDef);
                                    body.baseAcceleration *= Run.instance.participatingPlayerCount;
                                    body.AddBuff(RoR2Content.Buffs.LunarShell.buffIndex);
                                    body.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                                    body.skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(LunarBlink));
                                    break;
                                }
                                case 4: {
                                    break;
                                }
                            }
                            inventory.GiveItem(RoR2Content.Items.TeleportWhenOob.itemIndex);
                            body.healthComponent.ospTimer += 3f;
                            Util.CleanseBody(body, true, false, true, true, true, true);
                            break;
                        }
                        case BodyNameIndex.MiniVoidRaidCrabBodyPhase1:
                        case BodyNameIndex.MiniVoidRaidCrabBodyPhase2:
                        case BodyNameIndex.MiniVoidRaidCrabBodyPhase3: {
                            if (inventory.GetItemCount(DLC1Content.Items.VoidMegaCrabItem.itemIndex) > 0) {
                                return;
                            }
                            inventory.GiveItem(DLC1Content.Items.BearVoid.itemIndex, 4);
                            inventory.GiveItem(DLC1Content.Items.BleedOnHitVoid.itemIndex, 5);
                            inventory.GiveItem(DLC1Content.Items.ChainLightningVoid.itemIndex, 4);
                            inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                            inventory.GiveItem(DLC1Content.Items.ExplodeOnDeathVoid.itemIndex, 9);
                            inventory.GiveItem(DLC1Content.Items.MissileVoid.itemIndex);
                            inventory.GiveItem(DLC1Content.Items.MoreMissile.itemIndex);
                            inventory.GiveItem(DLC1Content.Items.SlowOnHitVoid.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.PersonalShield.itemIndex, 5);
                            break;
                        }
                    }
                }
            }
        }

        private void 造物难度_OnPlayerBodyFirstStartServer(CharacterBody body) {
            body.master.onBodyStart -= 造物难度_OnPlayerBodyFirstStartServer;
            if (!RunInfo.已选择大旋风难度 || Run.instance.stageClearCount != 0) {
                return;
            }
            var inventory = body.inventory;
            inventory.GiveItem(RoR2Content.Items.ExtraLife.itemIndex);
            inventory.GiveItem(RoR2Content.Items.Infusion.itemIndex);
            inventory.GiveItem(RoR2Content.Items.HealWhileSafe.itemIndex);
            if (BodyIndexToNameIndex.TryGetValue((int)body.bodyIndex, out var nameIndex)) {
                switch (nameIndex) {
                    case BodyNameIndex.ArbiterBody: {
                        if (Random.value < 0.5f) {
                            inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex, 2);
                        } else {
                            inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex, 2);
                            inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex);
                        }
                        break;
                    }
                    case BodyNameIndex.Bandit2Body: {
                        inventory.GiveItem(RoR2Content.Items.BleedOnHit.itemIndex, 2);
                        inventory.GiveItem(DLC1Content.Items.GoldOnHurt.itemIndex);
                        break;
                    }
                    case BodyNameIndex.CaptainBody: {
                        inventory.GiveItem(RoR2Content.Items.BarrierOnKill.itemIndex, 3);
                        break;
                    }
                    case BodyNameIndex.ChefBody: {
                        inventory.SetEquipmentIndex(RoR2Content.Equipment.Fruit.equipmentIndex);
                        break;
                    }
                    case BodyNameIndex.CommandoBody: {
                        if (Random.value < 0.5f) {
                            inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex, 2);
                            inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
                        } else {
                            inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine.itemIndex, 2);
                        }
                        break;
                    }
                    case BodyNameIndex.CrocoBody: {
                        inventory.GiveItem(RoR2Content.Items.Hoof.itemIndex, 2);
                        inventory.GiveItem(RoR2Content.Items.Tooth.itemIndex);
                        break;
                    }
                    case BodyNameIndex.EngiBody: {
                        inventory.GiveItem(RoR2Content.Items.ArmorPlate.itemIndex, 2);
                        inventory.GiveItem(RoR2Content.Items.WardOnLevel.itemIndex);
                        break;
                    }
                    case BodyNameIndex.HereticBody: {
                        break;
                    }
                    case BodyNameIndex.HuntressBody: {
                        inventory.GiveItem(DLC1Content.Items.MoveSpeedOnKill.itemIndex);
                        break;
                    }
                    case BodyNameIndex.LoaderBody: {
                        inventory.GiveItem(DLC1Content.Items.FreeChest.itemIndex);
                        break;
                    }
                    case BodyNameIndex.MageBody: {
                        if (Random.value < 0.5f || body.skillLocator.secondary.skillDef.skillName == "IceBomb") {
                            inventory.GiveItem(RoR2Content.Items.IceRing.itemIndex);
                        } else {
                            inventory.GiveItem(RoR2Content.Items.FireRing.itemIndex);
                        }
                        break;
                    }
                    case BodyNameIndex.MercBody: {
                        inventory.GiveItem(RoR2Content.Items.NearbyDamageBonus.itemIndex, 3);
                        break;
                    }
                    case BodyNameIndex.PathfinderBody: {
                        inventory.SetEquipmentIndex(RoR2Content.Equipment.DroneBackup.equipmentIndex);
                        break;
                    }
                    case BodyNameIndex.RailgunnerBody: {
                        inventory.SetEquipmentIndex(RoR2Content.Equipment.CritOnUse.equipmentIndex);
                        break;
                    }
                    case BodyNameIndex.RedMistBody: {
                        break;
                    }
                    case BodyNameIndex.RobPaladinBody: {
                        inventory.GiveItem(RoR2Content.Items.ExecuteLowHealthElite.itemIndex);
                        break;
                    }
                    case BodyNameIndex.SniperClassicBody: {
                        inventory.SetEquipmentIndex(RoR2Content.Equipment.CritOnUse.equipmentIndex);
                        break;
                    }
                    case BodyNameIndex.ToolbotBody: {
                        inventory.GiveItem(RoR2Content.Items.Crowbar.itemIndex);
                        inventory.GiveItem(RoR2Content.Items.StickyBomb.itemIndex);
                        inventory.GiveItem(RoR2Content.Items.StunChanceOnHit.itemIndex);
                        break;
                    }
                    case BodyNameIndex.TreebotBody: {
                        inventory.GiveItem(RoR2Content.Items.TPHealingNova.itemIndex);
                        break;
                    }
                    case BodyNameIndex.VoidSurvivorBody: {
                        inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                        break;
                    }
                }
            }
        }
    }
}