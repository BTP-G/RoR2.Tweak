using BtpTweak.Tweaks.ItemTweaks;
using RoR2;
using RoR2.Orbs;

namespace BtpTweak.Pools.OrbPools {

    internal sealed class ThornsOrbPool : Pool<ThornsOrbPool, OrbPoolKey, LightningOrb> {
        protected override float Interval => ThornsTweak.Interval;

        public void AddOrb(in OrbPoolKey simpleOrbInfo, DamageReport damageReport) {
            if (pool.TryGetValue(simpleOrbInfo, out var lightningOrb)) {
                lightningOrb.damageValue += (ThornsTweak.BaseDamageCoefficient + ThornsTweak.StackDamageCoefficient * simpleOrbInfo.通用浮点数) * damageReport.damageDealt;
                lightningOrb.damageType |= damageReport.damageInfo.damageType;
                if (!lightningOrb.target && damageReport.attackerTeamIndex != lightningOrb.teamIndex) {
                    lightningOrb.target = simpleOrbInfo.target;
                }
            } else {
                lightningOrb = new() {
                    attacker = simpleOrbInfo.attackerBody.gameObject,
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = damageReport.damageInfo.damageType,
                    damageValue = (ThornsTweak.BaseDamageCoefficient + ThornsTweak.StackDamageCoefficient * simpleOrbInfo.通用浮点数) * damageReport.damageDealt,
                    isCrit = simpleOrbInfo.isCrit,
                    lightningType = LightningOrb.LightningType.RazorWire,
                    procCoefficient = simpleOrbInfo.attackerBody.healthComponent.itemCounts.invadingDoppelganger > 0 ? 0 : 0.5f,
                    range = ThornsTweak.BaseRadius + ThornsTweak.StackRadius * simpleOrbInfo.通用浮点数,
                    teamIndex = simpleOrbInfo.attackerBody.teamComponent.teamIndex,
                };
                if (damageReport.attackerTeamIndex != lightningOrb.teamIndex) {
                    lightningOrb.target = simpleOrbInfo.target;
                }
                pool.Add(simpleOrbInfo, lightningOrb);
            }
        }

        protected override void OnTimeOut(in OrbPoolKey orbInfo, in LightningOrb orb) {
            orb.origin = orbInfo.attackerBody.corePosition;
            if (!orb.target) {
                orb.bouncedObjects = [];
                if (orb.target = orb.PickNextTarget(orb.origin)) {
                    orb.procChainMask.AddProc(ProcType.Thorns);
                    OrbManager.instance.AddOrb(orb);
                }
            } else {
                orb.procChainMask.AddProc(ProcType.Thorns);
                OrbManager.instance.AddOrb(orb);
            }
        }
    }
}