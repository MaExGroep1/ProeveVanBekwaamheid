using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class DefenseUpgradeBehaviour : BaseUpgradePart
    {
        /// <summary>
        /// changes the stats used for gameplay upon upgrade
        /// </summary>
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.DefenseFuelDrain = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}
