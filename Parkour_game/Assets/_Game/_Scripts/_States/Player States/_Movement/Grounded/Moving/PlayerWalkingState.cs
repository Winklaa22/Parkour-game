namespace _Game._Scripts._States.Player_States._Movement.Grounded.Moving
{
    public class PlayerWalkingState : PlayerMovementState
    {
        public PlayerWalkingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            SetGroundAcceleration(m_movementData.WalkingAcceleration);
            SetMaxGroundSpeed(m_movementData.WalkingMaxSpeed);
        }

        public override void Update()
        {
            base.Update();
            
            if (GetEnvironmentState() == EnvironmentState.AIR)
                _stateMachine.ChangeState(_stateMachine.AirIdleState);

            if (!IsMoving)
                _stateMachine.ChangeState(_stateMachine.IdleState);

        }
    }
}
