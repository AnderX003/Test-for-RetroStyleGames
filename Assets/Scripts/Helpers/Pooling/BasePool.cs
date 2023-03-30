using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Helpers.Pooling
{
    public abstract class BasePool<T> : MonoBehaviour where T : Object, IPoolable
    {
        [SerializeField] private bool initOnAwake;
        [SerializeField] private Transform poolParent;
        [SerializeField] private T prefab;
        [Space]
        [SerializeField] private bool validatePredefinedObjects;
        [SerializeField] private List<T> predefinedObjects;

        private readonly Queue<T> pool = new();

        private void OnValidate()
        {
            if (!validatePredefinedObjects) return;
            predefinedObjects = poolParent.GetComponentsInChildren<T>().ToList();
        }

        public void Awake()
        {
            if (initOnAwake)
            {
                Init();
            }
        }

        public void Init()
        {
            foreach (var item in predefinedObjects)
            {
                item.Init().HideToPool(transform);
                pool.Enqueue(item);
            }
        }

        public T PopItem(Transform parent = null)
        {
            if (pool.Count == 0)
            {
                var popItem = CreateNewItem().Setup(parent);
                return (T) popItem;
            }
            else
            {
                var popItem = pool.Dequeue().Setup(parent);
                return (T) popItem;
            }
        }

        protected virtual IPoolable CreateNewItem()
        {
            return Instantiate(prefab).Init();
        }

        public void PushItem(T item)
        {
            pool.Enqueue((T) item.HideToPool(poolParent));
        }
    }
}
