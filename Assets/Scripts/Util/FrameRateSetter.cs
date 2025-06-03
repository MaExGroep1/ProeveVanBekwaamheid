using UnityEngine;

namespace Util
{
    public class FrameRateSetter : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;  // the target FPS of the game
        
        /// <summary>
        /// Sets the target frame rate to the "TargetFrameRate"
        /// </summary>
        void Start() => Application.targetFrameRate = targetFrameRate;
    }
}
