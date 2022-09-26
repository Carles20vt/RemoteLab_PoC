using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class AvatarMovementController : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private Transform otherAvatar;

    [SerializeField] private Transform[] localPlayerBodyParts;

    [SerializeField] private MapTransform head;
    [SerializeField] private MapTransform leftHand;
    [SerializeField] private MapTransform rightHand;
/*
    [SerializeField] private string mainCameraRigName;
    [SerializeField] private string leftHandControllerRigName;
    [SerializeField] private string rightHandControllerRigName;
*/
    [SerializeField] private float turnSmoothness;
    [SerializeField] private Transform ikHead;
    [SerializeField] private Vector3 headBodyOffset;

    #endregion

    #region Private Properties

    private PhotonView photonView;

    #endregion

    /*private void Awake()
    {
        ConfigureNetworkPlayerRig();
    }*/

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        name = name + "_" + photonView.ViewID;

        DisableMyNetworkPlayerObjects();
    }

    private void FixedUpdate()
    {
        MapMovement();
    }

    private void MapMovement()
    {
        otherAvatar.position = head.vrTarget.position;  
        //otherAvatar.rotation = head.vrTarget.rotation;
        /*var rotationPoint = new Vector3(0, head.vrTarget.rotation.y, 0);
        otherAvatar.rotation = otherAvatar.rotation * Quaternion.Euler(rotationPoint);
        */
        
        /*
        if (!photonView.IsMine)
        {
            //transform.position = ikHead.position + headBodyOffset;
            return;
        }
        */

        /*
        transform.forward = Vector3.Lerp(
            transform.forward,
            Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized,
            Time.deltaTime * turnSmoothness);
        */
        
        head.MapVRAvatar();
        leftHand.MapVRAvatar();
        rightHand.MapVRAvatar();
        
/*
        MapPosition(head.ikTarget, head.vrTarget);
        MapPosition(leftHand.ikTarget, leftHand.ikTarget);
        MapPosition(rightHand.ikTarget, rightHand.ikTarget);
*/
    }

    private void MapPosition(Transform targetTransform, Transform rigTransform)
    {
        if (rigTransform == null)
        {
            return;
        }
        
        targetTransform.position = rigTransform.position;
        targetTransform.rotation = rigTransform.rotation;
    }

    private void DisableMyNetworkPlayerObjects()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        foreach (var bodyPart in localPlayerBodyParts)
        {
            bodyPart.gameObject.SetActive(true);
        }

        foreach (var playerRendererComponent in GetComponentsInChildren<Renderer>())
        {
            playerRendererComponent.gameObject.SetActive(false);
            //playerRendererComponent.enabled = false;
        }
    }
/*
    private void ConfigureNetworkPlayerRig()
    {
        var xrOrigin = FindObjectOfType<XROrigin>();
        head.vrTarget = xrOrigin.transform.Find(mainCameraRigName);
        leftHand.vrTarget = xrOrigin.transform.Find(leftHandControllerRigName);
        rightHand.vrTarget = xrOrigin.transform.Find(rightHandControllerRigName);
    }
    */
}