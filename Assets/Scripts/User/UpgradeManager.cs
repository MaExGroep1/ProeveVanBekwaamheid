using System;
using System.Collections.Generic;
using Blocks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace User
{
    [Serializable]
    public struct UiElement
    {
        public BlockType type;
        public UpgradeUiBehaviour behaviour;
    }
    public class UpgradeManager : Util.Singleton<UpgradeManager>
    {
        [Tooltip("The starting amount of upgrade points required for the first upgrade.")]
        [SerializeField] private int upgradePointsBaseRequirement;      //The starting amount of upgrade points required for the first upgrade
        
        [Tooltip("The amount of extra points required after a part has been upgraded.")]
        [SerializeField] private int upgradePointsRequirementIncrease;  //The amount of extra points required after a part has been upgraded

        //[SerializeField] private UpgradeUiBehaviour[] upgradeUiBehavioursObjects = new UpgradeUiBehaviour[Enum.GetNames(typeof(BlockType)).Length];
        [SerializeField] private UiElement[] uiElements = new UiElement[Enum.GetNames(typeof(BlockType)).Length];

        private Dictionary<BlockType, int> _upgradePoints;
        private Dictionary<BlockType, int> _requiredUpgradePoints;
        private Dictionary<BlockType, UpgradeUiBehaviour> _upgradeUiBehaviours;
        
        public Action<BlockType> OnUpgrade;


        private void Awake()
        {
            AssignUpgradePoints();
            AssignRequiredUpgradePoints();
            AssignUpgradeUiBehaviours();
        }

        public void increaseUpgradePoints(BlockType upgradeType, int upgradePoints)
        {
            _upgradePoints[upgradeType] += upgradePoints;
            
            if (_upgradePoints[upgradeType] <= _requiredUpgradePoints[upgradeType]) return;
            if (_upgradeUiBehaviours[upgradeType] != null) _upgradeUiBehaviours[upgradeType].ReadyForUpgrade();
            //todo add upgrade UI logic
            // should activate the UI to show the user that they can upgrade a part of their vehicle.
            // Upon upgrade it should retract the amount of points used for the upgrade from _upgradePoints and increase _requiredUpgradePoints by upgradePointsRequirementIncreased and update the ui
        }

        private void Upgrade(BlockType upgradeType)
        {
            _upgradePoints[upgradeType] -= _requiredUpgradePoints[upgradeType];
            _requiredUpgradePoints[upgradeType] += upgradePointsRequirementIncrease;
            //todo upgrade car
            //todo update UI
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

            _requiredUpgradePoints = requiredUpgradePoints;
        }
        private void AssignUpgradeUiBehaviours()
        {
            foreach (var uiElement in uiElements)
            {
                _upgradeUiBehaviours.Add(uiElement.type, uiElement.behaviour);
            }
        }
    }
}
