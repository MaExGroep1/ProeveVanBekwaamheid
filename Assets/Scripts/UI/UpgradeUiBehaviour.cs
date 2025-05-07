using Blocks;
using UnityEngine;
using Upgrade;
using User;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class UpgradeUiBehaviour : MonoBehaviour
    {
        [SerializeField] private BlockType upgradeType;     //The type of upgrade the UI element represents
        [SerializeField] private Image completedImage;      //The reference to the coloured image that represents the amount of upgrade points you have gained
        [SerializeField] private Button upgradeButton;      //The reference to the button that you click to upgrade your vehicle

        private float _progressAmountPerPointPercentage;    //The percentage that the completedImage should fill per point
    
        /// <summary>
        /// Assigns events and calculates the _progressAmountPerPointPercentage
        /// Start instead of awake to make sure that UpgradeManager.RequiredUpgradePoints is assigned before it is used by CalculateProgressAmountPerPoint()
        /// </summary>
        private void Start()
        {
            AssignEvents();
            _progressAmountPerPointPercentage = CalculateProgressAmountPerPoint(UpgradeManager.Instance.RequiredUpgradePoints[upgradeType]);
        }

        /// <summary>
        /// Updates the progressbar by filling the complete image
        /// Fills the completeImage by multiplying points and _progressAmountPerPointPercentage
        /// </summary>
        /// <param name="points"></param>
        private void UpdateProgressBar(int points)
        {
            completedImage.fillAmount += _progressAmountPerPointPercentage * points;
        }
    
        /// <summary>
        /// Gets called when the associated upgrade type is ready to upgrade
        /// turns the upgrade button on and sets the completed image to 100% fill
        /// </summary>
        private void ReadyForUpgrade()
        {
            upgradeButton.interactable = true;
            completedImage.fillAmount = 1;
        }
    
        /// <summary>
        /// Gets called when the associated upgrade type is getting upgraded
        /// turns the upgrade button off, re-calculates the _progressAmountPerPointPercentage and sets the completed image to 0% fill
        /// </summary>
        private void Upgrade()
        {
            upgradeButton.interactable = false;
            _progressAmountPerPointPercentage = CalculateProgressAmountPerPoint(UpgradeManager.Instance.RequiredUpgradePoints[upgradeType]);
            completedImage.fillAmount = 0;
        }

        /// <summary>
        /// calculates the percentage the completed image should fill for every upgrade point
        /// Formula: upgradePercentage = 100 / upgradePointsRequired
        /// Then divides it with 100 to return a float percentage (100% = 1)
        /// </summary>
        /// <param name="upgradePointsRequired"></param>
        /// <returns></returns>
        private float CalculateProgressAmountPerPoint(int upgradePointsRequired)
        {
            if (upgradePointsRequired == 0) return 0;
            var increasePerPoint = 100f / upgradePointsRequired;
            return increasePerPoint / 100f;
        }

        /// <summary>
        /// Adds items to UpgradeManager dictionaries and assigns associated functions to the events
        /// </summary>
        private void AssignEvents()
        {
            if (!UpgradeManager.Instance.OnPointIncreaseByType.TryAdd(upgradeType, UpdateProgressBar)) UpgradeManager.Instance.OnPointIncreaseByType[upgradeType] += UpdateProgressBar;
            if (!UpgradeManager.Instance.OnUpgradeRequirementReached.TryAdd(upgradeType, ReadyForUpgrade)) UpgradeManager.Instance.OnUpgradeRequirementReached[upgradeType] += ReadyForUpgrade;
            if (!UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade)) UpgradeManager.Instance.OnUpgrade[upgradeType] += Upgrade;
        }
    }
}
