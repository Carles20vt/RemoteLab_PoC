using TMPro;
using TreeislandStudio.Engine;
using UnityEngine;

namespace RemoteLab.Machinery.UI.Slider
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(UnityEngine.UI.Slider))]
    public class TextSlider : TreeislandBehaviour
    {
        #region Public Properties

        [SerializeField] private TMP_Text textToUpdate;
        [SerializeField] private string textDescription;
        [SerializeField] [Range(0f, 1f)] private float disabledTextAlpha = 0.5f;

        #endregion
        
        #region Private Properties

        private UnityEngine.UI.Slider slider;
        
        #endregion

        #region Unity Callbacks

        private void Start()
        {
            GatherDependencies();
        }

        private void GatherDependencies()
        {
            textToUpdate = textToUpdate == null ? GetComponentInChildren<TMP_Text>() : textToUpdate;
            slider = slider == null ? GetComponent<UnityEngine.UI.Slider>() : slider;
        }

        private void FixedUpdate()
        {
            UpdateTextWithSliderValue();
        }

        private void OnValidate()
        {
            GatherDependencies();
            UpdateTextWithSliderValue();
        }

        #endregion

        #region Private Methods

        private void UpdateTextWithSliderValue()
        {
            var parsedSliderValue = (int) slider.value;
            slider.value = parsedSliderValue;
            textToUpdate.text = parsedSliderValue + " " + textDescription;

            if (!slider.interactable)
            {
                textToUpdate.alpha = disabledTextAlpha;
                return;
            }

            textToUpdate.alpha = 1f;
        }

        #endregion
    }
}