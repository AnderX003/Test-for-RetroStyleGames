using Bullets;
using Helpers.Pooling;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IPoolable, IHittable
    {
        [SerializeField] private int maxHp;
        [SerializeField] public int killForceBonus;
        [SerializeField] private Collider collider;
        private int hp;

        public bool IsAlive { get; private set; }

        Transform IPoolable.Transform => transform;

        GameObject IPoolable.GameObject => gameObject;


        public virtual void Launch()
        {
            hp = maxHp;
            IsAlive = true;
            SceneC.Instance.HittablesHolder.RegisterHittable(this, collider);
        }

        public virtual void Hit(BulletDamage damage)
        {
            hp = Mathf.Clamp(hp - damage.HPInjury, 0, maxHp);

            if (hp == 0)
            {
                Die(killForceBonus);
            }
        }

        public void KillByUlt()
        {
            Die(0);
        }

        private void Die(int killForceBonus)
        {
            IsAlive = false;
            SceneC.Instance.EnemiesHolder.OnEnemyDead(killForceBonus);
            Dissolve();
        }

        protected virtual void Dissolve()
        {
            var sceneC = SceneC.Instance;
            sceneC.HittablesHolder.UnregisterHittable(collider);
            sceneC.EnemiesHolder.OnEnemyDissolve(this);
        }
    }
}
