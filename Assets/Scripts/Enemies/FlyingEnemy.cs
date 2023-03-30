using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Bullets;
using Helpers;

namespace Enemies
{
    public class FlyingEnemy : Enemy
    {
        [SerializeField] private float riseDelay;
        [SerializeField] private float riseAmount;
        [SerializeField] private float riseDuration;
        [SerializeField] private float flyDelay;
        [SerializeField] private float flySpeed;
        [SerializeField] private float flyError;
        [SerializeField] private int damage;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask clashLayer;
        [SerializeField] private NavMeshAgent agent;
        private Coroutine flyCoroutine;

        public override void Launch()
        {
            base.Launch();
            agent.enabled = true;
            flyCoroutine = StartCoroutine(RiseCoroutine());
        }

        private IEnumerator RiseCoroutine()
        {
            yield return new WaitForSeconds(riseDelay);
            agent.enabled = false;

            var t = transform;
            var risen = 0f;
            while (risen < riseAmount)
            {
                var percent = Time.deltaTime / riseDuration;
                var step = percent * riseAmount;
                risen += step;
                t.position += Vector3.up * step;
                yield return null;
            }

            yield return StartCoroutine(FlyToPlayer());
        }

        private IEnumerator FlyToPlayer()
        {
            yield return new WaitForSeconds(flyDelay);

            var t = transform;
            var targetPos = SceneC.Instance.Player.TrackingPoint.Position;
            var direction = (targetPos - t.position).normalized;
            while ((t.position - targetPos).magnitude > flyError)
            {
                t.position += direction * (Time.deltaTime * flySpeed);
                yield return null;
            }

            Dissolve();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var objectLayer = collision.gameObject.layer;
            if (playerLayer.Includes(objectLayer))
            {
                var playerCollider = collision.collider;
                var player = SceneC.Instance.HittablesHolder.GetHittable(playerCollider);
                player.Hit(new BulletDamage
                {
                    HPInjury = damage,
                    ForceInjury = 0
                });

                Dissolve();
            }
            else if (clashLayer.Includes(objectLayer))
            {
                Dissolve();
            }
        }

        protected override void Dissolve()
        {
            base.Dissolve();
            StopCoroutine(flyCoroutine);
            SceneC.Instance.PoolsHolder.FlyingEnemiesPool.PushItem(this);
        }
    }
}
