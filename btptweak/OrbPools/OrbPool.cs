using HG;
using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
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
        protected Dictionary<TKey, TOrb> Pool { get; } = CollectionPool<KeyValuePair<TKey, TOrb>, Dictionary<TKey, TOrb>>.RentCollection();

        protected abstract void ModifyOrb(ref TOrb orb);

        private void Awake() {
            enabled = NetworkServer.active;
        }

        private void FixedUpdate() {
            if ((_orbTimer -= Time.fixedDeltaTime) < 0) {
                if (Pool.Count > 0) {
                    var first = Pool.First();
                    var orb = first.Value;
                    ModifyOrb(ref orb);
                    if (orb.target) {
                        OrbManager.instance.AddOrb(orb);
                    }
                    Pool.Remove(first.Key);
                    _orbTimer = OrbInterval;
                }
            }
        }

        private void OnDestroy() => CollectionPool<KeyValuePair<TKey, TOrb>, Dictionary<TKey, TOrb>>.ReturnCollection(Pool);
    }
}