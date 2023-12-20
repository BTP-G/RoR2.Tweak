using BtpTweak.Utils;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

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

    internal class BehemothPool : MonoBehaviour {
        public const float Interval = 0.1f;
        private readonly Dictionary<BlastAttackInfo, BlastAttack> Pool = [];
        private float timer;

        public void AddAttack(in BlastAttackInfo attackInfo, in Vector3 position, float damageValue) {
            attackInfo.procChainMask.AddProc(ProcType.Behemoth);
            if (Pool.TryGetValue(attackInfo, out var blastAttack)) {
                blastAttack.position = position;
                blastAttack.baseDamage += damageValue;
            } else {
                Pool.Add(attackInfo, new() {
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

        private void Awake() {
            enabled = NetworkServer.active;
        }

        private void FixedUpdate() {
            if ((timer -= Time.fixedDeltaTime) > 0) {
                return;
            }
            if (Pool.Count > 0) {
                var blastAttacks = Pool.Values;
                for (var i = 0; i < blastAttacks.Count; ++i) {
                    var blastAttack = blastAttacks.ElementAt(i);
                    blastAttack.Fire();
                    EffectManager.SpawnEffect(AssetReferences.omniExplosionVFXQuick, new EffectData {
                        origin = blastAttack.position,
                        scale = blastAttack.radius,
                        rotation = Quaternion.identity,
                    }, true);
                }
                Pool.Clear();
                timer = Interval;
            }
        }

        private void OnDestroy() {
            Pool.Clear();
        }
    }
}