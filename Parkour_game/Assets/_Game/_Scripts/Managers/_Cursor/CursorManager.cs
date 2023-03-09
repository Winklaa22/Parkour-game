using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SceneSingleton<CursorManager>
{
    private CursorState m_state;

    protected override void OnStart()
    {
        base.OnStart();
        SetCursorState(CursorState.GAMEPLAY);
    }

    public void SetCursorState(CursorState state)
    {
        m_state = state;
        
        switch (state)
        {
            case CursorState.GAMEPLAY:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            
            case CursorState.PAUSE:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }
}
