using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class AttackUpgradeBehaviour : BaseUpgradePart
    {
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.AttackSpeedReduction = upgradeValues[_upgradeLevel].newUpgradeValue;
        }
    }
}
