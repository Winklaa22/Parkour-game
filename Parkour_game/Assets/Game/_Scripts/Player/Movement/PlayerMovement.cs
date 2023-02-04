using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody m_rigidbody;

    [Header("Air")]
    [SerializeField] private float m_airSpeed;
    [SerializeField] private float m_airAcceleration;
    [SerializeField] private float m_jumpVelocity;

    [Header("Ground")]
    [SerializeField] private float m_sphereRadius;
    [SerializeField] private float m_friction;
    [SerializeField] private float m_groundAcceleration;
    [SerializeField] private float m_groundMaxSpeed;
    [SerializeField] private float m_spherePos;

    [Header("Mouse")]
    [SerializeField] private float m_mouseSensivity = 1.0f;
    [SerializeField] private float m_jumpForce = 15f;

    [Header("Camera")]
    [SerializeField] private Transform m_cameraTransform;

    [Header("States")]
    [SerializeField] private PlayerState m_playerState;
    [SerializeField] private EnvironmentState m_environmentState = EnvironmentState.GROUND;

    private float3 velocity;
    private float pitch;
    private float yaw;
    private float2 inputDirection;
    private float inputLength;
    
    private RaycastHit groundHitInfo;

    private void CheckInputValues()
    {
        var input = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float2 inputWorld;
        {
            float input_lenghtsq = math.lengthsq(input);

            if (input_lenghtsq > 1.0f)
                input /= math.sqrt(input_lenghtsq);

            var yawRad = math.radians(yaw);
            var yawSin = math.sin(yawRad);
            var yawCos = math.cos(yawRad);

            inputWorld = math.mul(new float2x2(yawCos, yawSin, -yawSin, yawCos), input);
        }

        inputLength = math.length(inputWorld);
        inputDirection = math.normalizesafe(inputWorld);
    }

    private void Accelerate(float2 dirH, float accel, float limit)
    {
        var proj = math.dot(velocity.xz, dirH);
        var dv = accel * Time.deltaTime;
        dv = math.min(dv, limit - proj);
        dv = math.max(dv, 0.0f);
        velocity.xz += dirH * dv;
    }

    private void AirMovement() => Accelerate(inputDirection, m_airAcceleration, m_airSpeed * inputLength);

    private void GroundMovement()
    {
        velocity.xz *= math.max(1.0f - m_friction * Time.deltaTime, 0.0f);
        Accelerate(inputDirection, m_groundAcceleration, m_groundMaxSpeed * inputLength);
    }

    private EnvironmentState GetEnvironmentState()
    {
        return Physics.SphereCast(transform.position - new Vector3(0, m_spherePos, 0), m_sphereRadius, Vector3.down, out groundHitInfo, .1f) 
            ? EnvironmentState.GROUND 
            : EnvironmentState.AIR;
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

    private void SetCameraRotate()
    {
        var mouse = new float2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        pitch -= mouse.y * m_mouseSensivity;
        yaw += mouse.x * m_mouseSensivity;

        pitch = math.clamp(pitch, -90.0f, 90.0f);
        yaw -= math.floor(yaw / 360.0f) * 360.0f;
        m_cameraTransform.localRotation = Quaternion.Euler(pitch, yaw, 0.0f);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GetEnvironmentState().Equals(EnvironmentState.GROUND))
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        CheckInputValues();
        SetMovement();
        SetCameraRotate();
        SetEnvironmentState();
    }

    private void SetEnvironmentState()
    {
        m_environmentState = GetEnvironmentState();
    }
}
