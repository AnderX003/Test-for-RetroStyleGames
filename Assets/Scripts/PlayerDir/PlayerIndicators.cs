using System;
using Bullets;
using UnityEngine;

namespace PlayerDir
{
    [Serializable]
    public class PlayerIndicators : IHittable
    {
        public event Action<int, int> OnHpChange;
        public event Action<int, int> OnForceChange;
        public event Action<bool> OnCanUltStateChanged;

        [SerializeField] private int startHP;
        [SerializeField] private int maxHP;
        [SerializeField] private int startForce;
        [SerializeField] private int maxForce;
        [SerializeField] private int minForce;
        [SerializeField] private int forceRicochetBonus;
        [SerializeField] private int ultForceRequired;

        private Player player;
        private int hp;
        private int force;
        private bool canUlt;

        public bool IsAlive { get; private set; }

        private int HP
        {
            get => hp;
            set
            {
                var newHp = Mathf.Clamp(value, 0, maxHP);
                if (hp != newHp)
                {
                    OnHpChange?.Invoke(newHp, maxHP);
                    hp = newHp;
                }
            }
        }

        public int Force
        {
            get => force;
            private set
            {
                var newForce = Mathf.Clamp(value, minForce, maxForce);
                if (force != newForce)
                {
                    OnForceChange?.Invoke(newForce, maxForce);
                    CanUlt = value >= ultForceRequired;
                    force = newForce;
                }
            }
        }

        public bool CanUlt
        {
            get => canUlt;
            set
            {
                if (canUlt != value)
                {
                    OnCanUltStateChanged?.Invoke(value);
                    canUlt = value;
                }
            }
        }

        public void Init(Player player)
        {
            this.player = player;
            HP = startHP;
            Force = startForce;
            IsAlive = true;
            SceneC.Instance.HittablesHolder.RegisterHittable(this, player.Collider);
        }

        public void Hit(BulletDamage damage)
        {
            HP -= damage.HPInjury;
            Force -= damage.ForceInjury;

            if (HP == 0)
            {
                IsAlive = false;
                player.OnZeroHp();
            }
        }

        public void AddForce(int force)
        {
            Force += force;
        }

        public void AddRicochetBonus()
        {
            HP += maxHP / 2; //add half of hp
            Force += forceRicochetBonus;
        }

        public void UseAllForce()
        {
            Force = minForce;
        }
    }
}
