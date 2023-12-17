using BtpTweak.Tweaks.MithrixTweaks.MithrixEntityStates;
using EntityStates;
using EntityStates.BrotherMonster;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static BtpTweak.RoR2Indexes.BodyIndexes;

namespace BtpTweak.Tweaks {

    internal class CharacterBodyStartTweak : TweakBase<CharacterBodyStartTweak>, IOnModLoadBehavior {
        private int _造物难度敌人血量提升物品数量;

        void IOnModLoadBehavior.OnModLoad() {
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            CharacterMaster.onStartGlobal += CharacterMaster_onStartGlobal;
            Run.onRunAmbientLevelUp += RecalculateBoostHpCount;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        public void Run_onRunStartGlobal(Run run) {
            _造物难度敌人血量提升物品数量 = 0;
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody body) {
            if (NetworkServer.active) {
                body.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.3f + Random.value);
                if (RunInfo.位于天文馆 && body.teamComponent.teamIndex != TeamIndex.Void) {
                    body.AddBuff(TPDespair.ZetAspects.Catalog.Buff.ZetWarped.buffIndex);
                }
                var inventory = body.inventory;
                if (inventory) {
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                }
            }
        }

        private void CharacterMaster_onStartGlobal(CharacterMaster master) {
            if (NetworkServer.active && RunInfo.是否选择造物难度) {
                master.onBodyStart += 造物难度_onBodyStart;
            }
        }

        private void 造物难度_onBodyStart(CharacterBody body) {
            var master = body.master;
            var inventory = body.inventory;
            master.onBodyStart -= 造物难度_onBodyStart;
            if (master.teamIndex != TeamIndex.Player) {
                body.baseArmor += body.level;
                inventory.GiveItem(RoR2Content.Items.BoostHp.itemIndex, _造物难度敌人血量提升物品数量);
            }
            if (BodyIndexToNameIndex.TryGetValue((int)body.bodyIndex, out var nameIndex)) {
                switch (nameIndex) {
                    case BodyNameIndex.ArbiterBody: {
                        if (Random.value > 0.5f) {
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
                    case BodyNameIndex.CHEF: {
                        inventory.SetEquipmentIndex(RoR2Content.Equipment.Fruit.equipmentIndex);
                        break;
                    }
                    case BodyNameIndex.CommandoBody: {
                        if (Util.CheckRoll(50)) {
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
                        if (Random.value > 0.5f || body.skillLocator.secondary.skillDef.skillName == "IceBomb") {
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
                    case BodyNameIndex.BrotherBody:
                    case BodyNameIndex.BrotherHurtBody: {
                        switch (PhaseCounter.instance?.phase) {  // Give Mithrix the Scourge items
                            case 1: {
                                inventory.GiveItem(BtpContent.Items.MoonscourgeAccursedItem);
                                body.skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(SlideIntroState));
                                break;
                            }
                            case 2: {
                                inventory.GiveItem(BtpContent.Items.StormscourgeAccursedItem);
                                body.skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(LunarBlink));
                                break;
                            }
                            case 3: {
                                inventory.GiveItem(BtpContent.Items.HelscourgeAccursedItemDef);
                                body.baseAcceleration *= Run.instance.participatingPlayerCount;
                                body.AddBuff(RoR2Content.Buffs.LunarShell.buffIndex);
                                body.skillLocator.utility.skillDef.activationState = new SerializableEntityStateType(typeof(LunarBlink));
                                break;
                            }
                            case 4: {
                                break;
                            }
                        }
                        inventory.GiveItem(RoR2Content.Items.TeleportWhenOob.itemIndex);
                        body.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 3f);
                        break;
                    }
                    case BodyNameIndex.MiniVoidRaidCrabBodyPhase1:
                    case BodyNameIndex.MiniVoidRaidCrabBodyPhase2:
                    case BodyNameIndex.MiniVoidRaidCrabBodyPhase3: {
                        inventory.GiveItem(DLC1Content.Items.BearVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.BleedOnHitVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.ChainLightningVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.ExplodeOnDeathVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.MissileVoid.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.MoreMissile.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.SlowOnHitVoid.itemIndex);
                        break;
                    }
                }
            }
        }

        private void RecalculateBoostHpCount(Run run) {
            _造物难度敌人血量提升物品数量 = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(run.ambientLevel * 0.1f, RunInfo.往日不再 ? 1 + 0.1f * run.stageClearCount : 1), 10000000));
        }
    }
}