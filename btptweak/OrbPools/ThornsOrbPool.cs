using BtpTweak.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace BtpTweak.OrbPools {

    [RequireComponent(typeof(HealthComponent))]
    internal class ThornsOrbPool : OrbPool<SimpleOrbInfo, LightningOrb> {
        private HealthComponent _healthComponent;
        protected override float OrbInterval => 0.1f;

        public void AddOrb(in SimpleOrbInfo simpleOrbInfo, DamageReport damageReport) {
            if (Pool.TryGetValue(simpleOrbInfo, out var lightningOrb)) {
                lightningOrb.damageValue += ThornsTweak.BaseDamageCoefficient * damageReport.damageDealt;
                lightningOrb.damageType |= damageReport.damageInfo.damageType;
                if (!lightningOrb.target && damageReport.attackerTeamIndex != lightningOrb.teamIndex) {
                    lightningOrb.target = damageReport.attackerBody?.mainHurtBox;
                }
            } else {
                lightningOrb = new() {
                    attacker = simpleOrbInfo.attacker,
                    bouncedObjects = new(),
                    bouncesRemaining = _healthComponent.itemCounts.thorns - 1,
                    damageCoefficientPerBounce = 1f,
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = damageReport.damageInfo.damageType,
                    damageValue = ThornsTweak.BaseDamageCoefficient * damageReport.damageDealt,
                    isCrit = simpleOrbInfo.isCrit,
                    lightningType = LightningOrb.LightningType.RazorWire,
                    procCoefficient = _healthComponent.itemCounts.invadingDoppelganger > 0 ? 0 : 0.5f,
                    range = ThornsTweak.Radius * _healthComponent.itemCounts.thorns,
                    teamIndex = _healthComponent.body.teamComponent.teamIndex,
                };
                if (damageReport.attackerTeamIndex != lightningOrb.teamIndex) {
                    lightningOrb.target = damageReport.attackerBody?.mainHurtBox;
                }
                Pool.Add(simpleOrbInfo, lightningOrb);
            }
        }

        protected override void ModifyOrb(ref LightningOrb orb) {
            orb.origin = transform.position;
            orb.target ??= orb.PickNextTarget(orb.origin);
            orb.procChainMask.AddProc(ProcType.Thorns);
        }

        private void Awake() {
            _healthComponent = GetComponent<HealthComponent>();
        }
    }
}