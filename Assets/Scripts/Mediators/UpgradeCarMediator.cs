using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using UnityEngine.Serialization;
using User;

public class UpgradeCarMediator : MonoBehaviour
{
    [SerializeField] private BlockType upgradeType;
    
    public void UpgradeCar()
    {
        UpgradeManager.Instance.Upgrade(upgradeType);
    }
}
