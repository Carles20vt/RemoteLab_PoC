using Photon.Pun;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class RpmSetAvatarParentOfXriOrigin : MonoBehaviour
{
    #region Public Properties

    [SerializeField] private string otherPlayersContainerName = "OtherPlayers";

    #endregion
    
    #region Private Properties

    private XROrigin xrOrigin;
    private PhotonView photonView;
    private Transform otherPlayersContainer;

    #endregion

    #region Unity CallBacks
    
    private void Start()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        photonView = GetComponent<PhotonView>();
        
        otherPlayersContainer = GameObject.Find(otherPlayersContainerName).transform;
        otherPlayersContainer = otherPlayersContainer == null ? transform.parent : otherPlayersContainer;

        SetParent();
    }

    #endregion

    #region Private Methods

    private void SetParent()
    {
        if (xrOrigin == null)
        {
            return;
        }
        
        if (photonView.IsMine)
        {
            transform.SetParent(xrOrigin.transform);
            return;
        }
        
        transform.SetParent(otherPlayersContainer);
    }

    #endregion
}
