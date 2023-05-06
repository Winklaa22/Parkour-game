using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game._Scripts._States.Player_States._Movement
{
    public abstract class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine _stateMachine;
        
        [Header("Movement data")] 
        protected MovementData m_movementData;
        protected float _groundAccelaration;
        protected float _maxGroundSpeed;
        protected float3 _velocity;
        
        [Header("Movements conditions")] 
        protected bool _shouldWalk;

        [Header("Vaulting")] 
        protected Vector3 _vaultPos;

        private bool _shouldVault;

        [Header("Jumping")] 
        private bool _shouldJump;


        protected PlayerMovementState(PlayerMovementStateMachine stateMachine)
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
            
            _stateMachine.Player.PlayerActions.Jump.started += delegate(InputAction.CallbackContext context)
            {
                if (CanVault())
                    _shouldVault = true;
                else
                    _shouldJump = true;
            };
            
            _stateMachine.Player.PlayerActions.Jump.canceled += delegate(InputAction.CallbackContext context)
            {
                _shouldVault = false;
                _shouldJump = false;
            };
        }
        
        public virtual void Enter()
        {
            Debug.Log($"{GetType().Name} enter");
        }

        public virtual void Exit()
        {
            // Debug.Log($"{GetType().Name} exit");
        }

        public virtual void HandleInput() { }

        public virtual void Update()
        {
            TryToVault();
        }

        public virtual void PhysicsUpdate() { }

        protected void TryToVault()
        {
            if(_shouldVault && !_stateMachine.CurrentState.Equals(_stateMachine.VaultingState))
                _stateMachine.ChangeState(_stateMachine.VaultingState);
        }

        protected void SetMaxGroundSpeed(float value)
        {
            _stateMachine.Player.GroundMaxSpeed = value;
        }
        
        protected void SetGroundAcceleration(float value)
        {
            _stateMachine.Player.GroundAcceleration = value;
        }

        protected void TryToJump()
        {
            if (!_shouldJump)
                return;
            
            Debug.Log("Sadsd");
            _stateMachine.Player.Jump();
            _shouldJump = false;
        }
        
        private bool CanVault()
        {
            var camera = _stateMachine.Player.CameraTransform;
            var collider = _stateMachine.Player.PlayerCollider;
            
            if (Physics.Raycast(camera.position, camera.forward, out var firstHit, 1f, ~LayerMask.NameToLayer("VaultLayer")))
            {
                if (Physics.Raycast(firstHit.point + camera.forward * collider.radius + Vector3.up * 0.6f * collider.height, Vector3.down, out var secondHit, collider.height))
                {
                    _vaultPos = secondHit.point + Vector3.up;
                    return true;
                }
            }

            return false;
        }

        protected EnvironmentState GetEnvironmentState() => _stateMachine.Player.GetEnvironmentState();
        protected bool IsMoving => _stateMachine.Player.Inputs != Vector2.zero;

    }
}
