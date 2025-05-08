using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad;           // the scene to load when starting the game   
        [SerializeField] private Button playButton;         // the button to start the game

        private void Awake() => playButton.onClick.AddListener(StartGame);
        
        /// <summary>
        /// Loads the main game scene
        /// </summary>
        private void StartGame() => SceneManager.LoadScene(sceneToLoad);
        
    }
}
