using UnityEngine;

namespace Util
{
    public class SceneManager : Singleton<SceneManager>
    {

        [SerializeField] private float defaultLoadTime;
        [SerializeField] private CanvasGroup transitionFade;
        
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName, float loadTime = Mathf.Infinity)
        {
            loadTime = float.IsPositiveInfinity(loadTime) ? defaultLoadTime : loadTime;
            StartSceneLoad(sceneName, loadTime);
        }

        private void StartSceneLoad(string sceneName, float loadTime)
        {
            LeanTween.alphaCanvas(transitionFade, 1f, loadTime / 2)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeOutSine)
                .setOnComplete(() => EndSceneLoad(sceneName, loadTime));
        }

        private void EndSceneLoad(string sceneName, float loadTime)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            LeanTween.alphaCanvas(transitionFade, 0f, loadTime / 2)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInSine);
        }
    }
}
