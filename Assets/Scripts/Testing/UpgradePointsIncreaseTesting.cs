using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using User;

public class UpgradePointsIncreaseTesting : MonoBehaviour
{
   public void IncreaseUpgradePointsAttack(int upgradeAmount)
   {
      UpgradeManager.Instance.IncreaseUpgradePoints(BlockType.Attack, upgradeAmount);
   }
   
   public void IncreaseUpgradePointsDefense(int upgradeAmount)
   {
      UpgradeManager.Instance.IncreaseUpgradePoints(BlockType.Defense, upgradeAmount);
   }
   
   public void IncreaseUpgradePointsFuel(int upgradeAmount)
   {
      UpgradeManager.Instance.IncreaseUpgradePoints(BlockType.Fuel, upgradeAmount);
   }
   
   public void IncreaseUpgradePointsSpeed(int upgradeAmount)
   {
      UpgradeManager.Instance.IncreaseUpgradePoints(BlockType.Speed, upgradeAmount);
   }
   
   public void IncreaseUpgradePointsWeapon(int upgradeAmount)
   {
      UpgradeManager.Instance.IncreaseUpgradePoints(BlockType.Weapon, upgradeAmount);
   }
}
