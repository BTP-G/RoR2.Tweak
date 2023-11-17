using BtpTweak.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;

namespace BtpTweak.OrbPools {

    internal class LightningOrbPool : OrbPool<SimpleOrbInfo, LightningOrb> {
        private List<HealthComponent> _healthComponent;
        protected override float OrbInterval => 0.2f;
        private List<HealthComponent> HealthComponent => _healthComponent ??= new() { GetComponent<HealthComponent>() };

        public void AddOrb(in SimpleOrbInfo lightningOrbInfo, float damageValue, int stack, TeamIndex teamIndex) {
            if (Pool.TryGetValue(lightningOrbInfo, out var lightningOrb)) {
                lightningOrb.damageValue += damageValue;
            } else {
                Pool.Add(lightningOrbInfo, new() {
                    attacker = lightningOrbInfo.attacker,
                    bouncedObjects = HealthComponent,
                    bouncesRemaining = ChainLightningTweak.Bounces,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = lightningOrbInfo.isCrit,
                    lightningType = LightningOrb.LightningType.Ukulele,
                    procChainMask = lightningOrbInfo.procChainMask,
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
    }
}