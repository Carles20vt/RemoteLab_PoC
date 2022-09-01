using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
public class XRGrabNetworkInteractable : XRGrabInteractable
{

    #region Private Properties

    private PhotonView photonView;
    private Rigidbody interactableRigidbody;

    #endregion
    
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        interactableRigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        photonView.RequestOwnership();
        base.OnSelectEntering(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        interactableRigidbody.isKinematic = true;
        interactableRigidbody.detectCollisions = false;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        interactableRigidbody.isKinematic = false;
        interactableRigidbody.detectCollisions = true;
        base.OnSelectExited(args);
    }
}
