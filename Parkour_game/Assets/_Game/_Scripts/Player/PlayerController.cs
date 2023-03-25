using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementStateMachine m_movementStateMachine;
    
    private void Awake()
    {
        m_movementStateMachine = new PlayerMovementStateMachine(this);
    }

    private void Start()
    {
        m_movementStateMachine.ChangeState(m_movementStateMachine.IdleState);
    }

    private void Update()
    {
        m_movementStateMachine.HandleInput();
        
        m_movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        m_movementStateMachine.PhysicsUpdate();
    }
}
