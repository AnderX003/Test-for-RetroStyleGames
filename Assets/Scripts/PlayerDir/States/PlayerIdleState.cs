namespace PlayerDir.States
{
    public class PlayerIdleState : PlayerState
    {
        private readonly Joystick joystick;

        public PlayerIdleState(Player context) : base(context)
        {
            joystick = SceneC.Instance.UIHolder.GameUI.Joystick;
        }

        public override void UpdateMovement()
        {
            if (CheckTeleportRadius())
            {
                Context.SetState(new PlayerTeleportState(Context));
                return;
            }
            
            if (joystick.Direction.magnitude >= joystick.DeadZone)
            {
                Context.SetState(new PlayerMoveState(Context));
            }
        }
    }
}
