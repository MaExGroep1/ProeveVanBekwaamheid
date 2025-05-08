using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class HighScoreButton : MonoBehaviour
    {
        [SerializeField] private Button highScoreButton;        // the button to bring up your high score board
        [SerializeField] private GameObject highScoreCanvas;    // the button to start the game

        private void Awake() => highScoreButton.onClick.AddListener(ShowHighScore);
        
        /// <summary>
        /// Creates the high score menu
        /// </summary>
        private void ShowHighScore() => Instantiate(highScoreCanvas);
    }
}
