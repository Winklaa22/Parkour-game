using System;
using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementStateMachine m_movementStateMashine;
    
    private void Awake()
    {
        m_movementStateMashine = new PlayerMovementStateMachine(this);
    }

    private void Start()
    {
        m_movementStateMashine.ChangeState(m_movementStateMashine.IdleState);
    }

    private void Update()
    {
        m_movementStateMashine.HandleInput();
        
        m_movementStateMashine.Update();
    }

    private void FixedUpdate()
    {
        m_movementStateMashine.PhysicsUpdate();
    }
}
