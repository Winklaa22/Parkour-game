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
        
        [Header("Inputs")]
        protected float2 _inputDirection;
        protected float _inputLength;
        private Vector2 m_inputs;
        

        public PlayerMovementState(PlayerMovementStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            m_movementData = _stateMachine.Player.MovementData;
        }
        
        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void HandleInput()
        {
            
        }

        public virtual void Update()
        {
            CheckInputValues();
        }

        public virtual void PhysicsUpdate()
        {
            
        }
        
        private void CheckInputValues()
        {
            m_inputs = _stateMachine.Player.PlayerActions.Movement.ReadValue<Vector2>();

            float2 inputWorld;
            {
                float input_lenghtsq = math.lengthsq(m_inputs);

                if (input_lenghtsq > 1.0f)
                    m_inputs /= math.sqrt(input_lenghtsq);

                var yawRad = math.radians(_stateMachine.Player.Yaw);
                var yawSin = math.sin(yawRad);
                var yawCos = math.cos(yawRad);

                inputWorld = math.mul(new float2x2(yawCos, yawSin, -yawSin, yawCos), m_inputs);
            }

            _inputLength = math.length(inputWorld);
            _inputDirection = math.normalizesafe(inputWorld);
        }
        
        private EnvironmentState GetEnvironmentState()
        {
            return Physics.SphereCast( _stateMachine.Player.transform.position - new Vector3(0, m_movementData.DetectGroundSpherePos, 0), m_movementData.DetectGroundSphereRadius, Vector3.down, out groundHitInfo, .1f) 
                ? EnvironmentState.GROUND 
                : EnvironmentState.AIR;
        }

        protected void GroundMove()
        {
            Accelerate(_inputDirection, _groundAccelaration, _maxGroundSpeed * _inputLength);
        }

        protected void AirMove()
        {
            Accelerate(_inputDirection, m_movementData.AirAcceleration, m_movementData.AirSpeed * _inputLength);
        }
        
        protected void Accelerate(float2 dirH, float accel, float limit)
        {
            var proj = math.dot(_velocity.xz, dirH);
            var dv = accel * Time.deltaTime;
            dv = math.min(dv, limit - proj);
            dv = math.max(dv, 0.0f);
            _velocity.xz += dirH * dv;
        }
    }
}
