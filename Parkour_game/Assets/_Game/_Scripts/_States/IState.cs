using UnityEngine;

namespace _Game._Scripts._States
{
    public interface IState
    {
        void Enter();

        void Exit()
            ;
        void HandleInput();
        
        void Update();

        void PhysicsUpdate();
    }
}
