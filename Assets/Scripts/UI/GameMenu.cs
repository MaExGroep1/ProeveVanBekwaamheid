using UnityEngine;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        private void Awake() => Time.timeScale = 0f;

        private void OnDestroy() => Time.timeScale = 1f;
    }
}
