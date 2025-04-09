using Car;
using User;

namespace Upgrade.UpgradeParts
{
    public class AttackUpgradeBehaviour : BaseUpgradePart
    {
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.AttackSpeedReduction = upgradeValues[_upgradeLevel].newUpgradeValue;
        }

        protected override void ChangeCarVisuals()
        {
        }
    }
}
