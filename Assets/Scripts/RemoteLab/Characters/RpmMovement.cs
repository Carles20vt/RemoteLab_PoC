using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
namespace RemoteLab.Characters
{
    [RequireComponent(typeof(CharacterController),
        typeof(PhotonView))]
    public class RpmMovement : MonoBehaviour
    {
    #region Public Properties

        [SerializeField][Range(0f, 10f)] private float movementSpeed = 3f;

    #endregion
    
    #region Private Properties
        private PhotonView photonView;
        private CharacterController characterController;

        private Vector3 moveVector;

    #endregion
    
    #region Unity Callback

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            photonView = GetComponent<PhotonView>();
        }

        private void Update()
        {
            characterController.Move(moveVector * Time.deltaTime);
        }

        public void GetMovementInput(InputAction.CallbackContext callbackContext)
        {
            /*
        if (!photonView.IsMine)
        {
            return;
        }
        */
        
            moveVector = callbackContext.ReadValue<Vector3>() * movementSpeed;
        }

    #endregion
    }
}
