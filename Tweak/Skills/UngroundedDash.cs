using BTP.RoR2Plugin.Tweaks;
using EntityStates;
using EntityStates.BrotherMonster;
using R2API;
using RoR2.Skills;
using UnityEngine;

namespace BTP.RoR2Plugin.Skills {

    internal class UngroundedDash : TweakBase<UngroundedDash>, IOnModLoadBehavior {
        public static SkillDef SkillDef { get; private set; }

        void IOnModLoadBehavior.OnModLoad() {
            SkillDef = ScriptableObject.CreateInstance<SkillDef>();
            SkillDef.skillName = "LunarDash";
            SkillDef.skillNameToken = "LUNARDASH_NAME";
            SkillDef.skillDescriptionToken = "LUNARDASH_DESCRIPTION";
            SkillDef.activationState = new SerializableEntityStateType(typeof(SlideIntroState));
            SkillDef.activationStateMachineName = "Body";
            SkillDef.baseMaxStock = 1;
            SkillDef.baseRechargeInterval = 5f;
            SkillDef.beginSkillCooldownOnSkillEnd = false;
            SkillDef.canceledFromSprinting = false;
            SkillDef.cancelSprintingOnActivation = true;
            SkillDef.dontAllowPastMaxStocks = false;
            SkillDef.forceSprintDuringState = false;
            SkillDef.fullRestockOnAssign = true;
            SkillDef.interruptPriority = InterruptPriority.Skill;
            SkillDef.isCombatSkill = false;
            SkillDef.rechargeStock = 2;
            SkillDef.requiredStock = 1;
            SkillDef.stockToConsume = 1;
            if (ContentAddition.AddSkillDef(SkillDef)) {
                return;
            }
            "CreateUngroundedDash failed!".LogError();
        }
    }
}