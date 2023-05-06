using _Game._Scripts._States.Player_States._Movement.Grounded;
using _Game._Scripts._States.Player_States._Movement.Grounded.Moving;

namespace _Game._Scripts._States.Player_States._Movement
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public PlayerController Player { get; }
        public PlayerIdleState IdleState { get; }
        public PlayerFallingState FallingState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }
        public PlayerVaultingState VaultingState { get; }
        public PlayerJumpingState JumpingState { get; }
        public PlayerSlidingState SlidingState { get; }
        public PlayerCrouchingState CrouchingState { get; }

        public PlayerMovementStateMachine(PlayerController player)
        {
            Player = player;
            IdleState = new PlayerIdleState(this);
            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
            FallingState = new PlayerFallingState(this);
            VaultingState = new PlayerVaultingState(this);
            JumpingState = new PlayerJumpingState(this);
            CrouchingState = new PlayerCrouchingState(this);
            SlidingState = new PlayerSlidingState(this);
        }
    }
}
