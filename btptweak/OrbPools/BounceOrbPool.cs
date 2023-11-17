using BtpTweak.Utils;
using HG;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;

namespace BtpTweak.OrbPools {

    internal class BounceOrbPool : OrbPool<SimpleOrbInfo, BounceOrb> {
        private List<HealthComponent> _healthComponent;
        protected override float OrbInterval => 0.33f;
        private List<HealthComponent> HealthComponent => _healthComponent ??= new List<HealthComponent> { GetComponent<HealthComponent>() };

        public void AddOrb(in SimpleOrbInfo simpleOrbInfo, float damageValue, TeamIndex teamIndex) {
            if (Pool.TryGetValue(simpleOrbInfo, out var bounceOrb)) {
                bounceOrb.damageValue += damageValue;
            } else {
                Pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attacker,
                    damageColorIndex = DamageColorIndex.Item,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.33f,
                    teamIndex = teamIndex,
                });
            }
        }

        protected override void ModifyOrb(ref BounceOrb orb) {
            orb.procChainMask.AddProc(ProcType.BounceNearby);
            var search = new BullseyeSearch();
            var list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
            var list2 = CollectionPool<HealthComponent, List<HealthComponent>>.RentCollection();
            if (HealthComponent.Count > 0) {
                list2.AddRange(HealthComponent);
            }
            BounceOrb.SearchForTargets(search, orb.teamIndex, transform.position, 33f, 6, list, list2);
            CollectionPool<HealthComponent, List<HealthComponent>>.ReturnCollection(list2);
            for (int i = 0, count = list.Count; i < count; ++i) {
                var target = list[i];
                if (target) {
                    var bounceOrb = new BounceOrb() {
                        origin = transform.position,
                        damageValue = orb.damageValue,
                        isCrit = orb.isCrit,
                        teamIndex = orb.teamIndex,
                        attacker = orb.attacker,
                        procChainMask = orb.procChainMask,
                        procCoefficient = 0.33f,
                        damageColorIndex = DamageColorIndex.Item,
                        bouncedObjects = HealthComponent,
                        target = target
                    };
                    OrbManager.instance.AddOrb(bounceOrb);
                }
            }
            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
        }
    }
}