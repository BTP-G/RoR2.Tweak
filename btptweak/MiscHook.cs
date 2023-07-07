using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class MiscHook {
        public static ushort 战斗祭坛额外奖励数量 = 0;

        public static void AddHook() {
            On.RoR2.CharacterMaster.OnBodyStart += CharacterMaster_OnBodyStart;
            On.RoR2.GlobalEventManager.OnCharacterDeath += 灭绝;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += 浸剂修改;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter += ChargedState_OnEnter;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer += ShrineCombatBehavior_OnDefeatedServer;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection += SceneDirector_GenerateInteractableCardSelection;
            On.RoR2.CombatDirector.Simulate += CombatDirector_Simulate;
            On.RoR2.GravitatePickup.Start += GravitatePickup_Start;
        }

        public static void RemoveHook() {
            On.RoR2.CharacterMaster.OnBodyStart -= CharacterMaster_OnBodyStart;
            On.RoR2.GlobalEventManager.OnCharacterDeath -= 灭绝;
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= 浸剂修改;
            On.RoR2.TeleporterInteraction.ChargedState.OnEnter -= ChargedState_OnEnter;
            On.RoR2.ShrineCombatBehavior.OnDefeatedServer -= ShrineCombatBehavior_OnDefeatedServer;
            On.RoR2.SceneDirector.GenerateInteractableCardSelection -= SceneDirector_GenerateInteractableCardSelection;
            On.RoR2.CombatDirector.Simulate -= CombatDirector_Simulate;
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

        private static void 灭绝(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport) {
            orig(self, damageReport);
            if (NetworkServer.active) {  //=== 灭绝之歌
                if (damageReport.victimBody?.HasBuff(AncientScepter.AncientScepterMain.perishSongDebuff) ?? false) {
                    foreach (CharacterMaster characterMaster in CharacterMaster.readOnlyInstancesList) {
                        if (characterMaster.masterIndex == damageReport.victimMaster.masterIndex) {
                            characterMaster.TrueKill(damageReport.attacker, null, DamageType.Generic);
                        }
                    }
                }
            }
        }

        private static void 浸剂修改(ILContext il) {
            ILCursor ilcursor = new ILCursor(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[4];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 43);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcI4(x, 100);
            array[2] = (Instruction x) => ILPatternMatchingExt.MatchMul(x);
            array[3] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 63);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.RemoveRange(4);
                ilcursor.Emit(OpCodes.Ldc_I4, int.MaxValue);
                ilcursor.Emit(OpCodes.Stloc, 63);
                //===
                ilcursor.Emit(OpCodes.Ldloc, 43);
                ilcursor.Emit(OpCodes.Ldc_I4, BtpTweak.浸剂击杀奖励倍率_.Value);
                ilcursor.Emit(OpCodes.Mul);
                ilcursor.Emit(OpCodes.Stloc, 43);
            } else {
                BtpTweak.logger_.LogError("Infusion Hook Error");
            }
        }

        private static void ChargedState_OnEnter(On.RoR2.TeleporterInteraction.ChargedState.orig_OnEnter orig, EntityStates.BaseState self) {
            orig(self);
            if (BtpTweak.是否选择造物难度_) {
                BtpTweak.玩家生命值提升系数_ = 0.1f * (1 + Run.instance.stageClearCount);
                BtpTweak.玩家生命值提升倍数_ = 0.1f * Run.instance.stageClearCount;
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
                Vector3 velocity = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 2.5f);
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

        private static void CombatDirector_Simulate(On.RoR2.CombatDirector.orig_Simulate orig, CombatDirector self, float deltaTime) {
            if (TeamComponent.GetTeamMembers(TeamIndex.Monster).Count > 36) {
                return;
            }
            orig(self, deltaTime);
        }

        private static void GravitatePickup_Start(On.RoR2.GravitatePickup.orig_Start orig, GravitatePickup self) {
            orig(self);
            if (self.transform.parent?.name.StartsWith("BonusMoneyPack") ?? false) {
                self.maxSpeed = 50;
                self.gravitateTarget = Helpers.GetClosestPlayerTransform(TeamComponent.GetTeamMembers(TeamIndex.Player), self.transform.position);
            }
        }
    }
}