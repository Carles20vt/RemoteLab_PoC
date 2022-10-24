using UnityEngine;

namespace RemoteLab.DrinkMachine
{
    [RequireComponent(typeof(MeshRenderer), typeof(Animator))]
    public class CoinControl : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private string coinGameObjectName = "Coin";
        
        public delegate void OnCoinInserted(bool hasCoins);
        public OnCoinInserted ONCoinInserted;
        
        #endregion
        
        #region Private Properties

        private MeshRenderer meshRenderer;
        private Animator animator;
        private static readonly int IsEnabled = Animator.StringToHash("IsEnabled");

        private int coins;

        private bool IsEnoughCredits => coins > 0;

        #endregion
        
        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            animator = GetComponent<Animator>();
            
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
                SetCoinControlEnabledStatus(false);
                return;
            }

            SetCoinControlEnabledStatus(true);
        }

        private void SetCoinControlEnabledStatus(bool status)
        {
            animator.SetBool(IsEnabled, status);
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

        public void EnableCoinControl()
        {
            meshRenderer.material.color = Color.green;
        }

        public void DisableCoinControl()
        {
            meshRenderer.material.color = Color.red;
        }
    }
}
