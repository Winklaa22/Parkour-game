using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using UnityEngine;

public class PlayerAirMovementState : PlayerMovementState
{
    public PlayerAirMovementState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();
        
        if (GetEnvironmentState() == EnvironmentState.GROUND)
            _stateMachine.ChangeState(_stateMachine.AirIdleState);
        
        if(IsMoving)
            _stateMachine.ChangeState(_stateMachine.AirMovementState);
        
    }
}
