using Photon.Pun;
using UnityEngine;

namespace DrinkMachine
{
    public class Coin : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private string coinControlGameObjectName = "CoinControl";

        #endregion
        
        #region Private Properties

        #endregion
        
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
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
