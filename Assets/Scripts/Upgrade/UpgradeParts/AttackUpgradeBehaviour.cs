using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class AttackUpgradeBehaviour : BaseUpgradePart
    {
        [SerializeField] private Transform removeTransform;
        [SerializeField] private float removeSpeed;
        private GameObject _currentUpgradeVisuals;
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.AttackSpeedReduction = upgradeValues[_upgradeLevel].newUpgradeValue;
        }

        protected override void ChangeCarVisuals()
        {
            var upgradeVisual = upgradeValues[_upgradeLevel].visuals;
           if (upgradeVisual == null) return;
           
           var bumperPart = Instantiate(upgradeVisual, startPosition.position, upgradeVisual.transform.rotation, transform);

           if (_currentUpgradeVisuals != null)
           {
               LeanTween.move(_currentUpgradeVisuals, removeTransform.position, removeSpeed);
           }

           LeanTween.move(bumperPart.gameObject, transform.position, moveSpeed);
           _currentUpgradeVisuals = bumperPart;
        }
    }
}
