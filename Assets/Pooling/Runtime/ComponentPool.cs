using System;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Lustie.Pooling
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentPool<T> where T : Component
    {
        public readonly Queue<T> pool;

        private Transform parent;
        private readonly T prefab;

        public ComponentPool()
        {
            this.pool = new();
        }

        public ComponentPool(T prefab) : this()
        {
            this.prefab = prefab;
        }

        public ComponentPool(T prefab, Transform parent) : this()
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        public ComponentPool(T prefab, int defaultSize, Transform parent, Action<T> OnCreated = null) : this(prefab, parent)
        {
            Create(defaultSize, OnCreated);
        }

        public void Create(int size, Action<T> OnCreated = null)
        {
            if (prefab)
                Create(prefab, size, OnCreated);
            else
                Debug.LogWarning("No prefab has been assigned");
        }

        public void Create(T component, int size, Action<T> OnCreated = null)
        {
            for (int i = 0; i < size; i++)
            {
                T go = Object.Instantiate(component);
                go.gameObject.SetActive(false);
                if (parent)
                    go.transform.SetParent(parent, false);
                pool.Enqueue(go);

                OnCreated?.Invoke(go);
            }
        }

        public T Get(bool activeObject = true, Action<T> OnGet = null)
        {
            if (!pool.TryDequeue(out T cpn))
            {
                return null;
            }
            OnGet?.Invoke(cpn);
            cpn.gameObject.SetActive(activeObject);
            return cpn;
        }

        public T Get(Vector3 position, Quaternion rotation, bool activeObject = true)
        {
            if (!pool.TryDequeue(out T cpn))
            {
                return null;
            }
            cpn.transform.SetPositionAndRotation(position, rotation);
            cpn.gameObject.SetActive(activeObject);
            return cpn;
        }

        public T Get(Vector3 position, Quaternion rotation, Action<T> OnGet, bool activeObject = true)
        {
            if (!pool.TryDequeue(out T cpn))
            {
                return null;
            }
            OnGet?.Invoke(cpn);
            cpn.transform.SetPositionAndRotation(position, rotation);
            cpn.gameObject.SetActive(activeObject);
            return cpn;
        }

        public T GetOrCreate(bool activeObject = true, int createCount = 1)
        {
            if (pool.Count == 0)
                Create(createCount);

            return Get(activeObject);
        }

        public T GetOrCreate(Vector3 position, Quaternion rotation, bool activeObject = true, int createCount = 1)
        {
            if (pool.Count == 0)
                Create(createCount);

            return Get(position, rotation, activeObject);
        }

        public T GetOrCreate(T component, Vector3 position, Quaternion rotation, bool activeObject = true, int createCount = 1)
        {
            if (pool.Count == 0)
                Create(component, createCount);

            return Get(position, rotation, activeObject);
        }

        public T GetOrCreate(T component, Vector3 position, Quaternion rotation, Action<T> OnGet, bool activeObject = true, int createCount = 1)
        {
            if (pool.Count == 0)
                Create(component, createCount);

            return Get(position, rotation, OnGet, activeObject);
        }

        public void AddExisting(T component)
        {
            pool.Enqueue(component);
        }

        public void Return(T component, bool active = false)
        {
            pool.Enqueue(component);
            component.gameObject.SetActive(active);
        }
    }
}