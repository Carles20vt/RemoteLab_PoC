using Photon.Pun;
using TreeislandStudio.Engine.Environment;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

namespace RemoteLab.Characters
{
    [RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
    public class XRGrabNetworkInteractable : XRGrabInteractable
    {
        #region Private Properties

        private PhotonView photonView;
        private XRSocketInteractor otherXRSocketInteractor;
        private new Rigidbody rigidbody;
        
        #endregion
        
        #region Dependencies
        
        /// <summary>
        /// Determines if PUN is enabled or not.
        /// </summary>
        private bool isPunEnabled;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="environmentSetUp"></param>
        [Inject]
        private void Initialize(IEnvironmentSetUp environmentSetUp)
        {
            isPunEnabled = environmentSetUp.GameConfiguration.IsMultiPlayerEnabled;
        }

        #endregion

        #region Unity CallBacks

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            rigidbody = GetComponent<Rigidbody>();

            PhotonNetwork.OfflineMode = !isPunEnabled;
        }

        #endregion

        #region Events

        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            if (isPunEnabled)
            {
                photonView.RequestOwnership();
                photonView.RPC(nameof(DisableXrSocketInteractor), RpcTarget.All);
            }
            else
            {
                DisableXrSocketInteractor();
            }

            base.OnSelectEntering(args);
        }
        
        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            if (isPunEnabled)
            {
                photonView.TransferOwnership(0);
                photonView.RPC(nameof(EnableXrSocketInteractor), RpcTarget.All);
            }
            else
            {
                EnableXrSocketInteractor();
            }
            
            base.OnSelectExiting(args);
        }

        private void OnCollisionEnter(Collision collision)
        {
            SetXRSocketInteractor(collision.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            SetXRSocketInteractor(other);
        }

        private void SetXRSocketInteractor(Component component)
        {
            var newXRSocketInteractor = component.GetComponent<XRSocketInteractor>();
            
            if (newXRSocketInteractor == null)
            {
                return;
            }
            
            if (otherXRSocketInteractor != null)
            {
                otherXRSocketInteractor.socketActive = true;
            }
                
            otherXRSocketInteractor = newXRSocketInteractor;
        }

        #endregion
        
        #region Private Methods
        
        [PunRPC]
        private void EnableXrSocketInteractor()
        {
            if (otherXRSocketInteractor == null ||
                photonView.IsMine)
            {
                return;
            }
            
            otherXRSocketInteractor.socketActive = true;
            
            Debug.Log($"Player {photonView.name} is enabling " +
                      $"socket interactor {otherXRSocketInteractor.name} for the " +
                      $"GameObject {otherXRSocketInteractor.gameObject.name}");
            
            rigidbody.isKinematic = true;
                
            Debug.Log($"Player {photonView.name} is enabling the Kinematic " +
                      $"for the GameObject {rigidbody.gameObject.name}. The current " +
                      $"owner is {photonView.Owner}");
        }
        
        [PunRPC]
        private void DisableXrSocketInteractor()
        {
            if (otherXRSocketInteractor == null ||
                photonView.IsMine)
            {
                return;
            }
            
            otherXRSocketInteractor.socketActive = false;
            
            Debug.Log($"Player {photonView.name} is disabling " +
                      $"socket interactor {otherXRSocketInteractor.name} for the " +
                      $"GameObject {otherXRSocketInteractor.gameObject.name}");
            
            rigidbody.isKinematic = false;
                
            Debug.Log($"Player {photonView.name} is disabling the Kinematic " +
                      $"for the GameObject {rigidbody.gameObject.name}. The current " +
                      $"owner is {photonView.Owner}");
        }
        
        #endregion
    }
}