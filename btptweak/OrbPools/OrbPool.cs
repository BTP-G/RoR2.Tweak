using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BtpTweak.OrbPools {

    public abstract class OrbPool<T, T2> : MonoBehaviour where T2 : Orb {
        private float _orbTimer;
        protected abstract float OrbInterval { get; }
        protected Dictionary<GameObject, Dictionary<T, T2>> Pool { get; set; } = new();

        public abstract void AddOrb(GameObject attacker, T key, T2 orb);

        private void Awake() => enabled = NetworkServer.active;

        private void FixedUpdate() {
            if ((_orbTimer -= Time.fixedDeltaTime) < 0) {
                if (Pool.Count > 0) {
                    int allCount = 1;
                    foreach (var kvp in Pool) {
                        var orbs = kvp.Value;
                        if (orbs.Count > 0) {
                            var keyOrb = orbs.ElementAt(Random.Range(0, orbs.Count));
                            var orb = keyOrb.Value;
                            ModifyOrb(ref orb);
                            if (orb.target) {
                                OrbManager.instance.AddOrb(orb);
                            }
                            orbs.Remove(keyOrb.Key);
                            allCount += orbs.Count;
                        }
                    }
                    _orbTimer = OrbInterval / allCount;
                }
            }
        }

        protected abstract void ModifyOrb(ref T2 orb);

        private void OnDestroy() {
            foreach (var kvp in Pool) {
                kvp.Value.Clear();
            }
            Pool.Clear();
        }
    }
}