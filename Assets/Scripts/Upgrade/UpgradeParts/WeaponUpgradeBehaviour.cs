using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class WeaponUpgradeBehaviour : BaseUpgradePart
    {
        
        /// <summary>
        /// changes the stats used for gameplay upon upgrade
        /// </summary>
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.WeaponFireRate = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}
