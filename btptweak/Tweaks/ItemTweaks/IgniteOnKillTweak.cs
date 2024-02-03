using RoR2;
using UnityEngine;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class IgniteOnKillTweak : TweakBase<IgniteOnKillTweak>, IOnModLoadBehavior {
        public const float ExplosionBaseDamageCoefficient = 1.2f;
        public const float IgniteDamageCoefficient = 0.6f;
        public const int BaseRadius = 12;
        public const int StackRadius = 6;

        private static readonly BlastAttack _blastAttack = new() {
            attackerFiltering = AttackerFiltering.Default,
            damageColorIndex = DamageColorIndex.Item,
            damageType = DamageType.AOE,
            falloffModel = BlastAttack.FalloffModel.Linear,
            procCoefficient = 0f,
        };

        public void OnModLoad() {
            On.RoR2.GlobalEventManager.ProcIgniteOnKill += GlobalEventManager_ProcIgniteOnKill;
        }

        private void GlobalEventManager_ProcIgniteOnKill(On.RoR2.GlobalEventManager.orig_ProcIgniteOnKill orig, DamageReport damageReport, int igniteOnKillCount, CharacterBody victimBody, TeamIndex attackerTeamIndex) {
            var attackerBody = damageReport.attackerBody;
            _blastAttack.attacker = damageReport.attacker;
            _blastAttack.baseDamage = Util.OnKillProcDamage(attackerBody.damage, ExplosionBaseDamageCoefficient);
            _blastAttack.crit = attackerBody.RollCrit();
            _blastAttack.position = damageReport.damageInfo.position;
            _blastAttack.radius = BaseRadius + StackRadius * (igniteOnKillCount - 1) + 0.4f * victimBody.bestFitRadius;
            _blastAttack.teamIndex = attackerTeamIndex;
            var result = _blastAttack.Fire();
            EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, new EffectData {
                origin = _blastAttack.position,
                scale = _blastAttack.radius,
            }, true);
            if (result.hitCount > 0) {
                var baseDotInfo = new InflictDotInfo {
                    attackerObject = damageReport.attacker,
                    damageMultiplier = IgniteDamageCoefficient * igniteOnKillCount,
                    dotIndex = DotController.DotIndex.Burn,
                    totalDamage = Util.OnHitProcDamage(_blastAttack.baseDamage, attackerBody.damage, IgniteDamageCoefficient * igniteOnKillCount * (_blastAttack.crit ? attackerBody.critMultiplier : 1f)),
                };
                StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.inventory, ref baseDotInfo);
                foreach (var hitPoint in result.hitPoints) {
                    var dotInfo = baseDotInfo;
                    dotInfo.victimObject = hitPoint.hurtBox.healthComponent.gameObject;
                    var reduceCoefficient = 1f - Mathf.Clamp01(Mathf.Sqrt(hitPoint.distanceSqr) / _blastAttack.radius);
                    dotInfo.damageMultiplier *= reduceCoefficient;
                    dotInfo.totalDamage *= reduceCoefficient;
                    DotController.InflictDot(ref dotInfo);
                }
            }
        }
    }
}