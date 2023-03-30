using System;

namespace PlayerDir
{
    [Serializable]
    public abstract class PlayerState
    {
        protected Player Context;

        protected PlayerState(Player context)
        {
            Context = context;
        }

        public abstract void UpdateMovement();

        protected bool CheckTeleportRadius()
        {
            return Context.Transform.position.magnitude >= Context.TeleportBorder;
        }
    }
}
