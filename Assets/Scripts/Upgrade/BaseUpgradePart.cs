using Blocks;
using UnityEngine;
using User;

namespace Upgrade
{
    public abstract class BaseUpgradePart : MonoBehaviour
    {
        [SerializeField] protected BlockType upgradeType; 
        [SerializeField] protected UpgradeValues[] upgradeValues;
        [SerializeField] protected float appearSpeed;
        
        protected int _upgradeLevel;

        protected virtual void Start()
        {
            AssignEvents(); 
            IncreaseUpgradeStats();
        }

        protected abstract void IncreaseUpgradeStats();
        private void ChangeCarVisuals()
        {
            var carPart = upgradeValues[_upgradeLevel].visuals;
            if (!carPart) return;
           
            var bumperPart = Instantiate(carPart, transform.position, transform.rotation, transform);
            bumperPart.transform.localScale = Vector3.zero;

            LeanTween.scale(bumperPart.gameObject, new Vector3(-1, 1, 1), appearSpeed).setEaseOutBack();
        }
        
        private void AssignEvents()
        {
            if (!UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade)) UpgradeManager.Instance.OnUpgrade[upgradeType] += Upgrade;
        }

        private void Upgrade()
        {
            _upgradeLevel++;
            if (_upgradeLevel >= upgradeValues.Length) return;
            IncreaseUpgradeStats();
            ChangeCarVisuals();
        }
    }
}
