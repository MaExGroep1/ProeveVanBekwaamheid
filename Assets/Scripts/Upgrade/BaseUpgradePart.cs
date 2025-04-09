using Blocks;
using UnityEngine;
using User;

namespace Upgrade
{
    public abstract class BaseUpgradePart : MonoBehaviour
    {
        [SerializeField] protected BlockType upgradeType; 
        [SerializeField] protected UpgradeValues[] upgradeValues;
        
        protected int _upgradeLevel;

        protected virtual void Start()
        {
            AssignEvents(); 
            IncreaseUpgradeStats();
        }
        
        protected abstract void IncreaseUpgradeStats();
        protected abstract void ChangeCarVisuals();
        
        private void AssignEvents()
        {
            if (!UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade)) UpgradeManager.Instance.OnUpgrade[upgradeType] += Upgrade;
        }

        private void Upgrade()
        {
            _upgradeLevel++;
            IncreaseUpgradeStats();
            ChangeCarVisuals();
        }
    }
}
