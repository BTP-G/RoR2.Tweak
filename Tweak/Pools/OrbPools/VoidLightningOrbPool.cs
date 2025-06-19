using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;

namespace BTP.RoR2Plugin.Pools.OrbPools {

    internal sealed class VoidLightningOrbPool : Pool<VoidLightningOrbPool, OrbPoolKey, VoidLightningOrb> {
        protected override float Interval => ChainLightningVoidTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, float damageValue) {
            if (pool.TryGetValue(simpleOrbInfo, out var voidLightningOrb)) {
                voidLightningOrb.damageValue += damageValue;
            } else {
                pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    damageColorIndex = DamageColorIndex.Void,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.2f,
                    secondsPerStrike = 0.1f,
                    target = simpleOrbInfo.target,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                    totalStrikes = ChainLightningVoidTweak.TotalStrikes,
                });
            }
        }

        protected override void OnTimeOut(in OrbPoolKey key, in VoidLightningOrb orb) {
            orb.origin = orb.target.transform.position;
            orb.procChainMask.AddProc(ProcType.ChainLightning);
            OrbManager.instance.AddOrb(orb);
        }
    }
}