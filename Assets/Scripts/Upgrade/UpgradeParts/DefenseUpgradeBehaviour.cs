using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class DefenseUpgradeBehaviour : BaseUpgradePart
    {
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.DefenseFuelDrain = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}
