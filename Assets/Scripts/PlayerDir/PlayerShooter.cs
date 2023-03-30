using System;
using UnityEngine;
using Bullets;
using Pools;

namespace PlayerDir
{
    [Serializable]
    public class PlayerShooter
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private LayerMask enemyLayer;
        private Player player;
        private RicochetBulletsPool bulletsPool;

        public void Init(Player player)
        {
            this.player = player;
            var gameUI = SceneC.Instance.UIHolder.GameUI;
            gameUI.OnShootButtonClick += OnShootButtonClick;
            gameUI.OnUltButtonClick += OnUltButtonClick;
            bulletsPool = SceneC.Instance.PoolsHolder.RicochetBulletsPool;
        }

        private void OnShootButtonClick()
        {
            var bullet = bulletsPool.PopItem();
            var bulletT = bullet.transform;
            bulletT.position = shootPoint.position;
            bulletT.forward = shootPoint.forward;
            bullet.SetRicochetHitKillCallback(RicochetHitKillCallback);
            bullet.SetRicochetChance(0.5f); //todo
            bullet.Launch(enemyLayer,
                new BulletDamage
                {
                    HPInjury = player.Indicators.Force,
                    ForceInjury = 0
                });
        }

        private void OnUltButtonClick()
        {
            if (!player.Indicators.CanUlt) return;

            player.Indicators.UseAllForce();
            SceneC.Instance.EnemiesHolder.KillAllEnemies();
        }

        private void RicochetHitKillCallback(IHittable hittable)
        {
            player.Indicators.AddRicochetBonus();
        }
    }
}
