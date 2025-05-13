using UnityEngine;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        /// <summary>
        /// Pauses the game
        /// </summary>
        private void Awake() => Time.timeScale = 0f;

        /// <summary>
        /// Unpauses the game
        /// </summary>
        private void OnDestroy() => Time.timeScale = 1f;
    }
}
