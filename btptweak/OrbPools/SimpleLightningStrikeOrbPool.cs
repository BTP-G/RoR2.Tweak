using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;

namespace BtpTweak.OrbPools {

    internal class SimpleLightningStrikeOrbPool : OrbPool<ProcChainMask, SimpleLightningStrikeOrb> {
        protected override float OrbInterval => 0.5f;

        public override void AddOrb(GameObject attacker, ProcChainMask key, SimpleLightningStrikeOrb orb) {
            if (!orb.isCrit) {
                key.AddProc(ProcType.LightningStrikeOnHit);
            }
            if (Pool.TryGetValue(attacker, out var pool)) {
                if (pool.TryGetValue(key, out var lightningStrikeOrb)) {
                    lightningStrikeOrb.damageValue += orb.damageValue;
                } else {
                    pool.Add(key, orb);
                }
            } else {
                Pool.Add(attacker, new Dictionary<ProcChainMask, SimpleLightningStrikeOrb>() { { key, orb } });
            }
        }

        protected override void ModifyOrb(ref SimpleLightningStrikeOrb orb) {
            orb.procChainMask.AddProc(ProcType.LightningStrikeOnHit);
        }
    }
}