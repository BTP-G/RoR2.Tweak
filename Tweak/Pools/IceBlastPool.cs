using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using RoR2;
using System;
using UnityEngine;

namespace BTP.RoR2Plugin.Pools {

    public struct IceBlastPoolKey {
        public bool crit;
        public GameObject attacker;
        public ProcChainMask procChainMask;
        public TeamIndex teamIndex;
    }

    internal sealed class IceBlastPool : Pool<IceBlastPool, IceBlastPoolKey, BlastAttack> {
        protected override float Interval => RingsTweak.IceRingInterval;

        public void AddIceBlast(in IceBlastPoolKey attackInfo, in Vector3 position, float damageValue) {
            if (pool.TryGetValue(attackInfo, out var blastAttack)) {
                blastAttack.position = position;
                blastAttack.baseDamage += damageValue;
            } else {
                pool.Add(attackInfo, new() {
                    attacker = attackInfo.attacker,
                    baseDamage = damageValue,
                    canRejectForce = true,
                    crit = attackInfo.crit,
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.AOE,
                    falloffModel = BlastAttack.FalloffModel.None,
                    position = position,
                    procChainMask = attackInfo.procChainMask,
                    procCoefficient = RingsTweak.IceRingProcCoefficient,
                    radius = RingsTweak.IceRingRadius,
                    teamIndex = attackInfo.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in IceBlastPoolKey key, in BlastAttack blastAttack) {
            blastAttack.procChainMask.AddProc(ProcType.Rings);
            foreach (var hitPoint in blastAttack.Fire().hitPoints.AsSpan()) {
                var healthComponent = hitPoint.hurtBox.healthComponent;
                if (healthComponent.alive) {
                    healthComponent.body.AddTimedBuff(RoR2Content.Buffs.Slow80, RingsTweak.IceRingSlow80BuffDuration);
                }
            }
            EffectManager.SpawnEffect(AssetReferences.iceRingExplosion, new EffectData {
                origin = blastAttack.position,
                scale = blastAttack.radius
            }, true);
        }
    }
}