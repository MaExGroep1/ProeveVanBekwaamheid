using Car;

namespace Upgrade.UpgradeParts
{
    public class WeaponUpgradeBehaviour : BaseUpgradePart
    {
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.WeaponFireRate = upgradeValues[_upgradeLevel].newUpgradeValue;
        }

        protected override void ChangeCarVisuals()
        {
        }
    }
}
