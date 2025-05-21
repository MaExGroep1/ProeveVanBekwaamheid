using System;
using TMPro;
using UnityEngine;
using User;

namespace UI
{
    public class DistanceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI display;   // the text to update

        /// <summary>
        /// Sets the display to the distance the car has driven
        /// </summary>
        private void Update() =>
            display.text = $"{Mathf.RoundToInt(UserData.Instance.DistanceScore).ToString("N0").Replace(',', '.')} cm";
    }
}
