using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using User;

namespace Upgrade
{
    public abstract class BaseUpgradePart : MonoBehaviour
    {
        [SerializeField] protected UpgradeValues[] upgradeValues;      //The upgrade stats used for gameplay and the associated visuals.
        [SerializeField] protected BlockType upgradeType;              //The type of upgrade belonging to this upgrade part.
        [SerializeField] protected Transform startTransform;           //The transform of the object where the upgrade parts should be instantiated.
        [SerializeField] protected float appearTime;                   //The time it takes for the upgrade parts to move from their start transform to the right position on the car
        
        protected int _upgradeLevel;                                   //The current level of upgrades that this upgrade type has

        protected virtual void Start()
        {
            AssignEvents(); 
            IncreaseUpgradeStats();
        }

        /// <summary>
        /// changes the associated stats used for gameplay upon upgrade: increasing fuel amount or increasing speed for example
        /// </summary>
        protected abstract void IncreaseUpgradeStats();
        
        /// <summary>
        /// Add the new upgrade visuals to the car
        /// </summary>
        protected virtual void ChangeCarVisuals()
        {
            var carPart = upgradeValues[_upgradeLevel].visuals;
            if (!carPart) return;
           
            var upgradePart = Instantiate(carPart, startTransform.position, transform.rotation, transform);
            StartCoroutine(MoveToCar(upgradePart));
        }
        private IEnumerator MoveToCar(GameObject upgradePart)
        {
            var elapsedTime = 0f;
            var duration = appearTime;
            Vector3 startPosition = upgradePart.transform.position;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
                upgradePart.transform.position = Vector3.Lerp(startPosition, transform.position, t);
                
                yield return null;
            }

            upgradePart.transform.position = transform.position;
            /*Vector3 position = transform.position;
            Vector3 positionDiffrence;
            float timeLeft = appearTime;
            LeanTween.move(upgradePart, transform.position, appearTime).setEaseOutQuint();
            while (LeanTween.isTweening(upgradePart))
            {
                timeLeft -= Time.deltaTime;
                positionDiffrence = transform.position - position;
                LeanTween.cancel(upgradePart);
                upgradePart.transform.position += positionDiffrence;
                LeanTween.move(upgradePart, transform.position, timeLeft).setEaseOutQuint();
                yield return null;
            }*/
        }
        
        
        /// <summary>
        /// Assigns the event to listen to when an upgrade type should be upgraded
        /// </summary>
        private void AssignEvents()
        {
            if (!UpgradeManager.Instance.OnUpgrade.TryAdd(upgradeType, Upgrade)) UpgradeManager.Instance.OnUpgrade[upgradeType] += Upgrade;
        }

        /// <summary>
        /// Calls the associated functions needed for upgrading the car and keeps track of the current upgrade level
        /// </summary>
        private void Upgrade()
        {
            _upgradeLevel++;
            if (_upgradeLevel >= upgradeValues.Length) return;
            IncreaseUpgradeStats();
            ChangeCarVisuals();
        }
    }
}
