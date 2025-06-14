using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lustie.Pooling
{
    public class MultiComponentPool<TKey, TValue> where TValue : Component
    {
        private readonly Dictionary<TKey, ComponentPool<TValue>> m_Pools = new();
        private int capacity = 1;

        public MultiComponentPool() : this(1) { }

        public MultiComponentPool(int capacity)
        {
            this.m_Pools = new(capacity);
            this.capacity = capacity;
        }

        public void Create(TKey key, TValue component, int size)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(component);
                m_Pools.Add(key, pool);
            }

            pool.Create(component, size);
        }

        public TValue Get(TKey key, Vector3 position, Quaternion rotation)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                return null;
            }

            return pool.Get(position, rotation);
        }

        public TValue GetOrCreate(TKey key, TValue component, Vector3 position, Quaternion rotation, bool activeObject = true)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(component);
                m_Pools.Add(key, pool);
            }

            return pool.GetOrCreate(component, position, rotation, activeObject, capacity);
        }

        public T GetOrCreate<T>(TKey key, TValue component, Vector3 position, Quaternion rotation, bool activeObject = true) where T : TValue
        {
            return (T)GetOrCreate(key, component, position, rotation, activeObject);
        }

        public TValue GetOrCreate(TKey key, TValue component, Vector3 position, Quaternion rotation, Action<TValue> OnGet, bool activeObject = true)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(component);
                m_Pools.Add(key, pool);
            }

            return pool.GetOrCreate(component, position, rotation, OnGet, activeObject, capacity);
        }

        public void Return(TKey key, TValue component)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(component);
                m_Pools.Add(key, pool);
            }

            pool.Return(component);
        }
    }
}