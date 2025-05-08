using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp
{
    [RequireComponent(typeof(Button))]
    public class CreatePopUpButton : MonoBehaviour
    {
        [SerializeField] private PopUp popUp;

        private void Awake() => 
            GetComponent<Button>()
                .onClick.AddListener(CreatePopUp);

        private void CreatePopUp() => Instantiate(popUp);
    }
}
