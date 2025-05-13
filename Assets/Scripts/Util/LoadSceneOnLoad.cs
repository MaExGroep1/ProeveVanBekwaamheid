using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class LoadSceneOnLoad : MonoBehaviour
    {
        [SerializeField] private string sceneName;          // the scene to switch to
        [SerializeField] private float fadeInTime = 0.5f;   // the time to fade

        private void Start() => SceneManager.Instance.LoadScene(sceneName,fadeInTime);
    }
}
