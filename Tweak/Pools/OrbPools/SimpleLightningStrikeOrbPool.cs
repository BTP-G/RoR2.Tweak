using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;

namespace BTP.RoR2Plugin.Pools.OrbPools {

    internal sealed class SimpleLightningStrikeOrbPool : Pool<SimpleLightningStrikeOrbPool, OrbPoolKey, SimpleLightningStrikeOrb> {
        protected override float Interval => LightningStrikeOnHitTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, float damageValue) {
            if (pool.TryGetValue(simpleOrbInfo, out var simpleLightningStrikeOrb)) {
                simpleLightningStrikeOrb.damageValue += damageValue;
                if (!simpleLightningStrikeOrb.target) {
                    simpleLightningStrikeOrb.target = simpleOrbInfo.target;
                }
            } else {
                pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = LightningStrikeOnHitTweak.ProcCoefficient,
                    target = simpleOrbInfo.target,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in OrbPoolKey orbInfo, in SimpleLightningStrikeOrb orb) {
            orb.procChainMask.AddProc(ProcType.LightningStrikeOnHit);
            OrbManager.instance.AddOrb(orb);
        }
    }
}