using Car;
using UnityEngine;

namespace Upgrade.UpgradeParts
{
    public class AttackUpgradeBehaviour : BaseUpgradePart
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform removeTransform;
        [SerializeField] private float removeSpeed;
        [SerializeField] private bool isAppearing;
        private GameObject _currentUpgradeVisuals;
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.AttackSpeedReduction = upgradeValues[_upgradeLevel].newUpgradeValue;
        }

        protected override void ChangeCarVisuals()
        { 
            var upgradeVisual = upgradeValues[_upgradeLevel].visuals;
           if (upgradeVisual == null) return;

           
           var bumperPart = Instantiate(upgradeVisual, startTransform.position, upgradeVisual.transform.rotation, transform);


           if (isAppearing)
           {
               bumperPart.transform.localScale = Vector3.zero;
               bumperPart.transform.position = targetTransform.position;
               if (_currentUpgradeVisuals != null)
               {
                   LeanTween.scale(_currentUpgradeVisuals, Vector3.zero, removeSpeed);
               }

               LeanTween.scale(bumperPart.gameObject, Vector3.one, moveSpeed);
               _currentUpgradeVisuals = bumperPart;
               return;
           }
           
           if (_currentUpgradeVisuals != null)
           {
               LeanTween.move(_currentUpgradeVisuals, removeTransform.position, removeSpeed);
           }

           LeanTween.move(bumperPart.gameObject, targetTransform, moveSpeed);
           _currentUpgradeVisuals = bumperPart;
        }
    }
}
