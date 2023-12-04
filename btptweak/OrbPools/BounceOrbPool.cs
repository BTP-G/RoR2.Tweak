using BtpTweak.Tweaks.ItemTweaks;
using HG;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;

namespace BtpTweak.OrbPools {

    internal class BounceOrbPool : OrbPool<SimpleOrbInfo, BounceOrb> {
        private HealthComponent _healthComponent;
        protected override float OrbInterval => 0.33f;

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
            if (_healthComponent) {
                list2.Add(_healthComponent);
            }
            BounceOrb.SearchForTargets(search, orb.teamIndex, transform.position, BounceNearbyTweak.BaseRadius, BounceNearbyTweak.BaseMaxTargets, list, list2);
            CollectionPool<HealthComponent, List<HealthComponent>>.ReturnCollection(list2);
            for (int i = 0, count = list.Count; i < count; ++i) {
                var target = list[i];
                if (target) {
                    OrbManager.instance.AddOrb(new BounceOrb() {
                        origin = transform.position,
                        damageValue = orb.damageValue,
                        isCrit = orb.isCrit,
                        teamIndex = orb.teamIndex,
                        attacker = orb.attacker,
                        procChainMask = orb.procChainMask,
                        procCoefficient = 0.33f,
                        damageColorIndex = DamageColorIndex.Item,
                        bouncedObjects = new() { _healthComponent },
                        target = target
                    });
                }
            }
            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
        }

        private void Awake() {
            _healthComponent = GetComponent<HealthComponent>();
        }
    }
}