using BtpTweak.RoR2Indexes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using BtpTweak.Utils.RoR2ResourcesPaths;
using BtpTweak.Utils;
using RoR2.Skills;
using EntityStates.Treebot.Weapon;

namespace BtpTweak.Tweaks.SurvivorTweaks {

    internal class TreebotTweak : TweakBase<TreebotTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {
        public const float HurtBoxSizeMultiplier = 0.66f;
        public const float FirePlantSonicBoomDamageCoefficient = 5f;

        void IOnModLoadBehavior.OnModLoad() {
            IL.RoR2.HealthComponent.Heal += HealthComponent_Heal;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            var capsule = RoR2Content.Survivors.Treebot.bodyPrefab.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).Find("TempHurtbox").GetComponent<CapsuleCollider>();
            capsule.radius = 1.42f * HurtBoxSizeMultiplier;
            capsule.height = 4.26f * HurtBoxSizeMultiplier;
            SkillDefPaths.TreebotBodyPlantSonicBoom.Load<SkillDef>().baseRechargeInterval = 2.5f;
            FirePlantSonicBoom.damageCoefficient = FirePlantSonicBoomDamageCoefficient;
        }

        private void HealthComponent_Heal(ILContext il) {
            var cursor = new ILCursor(il);
            if (cursor.TryGotoNext(x => ILPatternMatchingExt.MatchLdfld<HealthComponent.ItemCounts>(x, "increaseHealing"))) {
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
                Main.Logger.LogError("Treebot HealHook Failed!");
            }
        }
    }
}