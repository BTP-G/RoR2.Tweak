using BtpTweak.RoR2Indexes;
using BtpTweak.Utils;
using BtpTweak.Utils.RoR2ResourcesPaths;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;

namespace BtpTweak.Tweaks.MithrixTweaks {

    internal class BrotherTweak : TweakBase<BrotherTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.RoR2.PhaseCounter.OnEnable += PhaseCounter_OnEnable;
            On.RoR2.PhaseCounter.OnDisable += PhaseCounter_OnDisable;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            // AI 调整
            var aISkillDrivers = GameObjectPaths.BrotherMaster.LoadComponents<AISkillDriver>();
            for (int i = 0; i < aISkillDrivers.Length; ++i) {
                var aISkillDriver = aISkillDrivers[i];
                if (aISkillDriver.customName.EndsWith("Slam")) {  // Increasing slam distance
                    aISkillDriver.maxDistance = 60f;
                    Main.Logger.LogInfo("Skill Slam Change Finished");
                } else if (aISkillDriver.customName.EndsWith("FireLunarShards")) {  // Making shards more aggressive
                    aISkillDriver.minDistance = 12f;
                    Main.Logger.LogInfo("Skill FireLunarShards Change Finished");
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
                var victimBodyArmor = victimBody.armor;
                float curseStacks = (float)((double)damageReport.damageDealt / damageReport.victim.fullCombinedHealth * (10 * PhaseCounter.instance.phase));
                if ((curseStacks -= victimBodyArmor / (victimBodyArmor > 0 ? 100f + victimBodyArmor : 100f - victimBodyArmor) * curseStacks) > 100f) {
                    curseStacks = 100f;
                }
                while (curseStacks-- > 0) {
                    victimBody.AddBuff(RoR2Content.Buffs.PermanentCurse.buffIndex);
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