using System.Collections;
using UnityEngine;
using Bullets;
using Pools;

namespace Enemies
{
    public class ShootingEnemy : Enemy
    {
        [SerializeField] private int force;
        [SerializeField] private float shootInterval;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform shootPointDirector;
        [SerializeField] private Transform shootPoint;
        private Coroutine shootCoroutine;
        private FollowingBulletsPool bulletsPool;

        public override void Launch()
        {
            base.Launch();
            shootCoroutine = StartCoroutine(ShootTimer());
            bulletsPool = SceneC.Instance.PoolsHolder.FollowingBulletsPool;
        }

        private IEnumerator ShootTimer()
        {
            var wait = new WaitForSeconds(shootInterval);
            while (true)
            {
                yield return wait;
                Shoot();
            }
        }

        protected override void Dissolve()
        {
            base.Dissolve();
            StopCoroutine(shootCoroutine);
            SceneC.Instance.PoolsHolder.ShootingEnemiesPool.PushItem(this);
        }

        private void Shoot()
        {
            var player = SceneC.Instance.Player;
            shootPointDirector.forward =
                player.Transform.position - shootPointDirector.position;
            var bullet = bulletsPool.PopItem();
            var bulletT = bullet.transform;
            bulletT.position = shootPoint.position;
            bulletT.forward = shootPoint.forward;
            bullet.SetTarget(player.TrackingPoint);
            bullet.Launch(playerLayer,
                new BulletDamage
                {
                    ForceInjury = force,
                    HPInjury = 0
                });
        }
    }
}
