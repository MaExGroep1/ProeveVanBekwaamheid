using Car;
using UnityEngine;
using Upgrade.UpgradeParts.Wheel_data;
using System;

namespace Upgrade.UpgradeParts
{
    public class SpeedUpgradeBehaviour : BaseUpgradePart
    {
        [SerializeField] private float disappearSpeed;                                  //The speed at which the old tires should disappear.
        [SerializeField] private WheelUpgradeMovementData[] wheelMovementData;          //Struct which collects the wheel gameObjects and associated location and spawning location.
        [SerializeField] private int hoverWheelsUpgrade;                                //Upgrade level when the hoverwheels will be added, used for disabling rotation

        public Action OnUpgrade;                                                        //Action called whenever the wheels visuals upgrade
        public Action OnUpgradeComplete;                                                //Action called whenever the wheels visual has upgraded
        public Action OnHoverWheels;                                                    //Action called whenever the hoverwheels are aquired
        
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
            if (!upgradeValue)  return; 
            if (_upgradeLevel >= hoverWheelsUpgrade) OnHoverWheels?.Invoke();
            OnUpgrade?.Invoke();

            for (int i = 0; i < wheelMovementData.Length; i++)
            {
                var wheelData = wheelMovementData[i];
                var newWheelData = wheelData;
                
                var newWheel = Instantiate(upgradeValue, wheelData.wheelSpawnLocation.transform.position, wheelData.wheel.transform.rotation, wheelData.wheel.transform.parent);

                LeanTween.scale(wheelData.wheel, Vector3.zero, disappearSpeed).setEaseInBack()
                    .setOnComplete(() => ChangeWheels(newWheel));
                
                newWheelData.wheel = newWheel;
                wheelMovementData[i] = newWheelData;
            }
        }

        /// <summary>
        /// Adds the new tires by using scaling and moving.
        /// </summary>
        /// <param name="newWheel">the new wheels that are going to be added to the car</param>
        /// <param name="wheelLocation">the location where the wheels should end up</param>
        private void ChangeWheels(GameObject newWheel)
        {
            newWheel.transform.localScale = Vector3.zero;
            LeanTween.moveLocal(newWheel, Vector3.zero, appearTime).setEaseOutQuint().setOnComplete(() => OnUpgradeComplete?.Invoke());
            LeanTween.scale(newWheel, Vector3.one, disappearSpeed).setEaseOutBack();
        }
    }
}
