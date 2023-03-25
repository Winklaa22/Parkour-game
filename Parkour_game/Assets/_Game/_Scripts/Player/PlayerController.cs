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
    [SerializeField] private Rigidbody m_rigidbody;
    public Rigidbody PlayerRigidbody => m_rigidbody;

    [Header("Movement")] 
    [SerializeField] private MovementData m_movementData;
    public MovementData MovementData => m_movementData;
    
    [Header("Looking")]
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private float m_mouseSensivity = 1.0f;
    private float m_pitch;
    private float m_yaw;
    public float Yaw => m_yaw;

    [Header("Inputs")] 
    private InputActions.PlayerActions m_playerActions;
    public InputActions.PlayerActions PlayerActions => m_playerActions;


    private void Awake()
    {
        m_movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void Start()
    {
        m_playerActions = InputManager.Instance.PlayerActions;
        m_movementStateMachine.ChangeState(m_movementStateMachine.IdleState);
    }

    private void Update()
    {
        SetCameraRotate();
        m_movementStateMachine.HandleInput();
        m_movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        m_movementStateMachine.PhysicsUpdate();
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
}
