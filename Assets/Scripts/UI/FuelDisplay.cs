using System;
using Blocks;
using Car;
using Grid;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class FuelDisplay : MonoBehaviour
    {
        [SerializeField] private Slider fuelSlider;         // the fuel bar fill
        [SerializeField] private CarMovement carMovement;   // the car which fuel to display
        [SerializeField] private Image fuelMask;            // the mask of the fuel bar
        [Header("Fuel Display")]
        [SerializeField] private SquishAndStretch warp;     // the warp of the fuel bar

        /// <summary>
        /// Starts listening to the match 3
        /// </summary>
        private void Awake()
        {
            GridManager.Instance.ListenToOnFirstMatch(StartUpdate);
            GridManager.Instance.ListenToOnMatch((_, _) => warp.WarpObject());
            enabled = false;
        }
        
        private void Update() => UpdateDisplay();
        
        /// <summary>
        /// Sets the script enabled
        /// </summary>
        private void StartUpdate() => enabled = true;
        
        /// <summary>
        /// Sets the fill of the display to the cars fill
        /// </summary>
        private void UpdateDisplay()
        {
            fuelSlider.value = carMovement.Fill;
            fuelMask.fillAmount = carMovement.Fill;
        }
    }
}
