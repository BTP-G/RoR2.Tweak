using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.ProjectileFountains {

    [RequireComponent(typeof(CharacterBody))]
    public abstract class ProjectileFountain : MonoBehaviour {
        protected CharacterBody victimBody;
        private readonly Dictionary<uint, FireProjectileInfo> _fountain = new();
        private float fireTimer;

        public virtual void AddProjectile(GameObject projectilePrefab, GameObject attacker, float baseDamage, bool isCrit, ProcChainMask procChainMask) {
            if (_fountain.TryGetValue(procChainMask.mask, out var fireProjectileInfo)) {
                fireProjectileInfo.damage += baseDamage;
                if (fireProjectileInfo.owner == null) { fireProjectileInfo.owner = attacker; }
                _fountain[procChainMask.mask] = fireProjectileInfo;
            } else {
                _fountain.Add(procChainMask.mask, new FireProjectileInfo {
                    crit = isCrit,
                    damage = baseDamage,
                    damageColorIndex = DamageColorIndex.Item,
                    force = 100f,
                    fuseOverride = 3,
                    owner = attacker,
                    position = default,
                    procChainMask = procChainMask,
                    projectilePrefab = projectilePrefab,
                    rotation = Util.QuaternionSafeLookRotation(new(UnityEngine.Random.Range(-2f, 2f), 6f, UnityEngine.Random.Range(-2f, 2f))),
                    speedOverride = UnityEngine.Random.Range(15, 30),
                    target = gameObject,
                    useFuseOverride = true,
                    useSpeedOverride = true,
                });
            }
        }

        protected abstract void Fire(FireProjectileInfo info);

        private void FixedUpdate() {
            if ((fireTimer -= Time.fixedDeltaTime) < 0) {
                if (_fountain.Count > 0) {
                    foreach (var f in _fountain) { Fire(f.Value); }
                    _fountain.Clear();
                    fireTimer = 0.5f;
                }
            }
        }

        private void OnDestroy() => _fountain.Clear();

        private void Start() {
            if (!NetworkServer.active) {
                enabled = false;
                return;
            }
            victimBody = GetComponent<CharacterBody>();
        }
    }
}