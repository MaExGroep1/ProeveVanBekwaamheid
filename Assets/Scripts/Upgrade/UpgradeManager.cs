using System;
using System.Collections.Generic;
using Blocks;
using Grid;
using UnityEngine;
using Util;

namespace User
{
    public class UpgradeManager : Singleton<UpgradeManager>
    {
        [Tooltip("The amount of upgrade points required for upgrades, increases every upgrade.")]
        [SerializeField] private int upgradePointsBaseRequirement;                      //The base amount of points needed for an upgrade
        
        [Tooltip("The amount of extra points required after a part has been upgraded.")]
        [SerializeField] private int upgradePointsRequirementIncrease;                  //The amount of extra points required after a part has been upgraded
        
        private Dictionary<BlockType, int> _upgradePoints;                              //The amount of points gained for an upgrade per upgrade type <upgradeType, upgradePoints>
        private Dictionary<BlockType, int> _requiredUpgradePoints;                      //The amount of points required for the nest upgrade per upgrade type <upgradeType, requiredUpgradePoints>
        private Dictionary<BlockType, Action<int>> _onPointIncreaseByType = new();      //Events that get invoked whenever you gain upgrade points per upgrade type <upgradeType, event<upgradePoints>>
        private Dictionary<BlockType, Action> _onUpgradeRequirementReached = new();     //Events that get invoked whenever you have enough points for an upgrade per upgrade type <upgradeType, event>
        private Dictionary<BlockType, Action> _onUpgrade = new();                       //Events that get called whenever you upgrade your car per upgrade type <upgradeType, event>
        
        public Dictionary<BlockType, int> RequiredUpgradePoints  { get => _requiredUpgradePoints; private set => _requiredUpgradePoints = value; }                  //getter/setter for _requiredUpgradePoints
        public Dictionary<BlockType, Action<int>> OnPointIncreaseByType { get => _onPointIncreaseByType; set => _onPointIncreaseByType = value; }                   //getter/setter for _onPointIncreaseByType
        public Dictionary<BlockType, Action> OnUpgradeRequirementReached { get => _onUpgradeRequirementReached; set => _onUpgradeRequirementReached = value; }      //getter/setter for _onUpgradeRequirementReached
        public Dictionary<BlockType, Action> OnUpgrade { get => _onUpgrade; set => _onUpgrade = value; } 
        
        public Action<BlockType> TempOnUpgrade;
        //getter/setter for _onUpgrade
        
        /// <summary>
        /// assigns events and dictionaries
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            AssignEvents();
            AssignUpgradePoints();
        }
        
        /// <summary>
        /// Increase the points for a specific upgradeType by upgradePoints amount, in the associated dictionary.
        /// If the required amount of upgrade points is reached for an upgrade, it calls the associated event to signal that.
        /// If the points required are not reached, it only calls the associated event that points have been gained.
        /// </summary>
        /// <param name="upgradeType"></param>
        /// <param name="upgradePoints"></param>
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
        
        /// <summary>
        /// Gets called when the player upgrade their vehicle.
        /// retracts the amount of upgrade points needed for the upgrade, increases the requirement for the next upgrade,
        /// calls the associated event for that upgrade.
        /// If upgrade points are left, it resets the amount of points and then re-adds them to make sure that the associated are triggerd
        /// todo add upgrade car comments
        /// </summary>
        /// <param name="upgradeType"></param>
        public void Upgrade(BlockType upgradeType)
        {
            //var pointsLeft = _upgradePoints[upgradeType] - _requiredUpgradePoints[upgradeType];
            //_upgradePoints[upgradeType] -= _requiredUpgradePoints[upgradeType];
            
            _upgradePoints[upgradeType] = 0;
            
            _requiredUpgradePoints[upgradeType] += upgradePointsRequirementIncrease;
            OnUpgrade[upgradeType]?.Invoke();
            
            /*if (_upgradePoints[upgradeType] == 0) return;
            _upgradePoints[upgradeType] = 0;
            IncreaseUpgradePoints(upgradeType, pointsLeft);*/
            
            //todo upgrade car
            
            TempOnUpgrade.Invoke(upgradeType);
        }
        
        /// <summary>
        /// Assigns the dictionaries _upgradePoints and RequiredUpgradePoints
        /// makes the local associated dictionaries, then loops through all blockTypes
        /// assigns the blockTypes and 0 to upgradePoints (because you don't start with points)
        /// assigns the blockTypes and upgradePointsBaseRequirement to requiredUpgradePoints
        /// then assigns the local dictionaries to the associated global dictionaries
        /// </summary>
        private void AssignUpgradePoints()
        {
            string[] blockTypes = Enum.GetNames(typeof(BlockType));

            var upgradePoints = new Dictionary<BlockType, int>();
            var requiredUpgradePoints = new Dictionary<BlockType, int>();

            for (int i = 0; i < blockTypes.Length; i++)
            {
                upgradePoints.Add((BlockType)i, 0);
                requiredUpgradePoints.Add((BlockType)i, upgradePointsBaseRequirement);
            }

            _upgradePoints = upgradePoints;
            RequiredUpgradePoints = requiredUpgradePoints;
        }
        
        /// <summary>
        /// Assigns events
        /// </summary>
        private void AssignEvents()
        {
            GridManager.Instance.ListenToOnMatch(IncreaseUpgradePoints);
        }
    }
}
