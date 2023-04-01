using Unity.Mathematics;
using UnityEngine;

namespace _Game._Scripts._States.Player_States._Movement
{
    public abstract class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine _stateMachine;
        protected float3 _velocity;

        [Header("Movement data")] 
        protected MovementData m_movementData;
        protected float _groundAccelaration;
        protected float _maxGroundSpeed;

        [Header("Movements conditions")] 
        protected bool _shouldWalk;
        
        

        public PlayerMovementState(PlayerMovementStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            m_movementData = _stateMachine.Player.MovementData;
            SetEvents();
        }

        private void SetEvents()
        {
            _stateMachine.Player.PlayerActions.Walk.started += delegate
            {
                _shouldWalk = true;
            };

            _stateMachine.Player.PlayerActions.Walk.canceled += delegate
            {
                _shouldWalk = false;
            };
            
            // _stateMachine.Player.Entity_OnCollisionEntered += delegate
            // {
            //     _velocity = 0;
            // };
        }
        
        public virtual void Enter()
        {
            Debug.Log($"{GetType().Name} enter");
        }

        public virtual void Exit()
        {
            Debug.Log($"{GetType().Name} exit");
        }

        public virtual void HandleInput()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        protected void SetMaxGroundSpeed(float value)
        {
            _stateMachine.Player.GroundMaxSpeed = value;
        }
        
        protected void SetGroundAcceleration(float value)
        {
            _stateMachine.Player.GroundAcceleration = value;
        }

        protected EnvironmentState GetEnvironmentState() => _stateMachine.Player.GetEnvironmentState();
        protected bool IsMoving => _stateMachine.Player.Inputs != Vector2.zero;

    }
}
