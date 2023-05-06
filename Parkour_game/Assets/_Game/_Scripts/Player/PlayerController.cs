using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementStateMachine m_movementStateMachine;
    
    [Header("Physics")] 
    [SerializeField] private CapsuleCollider m_playerCollider;
    public CapsuleCollider PlayerCollider => m_playerCollider;

    [SerializeField] private Rigidbody m_rigidbody;
    private bool m_canMove = true;
    public bool CanMove
    {
        set => m_canMove = value;
    }
    
    public Rigidbody PlayerRigidbody => m_rigidbody;
    public delegate void OnCollisionEntered();
    public OnCollisionEntered Entity_OnCollisionEntered;

    [Header("Inputs")] 
    private float2 m_inputDirection;
    private float m_inputLength;
    private Vector2 m_inputs;
    public Vector2 Inputs => m_inputs;


    [Header("Movement")]
    [SerializeField] private MovementData m_movementData;
    private float m_groundAccelaration;
    public float GroundAcceleration
    {
        set => m_groundAccelaration = value;
    }

    private float m_groundMaxSpeed;
    public float GroundMaxSpeed
    {
        set => m_groundMaxSpeed = value;
    }
    public MovementData MovementData => m_movementData;
    private float3 m_velocity;
    public float3 Velocity
    {
        get => m_velocity;
        set => m_velocity = value;
    }

    [Header("Looking")]
    [SerializeField] private Transform m_cameraTransform;
    public Transform CameraTransform => m_cameraTransform;
    
    [SerializeField] private float m_mouseSensivity = 1.0f;
    private float m_pitch;
    private float m_yaw;

    [Header("Inputs")] 
    private InputActions.PlayerActions m_playerActions;
    public InputActions.PlayerActions PlayerActions => m_playerActions;
    public delegate void OnVault();
    public OnCollisionEntered Entity_OnVault;

    private void Start()
    {
        m_playerActions = InputManager.Instance.PlayerActions;
        m_movementStateMachine = new PlayerMovementStateMachine(this);
        m_movementStateMachine.ChangeState(m_movementStateMachine.IdleState);
    }

    private void Update()
    {
        m_inputs = m_playerActions.Movement.ReadValue<Vector2>();
        SetMovement();
        SetCameraRotate();
        CheckInputValues();
        m_movementStateMachine.HandleInput();
        m_movementStateMachine.Update();
    }

    public void SetMovement()
    {
        if(!m_canMove)
            return;
        
        switch (GetEnvironmentState())
        {
            case EnvironmentState.AIR:
                AirMove();
                break;
            
            case EnvironmentState.GROUND:
                GroundMove();
                break;
        }
        
        m_rigidbody.velocity = new float3(m_velocity.x, m_rigidbody.velocity.y, m_velocity.z);
    }

    private void FixedUpdate()
    {
        m_movementStateMachine.PhysicsUpdate();
    }

    public EnvironmentState GetEnvironmentState()
    {
        return Physics.SphereCast( transform.position - new Vector3(0, m_movementData.DetectGroundSpherePos, 0), m_movementData.DetectGroundSphereRadius, Vector3.down, out _, .1f) 
            ? EnvironmentState.GROUND 
            : EnvironmentState.AIR;
    }
    
    private void SetCameraRotate()
    {
        var mouse = (float2) m_playerActions.Look.ReadValue<Vector2>();;

        m_pitch -= mouse.y * m_mouseSensivity;
        m_yaw += mouse.x * m_mouseSensivity;

        m_pitch = math.clamp(m_pitch, -90.0f, 90.0f);
        m_yaw -= math.floor(m_yaw / 360.0f) * 360.0f;
        m_cameraTransform.localRotation = Quaternion.Euler(m_pitch, 0.0f, 0.0f);
        transform.localRotation = Quaternion.Euler(0.0f, m_yaw, 0.0f);
    }

    private void GroundMove()
    {
        Accelerate(m_inputDirection, m_groundAccelaration, m_groundMaxSpeed * m_inputLength);
        m_velocity.xz *= math.max(1.0f - m_movementData.Friction * Time.deltaTime, 0.0f);
    }

    private void AirMove()
    {
        Accelerate(m_inputDirection, m_movementData.AirAcceleration, m_movementData.AirSpeed * m_inputLength);
    }
    
    private void Accelerate(float2 dirH, float accel, float limit)
    {
        var proj = math.dot(m_velocity.xz, dirH);
        var dv = accel * Time.deltaTime;
        dv = math.min(dv, limit - proj);
        dv = math.max(dv, 0.0f);
        m_velocity.xz += dirH * dv;
    }

    private void CheckInputValues()
    {
        float2 inputWorld;
        {
            float input_lenghtsq = math.lengthsq(m_inputs);

            if (input_lenghtsq > 1.0f)
                m_inputs /= math.sqrt(input_lenghtsq);

            var yawRad = math.radians(m_yaw);
            var yawSin = math.sin(yawRad);
            var yawCos = math.cos(yawRad);

            inputWorld = math.mul(new float2x2(yawCos, yawSin, -yawSin, yawCos), m_inputs);
        }

        m_inputLength = math.length(inputWorld);
        m_inputDirection = math.normalizesafe(inputWorld);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Entity_OnCollisionEntered?.Invoke();
        m_velocity = float3.zero;
    }
}
