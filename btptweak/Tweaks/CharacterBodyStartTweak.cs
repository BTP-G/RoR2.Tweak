using BtpTweak.Utils;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks
{

    internal class CharacterBodyStartTweak : TweakBase {
        private readonly List<string> _已获得起始物品玩家列表 = new();
        private int _造物难度敌人珍珠;

        public override void AddHooks() {
            base.AddHooks();
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            Run.onRunAmbientLevelUp += RecalculatePearlCount;
        }

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            _已获得起始物品玩家列表.Clear();
            _造物难度敌人珍珠 = 0;
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody body) {
            if (NetworkServer.active && body.inventory) {
                Inventory inventory = body.inventory;
                if (Main.是否选择造物难度_ && body.isPlayerControlled) {
                    string userName = body.GetUserName();
                    if (!_已获得起始物品玩家列表.Contains(userName)) {
                        _已获得起始物品玩家列表.Add(userName);
                        if (RecalculateStatsTweak.BodyIndexToName_.TryGetValue(body.bodyIndex, out RecalculateStatsTweak.BodyName loc1)) {
                            switch (loc1) {
                                case RecalculateStatsTweak.BodyName.ArbiterBody: {
                                    inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.Bandit2Body: {
                                    inventory.GiveItem(RoR2Content.Items.BleedOnHit.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.DeathMark.itemIndex);
                                    inventory.GiveItem(DLC1Content.Items.GoldOnHurt.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.CaptainBody: {
                                    inventory.GiveItem(RoR2Content.Items.BarrierOnKill.itemIndex, 3);
                                    inventory.GiveItem(RoR2Content.Items.Behemoth.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.CHEF: {
                                    inventory.GiveItem(RoR2Content.Items.FlatHealth.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.Mushroom.itemIndex);
                                    inventory.SetEquipmentIndex(RoR2Content.Equipment.Fruit.equipmentIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.CommandoBody: {
                                    inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.CrocoBody: {
                                    inventory.GiveItem(RoR2Content.Items.Hoof.itemIndex, 2);
                                    inventory.GiveItem(RoR2Content.Items.Tooth.itemIndex, 2);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.EngiBody: {
                                    inventory.GiveItem(RoR2Content.Items.ArmorPlate.itemIndex, 4);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.HereticBody: {
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.HuntressBody: {
                                    inventory.GiveItem(RoR2Content.Items.Feather.itemIndex);
                                    inventory.GiveItem(DLC1Content.Items.MoveSpeedOnKill.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.LoaderBody: {
                                    inventory.GiveItem(RoR2Content.Items.PersonalShield.itemIndex);
                                    var skill = body.skillLocator.GetSkillAtIndex(3).skillDef.skillIndex;
                                    if (skill == "RoR2/Base/Loader/ThrowPylon.asset".Load<SkillDef>().skillIndex) {
                                        inventory.GiveItem(RoR2Content.Items.ShockNearby.itemIndex);
                                    } else if (skill == "RoR2/Base/Loader/GroundSlam.asset".Load<SteppedSkillDef>().skillIndex) {
                                        inventory.GiveItem(RoR2Content.Items.FallBoots.itemIndex);
                                    }
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.MageBody: {
                                    inventory.GiveItem(RoR2Content.Items.FireRing.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.IceRing.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.MercBody: {
                                    inventory.GiveItem(RoR2Content.Items.CritGlasses.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.NearbyDamageBonus.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.PathfinderBody: {
                                    inventory.GiveItem(DLC1Content.Items.DroneWeapons.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.RailgunnerBody: {
                                    inventory.GiveItem(DLC1Content.Items.CritDamage.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.RedMistBody: {
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.RobPaladinBody: {
                                    inventory.GiveItem(RoR2Content.Items.ExecuteLowHealthElite.itemIndex, 4);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.SniperClassicBody: {
                                    inventory.GiveItem(DLC1Content.Items.CritDamage.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.ToolbotBody: {
                                    inventory.GiveItem(RoR2Content.Items.StunChanceOnHit.itemIndex, 3);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.TreebotBody: {
                                    inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                                    inventory.GiveItem(RoR2Content.Items.TPHealingNova.itemIndex);
                                    break;
                                }
                                case RecalculateStatsTweak.BodyName.VoidSurvivorBody: {
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
                    if (RecalculateStatsTweak.BodyIndexToName_.TryGetValue(body.bodyIndex, out RecalculateStatsTweak.BodyName loc2)) {
                        switch (loc2) {
                            case RecalculateStatsTweak.BodyName.Bandit2Body: {
                                body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                                break;
                            }
                            case RecalculateStatsTweak.BodyName.EngiTurretBody: {
                                inventory.infusionBonus = body.masterObject.GetComponent<Deployable>().ownerMaster.inventory.infusionBonus;
                                break;
                            }
                            case RecalculateStatsTweak.BodyName.EngiWalkerTurretBody: {
                                inventory.infusionBonus = body.masterObject.GetComponent<Deployable>().ownerMaster.inventory.infusionBonus;
                                break;
                            }
                        }
                    }
                    if (SceneCatalog.GetSceneDefForCurrentScene().cachedName == "voidraid") {
                        body.AddBuff(TPDespair.ZetAspects.Catalog.Buff.ZetWarped.buffIndex);
                    }
                } else if (Main.是否选择造物难度_) {
                    body.baseArmor += body.level;
                    inventory.GiveItem(RoR2Content.Items.Pearl.itemIndex, _造物难度敌人珍珠);
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

        private void RecalculatePearlCount(Run run) {
            _造物难度敌人珍珠 = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(run.ambientLevel * 0.1f, Main.往日不再_ ? 1 + 0.1f * run.stageClearCount : 1), 10000000));
        }
    }
}