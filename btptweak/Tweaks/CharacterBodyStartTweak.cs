using BtpTweak.IndexCollections;
using BtpTweak.Utils;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.Tweaks {

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
            if (NetworkServer.active) {
                body.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.1f + Random.value);
                Inventory inventory = body.inventory;
                if (inventory) {
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                    if (GlobalInfo.CurrentSceneIndex == SceneIndexCollection.voidraid && body.teamComponent.teamIndex != TeamIndex.Void) {
                        body.AddBuff(TPDespair.ZetAspects.Catalog.Buff.ZetWarped.buffIndex);
                    }
                    if (GlobalInfo.是否选择造物难度) {
                        if (body.isPlayerControlled) {
                            GivePlayerStartingItems(body);
                        } else if (body.teamComponent.teamIndex != TeamIndex.Player) {
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
            }
        }

        private void GivePlayerStartingItems(CharacterBody body) {
            Inventory inventory = body.inventory;
            string userName = body.GetUserName();
            if (!_已获得起始物品玩家列表.Contains(userName)) {
                _已获得起始物品玩家列表.Add(userName);
                if (RecalculateStatsTweak.BodyIndexToName_.TryGetValue(body.bodyIndex, out BodyNameIndex loc1)) {
                    switch (loc1) {
                        case BodyNameIndex.ArbiterBody: {
                            inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex);
                            break;
                        }
                        case BodyNameIndex.Bandit2Body: {
                            inventory.GiveItem(RoR2Content.Items.BleedOnHit.itemIndex);
                            inventory.GiveItem(DLC1Content.Items.GoldOnHurt.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.Dagger.itemIndex);
                            break;
                        }
                        case BodyNameIndex.CaptainBody: {
                            inventory.GiveItem(RoR2Content.Items.BarrierOnKill.itemIndex, 3);
                            inventory.GiveItem(RoR2Content.Items.Behemoth.itemIndex);
                            break;
                        }
                        case BodyNameIndex.CHEF: {
                            inventory.GiveItem(RoR2Content.Items.FlatHealth.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.Mushroom.itemIndex);
                            inventory.SetEquipmentIndex(RoR2Content.Equipment.Fruit.equipmentIndex);
                            break;
                        }
                        case BodyNameIndex.CommandoBody: {
                            inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine.itemIndex);
                            break;
                        }
                        case BodyNameIndex.CrocoBody: {
                            inventory.GiveItem(RoR2Content.Items.Hoof.itemIndex, 2);
                            inventory.GiveItem(RoR2Content.Items.Tooth.itemIndex, 2);
                            break;
                        }
                        case BodyNameIndex.EngiBody: {
                            inventory.GiveItem(RoR2Content.Items.ArmorPlate.itemIndex, 4);
                            break;
                        }
                        case BodyNameIndex.HereticBody: {
                            break;
                        }
                        case BodyNameIndex.HuntressBody: {
                            inventory.GiveItem(RoR2Content.Items.Feather.itemIndex);
                            inventory.GiveItem(DLC1Content.Items.MoveSpeedOnKill.itemIndex);
                            break;
                        }
                        case BodyNameIndex.LoaderBody: {
                            inventory.GiveItem(RoR2Content.Items.PersonalShield.itemIndex);
                            var skill = body.skillLocator.GetSkillAtIndex(3).skillDef.skillIndex;
                            if (skill == "RoR2/Base/Loader/ThrowPylon.asset".Load<SkillDef>().skillIndex) {
                                inventory.GiveItem(RoR2Content.Items.ShockNearby.itemIndex);
                            } else if (skill == "RoR2/Base/Loader/GroundSlam.asset".Load<SteppedSkillDef>().skillIndex) {
                                inventory.GiveItem(RoR2Content.Items.FallBoots.itemIndex);
                            }
                            break;
                        }
                        case BodyNameIndex.MageBody: {
                            inventory.GiveItem(RoR2Content.Items.FireRing.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.IceRing.itemIndex);
                            break;
                        }
                        case BodyNameIndex.MercBody: {
                            inventory.GiveItem(RoR2Content.Items.NearbyDamageBonus.itemIndex, 3);
                            break;
                        }
                        case BodyNameIndex.PathfinderBody: {
                            inventory.GiveItem(DLC1Content.Items.DroneWeapons.itemIndex);
                            break;
                        }
                        case BodyNameIndex.RailgunnerBody: {
                            inventory.GiveItem(DLC1Content.Items.CritDamage.itemIndex);
                            break;
                        }
                        case BodyNameIndex.RedMistBody: {
                            break;
                        }
                        case BodyNameIndex.RobPaladinBody: {
                            inventory.GiveItem(RoR2Content.Items.ExecuteLowHealthElite.itemIndex, 4);
                            break;
                        }
                        case BodyNameIndex.SniperClassicBody: {
                            inventory.GiveItem(DLC1Content.Items.CritDamage.itemIndex);
                            break;
                        }
                        case BodyNameIndex.ToolbotBody: {
                            inventory.GiveItem(RoR2Content.Items.StunChanceOnHit.itemIndex, 3);
                            break;
                        }
                        case BodyNameIndex.TreebotBody: {
                            inventory.GiveItem(RoR2Content.Items.Syringe.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.TPHealingNova.itemIndex);
                            break;
                        }
                        case BodyNameIndex.VoidSurvivorBody: {
                            inventory.GiveItem(DLC1Content.Items.ElementalRingVoid.itemIndex);
                            break;
                        }
                        default: {
                            Main.Logger.LogMessage(body.name + "not set special start items");
                            break;
                        }
                    }
                }
            }
        }

        private void RecalculatePearlCount(Run run) {
            _造物难度敌人珍珠 = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(run.ambientLevel * 0.1f, GlobalInfo.往日不再 ? 1 + 0.1f * run.stageClearCount : 1), 10000000));
        }
    }
}