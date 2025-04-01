using Blocks;
using UnityEngine;
using User;

public class UpgradeCarMediator : MonoBehaviour
{
    [SerializeField] private BlockType upgradeType;     //The type of upgrade the UI element represents
    
    /// <summary>
    /// Calls upgrade in UpgradeManager and gives upgradeType as upgradeType
    /// designed to be used by a UI button
    /// </summary>
    public void UpgradeCar()
    {
        UpgradeManager.Instance.Upgrade(upgradeType);
    }
}
