using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    #region Public Properties

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

        name = name + "_" + photonView.ViewID;

        DisableMyNetworkPlayerObjects();
    }

    private void ConfigureNetworkPlayerRig()
    {
        var xrOrigin = FindObjectOfType<XROrigin>();
        headRig = xrOrigin.transform.Find(mainCameraRigName);
        leftHandRig = xrOrigin.transform.Find(leftHandControllerRigName);
        rightHandRig = xrOrigin.transform.Find(rightHandControllerRigName);
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
        || !handAnimator.isActiveAndEnabled)
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

    private void DisableMyNetworkPlayerObjects()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        head.gameObject.SetActive(false);
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);

        foreach (var playerRendererComponent in GetComponentsInChildren<Renderer>())
        {
            playerRendererComponent.enabled = false;
        }
    }    
}
