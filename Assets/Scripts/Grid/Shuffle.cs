using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    public class Shuffle : MonoBehaviour
    {
        [SerializeField] private Button shuffleButton;

        private void Awake() => shuffleButton.onClick.AddListener(StartShuffle);
        
        private void StartShuffle() => GridManager.Instance.Shuffle();
        
    }
}
