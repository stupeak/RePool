using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stupeak.RePool
{
    public class MultiComponentPool<TKey, TValue> //: IPoolContainer<TValue>
        where TValue : Component
    {
        const string CLONE_SUFFIX = "(Clone)";

        private readonly Dictionary<TKey, ComponentPool<TValue>> m_Pools = new();
        private int growthSize = 1;
        private bool keepOriginalName;

        public MultiComponentPool() : this(1) { }

        public MultiComponentPool(int growthSize, bool keepOriginalName = true)
        {
            this.m_Pools = new(growthSize);
            this.growthSize = growthSize;
            this.keepOriginalName = keepOriginalName;
        }

        public void Prewarm(TKey key, TValue instance, int size)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(instance, null, 1, 1, keepOriginalName);
                m_Pools.Add(key, pool);
            }

            pool.Prewarm(instance, size);
        }

        public T Rent<T>(TKey key, TValue instance, Vector3 position, Quaternion rotation, bool activeObject = true)
            where T : TValue
        {
            return (T)Rent(key, instance, position, rotation, activeObject);
        }

        public TValue Rent(TKey key, Vector3 position, Quaternion rotation)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                return null;
            }

            return pool.Rent(position, rotation);
        }

        public TValue Rent(TKey key, TValue instance, Vector3 position, Quaternion rotation, bool activeObject = true)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(instance, null, 1, 1, keepOriginalName);
                m_Pools.Add(key, pool);
            }

            return pool.Rent(position, rotation, activeObject);
        }

        public void Return(TKey key, TValue instance)
        {
            if (!m_Pools.TryGetValue(key, out ComponentPool<TValue> pool))
            {
                pool = new ComponentPool<TValue>(instance, null, 1, 1, keepOriginalName);
                m_Pools.Add(key, pool);
            }

            pool.Return(instance);
        }

        public void Return(TValue instance, bool nameCheck = true)
        {
            if (typeof(TKey) != typeof(string))
            {
                Debug.LogWarning("key is not string to be returned by name");
                return;
            }

            if (nameCheck)
            {
                //ReadOnlySpan<char> cloneSuffixSpan = CLONE_SUFFIX.AsSpan();
                ReadOnlySpan<char> nameSpan = instance.gameObject.name.AsSpan();

                if (nameSpan.Length >= CLONE_SUFFIX.Length && nameSpan.EndsWith(CLONE_SUFFIX))
                {
                    Debug.LogWarning($"ReturnClone aborted: GameObject.name \"{instance.gameObject.name}\" ends with \"(Clone)\". Key may not exist in pool.");
                    return;
                }
            }

            Return((TKey)(object)instance.gameObject.name, instance);
        }

        public void Return(TValue instance)
        {
            Return(instance, true);
        }

        public void Release(TValue instance)
        {

        }

        public void Clear()
        {
            foreach (var pool in m_Pools.Values)
            {
                pool.Clear();
            }

            m_Pools.Clear();
        }
    }
}