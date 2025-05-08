using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUp
{
    public class PopUp : MonoBehaviour
    {
        [Header("Animations")]
        [SerializeField] private Transform box;                 // the in game menu transform
        [SerializeField] private Transform restPosition;        // the resting position of the game menu transform
        [SerializeField] private CanvasGroup background;        // the background of the in game menu
        [SerializeField] private float appearTime;              // the time it takes for the menu to appear and disappear
        
        [Header("Buttons")]
        [SerializeField] private Button closePopUpButton;       // the button to close the menu and unpause the game
        
        private float _backgroundAlpha;                         // the target alpha of the background
        
        private void Awake() => OpenMenu();
        
        /// <summary>
        /// Makes the background fade in and the menu pop up
        /// Also pauses the game by setting the timescale to 0
        /// </summary>
        private void OpenMenu()
        {
            closePopUpButton.onClick.AddListener(CloseMenu);

            _backgroundAlpha = background.alpha;
            background.alpha = 0f;
            box.position = restPosition.position;

            LeanTween.alphaCanvas(background, _backgroundAlpha, appearTime/2)
                .setIgnoreTimeScale(true);
            LeanTween.moveLocalY(box.gameObject, 0, appearTime)
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
            background.alpha = _backgroundAlpha;
            box.localPosition = Vector3.one;

            LeanTween.alphaCanvas(background, 0, appearTime)
                .setIgnoreTimeScale(true);
            LeanTween.moveY(box.gameObject, restPosition.position.y, appearTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(DestroyPopUp);
        }
        
        /// <summary>
        /// Destroys the pop-up canvas
        /// </summary>
        private void DestroyPopUp() => Destroy(gameObject);
    }
}
