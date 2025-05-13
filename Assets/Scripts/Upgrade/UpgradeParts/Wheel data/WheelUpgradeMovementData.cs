using System;
using UnityEngine;

namespace Upgrade.UpgradeParts.Wheel_data
{
    [Serializable]
    public struct WheelUpgradeMovementData
    {
        public GameObject wheelSpawnLocation;   //The location where the wheel should spawn.
        public GameObject wheel;                //The refrence to the wheel gameObject.
    }
}
