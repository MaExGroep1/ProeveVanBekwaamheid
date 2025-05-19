using System;
using TMPro;
using UnityEngine;
using User;

namespace UI
{
    public class DistanceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI display;

        private void Update()
        {
            display.text = $"{Mathf.RoundToInt(UserData.Instance.DistanceScore).ToString("N0").Replace(',', '.')} cm";
        }
    }
}
