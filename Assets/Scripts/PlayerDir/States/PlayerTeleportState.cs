using UnityEngine;
using UnityEngine.AI;

namespace PlayerDir.States
{
    public class PlayerTeleportState : PlayerState
    {
        private readonly Joystick joystick;
        private bool teleported;

        public PlayerTeleportState(Player context) : base(context)
        {
            joystick = SceneC.Instance.UIHolder.GameUI.Joystick;
            if (FindTeleportPoint(out var tpPos))
            {
                Teleport(tpPos);
            }
        }

        private bool FindTeleportPoint(out Vector3 pos)
        {
            //todo
            const float maxDistance = 100f;
            //if (NavMesh.FindClosestEdge(Vector3.zero, out var hit, NavMesh.AllAreas))
            if (NavMesh.SamplePosition(Vector3.zero, out var hit, maxDistance, NavMesh.AllAreas))
            {
                pos = hit.position;
                return true;
            }
            Debug.LogError("NavMesh could not FindClosestEdge while teleportation");
            teleported = true;
            pos = default;
            return false;
        }

        private void Teleport(Vector3 tpPos)
        {
            Context.ReplaceTrackingPoint();
            Context.Transform.position = tpPos;
            teleported = true;
        }

        public override void UpdateMovement()
        {
            if(!teleported)return;

            Context.SetState(joystick.Direction.magnitude < joystick.DeadZone
                ? new PlayerIdleState(Context)
                : new PlayerMoveState(Context));
        }
    }
}
