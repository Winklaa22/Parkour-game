using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class MovementData
{
    [Header("Air")]
    public float AirSpeed = 1.0f;
    public float AirAcceleration = 20.0f;
    public float JumpVelocity = 5.0f;

    [Header("Celling")]
    public float DetectCellingSpherePos = .25f;
    public float DetectCellingSphereRadius = .7f;
    
    
    [Header("Ground")]
    public float DetectGroundSphereRadius = .25f;
    public float DetectGroundSpherePos = .7f;
    public float Friction = 3.0f;

    [Header("Walking")]
    public float WalkingAcceleration = 7.0f;
    public float WalkingMaxSpeed = 7.0f;
    
    [Header("Running")]
    public float RunningAcceleration = 20.0f;
    public float RunningMaxSpeed = 14.0f;
    

    [Header("Sliding")] 
    public float SlidingTime;
    public float SlidingColliderSize = 0.7237288f;
    public float SlidingColliderPos = -0.2763f;
    public float SlidingAnimationDuration = .25f;
    public Ease SlidingAnimationEase;
    
    [Header("Crouching")]
    public float CrouchColliderSize = 0.7237288f;
    public float CrouchColliderPos = -0.2763f;
    public float CrouchAnimationDuration = .25f;
    public float CrouchCameraSize;
    public Ease CrouchAnimationEase;
}
