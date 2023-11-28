using BtpTweak.Tweaks.ItemTweaks;
using BtpTweak.Utils;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.OrbPools {

    internal class LightningOrbPool : OrbPool<SimpleOrbInfo, LightningOrb> {
        private HealthComponent _healthComponent;
        protected override float OrbInterval => 0.2f;

        public void AddOrb(in SimpleOrbInfo simpleOrbInfo, float damageValue, int stack, TeamIndex teamIndex) {
            if (Pool.TryGetValue(simpleOrbInfo, out var lightningOrb)) {
                lightningOrb.damageValue += damageValue;
            } else {
                Pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attacker,
                    bouncedObjects = new() { _healthComponent },
                    bouncesRemaining = ChainLightningTweak.Bounces,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    lightningType = LightningOrb.LightningType.Ukulele,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.2f,
                    range = ChainLightningTweak.BaseRadius + ChainLightningTweak.StackRadius * (stack - 1),
                    teamIndex = teamIndex,
                });
            }
        }

        protected override void ModifyOrb(ref LightningOrb orb) {
            orb.origin = transform.position;
            orb.target = orb.PickNextTarget(orb.origin);
            orb.procChainMask.AddProc(ProcType.ChainLightning);
        }

        private void Awake() {
            _healthComponent = GetComponent<HealthComponent>();
        }
    }
}