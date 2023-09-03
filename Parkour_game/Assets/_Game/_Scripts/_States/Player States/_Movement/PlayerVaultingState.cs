using System.Collections;
using _Game._Scripts._States.Player_States._Movement;
using _Game._Scripts.Animations;
using UnityEngine;

public class PlayerVaultingState : PlayerMovementState
{
    public PlayerVaultingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        PlayerAnimationsManager.Instance.CameraHandler.SetTrigger(PlayerAnimations.VaultTrigger);
        PlayerAnimationsManager.Instance.PistolHandler.SetTrigger(PlayerAnimations.HideTrigger);
        _stateMachine.Player.StartCoroutine(LerpVault(_vaultPos, .5f));
    }

    private IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        var time = 0.0f;
        var player = _stateMachine.Player;

        var startPosition = player.transform.position;
        player.PlayerRigidbody.isKinematic = true;

        while (time < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        PlayerAnimationsManager.Instance.PistolHandler.SetTrigger(PlayerAnimations.ShowTrigger);
        player.PlayerRigidbody.isKinematic = false;
        player.transform.position = targetPosition;
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
