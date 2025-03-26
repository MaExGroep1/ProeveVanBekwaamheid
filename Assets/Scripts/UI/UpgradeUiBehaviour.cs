using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using User;

public class UpgradeUiBehaviour : MonoBehaviour
{
    [SerializeField] private BlockType upgradeType;
    public void ReadyForUpgrade()
    {
        Debug.Log(gameObject.name + " = ready for upgrade!");
    }

    public void Upgrade()
    {
        UpgradeManager.Instance.OnUpgrade.Invoke(upgradeType);
    }
}
