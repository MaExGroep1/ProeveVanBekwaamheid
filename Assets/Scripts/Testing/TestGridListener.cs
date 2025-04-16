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

        private void OnMatch(BlockType type, int amount)
        {
            Debug.Log($"{amount} of {type}");
        }
    }
}
