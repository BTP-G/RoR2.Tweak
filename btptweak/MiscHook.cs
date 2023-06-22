using RoR2;
using RoR2.Orbs;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class MiscHook {

        public static void AddHook() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter += ChargedState_OnEnter;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer += ShrineCombatBehavior_OnDefeatedServer;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection += SceneDirector_GenerateInteractableCardSelection;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterMaster.OnBodyStart -= CharacterMaster_OnBodyStart;
            On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter -= ChargedState_OnEnter;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer -= ShrineCombatBehavior_OnDefeatedServer;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection -= SceneDirector_GenerateInteractableCardSelection;
        }

        private static void CharacterMaster_OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody body) {
            orig(self, body);
            if (NetworkServer.active && TeamIndex.Player == body.teamComponent.teamIndex) {  // 保证强盗标记不因过关和死亡消失
                if (body.name.StartsWith("Band")) {
                    body.SetBuffCount(RoR2Content.Buffs.BanditSkull.buffIndex, BtpTweak.盗贼标记_);
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
                        InfusionOrb infusionOrb = new InfusionOrb {
                            origin = damageReport.victim.transform.position,
                            target = Util.FindBodyMainHurtBox(damageReport.attackerBody),
                            maxHpValue = item_infusion_count
                        };
                        if (inventory.infusionBonus >= max_hp) {
                            infusionOrb.maxHpValue *= BtpTweak.浸剂击杀奖励倍率_.Value;
                            OrbManager.instance.AddOrb(infusionOrb);
                        } else {
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
                BtpTweak.玩家生命值增加系数_ += 0.02f;
                BtpTweak.怪物生命值增加系数_ += Run.instance.stageClearCount * (Run.instance.loopClearCount > 0 ? Run.instance.stageClearCount : 1) * 0.01f;
                Debug.Log("now Monster health ==== " + BtpTweak.怪物生命值增加系数_);
                if (NetworkServer.active) {
                    Chat.SendBroadcastChat(new Chat.SimpleChatMessage() { baseToken = $"<color=green><size=100%>传送器充能完毕，获得最大生命值奖励！</size></color>" });
                }
            }
        }

        private static void ShrineCombatBehavior_OnDefeatedServer(On.RoR2.ShrineCombatBehavior.orig_OnDefeatedServer orig, ShrineCombatBehavior self) {
            orig(self);
            if (NetworkServer.active) {
                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableTier1DropList);
                Vector3 pos = self.transform.position + (6 * Vector3.up);
                Vector3 velocity = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
                Quaternion rotation = Quaternion.AngleAxis(360f / Run.instance.participatingPlayerCount, Vector3.up);
                for (int i = 0, n = Run.instance.participatingPlayerCount + Run.instance.stageClearCount; i < n; ++i) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, pos, velocity);
                    velocity = rotation * velocity;
                }
            }
        }

        private static WeightedSelection<DirectorCard> SceneDirector_GenerateInteractableCardSelection(On.RoR2.SceneDirector.orig_GenerateInteractableCardSelection orig, SceneDirector self) {
            if (NetworkServer.active) {
                self.interactableCredit += (int)(((Run.instance.stageClearCount % 6) + Run.instance.participatingPlayerCount) * (0.1f * self.interactableCredit));
            }
            return orig(self);
        }
    }
}