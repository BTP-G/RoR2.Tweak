using RoR2;
using RoR2.Orbs;

namespace BtpTweak.OrbPools {

    internal class SimpleLightningStrikeOrbPool : OrbPool<SimpleOrbInfo, SimpleLightningStrikeOrb> {
        protected override float OrbInterval => 0.5f;

        public void AddOrb(in SimpleOrbInfo simpleOrbInfo, float damageValue) {
            if (Pool.TryGetValue(simpleOrbInfo, out var simpleLightningStrikeOrb)) {
                simpleLightningStrikeOrb.damageValue += damageValue;
                if (!simpleLightningStrikeOrb.target) {
                    simpleLightningStrikeOrb.target = simpleOrbInfo.target;
                }
            } else {
                Pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attacker,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.5f,
                    target = simpleOrbInfo.target,
                });
            }
        }

        protected override void ModifyOrb(ref SimpleLightningStrikeOrb orb) {
            orb.procChainMask.AddProc(ProcType.LightningStrikeOnHit);
        }
    }
}