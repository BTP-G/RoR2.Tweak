using BTP.RoR2Plugin.RoR2Indexes;
using BTP.RoR2Plugin.Utils;
using EntityStates.Treebot.Weapon;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.SurvivorTweaks {

    internal class TreebotTweak : ModComponent, IModLoadMessageHandler, IRoR2LoadedMessageHandler {
        public const float HurtBoxSizeMultiplier = 0.66f;
        public const float FirePlantSonicBoomDamageCoefficient = 5f;

        void IModLoadMessageHandler.Handle() {
            IL.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }

        void IRoR2LoadedMessageHandler.Handle() {
            var capsule = RoR2Content.Survivors.Treebot.bodyPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("TempHurtbox").GetComponent<CapsuleCollider>();
            capsule.radius = 1.42f * HurtBoxSizeMultiplier;
            capsule.height = 4.26f * HurtBoxSizeMultiplier;
            SkillDefPaths.TreebotBodyPlantSonicBoom.Load<SkillDef>().baseRechargeInterval = 2.5f;
            FirePlantSonicBoom.damageCoefficient = FirePlantSonicBoomDamageCoefficient;
            RecalculateStatsTweak.AddRecalculateStatsActionToBody(BodyIndexes.Treebot, RecalculateTreebotStats);
        }

        private void RecalculateTreebotStats(CharacterBody body, Inventory inventory, RecalculateStatsAPI.StatHookEventArgs args) {
            args.cooldownMultAdd -= 1f - Mathf.Pow(0.9f, inventory.GetItemCount(RoR2Content.Items.EquipmentMagazine.itemIndex));
        }

        private void HealthComponent_Heal(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(x => x.MatchLdfld<HealthComponent.ItemCounts>("increaseHealing"))) {
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.EmitDelegate((HealthComponent healthComponent, float amount) => {
                    if (healthComponent.body.bodyIndex == BodyIndexes.Treebot) {
                        return amount * (1 + 0.2f * healthComponent.body.inventory.GetItemCount(RoR2Content.Items.TPHealingNova.itemIndex));
                    }
                    return amount;
                });
                cursor.Emit(OpCodes.Starg, 1);
            } else {
                LogExtensions.LogError("Treebot HealHook Failed!");
            }
        }
    }
}