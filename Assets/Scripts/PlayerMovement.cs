using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator),
    typeof(CharacterController),
    typeof(PhotonView))]
public class PlayerMovement : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private Transform mainCameraTransform;

    #endregion
    
    #region Private Properties

    private Animator animator;
    private CharacterController characterController;
    private PhotonView photonView;
    
    private static readonly int Magnitude = Animator.StringToHash("magnitude");

    #endregion
    
    #region Unity Callback

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        DoMovement();
    }

    private void OnMove(InputValue movementValue)
    {
        MoveCharacter();
    }

    private void OnTurn(InputValue movementValue)
    {
        //var turnVector = movementValue.Get<Vector2>();
        TurnCharacter();
    }

    #endregion

    private void DoMovement()
    {
        
    }

    private void MoveCharacter()
    {
        var direction = (transform.position - mainCameraTransform.position).normalized;
        var moveVector = new Vector3(direction.x, 0, direction.z);
        var magnitude = Vector3.Magnitude(moveVector);

        animator.SetFloat(Magnitude, magnitude);

        characterController.Move(moveVector);
    }
    
    private void TurnCharacter()
    {
        var q = Quaternion.FromToRotation(-transform.parent.forward, mainCameraTransform.forward);
        transform.rotation = q * transform.rotation;
        /*
        float angle = Vector2.Angle(rotation,position2);
        transform.Rotate(angle);
        */
    }
}
