using _Game._Scripts._States.Player_States._Movement.Grounded;
using _Game._Scripts._States.Player_States._Movement.Grounded.Moving;

namespace _Game._Scripts._States.Player_States._Movement
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public PlayerController Player { get; }
        public PlayerIdleState IdleState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }

        public PlayerMovementStateMachine(PlayerController player)
        {
            Player = player;
            IdleState = new PlayerIdleState(this);
            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
        }
    }
}
