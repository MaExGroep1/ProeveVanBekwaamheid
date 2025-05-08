using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    [RequireComponent(typeof(Button))]
    public class SceneTransitionButton : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        
        private void Awake() => 
            GetComponent<Button>()
                .onClick.AddListener(LoadScene);
        
        private void LoadScene() => SceneManager.Instance.LoadScene(sceneToLoad);
    }
}
