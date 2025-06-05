using System;
using System.Collections;
using Blocks;
using UnityEngine;
using Upgrade;
using Util;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class UpgradeUiBehaviour : MonoBehaviour
    {
        [SerializeField] private BlockType upgradeType;     // the type of upgrade the UI element represents
        [SerializeField] private Image completedImage;      // the reference to the coloured image that represents the amount of upgrade points you have gained
        [SerializeField] private float imageFillDelay;      // the time between each increment of filling the image
        [SerializeField] private SquishAndStretch warp;     // the warp of the upgrade bar
        
        private Action _onComplete;                         // when the bar reaches 100%
    
        /// <summary>
        /// Assigns events and calculates the _progressAmountPerPointPercentage
        /// Start instead of awake to make sure that UpgradeManager.RequiredUpgradePoints is assigned before it is used by CalculateProgressAmountPerPoint()
        /// </summary>
        private void Start() => 
            AssignEvents();
        

        /// <summary>
        /// Updates the progressbar by filling the complete image
        /// Fills the completeImage by multiplying points and _progressAmountPerPointPercentage
        /// </summary>
        /// <param name="points"> Increase the upgrade by this amount </param>
        private void UpdateProgressBar(int points) =>
            StartCoroutine(IncreaseFillAmount(points));
    
        /// <summary>
        /// Gets called when the associated upgrade type is getting upgraded
        /// turns the upgrade button off, re-calculates the _progressAmountPerPointPercentage and sets the completed image to 0% fill
        /// </summary>
        private void Upgrade()
        {
            completedImage.fillAmount = 0;
            _onComplete?.Invoke();
        }
        
        /// <summary>
        /// Adds items to UpgradeManager dictionaries and assigns associated functions to the events
        /// </summary>
        private void AssignEvents()
        {
            if (!UpgradeManager.Instance.OnPointIncreaseByType.TryAdd(upgradeType, UpdateProgressBar)) UpgradeManager.Instance.OnPointIncreaseByType[upgradeType] += UpdateProgressBar;
            if (!UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade)) UpgradeManager.Instance.OnUpgrade[upgradeType] += Upgrade;
        }

        /// <summary>
        /// Fills the upgrade bar little by little to the amount of points gained
        /// </summary>
        /// <param name="points"> Amount of points to increase by </param>
        /// <returns></returns>
        private IEnumerator IncreaseFillAmount(int points)
        {
            var usedPoints = 0;
            var image = completedImage;
            while (usedPoints <= points || image.fillAmount >= 1)
            {
                usedPoints += 1;
                warp.WarpObject();
                image.fillAmount = UpgradeManager.Instance.GetPercentFill(upgradeType,usedPoints - points);
                yield return new WaitForSeconds(imageFillDelay);
            }
        }
        
        /// <summary>
        /// Adds listeners to onComplete
        /// </summary>
        /// <param name="onComplete"> The added event </param>
        public void AddListener(Action onComplete) => _onComplete += onComplete;
    }
}
