using System;
using Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bullets
{
    public class RicochetBullet : Bullet
    {
        private float ricochetChance;
        private bool wasRicochet;
        private Action<IHittable> ricochetHitKillCallback;

        public void SetRicochetChance(float ricochetChance)
        {
            this.ricochetChance = ricochetChance;
        }

        public void SetRicochetHitKillCallback(Action<IHittable> ricochetHitKillCallback)
        {
            this.ricochetHitKillCallback = ricochetHitKillCallback;
        }

        private void Update()
        {
            transform.position += transform.forward.normalized * (Time.deltaTime * Speed);
        }

        protected override void OnHit(IHittable hittable)
        {
            base.OnHit(hittable);

            if (wasRicochet && !hittable.IsAlive)
            {
                ricochetHitKillCallback?.Invoke(hittable);
            }

            bool ricochet = Random.Range(0f, 1f) < ricochetChance;
            if (wasRicochet || ricochet)
            {
                Dissolve();
            }
            else
            {
                Ricochet();
            }
        }

        private void Ricochet()
        {
            wasRicochet = true;
            if (Random.Range(0, 2) == 0) //ricochet
            {
                ((Component) this).transform.rotation = StatFuncs.GetRandomQuaternionRotation();
            }
            //else fry through
        }

        protected override void Dissolve()
        {
            SceneC.Instance.PoolsHolder.RicochetBulletsPool.PushItem(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward);
        }
#endif
    }
}
