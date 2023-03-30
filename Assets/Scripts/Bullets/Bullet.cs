using Helpers;
using Helpers.Pooling;
using UnityEngine;

namespace Bullets
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        [SerializeField] protected float Speed;
        [SerializeField] private LayerMask envLayer;
        protected new Transform transform;
        private LayerMask targetLayer;
        private BulletDamage damage;

        Transform IPoolable.Transform => base.transform;
        GameObject IPoolable.GameObject => gameObject;


        public void Launch(LayerMask targetLayer, BulletDamage damage)
        {
            transform = base.transform;
            this.targetLayer = targetLayer;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            int objectLayer = other.gameObject.layer;
            if (targetLayer.Includes(objectLayer))
            {
                if (SceneC.Instance.HittablesHolder.TryGetHittable(other, out var hittable))
                {
                    OnHit(hittable);
                }
            }
            else if (envLayer.Includes(objectLayer))
            {
                Dissolve();
            }
        }

        protected virtual void OnHit(IHittable hittable)
        {
            hittable.Hit(damage);
        }

        protected virtual void Dissolve()
        {
            Destroy(gameObject);
        }
    }
}