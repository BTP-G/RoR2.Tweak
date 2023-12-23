using BtpTweak.Utils;
using HG;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.UIElements.UIR;
using static UnityEngine.UI.GridLayoutGroup;

namespace BtpTweak.Pools {

    internal abstract class Pool<TKey, TValue> {
        protected readonly Dictionary<TKey, TValue> pool = [];
        protected float fixedTimer;
        protected abstract float Interval { get; }

        protected virtual void FixedUpdate() {
            if ((fixedTimer -= Time.fixedDeltaTime) > 0) {
                return;
            }
            if (pool.Count > 0) {
                var first = pool.First();
                OnTimeOut(first.Value);
                pool.Remove(first.Key);
                fixedTimer = Interval;
            }
        }

        protected abstract void OnTimeOut(TValue value);
    }

    internal abstract class Pool<T, TKey, TValue> : Pool<TKey, TValue> where T : Pool<T, TKey, TValue> {
        private static readonly Dictionary<GameObject, T> _pools = [];
        private static readonly Stack<T> _poolStack = [];
        private GameObject _owner;

        public static T RentPool(GameObject owner) {
            if (!_pools.TryGetValue(owner, out var pool)) {
                pool = _poolStack.Pop() ?? Activator.CreateInstance<T>();
                pool._owner = owner;
                pool.fixedTimer = pool.Interval;
                _pools.Add(owner, pool);
                RoR2Application.onFixedUpdate += pool.FixedUpdate;
            }
            return pool;
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            if (!_owner) {
                RoR2Application.onFixedUpdate -= FixedUpdate;
                pool.Clear();
                if (_pools.Remove(_owner)) {
                    _poolStack.Push(this as T);
                }
                _owner = null;
            }
        }
    }
}