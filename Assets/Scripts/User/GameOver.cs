using UI.PopUp;
using UnityEngine;

namespace User
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private PopUp popUp;   // the game over prefab to instantiate

        /// <summary>
        /// Instantiates the game over pop up prefab
        /// </summary>
        private void CreatePopUp() => Instantiate(popUp);
    }
}
