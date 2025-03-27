using System;
using System.Collections.Generic;
using Blocks;
using UnityEngine;


namespace User
{
    public class UpgradeManager : Util.Singleton<UpgradeManager>
    {
        [Tooltip("The amount of upgrade points required for upgrades, increases every upgrade.")]
        [SerializeField] private int upgradePointsBaseRequirement;      //The base amount of points needed for an upgrade
        
        [Tooltip("The amount of extra points required after a part has been upgraded.")]
        [SerializeField] private int upgradePointsRequirementIncrease;  //The amount of extra points required after a part has been upgraded
        
        private Dictionary<BlockType, int> _upgradePoints;
        private Dictionary<BlockType, int> _requiredUpgradePoints;
        private Dictionary<BlockType, Action<int>> _onPointIncreaseByType = new();
        private Dictionary<BlockType, Action> _onUpgradeRequirementReached = new();
        private Dictionary<BlockType, Action> _onUpgrade = new();


        public Dictionary<BlockType, int> RequiredUpgradePoints  { get => _requiredUpgradePoints; private set => _requiredUpgradePoints = value; }
        public Dictionary<BlockType, Action<int>> OnPointIncreaseByType { get => _onPointIncreaseByType; set => _onPointIncreaseByType = value; }
        public Dictionary<BlockType, Action> OnUpgradeRequirementReached { get => _onUpgradeRequirementReached; set => _onUpgradeRequirementReached = value; }
        public Dictionary<BlockType, Action> OnUpgrade { get => _onUpgrade; set => _onUpgrade = value; }

        protected override void Awake()
        {
            base.Awake();
            
            AssignUpgradePoints();
            AssignRequiredUpgradePoints(); 
        }

        public void IncreaseUpgradePoints(BlockType upgradeType, int upgradePoints)
        {
            _upgradePoints[upgradeType] += upgradePoints;
            
            if (_upgradePoints[upgradeType] >= _requiredUpgradePoints[upgradeType])
            {
                if (!OnUpgradeRequirementReached.TryGetValue(upgradeType, out Action upgradeReachedAction)) return;
                upgradeReachedAction?.Invoke();
                return;
            }
            
            if (!OnPointIncreaseByType.TryGetValue(upgradeType, out Action<int> increasePointAction)) return;
            increasePointAction?.Invoke(upgradePoints);
        }
        
        
        public void Upgrade(BlockType upgradeType)
        {
            _upgradePoints[upgradeType] -= _requiredUpgradePoints[upgradeType];
            _requiredUpgradePoints[upgradeType] += upgradePointsRequirementIncrease;
            OnUpgrade[upgradeType]?.Invoke();
            
            if (_upgradePoints[upgradeType] == 0) return;
            var pointsLeft = _upgradePoints[upgradeType];
            _upgradePoints[upgradeType] = 0;
            IncreaseUpgradePoints(upgradeType, pointsLeft);
            //todo upgrade car
        }
        
        private void AssignUpgradePoints()
        {
            var upgradePoints = new Dictionary<BlockType, int>
            {
                { BlockType.Attack, 0 },
                { BlockType.Defense, 0 },
                { BlockType.Fuel, 0 },
                { BlockType.Speed, 0 },
                { BlockType.Weapon, 0 }
            };

            _upgradePoints = upgradePoints;
        }

        private void AssignRequiredUpgradePoints()
        {
            var requiredUpgradePoints = new Dictionary<BlockType, int>
            {
                { BlockType.Attack, upgradePointsBaseRequirement },
                { BlockType.Defense, upgradePointsBaseRequirement },
                { BlockType.Fuel, upgradePointsBaseRequirement },
                { BlockType.Speed, upgradePointsBaseRequirement },
                { BlockType.Weapon, upgradePointsBaseRequirement }
            };

            RequiredUpgradePoints = requiredUpgradePoints;
        }
    }
}
