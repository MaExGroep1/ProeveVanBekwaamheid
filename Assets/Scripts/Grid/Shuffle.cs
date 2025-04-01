using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    public class Shuffle : MonoBehaviour
    {
        [SerializeField] private Button shuffleButton;  // the shuffle button

        private void Awake() => shuffleButton.onClick.AddListener(StartShuffle);
        
        /// <summary>
        /// Starts to shuffle the grid
        /// </summary>
        private static void StartShuffle() => GridManager.Instance.Shuffle();
        
    }
}
