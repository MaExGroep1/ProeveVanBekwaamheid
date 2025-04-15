using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class SpeedUpgradeBehaviour : BaseUpgradePart
    {
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.Speed = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}
