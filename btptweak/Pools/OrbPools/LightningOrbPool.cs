using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;

namespace BTP.RoR2Plugin.Pools.OrbPools {

    internal sealed class LightningOrbPool : Pool<LightningOrbPool, OrbPoolKey, LightningOrb> {
        protected override float Interval => ChainLightningTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, float damageValue) {
            if (pool.TryGetValue(simpleOrbInfo, out var lightningOrb)) {
                lightningOrb.damageValue += damageValue;
            } else {
                pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    bouncesRemaining = ChainLightningTweak.Bounces,
                    canBounceOnSameTarget = false,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    lightningType = LightningOrb.LightningType.Ukulele,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.2f,
                    range = simpleOrbInfo.通用浮点数,
                    target = simpleOrbInfo.target,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in OrbPoolKey orbInfo, in LightningOrb orb) {
            orb.origin = orb.target.transform.position;
            orb.bouncedObjects = [orb.target.healthComponent];
            if (orb.target = orb.PickNextTarget(orb.origin)) {
                orb.procChainMask.AddProc(ProcType.ChainLightning);
                OrbManager.instance.AddOrb(orb);
            }
        }
    }
}