using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class AvatarController : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private Transform otherAvatar;

    [SerializeField] private Transform[] localPlayerBodyParts;

    [SerializeField] private MapTransform head;
    [SerializeField] private MapTransform leftHand;
    [SerializeField] private MapTransform rightHand;
    [SerializeField] private float turnSmoothness;
    [SerializeField] private Transform ikHead;
    [SerializeField] private Vector3 headBodyOffset;

    #endregion

    #region Private Properties

    private PhotonView photonView;

    #endregion

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
        var newPosition = new Vector3(head.vrTarget.position.x, 0, head.vrTarget.position.z);
        otherAvatar.position = newPosition; //head.vrTarget.position;  
/*       
        head.MapVRAvatar();
        leftHand.MapVRAvatar();
        rightHand.MapVRAvatar();
*/
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
            //playerRendererComponent.enabled = false;
        }
    }
}