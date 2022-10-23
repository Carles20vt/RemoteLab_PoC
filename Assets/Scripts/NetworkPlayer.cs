using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private Transform rpmAvatar;

    [SerializeField] private Transform body;
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [SerializeField] private string mainCameraRigName;
    [SerializeField] private string leftHandControllerRigName;
    [SerializeField] private string rightHandControllerRigName;
        
    [SerializeField] private Animator leftHandAnimator;
    [SerializeField] private Animator rightHandAnimator;

    #endregion

    #region Private Properties

    private PhotonView photonView;

    private Transform bodyRig;
    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;
    
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    private static readonly int Grip = Animator.StringToHash("Grip");

    #endregion

    private void Awake()
    {
        ConfigureNetworkPlayerRig();
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        body = body == null ? transform : body;

        SetupVrRig();
    }
    
    private void SetupVrRig()
    {
        if (!photonView.IsMine)
        {
            rpmAvatar.gameObject.SetActive(true);
            return;
        }

        rpmAvatar.gameObject.SetActive(false);
        
        leftHandAnimator.gameObject.SetActive(true);
        rightHandAnimator.gameObject.SetActive(true);
    }

    private void ConfigureNetworkPlayerRig()
    {
        var xrOriginTransform = FindObjectOfType<XROrigin>().transform;
        //bodyRig = xrOriginTransform;
        // TODO: Check bodyRig
        bodyRig = xrOriginTransform.Find(mainCameraRigName);
        headRig = xrOriginTransform.Find(mainCameraRigName);
        leftHandRig = xrOriginTransform.Find(leftHandControllerRigName);
        rightHandRig = xrOriginTransform.Find(rightHandControllerRigName);
    }

    private void Update()
    {
        MapNetworkPlayerObjects();

        UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
        UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
    }

    private void MapNetworkPlayerObjects()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        MapPosition(body, bodyRig);
        MapPosition(head, headRig);
        MapPosition(leftHand, leftHandRig);
        MapPosition(rightHand, rightHandRig);
    }

    private static void MapPosition(Transform targetTransform, Transform rigTransform)
    {
        if (rigTransform == null)
        {
            return;
        }
        
        targetTransform.position = rigTransform.position;
        targetTransform.rotation = rigTransform.rotation;
    }
    
    private void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (!photonView.IsMine 
        || handAnimator == null 
        || !handAnimator.isActiveAndEnabled
        || !handAnimator.gameObject.activeSelf)
        {
            return;
        }
        
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            handAnimator.SetFloat(Trigger, triggerValue);
        }
        else
        {
            handAnimator.SetFloat(Trigger, 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue))
        {
            handAnimator.SetFloat(Grip, gripValue);
        }
        else
        {
            handAnimator.SetFloat(Grip, 0);
        }
    }
}
