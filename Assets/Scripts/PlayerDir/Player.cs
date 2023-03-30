using System;
using Bullets;
using PlayerDir.States;
using UnityEngine;
using UnityEngine.AI;

namespace PlayerDir
{
    public class Player : MonoBehaviour
    {
        public event Action OnPlayerKilled;
        
        private PlayerState currentState;
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float TeleportBorder { get; private set; }
        [field: SerializeField] public PlayerIndicators Indicators { get; private set; }
        [field: SerializeField] public TrackingPoint TrackingPoint { get; set; }

        [SerializeField] private PlayerShooter shooter;
        [SerializeField] private PlayerRotator rotator;
        [SerializeField] private Transform followPoint;

        public void Init()
        {
            TrackingPoint = new TrackingPoint(followPoint);
            Indicators.Init(this);
            rotator.Init(this);
            shooter.Init(this);
            currentState = new PlayerIdleState(this);
        }

        private void Update()
        {
            rotator.UpdateRotation();
            currentState.UpdateMovement();
        }

        public void SetState(PlayerState state)
        {
            currentState = state;
        }

        public void ReplaceTrackingPoint()
        {
            TrackingPoint.Attached = false;
            TrackingPoint = new TrackingPoint(followPoint);
        }

        public void OnZeroHp()
        {
            OnPlayerKilled?.Invoke();
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Vector3.zero, TeleportBorder);
        }
#endif
    }
}