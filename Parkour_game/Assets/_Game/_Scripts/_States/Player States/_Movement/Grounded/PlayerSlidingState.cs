using System.Collections;
using System.Collections.Generic;
using _Game._Scripts._States.Player_States._Movement;
using _Game._Scripts.Animations;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerSlidingState : PlayerMovementState
{
    public PlayerSlidingState(PlayerMovementStateMachine stateMachine) : base(stateMachine) { }

    private PlayerController m_player => _stateMachine.Player;
    private Vector3 m_sliceDirection;


    public override void Enter()
    {
        base.Enter();
        Slide();
    }
    
    private void Slide()
    {
        m_player.IsSliding = true;
        m_player.CanMove = false;
        m_player.Velocity = float3.zero;
        m_sliceDirection = m_player.transform.forward;
        m_player.PlayerRigidbody.AddForce(m_sliceDirection * 2f, ForceMode.Impulse);
        m_player.PlayerCollider.transform.DOScaleY(m_movementData.SlidingColliderSize, .25f);
        m_player.PlayerCollider.transform
            .DOLocalMoveY(m_movementData.SlidingColliderPos, m_movementData.CrouchAnimationDuration)
            .SetEase(m_movementData.SlidingAnimationEase);
        m_player.CameraTransform.DOLocalMoveY(0, .25f);
        PlayerAnimationsManager.Instance.CameraHandler.SetBool(PlayerAnimations.SlidingBool, true);
        m_player.StartCoroutine(Sliding());
    }
    
    private IEnumerator Sliding()
    {
        float time = 0;

        while (time < m_movementData.SlidingTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        StopSliding();
    }


    private void StopSliding()
    {
        PlayerAnimationsManager.Instance.CameraHandler.SetBool(PlayerAnimations.SlidingBool, false);
        if (HasSomethingOverhead())
        {
            _stateMachine.ChangeState(_stateMachine.CrouchingState);
            m_player.CanMove = true;
            return;
        }
        m_player.PlayerCollider.transform.DOScaleY(1, m_movementData.SlidingAnimationDuration).SetEase(m_movementData.SlidingAnimationEase);
        m_player.PlayerCollider.transform.DOLocalMoveY(0, m_movementData.CrouchAnimationDuration).SetEase(m_movementData.SlidingAnimationEase);

        m_player.CameraTransform.DOLocalMoveY(.6f, m_movementData.SlidingAnimationDuration).SetEase(m_movementData.SlidingAnimationEase);
        m_player.CanMove = true;
        m_player.IsSliding = false;

        _stateMachine.ChangeState(_stateMachine.IdleState);
    }
}
