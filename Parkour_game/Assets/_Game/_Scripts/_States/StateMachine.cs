namespace _Game._Scripts._States
{
    public abstract class StateMachine
    {
        private IState _currentState;
        public IState CurrentState => _currentState;

        public void ChangeState(IState state)
        {
            _currentState?.Exit();
            
            _currentState = state;
            
            _currentState.Enter();
        }

        public void HandleInput()
        {
            _currentState?.HandleInput();
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            _currentState?.PhysicsUpdate();
        }
    }
}
