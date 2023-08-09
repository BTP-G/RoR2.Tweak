using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2.Orbs;
using RoR2;
using System;
using UnityEngine.Networking;
using UnityEngine;
using RoR2.Artifacts;

namespace BtpTweak {

    internal class GlobalEventHook {
        public static BodyIndex 工程师固定炮台_;
        public static BodyIndex 工程师移动炮台_;
        public static BodyIndex 雇佣兵_;
        public static float 牺牲保底概率_ = 0;

        public static void AddHook() {
            IL.RoR2.CharacterBody.OnInventoryChanged += IL_CharacterBody_OnInventoryChanged;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill += IL_GlobalEventManager_ProcIgniteOnKill;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onTeamLevelUp += GlobalEventManager_onTeamLevelUp;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.Artifacts.SacrificeArtifactManager.OnServerCharacterDeath += SacrificeArtifactManager_OnServerCharacterDeath;
        }

        public static void RemoveHook() {
            IL.RoR2.CharacterBody.OnInventoryChanged -= IL_CharacterBody_OnInventoryChanged;
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill -= IL_GlobalEventManager_ProcIgniteOnKill;
            GlobalEventManager.onCharacterDeathGlobal -= GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onTeamLevelUp -= GlobalEventManager_onTeamLevelUp;
            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private static void IL_CharacterBody_OnInventoryChanged(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[3];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 13f);
            array[2] = (Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0);
            if (ilcursor.TryGotoNext(array)) {
                ++ilcursor.Index;
                ilcursor.RemoveRange(11);
                ilcursor.EmitDelegate(delegate (CharacterBody body) {
                    float num = body.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite.itemIndex);
                    body.executeEliteHealthFraction = 0.5f * (num / (num + 4));
                });
            } else {
                Main.logger_.LogError("ExecuteLowHealthElite Hook Error");
            }
        }

        private static void IL_GlobalEventManager_OnCharacterDeath(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[4];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdloc(x, 43);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcI4(x, 100);
            array[2] = (Instruction x) => ILPatternMatchingExt.MatchMul(x);
            array[3] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 63);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.RemoveRange(25);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 17);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.Emit(OpCodes.Ldloc, 43);
                ilcursor.EmitDelegate(delegate (CharacterBody attackerBody, Inventory inventory, Vector3 pos, int itemCount5) {
                    if (inventory.infusionBonus < Convert.ToUInt64(attackerBody.level * attackerBody.levelMaxHealth * itemCount5)) {
                        InfusionOrb infusionOrb = new() {
                            origin = pos,
                            target = attackerBody.mainHurtBox,
                            maxHpValue = itemCount5
                        };
                        OrbManager.instance.AddOrb(infusionOrb);
                    }
                    if (attackerBody.bodyIndex == 工程师固定炮台_ || attackerBody.bodyIndex == 工程师移动炮台_) {
                        foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(attackerBody.teamComponent.teamIndex)) {
                            CharacterBody body = teamMember.body;
                            if (body.isPlayerControlled) {
                                if (body.inventory.infusionBonus < Convert.ToUInt64(body.level * body.levelMaxHealth * itemCount5)) {
                                    InfusionOrb infusionOrb = new() {
                                        origin = pos,
                                        target = body.mainHurtBox,
                                        maxHpValue = itemCount5
                                    };
                                    OrbManager.instance.AddOrb(infusionOrb);
                                }
                            }
                        }
                    }
                });
            } else {
                Main.logger_.LogError("Infusion Hook Error");
            }
            //======
            Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
            array2[0] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 53);
            if (ilcursor.TryGotoPrev(array2)) {
                ++ilcursor.Index;
                ilcursor.Emit(OpCodes.Ldc_R4, 4f);
                ilcursor.Emit(OpCodes.Stloc, 53);
            } else {
                Main.logger_.LogError("ExplodeOnDeath Hook Error");
            }
            //======
            array2[0] = (Instruction x) => ILPatternMatchingExt.MatchLdstr(x, "Prefabs/NetworkedObjects/BonusMoneyPack");
            if (ilcursor.TryGotoNext(array2)) {
                ilcursor.RemoveRange(15);
                ilcursor.Emit(OpCodes.Ldloc, 15);
                ilcursor.Emit(OpCodes.Ldloc, 18);
                ilcursor.Emit(OpCodes.Ldloc, 6);
                ilcursor.EmitDelegate(delegate (CharacterBody attacterBody, TeamIndex attacterTeamindex, Vector3 pos) {
                    GameObject BonusMoneyPack = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BonusMoneyPack"), pos, UnityEngine.Random.rotation);
                    TeamFilter TeamFilter = BonusMoneyPack.GetComponent<TeamFilter>();
                    if (TeamFilter) {
                        TeamFilter.teamIndex = attacterTeamindex;
                        BonusMoneyPack.GetComponentInChildren<GravitatePickup>().gravitateTarget = attacterBody.coreTransform;
                    }
                    NetworkServer.Spawn(BonusMoneyPack);
                });
            } else {
                Main.logger_.LogError("BonusGoldPackOnKill Hook Error");
            }
        }

        private static void IL_GlobalEventManager_ProcIgniteOnKill(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[2];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 8f);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 4f);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.RemoveRange(6);
                ilcursor.Emit(OpCodes.Ldc_R4, 20f);
            } else {
                Main.logger_.LogError("IgniteOnKill Hook Error");
            }
        }

        private static void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport) {
            if (NetworkServer.active) {  //=== 灭绝之歌
                if (damageReport.victimBody.HasBuff(AncientScepter.AncientScepterMain.perishSongDebuff.buffIndex)) {
                    foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(damageReport.victimTeamIndex)) {
                        if (teamMember.body.bodyIndex == damageReport.victimBodyIndex) {
                            teamMember.body.ClearTimedBuffs(AncientScepter.AncientScepterMain.perishSongDebuff.buffIndex);
                            teamMember.body.healthComponent.Suicide(damageReport.attacker);
                            if (Util.CheckRoll(1, damageReport.attackerMaster)) {
                                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableLunarItemDropList);
                                PickupDropletController.CreatePickupDroplet(pickupIndex, teamMember.transform.position, 3 * Vector3.up);
                            }
                        }
                    }
                }
                CharacterBody attackerBody = damageReport.attackerBody;
                if (attackerBody?.bodyIndex == 雇佣兵_) {
                    if (attackerBody.inventory.GetItemCount(SkillHook.古代权杖_) > 0) {
                        EntityState entityState = attackerBody.GetComponent<EntityStateMachine>()?.state;
                        if (entityState is EntityStates.Merc.Evis) {
                            if ((attackerBody.corePosition - damageReport.victimBody.corePosition).sqrMagnitude < (EntityStates.Merc.Evis.maxRadius * EntityStates.Merc.Evis.maxRadius)) {
                                attackerBody.AddTimedBuff(RoR2Content.Buffs.BanditSkull, 1);
                            }
                        }
                    }
                }
            }
        }

        private static void GlobalEventManager_onTeamLevelUp(TeamIndex teamIndex) {
            if (TeamIndex.Player == teamIndex) {
                SkillHook.LevelUp(Convert.ToInt32(TeamManager.instance.GetTeamLevel(teamIndex)));
            }
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
            orig(self, damageInfo, victim);
            if (NetworkServer.active) {
                CharacterBody victimBody = victim.GetComponent<CharacterBody>();
                if (victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[1].buffIndex) > 5 * victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[0].buffIndex)) {
                    DotController.InflictDot(victim, damageInfo.attacker, DeepRot.deepRotDOT, 20f + 10f * damageInfo.attacker.GetComponent<CharacterBody>().inventory.GetItemCount(SkillHook.古代权杖_));
                }
            }
        }

        private static void SacrificeArtifactManager_OnServerCharacterDeath(On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnServerCharacterDeath orig, DamageReport damageReport) {
            if (!damageReport.victimMaster) {
                return;
            }
            if (damageReport.attackerTeamIndex == damageReport.victimTeamIndex && damageReport.victimMaster.minionOwnership.ownerMaster) {
                return;
            }
            float expAdjustedDropChancePercent = Util.GetExpAdjustedDropChancePercent(5 + 牺牲保底概率_, damageReport.victim.gameObject);
            牺牲保底概率_ += 1f;
            Debug.LogFormat("Drop chance from {0} == {1}", damageReport.victimBody, expAdjustedDropChancePercent);
            if (Util.CheckRoll(expAdjustedDropChancePercent, 0f, null)) {
                PickupIndex pickupIndex = SacrificeArtifactManager.dropTable.GenerateDrop(SacrificeArtifactManager.treasureRng);
                if (pickupIndex != PickupIndex.none) {
                    PickupDropletController.CreatePickupDroplet(pickupIndex, damageReport.victimBody.corePosition, Vector3.up * 20f);
                    牺牲保底概率_ = 0;
                }
            }
        }
    }
}