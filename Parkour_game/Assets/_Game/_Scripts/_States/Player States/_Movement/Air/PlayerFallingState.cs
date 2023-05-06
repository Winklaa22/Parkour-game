using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFallingState : PlayerMovementState
{
    public PlayerFallingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (GetEnvironmentState() == EnvironmentState.GROUND)
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
