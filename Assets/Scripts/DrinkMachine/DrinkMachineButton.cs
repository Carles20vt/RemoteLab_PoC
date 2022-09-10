using UnityEngine;

namespace DrinkMachine
{
    [RequireComponent(typeof(MeshRenderer), typeof(Animator))]
    public class DrinkMachineButton : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private GameObject drinkItemPrefab;
        public delegate void OnButtonPressed(DrinkMachineButton pressedButton);
        public OnButtonPressed ONButtonPressed;

        public GameObject ItemPrefab => drinkItemPrefab;
        public bool IsButtonPressedByNetworkPlayer { get; private set; }

        #endregion
        
        #region Private Properties

        private MeshRenderer meshRenderer;
        private Animator animator;
        private static readonly int IsEnabled = Animator.StringToHash("IsEnabled");

        #endregion

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            animator = GetComponent<Animator>();

            SetButtonEnabledStatus(true);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                IsButtonPressedByNetworkPlayer = false;
                ButtonPressed();
                return;
            }

            if (!other.gameObject.CompareTag("NetworkPlayer"))
            {
                return;
            }
            
            IsButtonPressedByNetworkPlayer = true;
            ButtonPressed();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IsButtonPressedByNetworkPlayer = false;
                ButtonPressed();
                return;
            }

            if (!other.CompareTag("NetworkPlayer"))
            {
                return;
            }
            
            IsButtonPressedByNetworkPlayer = true;
            ButtonPressed();
        }

        private void ButtonPressed()
        {
            ONButtonPressed?.Invoke(this);
        }

        public void SetButtonEnabledStatus(bool status)
        {
            animator.SetBool(IsEnabled, status);
        }

        public void EnableButton()
        {
            meshRenderer.material.color = Color.green;
        }

        public void DisableButton()
        {
            meshRenderer.material.color = Color.red;
        }
    }
}