using System;
using Grid;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameMenuManager : MonoBehaviour
    {
        [SerializeField] private Transform menuItem;            // the in game menu transform
        [SerializeField] private Transform menuRestPosition;    // the resting position of the game menu transform
        [SerializeField] private CanvasGroup background;        // the background of the in game menu
        [SerializeField] private float backgroundAlpha;         // the target alpha of the background
        [SerializeField] private float menuAppearTime;          // the time it takes for the menu to appear and disappear
        [Header("Buttons")]
        [SerializeField] private Button openMenuButton;         // the button to open the menu and pause the game
        [SerializeField] private Button closeMenuButton;        // the button to close the menu and unpause the game
        [SerializeField] private Button mainMenuButton;         // the button to close the game and go to the main menu
        [SerializeField] private Button restartButton;          // the button to go back to the start of the game
        [SerializeField] private Button highScoreButton;        // the button to bring up your high score board
        [SerializeField] private Button shuffleButton;          // the button to shuffle the grid

        private void Awake()
        {
            openMenuButton.onClick.AddListener(OpenMenu);
            closeMenuButton.onClick.AddListener(CloseMenu);
            mainMenuButton.onClick.AddListener(OpenMainMenu);
            restartButton.onClick.AddListener(RestartGame);
            highScoreButton.onClick.AddListener(ShowHighScore);
            shuffleButton.onClick.AddListener(ShuffleGrid);
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Makes the background fade in and the menu pop up
        /// Also pauses the game by setting the timescale to 0
        /// </summary>
        private void OpenMenu()
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;
            background.alpha = 0f;
            menuItem.position = menuRestPosition.position;

            LeanTween.alphaCanvas(background, backgroundAlpha, menuAppearTime/2)
                .setIgnoreTimeScale(true);
            LeanTween.moveLocalY(menuItem.gameObject, 0, menuAppearTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeOutBack);
        }
        
        /// <summary>
        /// Fades the background out and makes the menu go to its resting position
        /// Then unpauses the game
        /// </summary>
        private void CloseMenu()
        {
            gameObject.SetActive(true);
            background.alpha = backgroundAlpha;
            menuItem.localPosition = Vector3.one;

            LeanTween.alphaCanvas(background, 0, menuAppearTime)
                .setIgnoreTimeScale(true);
            LeanTween.moveY(menuItem.gameObject, menuRestPosition.position.y, menuAppearTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(UnPauseGame);
        }
        
        private void OpenMainMenu()
        {
            throw new NotImplementedException();
        }
        
        private void RestartGame()
        {
            throw new NotImplementedException();
        }

        private void ShowHighScore()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Tells the grid to shuffle itself 
        /// </summary>
        private static void ShuffleGrid() => GridManager.Instance.Shuffle();

        /// <summary>
        /// Sets the timescale to 1 and hides the menu
        /// </summary>
        private void UnPauseGame()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
