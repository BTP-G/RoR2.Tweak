using BtpTweak.Tweaks.EliteTweaks;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Pools.OrbPools {

    internal sealed class FrostBladeOrbPool : Pool<FrostBladeOrbPool, OrbPoolKey, LightningOrb> {
        protected override float Interval => IceTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, float damageValue) {
            if (pool.TryGetValue(simpleOrbInfo, out var lightningOrb)) {
                lightningOrb.damageValue += damageValue;
            } else {
                pool.Add(simpleOrbInfo, new LightningOrb {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    bouncedObjects = null,
                    bouncesRemaining = 0,
                    damageCoefficientPerBounce = 1f,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    lightningType = LightningOrb.LightningType.RazorWire,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0f,
                    range = 0f,
                    target = simpleOrbInfo.target,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in OrbPoolKey orbInfo, in LightningOrb orb) {
            if (orbInfo.attackerBody) {
                orb.origin = orbInfo.attackerBody.corePosition;
                OrbManager.instance.AddOrb(orb);
            }
        }
    }
}