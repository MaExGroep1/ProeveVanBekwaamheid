using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp
{
    [RequireComponent(typeof(Button))]
    public class CreatePopUpButton : MonoBehaviour
    {
        [SerializeField] private PopUp popUp;   // the pop-up to instantiate

        /// <summary>
        /// Gets the button and adds a listener
        /// </summary>
        private void Awake() => 
            GetComponent<Button>()
                .onClick.AddListener(CreatePopUp);

        /// <summary>
        /// Instantiates the pop-up
        /// </summary>
        private void CreatePopUp() => Instantiate(popUp);
    }
}
