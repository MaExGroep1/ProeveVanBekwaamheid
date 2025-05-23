using System;
using Grid;
using UnityEngine;

namespace UI
{
    public class MatchToStart : MonoBehaviour
    {
        [SerializeField] private float bob;                 // the amount of bob the menu does
        [SerializeField] private float bobTime;             // the amount of time the menu needs to bob
        [SerializeField] private float moveTime;            // the amount of time the menu needs to move off-screen
        [SerializeField] private Transform targetOffScreen; // the place to move when going off-screen
        
        /// <summary>
        /// Makes the menu bob and starts listening to the match 3
        /// </summary>
        private void Awake()
        {
            LeanTween.scale(gameObject, Vector3.one * bob, bobTime)
                .setLoopPingPong()
                .setEase(LeanTweenType.easeInOutQuad);
            GridManager.Instance.ListenToOnFirstMatch(LeaveScreen);
        }
        
        /// <summary>
        /// Makes the menu leave the screen
        /// </summary>
        private void LeaveScreen()
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveY(gameObject, targetOffScreen.position.y, moveTime)
                .setEase(LeanTweenType.easeInBack)
                .setDestroyOnComplete(true);
        }
    }
}
