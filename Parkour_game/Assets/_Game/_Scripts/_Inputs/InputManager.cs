using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SceneSingleton<InputManager>
{
    private InputActions m_inputActions;
    public InputActions Actions => m_inputActions;

    private InputActions.PlayerActions m_playerActions;
    public InputActions.PlayerActions PlayerActions => m_playerActions;
    public float ScrollAxis => m_inputActions.Player.Scroll.ReadValue<float>();

    protected override void OnAwake()
    {
        base.OnAwake();
        m_inputActions = new InputActions();
        m_playerActions = m_inputActions.Player;
    }

    private void OnEnable()
    {
        m_inputActions.Enable();
    }

    private void OnDisable()
    {
        m_inputActions.Disable();
    }
}
