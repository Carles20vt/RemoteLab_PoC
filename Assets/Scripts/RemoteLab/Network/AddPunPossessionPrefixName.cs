using Photon.Pun;
using UnityEngine;
namespace RemoteLab.Network
{
    [RequireComponent(typeof(PhotonView))]
    public class AddPunPossessionPrefixName : MonoBehaviour
    {
    #region Private Properties
    
        private PhotonView photonView;
    
    #endregion
    
        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        
            SetPlayerNameType();
        }
    
        private void SetPlayerNameType()
        {
            name = name + "_" + photonView.ViewID;
        
            if (!photonView.IsMine)
            {
                name += "_Other";
                return;
            }
        
            name += "_Mine";
        }
    }
}
