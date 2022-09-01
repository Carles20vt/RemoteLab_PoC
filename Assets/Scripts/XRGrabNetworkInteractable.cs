using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhotonView))]
public class XRGrabNetworkInteractable : XRGrabInteractable
{

    #region Private Properties

    private PhotonView photonView;

    #endregion
    
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        photonView.RequestOwnership();
        base.OnSelectEntering(args);
    }
}
