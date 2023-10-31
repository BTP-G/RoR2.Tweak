using BtpTweak.Utils;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.MissilePools {

    [RequireComponent(typeof(CharacterBody))]
    public abstract class MissilePool : MonoBehaviour {
        private readonly Dictionary<ProcChainMask, MissileInfo> _pool = new();
        private CharacterBody _attackerBody;
        private float _missileTimer;
        protected virtual GameObject MissilePrefab => GlobalEventManager.CommonAssets.missilePrefab;

        public void AddMissile(float baseDamage, bool isCrit, GameObject target, ProcChainMask procChainMask) {
            if (!isCrit) {
                procChainMask.AddProc(ProcType.Missile);
            }
            if (_pool.TryGetValue(procChainMask, out var missileInfo)) {
                missileInfo.baseDamage += baseDamage;
                missileInfo.target ??= target;
                _pool[procChainMask] = missileInfo;
            } else {
                _pool.Add(procChainMask, new MissileInfo {
                    baseDamage = baseDamage,
                    isCrit = isCrit,
                    target = target,
                });
            }
        }

        private void Awake() {
            if (enabled = NetworkServer.active) {
                _attackerBody = GetComponent<CharacterBody>();
            }
        }

        private void FireMissile(in ProcChainMask procChainMask, in MissileInfo missileInfo) => MissileUtils.FireMissile(_attackerBody.corePosition, _attackerBody, procChainMask, missileInfo.target, missileInfo.baseDamage, missileInfo.isCrit, MissilePrefab, DamageColorIndex.Item, Vector3.up, 0f, missileInfo.isCrit);

        private void FixedUpdate() {
            if ((_missileTimer -= Time.fixedDeltaTime) < 0) {
                if (_pool.Count > 0) {
                    var m = _pool.ElementAt(Random.Range(0, _pool.Count));
                    var mask = m.Key;
                    mask.AddPoolProcs();
                    FireMissile(mask, m.Value);
                    _pool.Remove(m.Key);
                    _missileTimer = ModConfig.导弹发射间隔.Value / (1 + _pool.Count);
                }
            }
        }

        private void OnDestroy() => _pool.Clear();

        private struct MissileInfo {
            public float baseDamage;
            public bool isCrit;
            public GameObject target;
        }
    }
}