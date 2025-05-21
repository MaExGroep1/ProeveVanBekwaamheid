using System;
using Grid;
using UnityEngine;

namespace CarGame
{
    public class WaitForStart : MonoBehaviour
    {
        [SerializeField] private Behaviour script;  // the script to enable at the start of the car game
        
        /// <summary>
        /// Adds listeners to the grid onFirstMatch Action 
        /// </summary>
        private void Awake() =>
            GridManager.Instance.ListenToOnFirstMatch(SetActive);
        
        /// <summary>
        /// Sets active the script and destroys itself
        /// </summary>
        private void SetActive()
        {
            script.enabled = true;
            Destroy(this);
        } 
    }
}
