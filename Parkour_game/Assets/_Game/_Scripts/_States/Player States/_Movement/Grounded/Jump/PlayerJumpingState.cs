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
         Debug.Log("Jumping in " + _stateMachine.Player.GetEnvironmentState() + " environment");
        if(_stateMachine.Player.GetEnvironmentState() == EnvironmentState.AIR)
            return;
        
        _stateMachine.Player.PlayerRigidbody.AddForce(Vector3.up * m_movementData.JumpVelocity, ForceMode.Impulse);
    }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.Player.PlayerRigidbody.velocity.y < 0)
        {
            // _stateMachine.Player.IsJumping = false;
            _stateMachine.ChangeState(_stateMachine.FallingState);

        }
    }
}
