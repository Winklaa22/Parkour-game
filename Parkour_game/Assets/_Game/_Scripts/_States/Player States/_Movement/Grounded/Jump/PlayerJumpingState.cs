using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using UnityEngine;

public class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _stateMachine.Player.PlayerRigidbody.AddForce(Vector3.up * m_movementData.JumpVelocity, ForceMode.Impulse);
    }

    public override void Update()
    {
        base.Update();
        
        if(_stateMachine.Player.PlayerRigidbody.velocity.y < 0)
            _stateMachine.ChangeState(_stateMachine.FallingState);
    }
}
