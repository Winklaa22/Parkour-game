using _Game._Scripts._States.Player_States._Movement.Grounded;
using _Game._Scripts._States.Player_States._Movement.Grounded.Moving;

namespace _Game._Scripts._States.Player_States._Movement
{
    public class PlayerMovementStateMachine : StateMachine
    {
        private PlayerController m_player;
        
        public PlayerIdleState IdleState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }

        public PlayerMovementStateMachine(PlayerController player)
        {
            m_player = player;
            
            IdleState = new PlayerIdleState();
            WalkingState = new PlayerWalkingState();
            RunningState = new PlayerRunningState();
        }
    }
}
