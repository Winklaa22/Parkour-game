using UnityEngine;

namespace _Game._Scripts._States.Player_States._Movement
{
    public abstract class PlayerMovementState : IState
    {
        public virtual void Enter()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Exit()
        {
            throw new System.NotImplementedException();
        }

        public virtual void HandleInput()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

        public virtual void PhysicsUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
