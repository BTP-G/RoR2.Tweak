using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.MissilePools
{

    [RequireComponent(typeof(CharacterBody))]
    public abstract class MissilePool : MonoBehaviour
    {
        private readonly Dictionary<ProcChainMask, MissileInfo> _pool = new();
        private CharacterBody _attackerBody;
        private float _missileTimer;
        protected virtual GameObject MissilePrefab => GlobalEventManager.CommonAssets.missilePrefab;

        public void AddMissile(float baseDamage, bool isCrit, GameObject target, ProcChainMask procChainMask)
        {
            if (!isCrit) { procChainMask.AddProc(ProcType.Missile); }
            if (_pool.TryGetValue(procChainMask, out MissileInfo missileInfo))
            {
                missileInfo.baseDamage += baseDamage * Mathf.Pow(0.9f, missileInfo.count++);
                if (missileInfo.target == null) { missileInfo.target = target; }
                _pool[procChainMask] = missileInfo;
            }
            else
            {
                _pool.Add(procChainMask, new MissileInfo
                {
                    baseDamage = baseDamage,
                    isCrit = isCrit,
                    target = target,
                    count = 1,
                });
            }
        }

        private void FireMissile(in ProcChainMask procChainMask, in MissileInfo missileInfo) => MissileUtils.FireMissile(_attackerBody.corePosition, _attackerBody, procChainMask, missileInfo.target, missileInfo.baseDamage, missileInfo.isCrit, MissilePrefab, DamageColorIndex.Item, missileInfo.isCrit);

        private void FixedUpdate()
        {
            if (_missileTimer < 0)
            {
                if (_pool.Count > 0)
                {
                    foreach (var item in _pool) { FireMissile(item.Key, item.Value); }
                    _pool.Clear();
                    _missileTimer = 0.5f;
                }
            }
            else
            {
                _missileTimer -= Time.fixedDeltaTime;
            }
        }

        private void OnDestroy() => _pool.Clear();

        private void Start()
        {
            if (!NetworkServer.active)
            {
                enabled = false;
                return;
            }
            _attackerBody = GetComponent<CharacterBody>();
        }

        private struct MissileInfo
        {
            public float baseDamage;
            public int count;
            public bool isCrit;
            public GameObject target;
        }
    }
}