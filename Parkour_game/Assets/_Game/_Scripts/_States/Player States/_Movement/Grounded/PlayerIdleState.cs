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

        public override void Update()
        {
            base.Update();

            if (GetEnvironmentState() == EnvironmentState.AIR)
            {
                _stateMachine.ChangeState(_stateMachine.AirIdleState);
            }
            else
            {
                if(IsMoving)
                    _stateMachine.ChangeState(_stateMachine.RunningState);
            }
        }
    }
}
