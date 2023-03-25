using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SceneSingleton<InputManager>
{
    private InputActions m_inputActions;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_inputActions = new InputActions();
        m_inputActions.Enable();
    }

    public Vector2 GetMovementValues()
    {
        return m_inputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetLookValues()
    {
        return m_inputActions.Player.Look.ReadValue<Vector2>();
    }
}
