using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class FuelUpgradeBehaviour : BaseUpgradePart
    {
        /// <summary>
        /// changes the stats used for gameplay upon upgrade
        /// </summary>
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.FuelContinuousDrain = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}