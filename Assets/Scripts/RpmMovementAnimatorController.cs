using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RpmMovementAnimatorController : MonoBehaviour
{
    #region Public Properties
    
    [Space(10)]
    [Header("Network Player Objects")]
    [SerializeField] private Transform networkAvatarTransform;

    [Space(10)] 
    [Header("Movement Adjustments")]
    [SerializeField] [Range(0f, 100f)] private float walkMovementVelocity = 1f;
    [SerializeField] [Range(0f, 100f)] private float turnMovementVelocity = 20f;
    
    #endregion

    #region Private Properties

    private Animator animator;
    private static readonly int HmdRotation = Animator.StringToHash("hmdRotation");
    private static readonly int Magnitude = Animator.StringToHash("magnitude");

    private float initialFootRotation;
    private Vector3 lastAvatarPosition;
    private float lastAvatarRotation;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        animator = GetComponent<Animator>();
        networkAvatarTransform = networkAvatarTransform == null ? transform : networkAvatarTransform;

        lastAvatarPosition = transform.position;
        lastAvatarRotation = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        UpdateAvatarMovementFromHmd();
        UpdateAvatarRotationFromHmd();
    }
    
    #endregion

    #region Private Methods
    
    private void UpdateAvatarMovementFromHmd()
    {
        var direction = (networkAvatarTransform.position - lastAvatarPosition).normalized;
        var moveVector = new Vector3(direction.x, 0, direction.z);
        var magnitude = Math.Clamp(Vector3.Magnitude(moveVector) * walkMovementVelocity, 0, 1f);

        lastAvatarPosition = transform.position;
        
        //Set the animator
        animator.SetFloat(Magnitude, magnitude);
        //Debug.Log($"Moving player animator with magnitude: {magnitude}.");
    }
    
    private void UpdateAvatarRotationFromHmd()
    {
        var currentAvatarRotation = transform.rotation.eulerAngles.y;
        var avatarRotationDifference = lastAvatarRotation - currentAvatarRotation;

        var deltaAngle =  Math.Abs(avatarRotationDifference) < 0.01f ? 0 : avatarRotationDifference * turnMovementVelocity;

        animator.SetFloat(HmdRotation, deltaAngle );
        
        lastAvatarRotation = currentAvatarRotation;
        
        //Debug.Log($"HMD Rotation: {deltaAngle}");
    }
    
    #endregion
}
