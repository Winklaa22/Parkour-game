using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using UnityEngine;

public class PlayerVaultingState : PlayerMovementState
{
    public PlayerVaultingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        //todo Set Vault animation trigger
    }

    public override void Update()
    {
        base.Update();
        LerpVault(_vaultPos, .5f);
    }

    private void LerpVault(Vector3 targetPosition, float duration)
    {
        var time = 0.0f;
        var player = _stateMachine.Player;

        var startPosition = player.transform.position;
        player.PlayerRigidbody.isKinematic = true;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
        }

        player.PlayerRigidbody.isKinematic = false;
        player.transform.position = targetPosition;
        
    }
}
