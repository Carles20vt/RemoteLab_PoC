using UnityEngine;

namespace DrinkMachine
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DrinkMachineButton : MonoBehaviour
    {
        #region Public Properties

        [SerializeField] private GameObject drinkItemPrefab;
        public delegate void OnButtonPressed(DrinkMachineButton pressedButton);
        public OnButtonPressed ONButtonPressed;

        public Color ButtonColor
        {
            set => meshRenderer.material.color = value;
        }

        public GameObject ItemPrefab => drinkItemPrefab;

        #endregion
        
        #region Private Properties

        private MeshRenderer meshRenderer;

        #endregion

        private void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = Color.green;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            ButtonPressed();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            ButtonPressed();
        }

        private void ButtonPressed()
        {
            ONButtonPressed?.Invoke(this);
        }
    }
}