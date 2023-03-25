using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")] 
    [SerializeField] private CapsuleCollider m_playerCollider;
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private float m_friction;

    [SerializeField] private MovementData m_movementData;

    [Header("Mouse")]
    [SerializeField] private float m_mouseSensivity = 1.0f;
    [SerializeField] private float m_jumpForce = 15f;

    [Header("Camera")]
    [SerializeField] private Transform m_cameraTransform;

    [Header("States")]
    [SerializeField] private PlayerState m_playerState;
    [SerializeField] private EnvironmentState m_environmentState = EnvironmentState.GROUND;
    [SerializeField] private MovementStates m_movementState;

    [Header("Slicing")] 
    [SerializeField] private float m_slicingTime = 1;
    [SerializeField] private float m_sliceAnimDuration = .25f;
    [SerializeField] private Ease m_sliceAnimationEase;
    private Vector3 m_sliceDirection;

    [Header("Crouching")] 
    [SerializeField] private float m_crouchingAcceleration = 7f;
    [SerializeField] private float m_crouchingMaxSpeed = 7f;
    [SerializeField] private float m_crouchingPosition;
    [SerializeField] private float m_crouchingSize;
    [SerializeField] private float m_crouchingAnimDuration = .25f;
    [SerializeField] private float m_crouchingCameraPosition;
    [SerializeField] private Ease m_crouchingAnimationEase;
    
    [Header("Animations")] 
    [SerializeField] private Animator m_animator;

    private float3 velocity;
    private float pitch;
    private float yaw;
    private float2 inputDirection;
    private float inputLength;
    
    private RaycastHit groundHitInfo;
    private float2 m_inputs;
    private Ray m_climpRay;

    [Header("Inputs")] 
    private InputActions m_inputActions;
    private bool m_sliceInput;
    private bool m_crouchInput;


    [SerializeField] private float m_detectCellingSpherePos;
    [SerializeField] private float m_detectCellingSphereRadius;
    [SerializeField] private bool m_isCrouching;

    private void Awake()
    {
        m_inputActions = new InputActions();
        m_inputActions.Enable();
    }

    private void CheckInputValues()
    {
        m_inputs = m_inputActions.Player.Movement.ReadValue<Vector2>();

        float2 inputWorld;
        {
            float input_lenghtsq = math.lengthsq(m_inputs);

            if (input_lenghtsq > 1.0f)
                m_inputs /= math.sqrt(input_lenghtsq);

            var yawRad = math.radians(yaw);
            var yawSin = math.sin(yawRad);
            var yawCos = math.cos(yawRad);

            inputWorld = math.mul(new float2x2(yawCos, yawSin, -yawSin, yawCos), m_inputs);
        }

        inputLength = math.length(inputWorld);
        inputDirection = math.normalizesafe(inputWorld);
    }

    public void Accelerate(float2 dirH, float accel, float limit)
    {
        var proj = math.dot(velocity.xz, dirH);
        var dv = accel * Time.deltaTime;
        dv = math.min(dv, limit - proj);
        dv = math.max(dv, 0.0f);
        velocity.xz += dirH * dv;
    }

    private void AirMovement() => Accelerate(inputDirection, m_movementData.AirAcceleration, m_movementData.AirSpeed * inputLength);

    private void GroundMovement()
    {
        if (m_movementState != MovementStates.CROUCHING)
        {
            m_movementState = !Input.GetKey(KeyCode.LeftShift) ? MovementStates.RUNNING : MovementStates.WALKING;
        }

        switch (m_movementState)
        {
            case MovementStates.WALKING:
                m_movementData.GroundAcceleration = m_movementData.WalkingAcceleration;
                m_movementData.GroundMaxSpeed = m_movementData.WalkingMaxSpeed;
                break;
            
            case MovementStates.RUNNING:
                m_movementData.GroundAcceleration = m_movementData.RunningAcceleration;
                m_movementData.GroundMaxSpeed = m_movementData.RunningMaxSpeed;
                break;
            
            case MovementStates.CROUCHING:
                m_movementData.GroundAcceleration = m_crouchingAcceleration;
                m_movementData.GroundMaxSpeed = m_crouchingMaxSpeed;
                break;
        }

            velocity.xz *= math.max(1.0f - m_friction * Time.deltaTime, 0.0f);
        Accelerate(inputDirection, m_movementData.GroundAcceleration, m_movementData.GroundMaxSpeed * inputLength);
    }

    private EnvironmentState GetEnvironmentState()
    {
        return Physics.SphereCast(transform.position - new Vector3(0, m_movementData.DetectGroundSpherePos, 0), m_movementData.DetectGroundSphereRadius, Vector3.down, out groundHitInfo, .1f) 
            ? EnvironmentState.GROUND 
            : EnvironmentState.AIR;
    }

    private float MovementMagnitude()
    {
        return m_inputActions.Player.Movement.ReadValue<Vector2>().magnitude;
    }

    private void SetMovement()
    {
        m_rigidbody.velocity = new float3(velocity.x, m_rigidbody.velocity.y, velocity.z);

        switch (GetEnvironmentState())
        {
            case EnvironmentState.GROUND:
                GroundMovement();
                break;

            case EnvironmentState.AIR:
                AirMovement();
                break;
        }
    }

    private void StateMachine()
    {
        switch (m_playerState)
        {
            case PlayerState.IDLE:
            {
                m_movementState = MovementStates.WALKING;
                
                if (MovementMagnitude() > 0)
                    m_playerState = PlayerState.MOVING;
                break;
            }
            case PlayerState.MOVING:
                if (MovementMagnitude() <= 0)
                    m_playerState = PlayerState.IDLE;
                
                SetMovement();
                break;
            case PlayerState.CROUCHING:
                SetMovement();
                break;
        }
    }
    
    private void SetCameraRotate()
    {
        var mouse = (float2) m_inputActions.Player.Look.ReadValue<Vector2>();;

        pitch -= mouse.y * m_mouseSensivity;
        yaw += mouse.x * m_mouseSensivity;

        pitch = math.clamp(pitch, -90.0f, 90.0f);
        yaw -= math.floor(yaw / 360.0f) * 360.0f;
        m_cameraTransform.localRotation = Quaternion.Euler(pitch, 0.0f, 0.0f);
        transform.localRotation = Quaternion.Euler(0.0f, yaw, 0.0f);
    }

    private void Update()
    {
        CheckInputValues();
        SetCameraRotate();
        SetEnvironmentState();
        StateMachine();
        StatesCheck();
    }

    private bool HasSomethingOverhead()
    {
        return Physics.SphereCast(transform.position + new Vector3(0, m_detectCellingSpherePos, 0),
            m_detectCellingSphereRadius, Vector3.down, out var sth, .5f);
    }

    private void StatesCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetEnvironmentState().Equals(EnvironmentState.GROUND))
            {
                m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }

            Vault();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (GetEnvironmentState().Equals(EnvironmentState.GROUND))
            {
                if(m_movementState == MovementStates.RUNNING)
                    Slide();
                else
                    Crouch();
            }
        }

        if (!Input.GetKey(KeyCode.LeftControl) && m_playerState == PlayerState.CROUCHING && !HasSomethingOverhead())
        {
            StopCrouching();
        }
    }

    private void Vault()
    {
        if (Physics.Raycast(m_cameraTransform.position, m_cameraTransform.forward, out var firstHit, 1f,
                ~LayerMask.NameToLayer("VaultLayer")))
        {
            if (Physics.Raycast(
                    firstHit.point + (m_cameraTransform.forward * m_playerCollider.radius) +
                    (Vector3.up * 0.6f * m_playerCollider.height), Vector3.down, out var secondHit,
                    m_playerCollider.height))
                StartCoroutine(LerpVault(secondHit.point + Vector3.up, .5f));
        }
    }
    
    private IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        m_animator.SetTrigger("Vault");
        
        var startPosition = transform.position;
        m_rigidbody.isKinematic = true;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        m_rigidbody.isKinematic = false;
        transform.position = targetPosition;
    }

    #region sliding
    private void Slide()
    {
        velocity = float3.zero;
        m_playerState = PlayerState.SLIDING;
        m_sliceDirection = transform.forward;
        m_rigidbody.AddForce(transform.forward * 2f, ForceMode.Impulse);
        m_playerCollider.transform.DOLocalMoveY(-0.2763f, .25f);
        m_playerCollider.transform.DOScaleY(0.7237288f, .25f);
        m_cameraTransform.DOLocalMoveY(0, .25f);
        m_animator.SetBool("Sliding", true);
        StartCoroutine(Sliding());
    }
    
    private IEnumerator Sliding()
    {
        float time = 0;

        while (time < m_slicingTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        StopSliding();
    }


    private void StopSliding()
    {
        m_animator.SetBool("Sliding", false);
        if (HasSomethingOverhead())
        {
            Crouch();
            return;
        }
        
        
        m_playerCollider.transform.DOLocalMoveY(0, m_sliceAnimDuration).SetEase(m_sliceAnimationEase);
        m_playerCollider.transform.DOScaleY(1, m_sliceAnimDuration).SetEase(m_sliceAnimationEase);
        m_cameraTransform.DOLocalMoveY(.6f, m_sliceAnimDuration).SetEase(m_sliceAnimationEase);
        m_playerState = PlayerState.IDLE;
    }
    
    #endregion

    #region crouching

    private void Crouch()
    {
        m_isCrouching = true;
        m_animator.SetBool("Crouching", true);
        m_playerState = PlayerState.CROUCHING;
        m_movementState = MovementStates.CROUCHING;
        m_playerCollider.transform.DOLocalMoveY(m_crouchingPosition, m_crouchingAnimDuration).SetEase(m_crouchingAnimationEase);
        m_playerCollider.transform.DOScaleY(m_crouchingSize, m_crouchingAnimDuration).SetEase(m_crouchingAnimationEase);
        m_cameraTransform.DOLocalMoveY(m_crouchingCameraPosition, m_crouchingAnimDuration).SetEase(m_crouchingAnimationEase);;
    }

    private void StopCrouching()
    {
        m_isCrouching = false;
        m_animator.SetBool("Crouching", false);
        m_playerCollider.transform.DOLocalMoveY(0, m_crouchingAnimDuration).SetEase(m_crouchingAnimationEase);
        m_playerCollider.transform.DOScaleY(1, m_crouchingAnimDuration).SetEase(m_crouchingAnimationEase);
        m_cameraTransform.DOLocalMoveY(.6f, m_crouchingAnimDuration).SetEase(m_crouchingAnimationEase);;
        m_playerState = PlayerState.IDLE;
    }
    
    private void TryToStopCrouching()
    {
        StopCrouching();
    }

    #endregion
    
    private void SetEnvironmentState()
    {
        m_environmentState = GetEnvironmentState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_environmentState == EnvironmentState.AIR)
        {
            velocity = float3.zero;
        }

        if (m_playerState == PlayerState.SLIDING)
        {
            StopSliding();
            
            if (collision.gameObject.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(m_sliceDirection * 5, ForceMode.Impulse);
            }

            if (collision.gameObject.TryGetComponent<RagdollPart>(out var part))
            {
                part.Active();
            }
        }
    }
}
