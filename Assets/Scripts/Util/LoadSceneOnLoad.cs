using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class LoadSceneOnLoad : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private float fadeInTime = 0.5f;

        private void Start() => SceneManager.Instance.LoadScene(sceneName,fadeInTime);
    }
}
