using System;
using UnityEngine;

namespace Pools
{
    [Serializable]
    public struct PoolsHolder
    {
        [field: SerializeField]
        public FollowingBulletsPool FollowingBulletsPool { get; set; }

        [field: SerializeField]
        public RicochetBulletsPool RicochetBulletsPool { get; set; }

        [field: SerializeField]
        public EnemiesPool ShootingEnemiesPool { get; set; }

        [field: SerializeField]
        public EnemiesPool FlyingEnemiesPool { get; set; }

        public void Init()
        {
            FollowingBulletsPool.Init();
            RicochetBulletsPool.Init();
            ShootingEnemiesPool.Init();
            FlyingEnemiesPool.Init();
        }
    }
}
