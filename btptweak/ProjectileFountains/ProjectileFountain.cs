using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.ProjectileFountains {

    [RequireComponent(typeof(CharacterBody))]
    public abstract class ProjectileFountain : MonoBehaviour {
        private readonly Dictionary<SimpleProjectileInfo, ProjectileInfo> _pool = new();
        private CharacterBody _victimBody;
        private float _fireTimer;
        protected abstract GameObject ProjectilePrefab { get; }

        public virtual void AddProjectile(in SimpleProjectileInfo simpleProjectileInfo, float damageValue) {
            if (_pool.TryGetValue(simpleProjectileInfo, out var projectileInfo)) {
                projectileInfo.fireProjectileInfo.damage += damageValue;
            } else {
                _pool.Add(simpleProjectileInfo, new() {
                    fireProjectileInfo = new FireProjectileInfo {
                        crit = simpleProjectileInfo.isCrit,
                        damage = damageValue,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        fuseOverride = 1f,
                        owner = simpleProjectileInfo.attacker,
                        position = default,
                        procChainMask = simpleProjectileInfo.procChainMask,
                        projectilePrefab = ProjectilePrefab,
                        rotation = Random.rotationUniform,
                        speedOverride = Random.Range(10f, 30f),
                        target = gameObject,
                        useFuseOverride = true,
                        useSpeedOverride = true,
                    }
                });
            }
        }

        protected abstract void ModifyProjectile(ref FireProjectileInfo info);

        private void Awake() {
            if (enabled = NetworkServer.active) {
                _victimBody = GetComponent<CharacterBody>();
            }
        }

        private void FixedUpdate() {
            if ((_fireTimer -= Time.fixedDeltaTime) < 0) {
                if (_pool.Count > 0) {
                    foreach (var kvp in _pool) {
                        var projectileInfo = kvp.Value;
                        projectileInfo.fireProjectileInfo.position = _victimBody.corePosition + (_victimBody.bestFitRadius * 0.5f * Vector3.up);
                        ModifyProjectile(ref projectileInfo.fireProjectileInfo);
                        ProjectileManager.instance.FireProjectile(projectileInfo.fireProjectileInfo);
                    }
                    _pool.Clear();
                    _fireTimer = ModConfig.喷泉喷射间隔.Value;
                }
            }
        }

        private void OnDestroy() => _pool.Clear();

        public struct SimpleProjectileInfo {
            public GameObject attacker;
            public ProcChainMask procChainMask;
            public bool isCrit;
        }

        private class ProjectileInfo {
            public FireProjectileInfo fireProjectileInfo;
        }
    }
}