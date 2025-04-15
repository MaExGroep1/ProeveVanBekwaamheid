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
        [SerializeField] protected Transform spawnTransform;
        
        protected int _upgradeLevel;

        protected virtual void Start()
        {
            AssignEvents(); 
            IncreaseUpgradeStats();
        }

        protected abstract void IncreaseUpgradeStats();
        protected virtual void ChangeCarVisuals()
        {
            /*var upgradeValue = upgradeValues[_upgradeLevel].visuals;
            if (!upgradeValue) return;
           
            var carPart = Instantiate(upgradeValue, transform.position, transform.rotation, transform);
            carPart.transform.localScale = Vector3.zero;

            LeanTween.scale(carPart.gameObject, new Vector3(-1, 1, 1), appearSpeed).setEaseOutQuint();*/
            
            var carPart = upgradeValues[_upgradeLevel].visuals;
            if (!carPart) return;
           
            var upgradePart = Instantiate(carPart, spawnTransform.position, transform.rotation, transform);
            upgradePart.transform.localScale = new Vector3(-1, 1, 1);
            
            LeanTween.move(upgradePart, transform.position, appearSpeed).setEaseOutQuint();
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
