using UnityEngine;

namespace Helpers.Pooling
{
    public interface IPoolable
    {
        protected Transform Transform { get; }
        protected GameObject GameObject { get; }

        public virtual IPoolable Init()
        {
            return this;
        }

        public virtual IPoolable Setup(Transform parent)
        {
            Transform.SetParent(parent);
            GameObject.SetActive(true);
            return this;
        }

        public virtual IPoolable HideToPool(Transform poolParent)
        {
            Transform.SetParent(poolParent);
            GameObject.SetActive(false);
            return this;
        }
    }
}
