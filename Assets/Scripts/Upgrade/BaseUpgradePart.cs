using Blocks;
using UnityEngine;
using User;

namespace Interfaces
{
    public abstract class BaseUpgradePart : MonoBehaviour
    {
        [SerializeField] private BlockType upgradeType;
        
        private void Start()
        {
            AssignEvents(); 
        }
        
        protected abstract void IncreaseUpgradeStats();
        protected abstract void ChangeCarVisuals();
        
        private void AssignEvents()
        {
            UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade);
        }

        private void Upgrade()
        {
            IncreaseUpgradeStats();
            ChangeCarVisuals();
        }
    }
}
