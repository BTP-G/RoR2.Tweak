using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.ProjectileFountains {

    [RequireComponent(typeof(CharacterBody))]
    public abstract class ProjectileFountain : MonoBehaviour {
        private readonly Dictionary<GameObject, Dictionary<ProcChainMask, FireProjectileInfo>> _fountain = new();
        private CharacterBody _victimBody;
        private float _fireTimer;

        public virtual void AddProjectile(GameObject projectilePrefab, GameObject attacker, float baseDamage, bool isCrit, ProcChainMask procChainMask) {
            if (_fountain.TryGetValue(attacker, out var infos)) {
                if (infos.TryGetValue(procChainMask, out var info)) {
                    info.damage += baseDamage;
                    infos[procChainMask] = info;
                } else {
                    infos.Add(procChainMask, new FireProjectileInfo {
                        crit = isCrit,
                        damage = baseDamage,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        fuseOverride = 1f,
                        owner = attacker,
                        position = default,
                        procChainMask = procChainMask,
                        projectilePrefab = projectilePrefab,
                        rotation = Random.rotationUniform,
                        speedOverride = Random.Range(10f, 30f),
                        target = gameObject,
                        useFuseOverride = true,
                        useSpeedOverride = true,
                    });
                }
            } else {
                _fountain.Add(attacker, new() {
                    { procChainMask, new FireProjectileInfo {
                        crit = isCrit,
                        damage = baseDamage,
                        damageColorIndex = DamageColorIndex.Item,
                        force = 0f,
                        fuseOverride = 1f ,
                        owner = attacker,
                        position = default,
                        procChainMask = procChainMask,
                        projectilePrefab = projectilePrefab,
                        rotation = Random.rotationUniform,
                        speedOverride = Random.Range(10f, 30f),
                        target = gameObject,
                        useFuseOverride = true,
                        useSpeedOverride = true,
                        }
                    }
                });
            }
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo info) {
        }

        private void FixedUpdate() {
            if ((_fireTimer -= Time.fixedDeltaTime) < 0) {
                if (_fountain.Count > 0) {
                    int allCount = 1;
                    foreach (var kvp in _fountain) {
                        var infos = kvp.Value;
                        if (infos.Count > 0) {
                            var key_Info = infos.ElementAt(Random.Range(0, infos.Count));
                            var info = key_Info.Value;
                            info.position = _victimBody.corePosition + _victimBody.bestFitRadius * 0.5f * Vector3.up;
                            ModifyProjectile(ref info);
                            ProjectileManager.instance.FireProjectile(info);
                            infos.Remove(key_Info.Key);
                            allCount += infos.Count;
                        }
                    }
                    _fireTimer = ModConfig.喷泉喷射间隔.Value / allCount;
                }
            }
        }

        private void OnDestroy() {
            foreach (var kvp in _fountain) {
                kvp.Value.Clear();
            }
            _fountain.Clear();
        }

        private void Start() {
            if (enabled = NetworkServer.active) {
                _victimBody = GetComponent<CharacterBody>();
            }
        }
    }
}