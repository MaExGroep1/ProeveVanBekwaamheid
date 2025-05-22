using System;
using UnityEngine;

namespace Upgrade
{
    [Serializable]
    public struct UpgradeValues
    {
        [Tooltip("The value used in gameplay for this upgrade. The values are: attack damage, defenseFuelDrain, fuelContinuousDrain, speed and weapon damage multiplier")]
        public float newUpgradeValue;       //The value used for every upgrade level to upgrade the car gameplay wise
        public GameObject visuals;          //The visuals that represent an upgrade level
    }
}
