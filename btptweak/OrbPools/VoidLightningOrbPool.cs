using RoR2;
using RoR2.Orbs;
using System;

namespace BtpTweak.OrbPools {

    [Obsolete]
    internal class VoidLightningOrbPool : OrbPool<SimpleOrbInfo, VoidLightningOrb> {
        protected override float OrbInterval => 0.6f;

        protected override void ModifyOrb(ref VoidLightningOrb orb) {
            orb.origin = transform.position;
            orb.procChainMask.AddProc(ProcType.ChainLightning);
        }
    }
}