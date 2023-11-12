using RoR2;

namespace BtpTweak.Tweaks.ItemTweaks {

    internal class IgniteOnKillTweak : TweakBase<IgniteOnKillTweak> {
        public const float ExplosionBaseDamageCoefficient = 1.5f;
        public const float IgniteDamageCoefficient = 0.75f;
        public const int BaseRadius = 12;
        public const int StackRadius = 4;

        public override void SetEventHandlers() {
            On.RoR2.GlobalEventManager.ProcIgniteOnKill += GlobalEventManager_ProcIgniteOnKill;
        }

        public override void ClearEventHandlers() {
            On.RoR2.GlobalEventManager.ProcIgniteOnKill -= GlobalEventManager_ProcIgniteOnKill;
        }

        private void GlobalEventManager_ProcIgniteOnKill(On.RoR2.GlobalEventManager.orig_ProcIgniteOnKill orig, DamageReport damageReport, int igniteOnKillCount, CharacterBody victimBody, TeamIndex attackerTeamIndex) {
            var attackerBody = damageReport.attackerBody;
            var blastAttack = new BlastAttack {
                attacker = damageReport.attacker,
                attackerFiltering = AttackerFiltering.Default,
                baseDamage = Util.OnKillProcDamage(attackerBody.damage, ExplosionBaseDamageCoefficient),
                crit = attackerBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.AOE,
                falloffModel = BlastAttack.FalloffModel.Linear,
                position = damageReport.damageInfo.position,
                procCoefficient = damageReport.damageInfo.procCoefficient,
                radius = BaseRadius + StackRadius * (igniteOnKillCount - 1) + 0.4f * victimBody.bestFitRadius,
                teamIndex = attackerTeamIndex,
            };
            EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, new EffectData {
                origin = blastAttack.position,
                scale = blastAttack.radius,
                rotation = default,
            }, true);
            var result = blastAttack.Fire();
            if (result.hitCount > 0) {
                var baseInflictDotInfo = new InflictDotInfo {
                    attackerObject = damageReport.attacker,
                    totalDamage = attackerBody.damage * IgniteDamageCoefficient * igniteOnKillCount,
                    dotIndex = DotController.DotIndex.Burn,
                    damageMultiplier = 1f
                };
                StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.inventory, ref baseInflictDotInfo);
                foreach (var hitPoint in result.hitPoints) {
                    var healthComponent = hitPoint.hurtBox.healthComponent;
                    if (healthComponent.alive) {
                        InflictDotInfo inflictDotInfo = baseInflictDotInfo;
                        inflictDotInfo.victimObject = healthComponent.gameObject;
                        DotController.InflictDot(ref inflictDotInfo);
                    }
                }
            }
        }
    }
}