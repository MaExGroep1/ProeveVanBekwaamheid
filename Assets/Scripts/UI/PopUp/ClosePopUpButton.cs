using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp
{
    [RequireComponent(typeof(Button))]
    public class ClosePopUpButton : MonoBehaviour
    {

        /// <summary>
        /// Gets the button and adds a listener
        /// </summary>
        [Obsolete("Obsolete")]
        private void Awake() => 
            GetComponent<Button>()
                .onClick.AddListener(CloseAllPopUps);

        /// <summary>
        /// Closes all pop-ups
        /// </summary>
        [Obsolete("Obsolete")]
        private static void CloseAllPopUps()
        {
            var popUps = FindObjectsOfType<PopUp>();

            foreach (var popUp in popUps)
                if (popUp.HasOpened)
                    popUp.CloseMenu();
        }
    }
}