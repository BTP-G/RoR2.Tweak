using BtpTweak.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Pools.OrbPools {

    internal sealed class MissileVoidOrbPool : Pool<MissileVoidOrbPool, OrbPoolKey, MissileVoidOrb> {
        protected override float Interval => MissileVoidTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, float damageValue) {
            if (pool.TryGetValue(simpleOrbInfo, out var bounceOrb)) {
                bounceOrb.damageValue += damageValue;
            } else {
                pool.Add(simpleOrbInfo, new() {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    damageColorIndex = DamageColorIndex.Void,
                    damageValue = damageValue,
                    isCrit = simpleOrbInfo.isCrit,
                    origin = simpleOrbInfo.attackerBody.corePosition,
                    procChainMask = simpleOrbInfo.procChainMask,
                    procCoefficient = 0.2f,
                    target = simpleOrbInfo.target,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in OrbPoolKey orbInfo, in MissileVoidOrb orb) {
            orb.procChainMask.AddProc(ProcType.Missile);
            if (orbInfo.attackerBody) {
                orb.origin = orbInfo.attackerBody.corePosition;
            }
            if (orbInfo.通用浮点数 > 0) {
                orb.damageValue *= (orbInfo.通用浮点数 + 1f) * 0.5f;
                OrbManager.instance.AddOrb(new MissileVoidOrb {
                    attacker = orb.attacker,
                    damageColorIndex = orb.damageColorIndex,
                    damageValue = orb.damageValue,
                    isCrit = orb.isCrit,
                    origin = orb.origin,
                    procChainMask = orb.procChainMask,
                    procCoefficient = orb.procCoefficient,
                    target = orb.target,
                    teamIndex = orb.teamIndex,
                });
                OrbManager.instance.AddOrb(new MissileVoidOrb {
                    attacker = orb.attacker,
                    damageColorIndex = orb.damageColorIndex,
                    damageValue = orb.damageValue,
                    isCrit = orb.isCrit,
                    origin = orb.origin,
                    procChainMask = orb.procChainMask,
                    procCoefficient = orb.procCoefficient,
                    target = orb.target,
                    teamIndex = orb.teamIndex,
                });
            }
            OrbManager.instance.AddOrb(orb);
        }
    }
}