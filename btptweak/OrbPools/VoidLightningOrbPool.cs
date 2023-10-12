using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.OrbPools {

    internal class VoidLightningOrbPool : OrbPool<ProcChainMask, VoidLightningOrb> {
        protected override float OrbInterval => 0.6f;

        public override void AddOrb(GameObject attacker, ProcChainMask key, VoidLightningOrb orb) {
            if (!orb.isCrit) {
                key.AddProc(ProcType.ChainLightning);
            }
            if (Pool.TryGetValue(attacker, out var pool)) {
                if (pool.TryGetValue(key, out var voidLightningOrb)) {
                    voidLightningOrb.damageValue += orb.damageValue;
                } else {
                    pool.Add(key, orb);
                }
            } else {
                Pool.Add(attacker, new Dictionary<ProcChainMask, VoidLightningOrb>() { { key, orb } });
            }
        }

        protected override void ModifyOrb(ref VoidLightningOrb orb) {
            orb.origin = transform.position;
            orb.procChainMask.AddProc(ProcType.ChainLightning);
        }
    }
}