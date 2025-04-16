using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class AttackUpgradeBehaviour : BaseUpgradePart
    {
        /// <summary>
        /// changes the stats used for gameplay upon upgrade
        /// </summary>
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.AttackSpeedReduction = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}
