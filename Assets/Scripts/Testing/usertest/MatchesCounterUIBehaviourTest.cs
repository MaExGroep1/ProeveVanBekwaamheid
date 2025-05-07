using System.Collections;
using Blocks;
using Grid;
using TMPro;
using UnityEngine;
using Upgrade;

namespace Testing.usertest
{
    public class MatchesCounterUIBehaviourTest : MonoBehaviour
    {
        [SerializeField] private BlockType upgradeType;    
        [SerializeField] private TextMeshProUGUI textUi;
        private int _points;
        
        private void Start()
        {
            AssignEvents();
        }

        private void UpdateText(BlockType blockType, int points)
        {
            if (blockType != upgradeType) return;
            _points += points;
            textUi.text = _points.ToString();
        }

        private void AssignEvents()
        {
            GridManager.Instance.ListenToOnMatch(UpdateText);
        }
    }
}
