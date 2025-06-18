using BTP.RoR2Plugin.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace BTP.RoR2Plugin.Tweaks.MonsterTweaks {

    internal class BeetleTweak : TweakBase<BeetleTweak>, IOnModLoadBehavior, IOnRoR2LoadedBehavior {

        void IOnModLoadBehavior.OnModLoad() {
            On.EntityStates.BeetleQueenMonster.SummonEggs.SummonEgg += SummonEggs_SummonEgg;
        }

        void IOnRoR2LoadedBehavior.OnRoR2Loaded() {
            LegacyResourcesAPI.Load<SkillDef>("SkillDefs/BeetleQueen2Body/BeetleQueen2BodySpit").baseRechargeInterval = 6f;
            var body = GameObjectPaths.BeetleBody8.LoadComponent<CharacterBody>();
            body.baseMaxHealth *= 1.25f;
            body.levelMaxHealth *= 1.25f;
            body.baseMoveSpeed = 8;
            body.baseAttackSpeed = 1.5f;
            var impactExplosion = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/BeetleQueenSpit").GetComponent<ProjectileImpactExplosion>();
            impactExplosion.blastRadius = 6f;
            impactExplosion.childrenDamageCoefficient = 0.1f;
            var beetleQueenAcid = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/BeetleQueenAcid");
            beetleQueenAcid.transform.localScale *= 2.5f;
            var dotZone = beetleQueenAcid.GetComponent<ProjectileDotZone>();
            dotZone.overlapProcCoefficient = 0.5f;
            dotZone.resetFrequency = 5f;
            dotZone.lifetime = 20f;
        }

        private void SummonEggs_SummonEgg(On.EntityStates.BeetleQueenMonster.SummonEggs.orig_SummonEgg orig, EntityStates.BeetleQueenMonster.SummonEggs self) {
            orig(self);
            var selfPosition = self.characterBody.corePosition;
            foreach (var teamMember in TeamComponent.GetTeamMembers(self.teamComponent.teamIndex)) {
                if (teamMember.body && (teamMember.body.corePosition - selfPosition).sqrMagnitude < 900f) {
                    teamMember.body.AddTimedBuff(RoR2Content.Buffs.Warbanner, 15f);
                }
            }
            ;
        }
    }
}