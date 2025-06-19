using BTP.RoR2Plugin.Tweaks.ItemTweaks;
using RoR2;
using UnityEngine;

namespace BTP.RoR2Plugin.Pools {

    public struct BehemothPoolKey {
        public bool crit;
        public DamageType damageType;
        public float procCoefficient;
        public float radius;
        public GameObject attacker;
        public ProcChainMask procChainMask;
        public TeamIndex teamIndex;
    }

    internal sealed class BehemothPool : Pool<BehemothPool, BehemothPoolKey, BlastAttack> {
        protected override float Interval => BehemothTweak.Interval;

        public void AddBlastAttack(in BehemothPoolKey attackInfo, in Vector3 position, float damageValue) {
            if (pool.TryGetValue(attackInfo, out var blastAttack)) {
                blastAttack.position = position;
                blastAttack.baseDamage += damageValue;
            } else {
                pool.Add(attackInfo, new() {
                    attacker = attackInfo.attacker,
                    baseDamage = damageValue,
                    crit = attackInfo.crit,
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = attackInfo.damageType,
                    falloffModel = BlastAttack.FalloffModel.None,
                    position = position,
                    procChainMask = attackInfo.procChainMask,
                    procCoefficient = attackInfo.procCoefficient,
                    radius = attackInfo.radius,
                    teamIndex = attackInfo.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(in BehemothPoolKey key, in BlastAttack blastAttack) {
            blastAttack.procChainMask.AddProc(ProcType.Behemoth);
            blastAttack.Fire();
            EffectManager.SpawnEffect(AssetReferences.omniExplosionVFXQuick, new EffectData {
                origin = blastAttack.position,
                scale = blastAttack.radius,
                rotation = Quaternion.identity,
            }, true);
        }
    }
}