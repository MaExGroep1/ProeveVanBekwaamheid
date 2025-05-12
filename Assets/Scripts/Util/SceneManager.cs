using UnityEngine;

namespace Util
{
    public class SceneManager : Singleton<SceneManager>
    {

        [SerializeField] private float defaultLoadTime;         // the default time to switch to a different scene
        [SerializeField] private CanvasGroup transitionFade;    // the in between the scene's element
        [SerializeField] private string startSceneName;         // the scene to switch to at the start of the game
        
        /// <summary>
        /// Adds itself to the don't destroy on load then loads a different scene
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            LoadScene(startSceneName);
        }
        
        /// <summary>
        /// Checks the load time then starts the transition
        /// </summary>
        /// <param name="sceneName"> The scene to load </param>
        /// <param name="loadTime"> The time the whole load takes </param>
        public void LoadScene(string sceneName, float loadTime = Mathf.Infinity)
        {
            loadTime = float.IsPositiveInfinity(loadTime) ? defaultLoadTime : loadTime;
            StartSceneLoad(sceneName, loadTime);
        }
        
        /// <summary>
        /// Fades in the scene transition then starts the scene load
        /// </summary>
        /// <param name="sceneName"> The scene to load </param>
        /// <param name="loadTime"> The time the whole load takes </param>
        private void StartSceneLoad(string sceneName, float loadTime)
        {
            LeanTween.alphaCanvas(transitionFade ,1f , loadTime/2)
                .setEase(LeanTweenType.easeOutSine)
                .setOnComplete(() => EndSceneLoad(sceneName,loadTime));
        }

        /// <summary>
        /// Switches the scene then finishes the transition
        /// </summary>
        /// <param name="sceneName"> The scene to load </param>
        /// <param name="loadTime"> The time the whole load takes </param>
        private void EndSceneLoad(string sceneName, float loadTime)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            LeanTween.alphaCanvas(transitionFade, 0f, loadTime / 2)
                .setEase(LeanTweenType.easeInSine);
        }
    }
}
