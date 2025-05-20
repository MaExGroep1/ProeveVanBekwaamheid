using System;
using Car;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FuelDisplay : MonoBehaviour
    {
        [SerializeField] private Slider fuelSlider;         // the fuel bar fill
        [SerializeField] private CarMovement carMovement;   // the car which fuel to display
        [SerializeField] private RectTransform fuelDisplay; // the fuel display rect transform
        [SerializeField] private RectMask2D fuelMask;       // the mask of the fuel bar

        private float _rectSize;                            // the height of the fuel display

        private void Awake() => _rectSize = fuelDisplay.rect.height;

        private void Update() => UpdateDisplay();

        /// <summary>
        /// Sets the fill of the display to the cars fill
        /// </summary>
        private void UpdateDisplay()
        {
            fuelSlider.value = carMovement.Fill;
            fuelMask.padding = new Vector4(0, 0, 0, _rectSize - _rectSize * carMovement.Fill);
        }
    }
}
