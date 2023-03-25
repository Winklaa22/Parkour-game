using UnityEngine;

namespace _Game._Scripts._States.Player_States._Movement.Grounded
{
    public class PlayerIdleState : PlayerMovementState
    {
        public PlayerIdleState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}
