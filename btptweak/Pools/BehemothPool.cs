using EntityStates.Merc;
using HG;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements.UIR;

namespace BtpTweak.Pools {

    public struct BlastAttackInfo {
        public bool crit;
        public DamageType damageType;
        public float procCoefficient;
        public float radius;
        public GameObject attacker;
        public GameObject inflictor;
        public ProcChainMask procChainMask;
        public TeamIndex teamIndex;
    }

    internal class BehemothPool : Pool<BehemothPool, BlastAttackInfo, BlastAttack> {
        protected override float Interval => 0.1f;

        public void AddBlastAttack(in BlastAttackInfo attackInfo, in Vector3 position, float damageValue) {
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
                    inflictor = attackInfo.inflictor,
                    position = position,
                    procChainMask = attackInfo.procChainMask,
                    procCoefficient = attackInfo.procCoefficient,
                    radius = attackInfo.radius,
                    teamIndex = attackInfo.teamIndex,
                });
            }
        }

        protected override void OnTimeOut(BlastAttack blastAttack) {
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