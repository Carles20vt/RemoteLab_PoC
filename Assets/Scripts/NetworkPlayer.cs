using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(PhotonView))]
public class NetworkPlayer : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    #endregion

    #region Private Properties

    private PhotonView photonView;

    #endregion
    
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        DisableMyNetworkPlayerObjects();
    }

    private void Update()
    {
        MapNetworkPlayerObjects();
    }

    private void MapNetworkPlayerObjects()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        MapPosition(head, XRNode.Head);
        MapPosition(leftHand, XRNode.LeftHand);
        MapPosition(rightHand, XRNode.RightHand);
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
    }

    private static void MapPosition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out var position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation);

        target.position = position;
        target.rotation = rotation;
    }
}
