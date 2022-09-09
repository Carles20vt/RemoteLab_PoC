using UnityEngine;

namespace DrinkMachine
{
    [RequireComponent(typeof(MeshRenderer))]
    public class CoinControl : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private string coinGameObjectName = "Coin";
        
        public delegate void OnCoinInserted(bool hasCoins);
        public OnCoinInserted ONCoinInserted;
        
        #endregion
        
        #region Private Properties

        private MeshRenderer meshRenderer;

        private int coins;

        private bool IsEnoughCredits => coins > 0;

        #endregion
        
        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            
            AddCoin(0);
        }

        private void AddCoin(int coinAmount)
        {
            coins += coinAmount;

            CheckCoinStatus();
        }
        
        private void UseCoin(int coinAmount)
        {
            if (coinAmount > 0)
            {
                coins -= coinAmount;
            }

            CheckCoinStatus();
        }

        private void CheckCoinStatus()
        {
            if (coins <= 0)
            {
                meshRenderer.material.color = Color.red;
                return;
            }
            
            meshRenderer.material.color = Color.green;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.name.Contains(coinGameObjectName))
            {
                return;
            }

            CoinInserted();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.name.Contains(coinGameObjectName))
            {
                return;
            }

            CoinInserted();
        }

        private void CoinInserted()
        {
            AddCoin(1);
            ONCoinInserted?.Invoke(IsEnoughCredits);
        }
        
        public void CoinSpent()
        {
            UseCoin(1);
            ONCoinInserted?.Invoke(IsEnoughCredits);
        }
    }
}
