using System;
using Blocks;
using UnityEngine;

namespace Grid
{
    public class TestGridListener : MonoBehaviour
    {
        private void Start()
        {
            GridManager.Instance.ListenToOnMatch(OnMatch);
        }

        private void OnDestroy()
        {
            throw new NotImplementedException();
        }

        private void OnMatch(int amount, BlockType type)
        {
            Debug.Log($"{amount} of {type}");
        }
    }
}
