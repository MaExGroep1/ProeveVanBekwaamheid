using Enemy;
using UnityEngine;

namespace Weapon
{
    public class SlingShotWeapon : ProjectileWeapon
    {
        [SerializeField] private float fireTime;                    //The time it takes for the fireArm to shoot forward
        [SerializeField] private float reloadTime;                  //The time it takes for the fireArm to reset its position
        [SerializeField] private Transform fireLocation;            //The location of the fireArm
        [SerializeField] private GameObject fireArm;                //The fireArm
        [SerializeField] private LeanTweenType fireEaseType;        //The leanTween easing used to move the fireArm when shooting
        [SerializeField] private LeanTweenType reloadEaseType;      //The leanTween easing used to move the fireArm when reloading
        
        /// <summary>
        /// Move the fireArm forwards and takes the ammo with it
        /// on complete tells the ammo to shoot to the closest target,
        /// then resets the fire arm
        /// </summary>
        /// <param name="ammo">The ammunition that will be fired</param>
        protected override void Shoot(WeaponProjectile ammo)
        {
            var fireArmObject = fireArm;
            var fireArmLocation = fireArmObject.transform.localPosition;
            ammo.transform.parent = fireArmObject.transform;
            LeanTween.moveLocal(fireArmObject.gameObject, fireLocation.localPosition, fireTime).setEase(fireEaseType).setOnComplete(()=>
            {
                ammo.Shoot(GetClosestTarget());
                LeanTween.moveLocal(fireArmObject.gameObject, fireArmLocation, reloadTime).setEase(reloadEaseType).setOnComplete(ShootSequence);
            });
            
        }
    }
}
