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
            Accelerate(_inputDirection, _groundAccelaration, _maxGroundSpeed * _inputLength);
        }
    }
}
