using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using _Game._Scripts.Animations;
using DG.Tweening;
using UnityEngine;

public class PlayerCrouchingState : PlayerMovementState
{
    public PlayerCrouchingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }
    private PlayerController m_player => _stateMachine.Player;

    public override void Enter()
    {
        Debug.Log("Crouch");
        SetGroundAcceleration(m_movementData.WalkingAcceleration);
        SetMaxGroundSpeed(m_movementData.WalkingMaxSpeed);
        m_player.StartCoroutine(Crouch());
    }

    public override void Update()
    {
        base.Update();
        
        if(!_shouldSlide && !HasSomethingOverhead())
            m_player.StartCoroutine(StopCrouching());
    }



    private IEnumerator Crouch()
    {
        PlayerAnimationsManager.Instance.CameraHandler.SetBool(PlayerAnimations.CrouchingBool, true);
        if (!m_player.IsSliding)
        {
            m_player.PlayerCollider.transform
                .DOLocalMoveY(m_movementData.CrouchColliderPos, m_movementData.CrouchAnimationDuration)
                .SetEase(m_movementData.CrouchAnimationEase);
            m_player.PlayerCollider.transform
                .DOScaleY(m_movementData.CrouchColliderSize, m_movementData.CrouchAnimationDuration)
                .SetEase(m_movementData.CrouchAnimationEase);
            m_player.CameraTransform
                .DOLocalMoveY(m_movementData.CrouchCameraSize, m_movementData.CrouchAnimationDuration)
                .SetEase(m_movementData.CrouchAnimationEase);
            
        }
        else
        {
            m_player.IsSliding = false;
            m_player.PlayerRigidbody.isKinematic = true;
        }

        yield return new WaitForSeconds(m_movementData.CrouchAnimationDuration);
        m_player.PlayerRigidbody.isKinematic = false;

    }

    private IEnumerator StopCrouching()
    {
        Debug.Log($"Stop crouching: {HasSomethingOverhead()}");
        PlayerAnimationsManager.Instance.CameraHandler.SetBool(PlayerAnimations.CrouchingBool, false);
        m_player.PlayerCollider.transform.DOScaleY(1, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);
        m_player.PlayerCollider.transform.DOLocalMoveY(0, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);
        m_player.CameraTransform.DOLocalMoveY(.6f, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);
        _stateMachine.ChangeState(_stateMachine.IdleState);
        m_player.PlayerRigidbody.isKinematic = true;
        yield return new WaitForSeconds(m_movementData.CrouchAnimationDuration);
        m_player.PlayerRigidbody.isKinematic = false;
    }
}
