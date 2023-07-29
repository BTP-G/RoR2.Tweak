using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using PlasmaCoreSpikestripContent.Content.Skills;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak {

    internal class GlobalEventHook {

        public static void AddHook() {
            IL.RoR2.CharacterBody.OnInventoryChanged += IL_CharacterBody_OnInventoryChanged;
            IL.RoR2.GlobalEventManager.OnCharacterDeath += IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.OnHitEnemy += IL_GlobalEventManager_OnHitEnemy;
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill += IL_GlobalEventManager_ProcIgniteOnKill;
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        public static void RemoveHook() {
            IL.RoR2.CharacterBody.OnInventoryChanged -= IL_CharacterBody_OnInventoryChanged;
            IL.RoR2.GlobalEventManager.OnCharacterDeath -= IL_GlobalEventManager_OnCharacterDeath;
            IL.RoR2.GlobalEventManager.OnHitEnemy -= IL_GlobalEventManager_OnHitEnemy;
            IL.RoR2.GlobalEventManager.ProcIgniteOnKill -= IL_GlobalEventManager_ProcIgniteOnKill;
            On.RoR2.GlobalEventManager.OnCharacterDeath -= GlobalEventManager_OnCharacterDeath;
            On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private static void IL_CharacterBody_OnInventoryChanged(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[3];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 13f);
            array[2] = (Instruction x) => ILPatternMatchingExt.MatchLdarg(x, 0);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Index += 12;
                ilcursor.Emit(OpCodes.Ldarg, 0);
                ilcursor.EmitDelegate(delegate (CharacterBody body) {
                    float num = body.inventory.GetItemCount(RoR2Content.Items.ExecuteLowHealthElite.itemIndex);
                    body.executeEliteHealthFraction = 0.5f * (num / (num + 4));
                });
            } else {
                BtpTweak.logger_.LogError("ExecuteLowHealthElite Hook Error");
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
                ilcursor.RemoveRange(4);
                ilcursor.Emit(OpCodes.Ldloc, 43);
                ilcursor.EmitDelegate(delegate (int itemCount5) {
                    return 100 * BtpTweak.玩家等级_ * itemCount5;
                });
                ilcursor.Emit(OpCodes.Stloc, 63);
                //===
                ilcursor.Emit(OpCodes.Ldloc, 43);
                ilcursor.EmitDelegate(delegate () {
                    return BtpTweak.玩家等级_;
                });
                ilcursor.Emit(OpCodes.Mul);
                ilcursor.Emit(OpCodes.Stloc, 43);
            } else {
                BtpTweak.logger_.LogError("Infusion Hook Error");
            }
            //======
            Func<Instruction, bool>[] array2 = new Func<Instruction, bool>[1];
            array2[0] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 53);
            if (ilcursor.TryGotoPrev(array2)) {
                ++ilcursor.Index;
                ilcursor.Emit(OpCodes.Ldc_R4, 3.5f);
                ilcursor.Emit(OpCodes.Stloc, 53);
            } else {
                BtpTweak.logger_.LogError("ExplodeOnDeath Hook Error");
            }
        }

        private static void IL_GlobalEventManager_OnHitEnemy(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[2];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 1.8f);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchStloc(x, 78);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.Remove();
                ilcursor.Emit(OpCodes.Ldc_R4, 1.5f);
            } else {
                BtpTweak.logger_.LogError("StickyBomb Hook Error");
            }
        }

        private static void IL_GlobalEventManager_ProcIgniteOnKill(ILContext il) {
            ILCursor ilcursor = new(il);
            Func<Instruction, bool>[] array = new Func<Instruction, bool>[2];
            array[0] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 8f);
            array[1] = (Instruction x) => ILPatternMatchingExt.MatchLdcR4(x, 4f);
            if (ilcursor.TryGotoNext(array)) {
                ilcursor.RemoveRange(6);
                ilcursor.Emit(OpCodes.Ldc_R4, 16f);
            } else {
                BtpTweak.logger_.LogError("IgniteOnKill Hook Error");
            }
        }

        private static void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport) {
            orig(self, damageReport);
            if (NetworkServer.active) {  //=== 灭绝之歌
                BuffIndex buffIndex = AncientScepter.AncientScepterMain.perishSongDebuff.buffIndex;
                if (damageReport.victimBody.HasBuff(buffIndex)) {
                    foreach (TeamComponent teamMember in TeamComponent.GetTeamMembers(damageReport.victimTeamIndex)) {
                        if (teamMember.body.bodyIndex == damageReport.victimBodyIndex) {
                            teamMember.body.ClearTimedBuffs(buffIndex);
                            teamMember.body.healthComponent.Suicide(damageReport.attacker);
                            if (Util.CheckRoll(1, damageReport.attackerMaster)) {
                                PickupIndex pickupIndex = Run.instance.treasureRng.NextElementUniform(Run.instance.availableLunarItemDropList);
                                PickupDropletController.CreatePickupDroplet(pickupIndex, teamMember.transform.position, 3 * Vector3.up);
                            }
                        }
                    }
                }
                //======
                CharacterBody attackerBody = damageReport.attackerBody;
                if (attackerBody && attackerBody.name.StartsWith("Merc")) {
                    int itemCount = attackerBody.inventory.GetItemCount(SkillHook.权杖);
                    if (itemCount > 0) {
                        EntityState entityState = attackerBody.GetComponent<EntityStateMachine>()?.state;
                        if (entityState is EntityStates.Merc.Evis) {
                            if ((attackerBody.corePosition - damageReport.victimBody.corePosition).sqrMagnitude < (EntityStates.Merc.Evis.maxRadius * EntityStates.Merc.Evis.maxRadius)) {
                                attackerBody.AddBuff(RoR2Content.Buffs.BanditSkull.buffIndex);
                            }
                        }
                    }
                }
            }
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim) {
            orig(self, damageInfo, victim);
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[1].buffIndex) > 5 * victimBody.GetBuffCount(DeepRot.scriptableObject.buffs[0].buffIndex)) {
                DotController.InflictDot(victim, damageInfo.attacker, DeepRot.deepRotDOT, 20f + 10f * damageInfo.attacker.GetComponent<CharacterBody>().inventory.GetItemCount(SkillHook.权杖), 1f, null);
            }
        }
    }
}