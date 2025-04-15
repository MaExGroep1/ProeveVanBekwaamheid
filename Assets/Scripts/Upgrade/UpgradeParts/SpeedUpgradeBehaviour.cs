using Car;
using UnityEngine;
using Upgrade.UpgradeParts.Wheel_logica;

namespace Upgrade.UpgradeParts
{
    public class SpeedUpgradeBehaviour : BaseUpgradePart
    {
        [SerializeField] private float disappearSpeed;
        [SerializeField] private WheelUpgradeMovementData[] wheelMovementData;
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.Speed = upgradeValues[_upgradeLevel].newUpgradeValue;
        }

        protected override void ChangeCarVisuals()
        {
            var upgradeValue = upgradeValues[_upgradeLevel].visuals;
            if (!upgradeValue) return;

            for (int i = 0; i < wheelMovementData.Length; i++)
            {
                var wheelData = wheelMovementData[i];
                var newWheel = Instantiate(upgradeValue, wheelData.wheelSpawnLocation.transform.position, wheelData.wheelLocation.transform.rotation, wheelData.wheelLocation.transform);
                
                LeanTween.scale(wheelData.wheel, Vector3.zero, disappearSpeed).setEaseInBounce();
                LeanTween.move(newWheel, wheelData.wheelLocation.transform.position, appearSpeed).setEaseOutQuint();
                
                var newWheelData = wheelData;
                newWheelData.wheel = newWheel;
                wheelMovementData[i] = newWheelData;
            }
        }
    }
}
