using System;
using UnityEngine;

namespace Tiles
{
    public class TileLoader : MonoBehaviour
    {
        public Action _onLoad; // triggers when near the end of the tile
        
        /// <summary>
        /// Checks if the player collides with the loader
        /// </summary>
        /// <param name="other"> The hit object </param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _onLoad?.Invoke();
        }
    }
}
