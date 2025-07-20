using System;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Stupeak.RePool
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentPool<T> : IPoolContainer<T>
        where T : Component
    {
        public readonly Queue<T> pool;

        private Transform parent;
        private readonly T original;

        private int growthSize { get; set; }
        private bool keepOriginalName { get; set; }

        public ComponentPool(T original, Transform parent = null, int defaultSize = 1, int growthSize = 1, bool keepOriginalName = true, Action<T> OnCreateDefaultPool = null)
        {
            this.pool = new(defaultSize);
            this.original = original;
            this.parent = parent;
            this.growthSize = growthSize;
            this.keepOriginalName = keepOriginalName;

            Prewarm(defaultSize, OnCreateDefaultPool);
        }

        public void Prewarm(int size, Action<T> OnCreated = null)
        {
            if (original)
                Prewarm(original, size, OnCreated);
            else
                Debug.LogWarning("No original has been assigned");
        }

        public void Prewarm(T original, int size, Action<T> OnCreated = null)
        {
            for (int i = 0; i < size; i++)
            {
                T go = Object.Instantiate(original);
                go.gameObject.SetActive(false);

                if (keepOriginalName)
                    go.gameObject.name = this.original.name;

                if (parent)
                    go.transform.SetParent(parent, false);

                // assign pool container to pooled object.
                if (go is IPooledObject<T> pooledObject)
                    pooledObject.poolContainer = this;

                pool.Enqueue(go);

                OnCreated?.Invoke(go);
            }
        }

        public T Rent()
        {
            return Rent(true);
        }        

        public T Rent(bool activeObject)
        {
            //if (pool.Count == 0)
            if (!pool.TryDequeue(out T cpn))
            {
                Prewarm(growthSize);
            }
            cpn.gameObject.SetActive(activeObject);
            return cpn;
        }

        public T Rent(Vector3 position, Quaternion rotation, bool activeObject = true)
        {
            T instance = Rent(activeObject);
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(activeObject);
            return instance;
        }

        public void AddExisting(T instance)
        {
            pool.Enqueue(instance);
        }

        public void Return(T instance, bool active)
        {
            pool.Enqueue(instance);
            instance.gameObject.SetActive(active);
        }

        public void Return(T instance)
        {
            Return(instance, false);
        }

        public void Release(T instance)
        {

        }

        public void Clear()
        {
            pool?.Clear();
        }
    }
}