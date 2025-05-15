using Car;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FuelDisplay : MonoBehaviour
    {
        [SerializeField] private Slider fuelSlider;         // the fuel bar fill
        [SerializeField] private CarMovement carMovement;   // the car which fuel to display
        
        /// <summary>
        /// Sets the fill of the display to the cars fill
        /// </summary>
        void Update() =>
            fuelSlider.value = carMovement.Fill;
    }
}
