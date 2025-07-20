using UnityEngine;

namespace Stupeak.RePool
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameObjectPool : IPoolContainer<GameObject>
    {
        IPoolContainer<Transform> transformPool;

        public GameObjectPool(GameObject original, Transform parent)
        {
            transformPool = new ComponentPool<Transform>(original.transform, parent);
        }

        public GameObject Rent()
        {
            return transformPool.Rent().gameObject;
        }

        public void AddExisting(GameObject instance)
        {
            transformPool.AddExisting(instance.transform);
        }

        public void Release(GameObject instance)
        {
            transformPool.Release(instance.transform);
        }

        public void Return(GameObject instance)
        {
            transformPool.Return(instance.transform);
        }

        public void Clear()
        {
            transformPool.Clear();
        }
    }
}