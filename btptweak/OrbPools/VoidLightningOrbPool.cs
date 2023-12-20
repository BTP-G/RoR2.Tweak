using BtpTweak.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.OrbPools {

    internal class VoidLightningOrbPool : OrbPool<SimpleOrbInfo, VoidLightningOrb> {
        protected override float OrbInterval => 0.2f;

        public void AddOrb(in SimpleOrbInfo simpleOrbInfo, float damageValue, TeamIndex teamIndex) {
            if (Pool.TryGetValue(simpleOrbInfo, out var voidLightningOrb)) {
                voidLightningOrb.damageValue += damageValue;
            } else {
                Pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attacker,
                    damageColorIndex = DamageColorIndex.Void,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.2f,
                    secondsPerStrike = 0.1f,
                    target = simpleOrbInfo.target,
                    teamIndex = teamIndex,
                    totalStrikes = ChainLightningVoidTweak.TotalStrikes,
                });
            }
        }

        protected override void ModifyOrb(ref VoidLightningOrb orb) {
            orb.origin = transform.position;
            orb.procChainMask.AddProc(ProcType.ChainLightning);
        }
    }
}