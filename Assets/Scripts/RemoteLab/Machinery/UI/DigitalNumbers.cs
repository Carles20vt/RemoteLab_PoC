using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.RemoteLab.Machinery.UI
{
    public class DigitalNumbers : MonoBehaviour
    {
        [SerializeField] private int maxNumber;
        [SerializeField] private TMP_Text numberText;

        private int number, maxDigits;

        private void Start()
        {
            if (!int.TryParse(numberText.text, out number))
                number = 0;
            maxDigits = maxNumber.ToString().Length;
        }

        public void Add(int quantity)
        {
            number = Mathf.Clamp(number+quantity, 0, maxNumber);
            WriteText();
        }

        public void Substract(int quantity)
        {
            number = Mathf.Clamp(number-quantity, 0, maxNumber);
            WriteText();
        }

        private void WriteText()
        {
            numberText.text = number.ToString().PadLeft(maxDigits, '0');
        }
    }
}