using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BTP.RoR2Plugin.Pools {

    internal abstract class Pool<TKey, TValue> {
        protected readonly Dictionary<TKey, TValue> pool = [];
        protected float fixedTimer;
        protected abstract float Interval { get; }

        protected void FixedUpdate() {
            if ((fixedTimer -= Time.fixedDeltaTime) > 0) {
                return;
            }
            if (pool.Count > 0) {
                var first = pool.First();
                OnTimeOut(first.Key, first.Value);
                pool.Remove(first.Key);
                fixedTimer = Interval;
            }
        }

        protected abstract void OnTimeOut(in TKey key, in TValue value);
    }

    internal abstract class Pool<T, TKey, TValue> : Pool<TKey, TValue>, IFixedTickable where T : Pool<T, TKey, TValue> {
        private static readonly Dictionary<GameObject, T> _pools = [];
        private static readonly Stack<T> _poolStack = [];
        private GameObject _owner;
        public GameObject Owner => _owner;

        public static T RentPool(GameObject owner) {
            if (!_pools.TryGetValue(owner, out var pool)) {
                if (!_poolStack.TryPop(out pool)) {
                    pool = Activator.CreateInstance<T>();
                }
                pool._owner = owner;
                pool.fixedTimer = 0;
                _pools.Add(owner, pool);
                pool.RegisterFixedTick();
            }
            return pool;
        }

        void IFixedTickable.FixedTick() {
            if (!_owner) {
                this.UnregisterFixedTick();
                pool.Clear();
                if (_pools.Remove(_owner)) {
                    _poolStack.Push(this as T);
                }
                _owner = null;
                return;
            }
            FixedUpdate();
        }
    }
}