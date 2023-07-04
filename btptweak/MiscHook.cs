using R2API.Utils;
using RoR2;
using RoR2.Orbs;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class MiscHook {
        public static ushort 战斗祭坛额外奖励数量 = 0;

        public static void AddHook() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter += ChargedState_OnEnter;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer += ShrineCombatBehavior_OnDefeatedServer;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection += SceneDirector_GenerateInteractableCardSelection;
            On.RoR2.Projectile.SlowDownProjectiles.Start += SlowDownProjectiles_Start;
            On.RoR2.GravitatePickup.Start += GravitatePickup_Start;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterMaster.OnBodyStart -= CharacterMaster_OnBodyStart;
            On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter -= ChargedState_OnEnter;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer -= ShrineCombatBehavior_OnDefeatedServer;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection -= SceneDirector_GenerateInteractableCardSelection;
            On.RoR2.GravitatePickup.Start -= GravitatePickup_Start;
        }

        private static void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body) {
            orig(self, body);
            if (NetworkServer.active
                && body.isPlayerControlled
                && body.name.StartsWith("Bandit")) {  // 保证强盗标记不因过关和死亡消失
                if (BtpTweak.盗贼标记_.ContainsKey(body.playerControllerId)) {
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, BtpTweak.盗贼标记_[body.playerControllerId]);
                } else {
                    BtpTweak.盗贼标记_.Add(body.playerControllerId, 0);
                }
            }
        }

        private static void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport) {
            orig(self, damageReport);
            if (NetworkServer.active) {
                //=== 灭绝之歌
                if (damageReport.victimBody?.HasBuff(AncientScepter.AncientScepterMain.perishSongDebuff) ?? false) {
                    foreach (CharacterMaster characterMaster in CharacterMaster.readOnlyInstancesList) {
                        if (characterMaster.masterIndex == damageReport.victimMaster.masterIndex) {
                            characterMaster.TrueKill(damageReport.attacker, null, DamageType.Generic);
                        }
                    }
                }
                //=== 浸剂
                Inventory inventory = damageReport.attackerMaster?.inventory;
                if (inventory != null) {
                    int item_infusion_count = inventory.GetItemCount(RoR2Content.Items.Infusion);
                    if (item_infusion_count > 0) {
                        uint max_hp = (uint)(item_infusion_count * 100);
                        InfusionOrb infusionOrb = new() {
                            origin = damageReport.victim.transform.position,
                            target = Util.FindBodyMainHurtBox(damageReport.attackerBody),
                            maxHpValue = item_infusion_count
                        };
                        if (inventory.infusionBonus >= max_hp && BtpTweak.浸剂击杀奖励倍率_.Value > 0) {
                            infusionOrb.maxHpValue *= BtpTweak.浸剂击杀奖励倍率_.Value;
                            OrbManager.instance.AddOrb(infusionOrb);
                        } else if (BtpTweak.浸剂击杀奖励倍率_.Value > 1) {
                            infusionOrb.maxHpValue *= BtpTweak.浸剂击杀奖励倍率_.Value - 1;
                            OrbManager.instance.AddOrb(infusionOrb);
                        }
                    }
                }
                //======
            }
        }

        private static void ChargedState_OnEnter(On.RoR2.TeleporterInteraction.ChargedState.orig_OnEnter orig, EntityStates.BaseState self) {
            orig(self);
            if (BtpTweak.是否选择造物难度_) {
                BtpTweak.玩家生命值增加系数_ = 0.1f + 0.03f * (1 + Run.instance.stageClearCount);
                BtpTweak.玩家生命值倍数_ = 1 + 0.05f * Run.instance.stageClearCount;
                if (NetworkServer.active) {
                    ChatMessage.SendColored("传送器充能完毕，获得最大生命值奖励！", Color.green);
                }
            }
        }

        private static void ShrineCombatBehavior_OnDefeatedServer(On.RoR2.ShrineCombatBehavior.orig_OnDefeatedServer orig, ShrineCombatBehavior self) {
            orig(self);
            if (NetworkServer.active) {
                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableTier1DropList);
                Vector3 pos = self.transform.position + (6 * Vector3.up);
                Vector3 velocity = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
                Quaternion rotation = Quaternion.AngleAxis(360f / (Run.instance.participatingPlayerCount + 战斗祭坛额外奖励数量), Vector3.up);
                for (int i = 0, n = Run.instance.participatingPlayerCount + 战斗祭坛额外奖励数量; i < n; ++i) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, pos, velocity);
                    velocity = rotation * velocity;
                }
                ++战斗祭坛额外奖励数量;
            }
        }

        private static WeightedSelection<DirectorCard> SceneDirector_GenerateInteractableCardSelection(On.RoR2.SceneDirector.orig_GenerateInteractableCardSelection orig, SceneDirector self) {
            if (BtpTweak.是否选择造物难度_ && NetworkServer.active) {
                self.interactableCredit += (int)((Run.instance.loopClearCount + Run.instance.participatingPlayerCount) * (0.1f * self.interactableCredit));
            }
            return orig(self);
        }

        private static void SlowDownProjectiles_Start(On.RoR2.Projectile.SlowDownProjectiles.orig_Start orig, RoR2.Projectile.SlowDownProjectiles self) {
            orig(self);
            if (self.name.StartsWith("Rail")) {
                self.slowDownCoefficient = 0.03f;
            }
        }

        private static void GravitatePickup_Start(On.RoR2.GravitatePickup.orig_Start orig, GravitatePickup self) {
            orig(self);
            if (self.transform.parent?.name.StartsWith("BonusMoneyPack") ?? false) {
                self.maxSpeed = 50;
                self.gravitateTarget = GetClosestPlayerTransform(TeamComponent.GetTeamMembers(TeamIndex.Player), self.transform.position);
            }
        }

        private static Transform GetClosestPlayerTransform(ReadOnlyCollection<TeamComponent> players, Vector3 location) {
            Transform result = null;
            float num = float.MaxValue;
            foreach (TeamComponent teamComponent in players) {
                if (Util.LookUpBodyNetworkUser(teamComponent.gameObject)) {
                    float num2 = Vector3.Distance(teamComponent.body.corePosition, location);
                    if (num2 < num) {
                        result = teamComponent.body.coreTransform;
                        num = num2;
                    }
                }
            }
            return result;
        }
    }
}