using UnityEngine;
using UnityEngine.UI;

namespace RemoteLab.Machinery.UI
{
    public class DisplayImages : MonoBehaviour
    {
        [SerializeField] private Image[] images;
        [SerializeField] private float displayTime = 3;

        private int actualImgPos;

        private void OnEnable()
        {
            ResetDisplay();
            InvokeRepeating("ChangeImage", displayTime, displayTime);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void ResetDisplay()
        {
            actualImgPos = 0;
            foreach (Image image in images)
                image.gameObject.SetActive(false);
            images[actualImgPos].gameObject.SetActive(true);
        }

        private void ChangeImage()
        {
            images[actualImgPos].gameObject.SetActive(false);
            actualImgPos = (actualImgPos + 1) % images.Length;
            images[actualImgPos].gameObject.SetActive(true);
        }

    }
}