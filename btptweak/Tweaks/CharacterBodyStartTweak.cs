using BtpTweak.IndexCollections;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static BtpTweak.IndexCollections.BodyIndexCollection;

namespace BtpTweak.Tweaks {

    internal class CharacterBodyStartTweak : TweakBase {
        private readonly List<string> _已获得起始物品玩家列表 = new();
        private int _造物难度敌人珍珠;

        public override void AddHooks() {
            base.AddHooks();
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            CharacterMaster.onStartGlobal += CharacterMaster_onStartGlobal;
            Run.onRunAmbientLevelUp += RecalculatePearlCount;
        }

        public override void RunStartAction(Run run) {
            base.RunStartAction(run);
            _已获得起始物品玩家列表.Clear();
            _造物难度敌人珍珠 = 0;
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody body) {
            if (NetworkServer.active) {
                body.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.3f + Random.value);
                if (GlobalInfo.CurrentSceneIndex == SceneIndexCollection.voidraid && body.teamComponent.teamIndex != TeamIndex.Void) {
                    body.AddBuff(TPDespair.ZetAspects.Catalog.Buff.ZetWarped.buffIndex);
                }
                Inventory inventory = body.inventory;
                if (inventory) {
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, inventory.GetItemCount(JunkContent.Items.SkullCounter.itemIndex));
                }
            }
        }

        private void CharacterMaster_onStartGlobal(CharacterMaster master) {
            if (NetworkServer.active && GlobalInfo.是否选择造物难度) {
                master.onBodyStart += Master_onBodyStart;
            }
        }

        private void Master_onBodyStart(CharacterBody body) {
            var master = body.master;
            master.onBodyStart -= Master_onBodyStart;
            var inventory = body.inventory;
            if (master.teamIndex != TeamIndex.Player) {
                inventory.GiveItem(RoR2Content.Items.Pearl.itemIndex, _造物难度敌人珍珠);
            }
            if (BodyIndexToNameIndex.TryGetValue(body.bodyIndex, out var nameIndex)) {
                switch (nameIndex) {
                    case BodyNameIndex.ArbiterBody: {
                        if (Util.CheckRoll(50)) {
                            inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex);
                            inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex, 2);
                        } else {
                            inventory.GiveItem(DLC1Content.Items.AttackSpeedAndMoveSpeed.itemIndex, 2);
                            inventory.GiveItem(RoR2Content.Items.SprintBonus.itemIndex);
                        }
                        break;
                    }
                    case BodyNameIndex.Bandit2Body: {
                        inventory.GiveItem(RoR2Content.Items.BleedOnHit.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.GoldOnHurt.itemIndex);
                        inventory.GiveItem(DLC1Content.Items.PrimarySkillShuriken.itemIndex);
                        break;
                    }
                    case BodyNameIndex.CaptainBody: {
                        inventory.GiveItem(RoR2Content.Items.BarrierOnKill.itemIndex, 3);
                        break;
                    }
                    case BodyNameIndex.CHEF: {
                        inventory.GiveItem(RoR2Content.Items.FlatHealth.itemIndex);
                        inventory.GiveItem(RoR2Content.Items.Mushroom.itemIndex);
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
                        var skillLocator = body.skillLocator;
                        if (skillLocator.primary.skillDef.skillName == "FireFirebolt") {
                            inventory.GiveItem(RoR2Content.Items.FireRing.itemIndex);
                        } else if (skillLocator.secondary.skillDef.skillName == "IceBomb") {
                            inventory.GiveItem(RoR2Content.Items.IceRing.itemIndex);
                        } else if (skillLocator.secondary.skillDef.skillName == "NovaBomb") {
                            inventory.GiveItem(RoR2Content.Items.ChainLightning.itemIndex);
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
                    case BodyNameIndex.BrotherBody: {
                        inventory.GiveItem(RoR2Content.Items.TeleportWhenOob);
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

        private void RecalculatePearlCount(Run run) {
            _造物难度敌人珍珠 = Mathf.RoundToInt(Mathf.Min(Mathf.Pow(run.ambientLevel * 0.1f, GlobalInfo.往日不再 ? 1 + 0.1f * run.stageClearCount : 1), 10000000));
        }
    }
}