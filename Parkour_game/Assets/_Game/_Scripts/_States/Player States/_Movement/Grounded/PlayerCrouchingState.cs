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
        Crouch();
    }

    public override void Update()
    {
        base.Update();
        
        if(!_shouldSlide && !HasSomethingOverhead())
            StopCrouching();
    }

    private void Crouch()
    {
        PlayerAnimationsManager.Instance.CameraHandler.SetBool(PlayerAnimations.CrouchingBool, true);
        m_player.transform.DOScaleY(m_movementData.CrouchColliderSize, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);
        m_player.CameraTransform.DOLocalMoveY(m_movementData.CrouchCameraSize, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);;
    }

    private void StopCrouching()
    {
        PlayerAnimationsManager.Instance.CameraHandler.SetBool(PlayerAnimations.CrouchingBool, false);
        m_player.transform.DOScaleY(1, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);
        m_player.CameraTransform.DOLocalMoveY(.6f, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.CrouchAnimationEase);
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
