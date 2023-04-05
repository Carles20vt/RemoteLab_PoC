using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ReviewToDelete
{
    [RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
    public class MouseDragOnVR : MonoBehaviour
    {
        #region Private Properties
        
        private PhotonView photonView;
        private XRSocketInteractor otherXRSocketInteractor;
        private float distance;
        private bool isDragging;
        private Camera mainCamera;
        private bool isMainCameraNull;
        private new Rigidbody rigidbody;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            rigidbody = GetComponent<Rigidbody>();
            
            mainCamera = Camera.main;

            isMainCameraNull = mainCamera == null;
        }

        private void Update()
        {
            MouseDraggingMovement();
        }

        #endregion
        
        #region Events

        private void OnMouseDown()
        {
            photonView.RequestOwnership();
            photonView.RPC(nameof(DisableXrSocketInteractorForMouse), RpcTarget.All);
            
            isDragging = true;
            distance = Vector3.Distance(transform.position, mainCamera.transform.position);
        }
        
        private void OnMouseUp()
        {
            photonView.TransferOwnership(0);
            photonView.RPC(nameof(EnableXrSocketInteractorForMouse), RpcTarget.All);
            
            isDragging = false;
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
        private void EnableXrSocketInteractorForMouse()
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
        private void DisableXrSocketInteractorForMouse()
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

        private void MouseDraggingMovement()
        {
            if (!isDragging || isMainCameraNull)
            {
                return;
            }
            
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }

        #endregion
    }
}
