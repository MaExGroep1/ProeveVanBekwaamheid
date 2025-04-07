using System;
using UnityEngine;

namespace Tiles
{
    public class TileLoader : MonoBehaviour
    {
        public Action OnLoad;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnLoad?.Invoke();
        }
    }
}
