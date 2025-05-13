using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    [RequireComponent(typeof(Button))]
    public class SceneTransitionButton : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;    // the target scene
        
        /// <summary>
        /// Gets the button and adds a listener
        /// </summary>
        private void Awake() => 
            GetComponent<Button>()
                .onClick.AddListener(LoadScene);
        
        /// <summary>
        /// Starts a scene switch from the SceneManager
        /// </summary>
        private void LoadScene() => SceneManager.Instance.LoadScene(sceneToLoad);
    }
}
