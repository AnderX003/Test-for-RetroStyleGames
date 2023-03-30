using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pools;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemiesHolder : MonoBehaviour
    {
        [SerializeField] private float startSpawnInterval;
        [SerializeField] private float endSpawnInterval;
        [SerializeField] private float spawnIntervalStep;
        [SerializeField] private List<EnemyType> spawnPack;
        [SerializeField] private float spawnRadius;
        [SerializeField] private Transform spawnParent;

        private float spawnInterval;
        private int spawnLoopCounter;
        private Coroutine timerCoroutine;
        private EnemiesPool ShootingEnemiesPool;
        private EnemiesPool FlyingEnemiesPool;
        private List<Enemy> aliveEnemies = new();

        public void Init()
        {
            spawnInterval = startSpawnInterval;
            timerCoroutine = StartCoroutine(SpawnTimer());
            var poolsHolder = SceneC.Instance.PoolsHolder;
            ShootingEnemiesPool = poolsHolder.ShootingEnemiesPool;
            FlyingEnemiesPool = poolsHolder.FlyingEnemiesPool;
        }

        private IEnumerator SpawnTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                spawnInterval = Mathf.Clamp(spawnInterval - spawnIntervalStep,
                    endSpawnInterval, startSpawnInterval);

                Spawn(spawnPack[spawnLoopCounter]);
                spawnLoopCounter++;
                if (spawnLoopCounter >= spawnPack.Count)
                {
                    spawnLoopCounter = 0;
                    spawnPack.Shuffle();
                }
            }
        }

        private void Spawn(EnemyType type)
        {
            var pool = type switch
            {
                EnemyType.ShootingEnemy => ShootingEnemiesPool,
                EnemyType.FlyingEnemy => FlyingEnemiesPool,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            var enemy = pool.PopItem(spawnParent);
            aliveEnemies.Add(enemy);
            PlaceEnemy(enemy);
            enemy.Launch();
        }

        private void PlaceEnemy(Enemy enemy)
        {
            //todo
            var pointOnCircle = StatFuncs.GetRandomPointOnCircle(spawnRadius);
            var pointOnCircle3d = new Vector3(pointOnCircle.x, 0f, pointOnCircle.y);
            const float maxDistance = 100f;
            if (NavMesh.SamplePosition(pointOnCircle3d, out var hit, maxDistance, NavMesh.AllAreas))
            {
                enemy.transform.position = hit.position;
            }
            else
            {
                Debug.LogError("NavMesh could not FindClosestEdge while generating Enemy");
            }
        }

        public void OnEnemyDead(int killForceBonus)
        {
            SceneC.Instance.Player.Indicators.AddForce(killForceBonus);
            SceneC.Instance.GameProgress.AddScore();
        }

        public void OnEnemyDissolve(Enemy enemy)
        {
            aliveEnemies.Remove(enemy);
        }

        public void KillAllEnemies()
        {
            for (int i = aliveEnemies.Count - 1; i >= 0; i--)
            {
                aliveEnemies[i].KillByUlt();
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
        }
#endif
    }
}
