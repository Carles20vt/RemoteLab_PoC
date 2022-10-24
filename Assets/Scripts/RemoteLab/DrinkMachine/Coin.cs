using Photon.Pun;
using UnityEngine;

namespace RemoteLab.DrinkMachine
{
    [RequireComponent(typeof(PhotonView))]
    public class Coin : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private string coinControlGameObjectName = "CoinControl";

        #endregion
        
        #region Private Properties
        
        private PhotonView photonView;
        
        #endregion
        
        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.name.Contains(coinControlGameObjectName))
            {
                return;
            }

            DestroyMe();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains(coinControlGameObjectName))
            {
                return;
            }

            DestroyMe();
        }

        private void DestroyMe()
        {
            photonView.RequestOwnership();
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
