using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class FuelUpgradeBehaviour : BaseUpgradePart
    {
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.FuelContinuousDrain = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}