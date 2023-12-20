using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.OrbPools {

    public struct SimpleOrbInfo {
        public GameObject attacker;
        public HurtBox target;
        public ProcChainMask procChainMask;
        public bool isCrit;
    }

    public abstract class OrbPool<TKey, TOrb> : MonoBehaviour where TOrb : Orb {
        private float _orbTimer;
        protected abstract float OrbInterval { get; }
        protected Dictionary<TKey, TOrb> Pool { get; } = [];

        protected abstract void ModifyOrb(ref TOrb orb);

        private void Awake() {
            enabled = NetworkServer.active;
        }

        private void FixedUpdate() {
            if ((_orbTimer -= Time.fixedDeltaTime) < 0) {
                if (Pool.Count > 0) {
                    foreach (var kvp in Pool) {
                        var orb = kvp.Value;
                        ModifyOrb(ref orb);
                        if (orb.target) {
                            OrbManager.instance.AddOrb(orb);
                        }
                    }
                    Pool.Clear();
                    _orbTimer = OrbInterval;
                }
            }
        }

        private void OnDestroy() => Pool.Clear();
    }
}