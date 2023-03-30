using UnityEngine;
using UnityEngine.AI;

namespace PlayerDir.States
{
    public class PlayerMoveState : PlayerState
    {
        private readonly Joystick joystick;
        private readonly NavMeshAgent agent;

        public PlayerMoveState(Player context) : base(context)
        {
            agent = Context.Agent;
            joystick = SceneC.Instance.UIHolder.GameUI.Joystick;
        }

        public override void UpdateMovement()
        {
            if (CheckTeleportRadius())
            {
                Context.SetState(new PlayerTeleportState(Context));
                return;
            }

#if UNITY_EDITOR
            var pressedWASD = 
                Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.D);
#endif
            
            var joyDir = joystick.Direction;
            if (
                (joyDir.magnitude == 0f ||
                 joyDir.magnitude < joystick.DeadZone)
#if UNITY_EDITOR
                && !pressedWASD
#endif
            )
            {
                Context.SetState(new PlayerIdleState(Context));
            }
#if UNITY_EDITOR
            else if(pressedWASD)
            {
                MoveWASD();
            }
#endif
            else
            {
                Move(joyDir);
            }
        }

#if UNITY_EDITOR
        private void MoveWASD()
        {
            Vector2 wasdDir = default;
            if (Input.GetKey(KeyCode.A)) wasdDir += new Vector2(-1f, 0f);
            if (Input.GetKey(KeyCode.W)) wasdDir += new Vector2(0f, 1f);
            if (Input.GetKey(KeyCode.S)) wasdDir += new Vector2(0f, -1f);
            if (Input.GetKey(KeyCode.D)) wasdDir += new Vector2(1f, 0f);
            Move(wasdDir.normalized);
        }
#endif

        private void Move(Vector2 joyDir)
        {
            var forward = Context.Transform.forward;
            float speed = Time.deltaTime * Context.MovementSpeed * joyDir.magnitude;
            var angle = -Vector2.SignedAngle(new Vector2(0f, 1f), joyDir);
            var movementDirection = Quaternion.AngleAxis(angle, Vector3.up) * forward;
            agent.Move(movementDirection * speed);
        }
    }
}
