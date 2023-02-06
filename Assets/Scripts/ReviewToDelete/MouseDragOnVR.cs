using Photon.Pun;
using UnityEngine;

namespace RemoteLab.Characters
{
    public class MouseDragOnVR : MonoBehaviour
    {
        #region Private Properties
        
        private PhotonView photonView;
        private XRGrabNetworkInteractable xrGrabNetworkInteractable;
        private float distance;
        private bool isDragging;
        private Camera mainCamera;
        private bool isMainCameraNull;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            xrGrabNetworkInteractable = GetComponent<XRGrabNetworkInteractable>();
            
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
            return;
            /*
            if (xrGrabNetworkInteractable.IsDragging)
            {
                //return;
                
                xrGrabNetworkInteractable.ForceDrop();
            }
            
            isDragging = true;
            
            photonView.RequestOwnership();
            //xrGrabNetworkInteractable.enabled = false;
            
            distance = Vector3.Distance(transform.position, mainCamera.transform.position);
            */
        }
        
        private void OnMouseUp()
        {
            return;
            
            isDragging = false;
            //xrGrabNetworkInteractable.enabled = true;
        }

        #endregion
        
        #region Private Methods

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
