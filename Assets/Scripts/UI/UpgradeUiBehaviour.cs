using System;
using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using UnityEngine.UIElements;
using User;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UpgradeUiBehaviour : MonoBehaviour
{
    [SerializeField] private BlockType upgradeType;
    [SerializeField] private Image completedImage;
    [SerializeField] private Button upgradeButton;

    private float _progressAmountPerPointPercentage;
    private void Start()
    {
        AssignEvents();
        _progressAmountPerPointPercentage = CalculateProgressAmountPerPoint(UpgradeManager.Instance.RequiredUpgradePoints[upgradeType]);
    }

    private void UpdateProgressBar(int points)
    {
        completedImage.fillAmount += _progressAmountPerPointPercentage * points;
    }

    private void ReadyForUpgrade()
    {
        upgradeButton.interactable = true;
        completedImage.fillAmount = 1;
    }
    
    private void Upgrade()
    {
        upgradeButton.interactable = false;
        _progressAmountPerPointPercentage = CalculateProgressAmountPerPoint(UpgradeManager.Instance.RequiredUpgradePoints[upgradeType]);
        completedImage.fillAmount = 0;
    }

    private float CalculateProgressAmountPerPoint(int upgradePointsRequired)
    {
        if (upgradePointsRequired == 0) return 0;
        var increasePerPoint = 100f / upgradePointsRequired;
        return increasePerPoint / 100f;
    }

    private void AssignEvents()
    {
        UpgradeManager.Instance.OnPointIncreaseByType.TryAdd(upgradeType, UpdateProgressBar); 
        UpgradeManager.Instance.OnUpgradeRequirementReached.TryAdd(upgradeType, ReadyForUpgrade);
        UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade);
    }
}
