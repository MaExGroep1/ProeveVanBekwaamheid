using System;
using Grid;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ShuffleButton : MonoBehaviour
    {
        /// <summary>
        /// Adds the shuffle to the button
        /// </summary>
        private void Awake() =>
            GetComponent<Button>().onClick
                .AddListener(() => GridManager.Instance.Shuffle());
    }
}
