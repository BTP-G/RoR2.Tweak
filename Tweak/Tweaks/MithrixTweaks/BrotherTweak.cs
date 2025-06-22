﻿using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using GuestUnion;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.MithrixTweaks {

    internal class BrotherTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {

        void IModLoadMessageHandler.Handle() {
            On.RoR2.PhaseCounter.OnEnable += PhaseCounter_OnEnable;
            On.RoR2.PhaseCounter.OnDisable += PhaseCounter_OnDisable;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            // AI 调整
            var aISkillDrivers = GameObjectPaths.BrotherMaster.LoadComponents<AISkillDriver>();
            for (int i = 0; i < aISkillDrivers.Length; ++i) {
                var aISkillDriver = aISkillDrivers[i];
                if (aISkillDriver.customName.EndsWith("Slam")) {  // Increasing slam distance
                    aISkillDriver.maxDistance = 60f;
                    LogExtensions.LogInfo("Skill Slam Change Finished");
                } else if (aISkillDriver.customName.EndsWith("FireLunarShards")) {  // Making shards more aggressive
                    aISkillDriver.minDistance = 12f;
                    LogExtensions.LogInfo("Skill FireLunarShards Change Finished");
                }
            }
            // 防止被斩杀
            var body = GameObjectPaths.BrotherBody.LoadComponent<CharacterBody>();
            body.bodyFlags |= CharacterBody.BodyFlags.ImmuneToExecutes;
            // 防止被冰冻
            body.GetComponent<SetStateOnHurt>().canBeFrozen = false;
            // 移动
            body.baseJumpPower *= 2f;
            body.baseAcceleration *= 3f;
            // 质量 10000； 空中控制 1.5
            var motor = body.GetComponent<CharacterMotor>();
            motor.mass = 10000f;
            motor.airControl = 1.5f;
            // 血量
            body.baseMaxHealth = 30000f;
            body.levelMaxHealth = 9000f;
        }

        private void PhaseCounter_OnEnable(On.RoR2.PhaseCounter.orig_OnEnable orig, PhaseCounter self) {
            orig(self);
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = 10;
        }

        private void PhaseCounter_OnDisable(On.RoR2.PhaseCounter.orig_OnDisable orig, PhaseCounter self) {
            orig(self);
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            TeamCatalog.GetTeamDef(TeamIndex.Monster).softCharacterLimit = 40;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport) {
            if (damageReport.attackerBodyIndex == BodyIndexes.Brother) {
                var victimBody = damageReport.victimBody;
                var victimBodyArmor = (int)victimBody.armor;
                var curseStacks = (int)(damageReport.damageDealt / damageReport.victim.fullCombinedHealth) * 10 * PhaseCounter.instance.phase;
                var a = victimBodyArmor / (100f + victimBodyArmor.Abs());
                curseStacks -= (int)(a * curseStacks);
                if (curseStacks > 100) {
                    curseStacks = 100;
                }
                if (curseStacks > 0) {
                    var buffCount = victimBody.GetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex);
                    victimBody.SetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex, buffCount + curseStacks);
                }
            }
        }

        private class BrotherBodyComponent : MonoBehaviour, IOnTakeDamageServerReceiver {

            public void OnTakeDamageServer(DamageReport damageReport) {
                if (damageReport.hitLowHealth) {
                    damageReport.victimBody.AddBuff(RoR2Content.Buffs.TonicBuff.buffIndex);
                }
            }
        }
    }
}