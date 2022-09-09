using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace DrinkMachine
{
    public class DrinkDispenser : MonoBehaviourPunCallbacks
    {
        #region Public Properties

        [SerializeField] private DrinkMachineButton[] drinkMachineButtons;
        [SerializeField] private Transform itemDispatcher;
        [SerializeField] [Range(0, 10)] private float timeToServeDrink = 3f;

        [SerializeField] private CoinControl coinControl;

        #endregion

        #region Private Properties

        
        private List<GameObject> dispatchedDrinkItems;

        private bool isBusy;
        private bool isEnoughCredits;

        #endregion

        public override void OnEnable()
        {
            ConfigureCoinControl();
            
            ConfigureButtons();

            base.OnEnable();
        }

        public override void OnDisable()
        {
            RemoveCoinControl();
            
            RemoveButtons();

            base.OnDisable();
        }

        private void Start()
        {
            if (coinControl == null)
            {
                coinControl = GetComponentInChildren<CoinControl>();
            }
            
            dispatchedDrinkItems = new List<GameObject>();
        }

        private void ConfigureCoinControl()
        {
            coinControl.ONCoinInserted += CoinInserted;
        }

        private void ConfigureButtons()
        {
            foreach (var button in drinkMachineButtons)
            {
                button.ONButtonPressed += ButtonPressed;
            }
        }

        private void RemoveCoinControl()
        {
            coinControl.ONCoinInserted -= CoinInserted;
        }

        private void RemoveButtons()
        {
            foreach (var button in drinkMachineButtons)
            {
                button.ONButtonPressed -= ButtonPressed;
            }
        }

        private void CoinInserted(bool enoughCoins)
        {
            isEnoughCredits = enoughCoins;
        }

        private void ButtonPressed(DrinkMachineButton button)
        {
            if (isBusy || !isEnoughCredits)
            {
                return;
            }

            StartCoroutine(PrepareDrink(button));
        }

        private IEnumerator PrepareDrink(DrinkMachineButton button)
        {
            isBusy = true;

            button.ButtonColor = Color.red;
            Debug.Log($"Button {button.name} pressed.");

            yield return new WaitForSeconds(timeToServeDrink);

            try
            {
                var spawnedDrinkPrefab =
                    PhotonNetwork.Instantiate(
                        button.ItemPrefab.name,
                        itemDispatcher.position,
                        itemDispatcher.rotation);

                spawnedDrinkPrefab.name = button.ItemPrefab.name + "_" + dispatchedDrinkItems.Count;

                dispatchedDrinkItems.Add(spawnedDrinkPrefab);

                Debug.Log($"Dispatched {spawnedDrinkPrefab.name}");
                
                coinControl.CoinSpent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                button.ButtonColor = Color.green;
                isBusy = false;
            }
        }

        public override void OnLeftRoom()
        {
            foreach (var drinkItem in dispatchedDrinkItems)
            {
                Debug.Log($"The item {drinkItem.name} will be destroyed.");
                PhotonNetwork.Destroy(drinkItem);
            }

            base.OnLeftRoom();
        }
    }
}