using UnityEngine;

namespace _Game._Scripts._States
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void PhysicsUpdate();
    }
}
