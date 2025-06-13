using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using HG;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;

namespace BTP.RoR2Plugin.Pools.OrbPools {

    internal sealed class BounceOrbPool : Pool<BounceOrbPool, OrbPoolKey, BounceOrb> {
        private static readonly BullseyeSearch search = new();
        protected override float Interval => BounceNearbyTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, float damageValue) {
            if (pool.TryGetValue(simpleOrbInfo, out var bounceOrb)) {
                bounceOrb.damageValue += damageValue;
            } else {
                pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.33f,
                    target = simpleOrbInfo.target,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in OrbPoolKey orbInfo, in BounceOrb orb) {
            orb.procChainMask.AddProc(ProcType.BounceNearby);
            var position = orb.target.transform.position;
            var dest = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
            var 排除项 = CollectionPool<HealthComponent, List<HealthComponent>>.RentCollection();
            排除项.Add(orb.target.healthComponent);
            BounceOrb.SearchForTargets(search, orb.teamIndex, position, BounceNearbyTweak.BaseRadius, BounceNearbyTweak.BaseMaxTargets, dest, 排除项);
            CollectionPool<HealthComponent, List<HealthComponent>>.ReturnCollection(排除项);
            if (dest.Count > 0) {
                orb.origin = position;
                orb.target = dest[0];
                OrbManager.instance.AddOrb(orb);
            }
            for (int i = 1; i < dest.Count; ++i) {
                OrbManager.instance.AddOrb(new BounceOrb() {
                    origin = position,
                    damageValue = orb.damageValue,
                    isCrit = orb.isCrit,
                    teamIndex = orb.teamIndex,
                    attacker = orb.attacker,
                    procChainMask = orb.procChainMask,
                    procCoefficient = 0.33f,
                    damageColorIndex = DamageColorIndex.Item,
                    target = dest[i],
                });
            }
            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(dest);
        }
    }
}