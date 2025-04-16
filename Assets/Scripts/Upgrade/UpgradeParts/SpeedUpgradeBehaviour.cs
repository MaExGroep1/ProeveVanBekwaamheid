using Car;
using UnityEngine;
using Upgrade.UpgradeParts.Wheel_data;

namespace Upgrade.UpgradeParts
{
    public class SpeedUpgradeBehaviour : BaseUpgradePart
    {
        [SerializeField] private float disappearSpeed;                                  //The speed at which the old tires should disappear.
        [SerializeField] private WheelUpgradeMovementData[] wheelMovementData;          //Struct which collects the wheel gameObjects and associated location and spawning location.
        
        /// <summary>
        /// Changes the stats used for gameplay upon upgrade.
        /// </summary>
        protected override void IncreaseUpgradeStats()
        {
            CarData.Instance.Speed = upgradeValues[_upgradeLevel].newUpgradeValue;
        }

        /// <summary>
        /// Removes the old tires and adds the new ones.
        /// </summary>
        protected override void ChangeCarVisuals()
        {
            var upgradeValue = upgradeValues[_upgradeLevel].visuals;
            if (!upgradeValue) return;

            for (int i = 0; i < wheelMovementData.Length; i++)
            {
                var wheelData = wheelMovementData[i];
                var newWheel = Instantiate(upgradeValue, wheelData.wheelSpawnLocation.transform.position, wheelData.wheelLocation.transform.rotation, wheelData.wheelLocation.transform);
                
                LeanTween.scale(wheelData.wheel, Vector3.zero, disappearSpeed).setEaseInBack()
                    .setOnComplete(() => ChangeWheels(newWheel, wheelData.wheelLocation.transform.position));
                
                var newWheelData = wheelData;
                newWheelData.wheel = newWheel;
                wheelMovementData[i] = newWheelData;
            }
        }

        /// <summary>
        /// Adds the new tires by using scaling and moving.
        /// </summary>
        /// <param name="newWheel">the new wheels that are going to be added to the car</param>
        /// <param name="wheelLocation">the location where the wheels should end up</param>
        private void ChangeWheels(GameObject newWheel, Vector3 wheelLocation)
        {
            newWheel.transform.localScale = Vector3.zero;
            LeanTween.move(newWheel, wheelLocation, appearTime).setEaseOutQuint();
            LeanTween.scale(newWheel, Vector3.one, disappearSpeed).setEaseOutBack();
        }
    }
}
