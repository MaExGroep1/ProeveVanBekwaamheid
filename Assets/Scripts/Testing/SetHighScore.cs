using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace Testing
{
    public class SetHighScore : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;

        private void Awake() => button.onClick.AddListener(AddHighScore);

        private void AddHighScore() => HighScore.SetScore(int.Parse(inputField.text));
        
    }
}
