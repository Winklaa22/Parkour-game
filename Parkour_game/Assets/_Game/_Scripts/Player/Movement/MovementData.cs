using UnityEngine;

[System.Serializable]
public class MovementData
{
    [Header("Air")]
    public float AirSpeed = 1.0f;
    public float AirAcceleration = 20.0f;
    public float JumpVelocity = 5.0f;

    [Header("Ground")]
    public float DetectGroundSphereRadius = .25f;
    public float DetectGroundSpherePos = .7f;
    [HideInInspector] public float GroundAcceleration = 0.0f;
    [HideInInspector] public float GroundMaxSpeed = 0.0f;

    [Header("Walking")]
    public float WalkingAcceleration = 7.0f;
    public float WalkingMaxSpeed = 7.0f;
    
    [Header("Running")]
    public float RunningAcceleration = 20.0f;
    public float RunningMaxSpeed = 14.0f;
}
