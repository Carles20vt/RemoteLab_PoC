using System;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMovementController : MonoBehaviour
{
    #region Public Properties
    
    [Header("Local Player Objects")]
    [SerializeField] private Transform[] localPlayerBodyParts;
    
    [Space(10)]
    [Header("Network Player Objects")]
    [SerializeField] private Transform networkAvatarTransform;
    [SerializeField] private Transform footTransform;
    [SerializeField] private Transform mainCameraTransform;

    [Space(10)] 
    [Header("External Dependencies")] 
    [SerializeField] private NetworkPlayer networkPlayer;
    
    #endregion

    #region Private Properties
    
    private PhotonView photonView;
    private float cameraRotation;
    private float footRotation;

    private Animator animator;
    private static readonly int HmdRotation = Animator.StringToHash("hmdRotation");
    private static readonly int Magnitude = Animator.StringToHash("magnitude");

    private float initialFootRotation;
    private float lastMovementMagnitude;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        networkAvatarTransform = networkAvatarTransform == null ? transform : networkAvatarTransform;
        networkPlayer = networkPlayer == null ? FindObjectOfType<NetworkPlayer>() : networkPlayer;

        lastMovementMagnitude = 0f;
        
        Setup();

        DisableMyNetworkPlayerObjects();
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            return;
        }
        
        UpdateAvatarMovementFromHmd();
        UpdateAvatarRotationFromHmd();
    }
    
    #endregion

    #region Private Methods

    private void Setup()
    {
        initialFootRotation = footTransform.rotation.eulerAngles.y;
        name = name + "_" + photonView.ViewID;
    }
    
    private void DisableMyNetworkPlayerObjects()
    {
        if (!photonView.IsMine)
        {
            name += "_Other";
            return;
        }
        
        name += "_Mine";

        foreach (var bodyPart in localPlayerBodyParts)
        {
            bodyPart.gameObject.SetActive(true);
        }

        foreach (var playerRendererComponent in GetComponentsInChildren<Renderer>())
        {
            playerRendererComponent.gameObject.SetActive(false);
        }
    }
    
    private void UpdateAvatarMovementFromHmd()
    {
        var cameraPosition = mainCameraTransform.position;
/*
        var direction = (networkAvatarTransform.position - cameraPosition).normalized;
        var moveVector = new Vector3(direction.x, 0, direction.z);
        var magnitude = Math.Clamp(Vector3.Magnitude(moveVector) * 100, 0, 1f);
*/
        var magnitude = Math.Clamp(networkPlayer.GetPlayerMovementMagnitude() * Time.deltaTime * 100, 0, 1f);
        
        if (Math.Abs(lastMovementMagnitude - magnitude) < float.Epsilon)
        {
            return;
        }

        lastMovementMagnitude = magnitude;
        
        //Set the animator
        //animator.SetFloat(Magnitude, magnitude);
        Debug.Log($"Moving player animator with magnitude: {magnitude}.");
        
        //Set the movement
        networkAvatarTransform.position = new Vector3(cameraPosition.x, 0, cameraPosition.z);
    }
    
    private void UpdateAvatarRotationFromHmd()
    {
        cameraRotation = mainCameraTransform.rotation.eulerAngles.y;
        
        //PlayRotateNetworkPlayerAnimation();
        RotateNetworkPlayer();
    }

    private void PlayRotateNetworkPlayerAnimation()
    {
        footRotation = initialFootRotation - footTransform.rotation.eulerAngles.y;

        var deltaAngle = Mathf.DeltaAngle(cameraRotation, footRotation);

        animator.SetFloat(HmdRotation, deltaAngle);
        
        Debug.Log($"HMD Rotation: {deltaAngle}");
        Debug.Log($"Camera Rotation: {mainCameraTransform.rotation.eulerAngles.y} VS. Foot Rotation: {footTransform.rotation.eulerAngles.y}");
    }

    private void RotateNetworkPlayer()
    {
        if (Math.Abs(cameraRotation - networkAvatarTransform.rotation.eulerAngles.y) < 0.00001f)
        {
            return;
        }

        var avatarRotation = networkAvatarTransform.rotation.eulerAngles;
        networkAvatarTransform.eulerAngles = new Vector3(avatarRotation.x, cameraRotation, avatarRotation.z);

        Debug.Log($"Camera Y Rotation: {cameraRotation}, Network Y Rotation: {networkAvatarTransform.rotation.eulerAngles.y}");
        
    }

    #endregion
}
