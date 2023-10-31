using BtpTweak.Utils;
using HG;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.OrbPools {

    internal class BounceOrbPool : OrbPool<ProcChainMask, BounceOrb> {
        protected override float OrbInterval => 0.33f;

        public override void AddOrb(GameObject attacker, ProcChainMask key, BounceOrb orb) {
            if (!orb.isCrit) {
                key.AddProc(ProcType.BounceNearby);
            }
            if (Pool.TryGetValue(attacker, out var pool)) {
                if (pool.TryGetValue(key, out var lightningOrb)) {
                    lightningOrb.damageValue += orb.damageValue;
                } else {
                    pool.Add(key, orb);
                }
            } else {
                Pool.Add(attacker, new Dictionary<ProcChainMask, BounceOrb>() { { key, orb } });
            }
        }

        protected override void ModifyOrb(ref BounceOrb orb) {
            BullseyeSearch search = new();
            List<HurtBox> list = CollectionPool<HurtBox, List<HurtBox>>.RentCollection();
            List<HealthComponent> list2 = CollectionPool<HealthComponent, List<HealthComponent>>.RentCollection();
            var victimBody = GetComponent<CharacterBody>();
            if (victimBody && victimBody.healthComponent) {
                list2.Add(victimBody.healthComponent);
            }
            BounceOrb.SearchForTargets(search, orb.teamIndex, transform.position, 33f, 6, list, list2);
            CollectionPool<HealthComponent, List<HealthComponent>>.ReturnCollection(list2);
            orb.procChainMask.AddProc(ProcType.BounceNearby);
            orb.procChainMask.AddPoolProcs();

            for (int i = 0, count = list.Count; i < count; ++i) {
                HurtBox target = list[i];
                if (target) {
                    BounceOrb bounceOrb = new() {
                        origin = transform.position,
                        damageValue = orb.damageValue,
                        isCrit = orb.isCrit,
                        teamIndex = orb.teamIndex,
                        attacker = orb.attacker,
                        procChainMask = orb.procChainMask,
                        procCoefficient = 0.33f,
                        damageColorIndex = DamageColorIndex.Item,
                        bouncedObjects = new() { victimBody?.healthComponent },
                        target = target
                    };
                    OrbManager.instance.AddOrb(bounceOrb);
                }
            }
            CollectionPool<HurtBox, List<HurtBox>>.ReturnCollection(list);
        }
    }
}