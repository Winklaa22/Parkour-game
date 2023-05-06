namespace _Game._Scripts._States.Player_States._Movement.Grounded.Moving
{
    public class PlayerRunningState : PlayerMovementState
    {
        public PlayerRunningState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            SetGroundAcceleration(m_movementData.RunningAcceleration);
            SetMaxGroundSpeed(m_movementData.RunningMaxSpeed);
        }


        public override void Update()
        {
            base.Update();

            if (GetEnvironmentState() == EnvironmentState.AIR)
                _stateMachine.ChangeState(_stateMachine.FallingState);
        
            if(!IsMoving) 
                _stateMachine.ChangeState(_stateMachine.IdleState);

            if (_shouldWalk)
                _stateMachine.ChangeState(_stateMachine.WalkingState);
            
            TryToJump();
            TryToSlide();
        }
    }
}
