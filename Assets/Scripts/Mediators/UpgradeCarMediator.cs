using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using UnityEngine.Serialization;
using User;

public class UpgradeCarMediator : MonoBehaviour
{
    [SerializeField] private BlockType upgradeType;
    
    /// <summary>
    /// Calls upgrade in UpgradeManager and gives upgradeType as upgradeType
    /// designed to be used by a UI button
    /// </summary>
    public void UpgradeCar()
    {
        UpgradeManager.Instance.Upgrade(upgradeType);
    }
}
