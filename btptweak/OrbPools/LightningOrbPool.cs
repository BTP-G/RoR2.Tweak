using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.OrbPools {

    internal class LightningOrbPool : OrbPool<ProcChainMask, LightningOrb> {
        protected override float OrbInterval => 0.2f;

        public override void AddOrb(GameObject attacker, ProcChainMask key, LightningOrb orb) {
            if (!orb.isCrit) {
                key.AddProc(ProcType.ChainLightning);
            }
            if (Pool.TryGetValue(attacker, out var pool)) {
                if (pool.TryGetValue(key, out var lightningOrb)) {
                    lightningOrb.damageValue += orb.damageValue;
                } else {
                    pool.Add(key, orb);
                }
            } else {
                Pool.Add(attacker, new Dictionary<ProcChainMask, LightningOrb>() { { key, orb } });
            }
        }

        protected override void ModifyOrb(ref LightningOrb orb) {
            orb.origin = transform.position;
            orb.target = orb.PickNextTarget(orb.origin);
            orb.procChainMask.AddProc(ProcType.ChainLightning);
        }
    }
}