using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace RemoteLab.Characters
{
    [RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
    public class XRGrabNetworkInteractable : XRGrabInteractable
    {
        #region Private Properties

        private PhotonView photonView;

        #endregion

        #region Unity CallBacks

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        #endregion

        #region Private Methods

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            photonView.RequestOwnership();
            base.OnSelectEntering(args);
        }

        #endregion
    }
}